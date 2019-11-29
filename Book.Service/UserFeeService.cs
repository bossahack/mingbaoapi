using Book.Dal;
using Book.Dal.Model;
using Book.Model.Enums;
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

            UserFeeDal.GetInstance().Plus(user, total);
            UserFeeRecordDal.GetInstance().Create(new UserFeeRecord()
            {
                Fee = total,
                UserId = user,
                CreateTime = DateTime.Now,
                Type = (int)UserFeeType.ShopPay,

            });

        }
    }
}
