using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Book.Model.Enums
{
    public enum UserFeeType
    {
        /// <summary>
        /// 店铺付款
        /// </summary>
        ShopPay=1,
        
        /// <summary>
        /// 店铺注册
        /// </summary>
        ShopRegister=2,

        /// <summary>
        /// 提现
        /// </summary>
        TakeMoney=3,
    }
}
