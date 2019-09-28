using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Book.Model.Enums
{
    public enum ShopStatus
    {
        /// <summary>
        /// 正常
        /// </summary>
        Normal=0,

        /// <summary>
        /// 不营业
        /// </summary>
        Closed=10,

        /// <summary>
        /// 逾期欠费
        /// </summary>
        Arrears = 20

    }
}
