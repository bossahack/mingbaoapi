using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Book.Model.Enums
{
    public enum FoodStatus
    {
       /// <summary>
       /// 正常
       /// </summary>
        Normal=0,

        /// <summary>
        /// 已下架
        /// </summary>
        Offshelved=1,

        /// <summary>
        /// 已删除
        /// </summary>
        Removed=2
    }
}
