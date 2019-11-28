using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Book.Model.Enums
{
    public enum BillStatus
    {
        /// <summary>
        /// 初始
        /// </summary>
        Init=0,

        /// <summary>
        /// 待付款
        /// </summary>
        UnPay=10,

        /// <summary>
        /// 已付款
        /// </summary>
        Payed=20
    }
}
