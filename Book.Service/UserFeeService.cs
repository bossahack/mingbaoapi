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
        /// 店铺付款，推广者获得10%到30%的收益，推广者的推广者获得推广该推广者收益的10%
        /// </summary>
        /// <param name="order"></param>
        /// <param name="user">推广者</param>
        public void ShopPay(ShopMonthOrder order,int user)
        {
            if (order.ShouldPay <= 0)
                return;
            if (user <= 0)
                return;
            if (order.Status != (int)BillStatus.Payed)
                return;

            decimal rate = 0.0M;
            if (order.EffectQty < 300)
            {
                rate = 0.1M;
            }else if (order.EffectQty < 600)
            {
                rate = 0.2M;
            }else
            {
                rate = 0.3M;
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

            order.UserFeeStatus = 1;
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
