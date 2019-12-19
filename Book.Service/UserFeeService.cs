using Book.Dal;
using Book.Dal.Model;
using Book.Model.Enums;
using Book.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Book.Service
{
    public class UserFeeService
    {
        private static UserFeeService _Instance;
        public static UserFeeService GetInstance()
        {
            if (_Instance != null)
                return _Instance;
            _Instance = new UserFeeService();
            return _Instance;
        }

        public void Init(int userId)
        {
            if (UserFeeDal.GetInstance().Exist(userId))
                throw new Exception("用户金额账户已经初始化，无需重复执行");

            UserFee userFee = new UserFee() {
                UserId = userId,
                CreateTime = DateTime.Now,
                Total = 0
            };
            UserFeeDal.GetInstance().Create(userFee);
        }

        /// <summary>
        /// 店铺付款，推广者获得7%到27%的收益,如果周期内有效单量>=15收益增加3%，>=10增加2%，》=5增加1%，推广者的推广者获得推广该推广者收益的10%
        /// </summary>
        /// <param name="order"></param>
        /// <param name="user">推广者</param>
        public void ShopPay(ShopMonthOrder order,int user)
        {
            if (user <= 0)
                return;
            if (order.Status != (int)BillStatus.Payed)
                return;
            if (order.ShouldPay <= 0)
            {
                order.UserFeeStatus = (int)UserFeeStatus.Payed;
                order.UserFeePayDate = DateTime.Now;
                ShopMonthOrderDal.GetInstance().Update(order);
                return;
            }

            decimal rate = 0.0M;
            if (order.EffectQty < 300)
            {
                rate = 0.07M;
            }else if (order.EffectQty < 600)
            {
                rate = 0.17M;
            }else if(order.EffectQty<1200)
            {
                rate = 0.22M;
            }else
            {
                rate = 0.27M;
            }
            var dt = order.GenerateDate;
            var endDt = dt.AddDays(-2);
            var beginDt = endDt.AddMonths(-1);
            var totalOrderQty = OrderDal.GetInstance().GetUserOrderCount(beginDt, endDt, user);
            if (totalOrderQty >= 15)
            {
                rate += 0.03M;
            }else if (totalOrderQty >= 10)
            {
                rate += 0.02M;
            }else if (totalOrderQty >= 5)
            {
                rate += 0.01M;
            }

            var total = order.ShouldPay * rate;
            var now = DateTime.Now;
            UserFeeDal.GetInstance().Plus(user, total);
            UserFeeRecordDal.GetInstance().Create(new UserFeeRecord()
            {
                Fee = total,
                UserId = user,
                CreateTime = now,
                Type = (int)UserFeeType.ShopPay,

            });

            if (total < 0.1M)
                return;
            var recommenderParent = UserInfoDal.GetInstance().Get(user).Recommender;
            if (recommenderParent > 0)
            {
                var fee = total * 0.1M;
                UserFeeDal.GetInstance().Plus(recommenderParent, fee);
                UserFeeRecordDal.GetInstance().Create(new UserFeeRecord()
                {
                    Fee = fee,
                    UserId = recommenderParent,
                    CreateTime = now,
                    Type = (int)UserFeeType.RecommenderIncome,
                });

            }

            order.UserFeeStatus = (int)UserFeeStatus.Payed;
            order.UserFeePayDate = now;
            ShopMonthOrderDal.GetInstance().Update(order);

        }

        public decimal GetIncome()
        {
            var current = UserUtil.CurrentUser();
            var userfee= UserFeeDal.GetInstance().Get(current.Id);
            if (userfee == null)
                throw new Exception("您没有账户");
            return userfee.Total;
        }
    }
}
