using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Book.Model.Enums
{
    public enum OrderStatus
    {
        /// <summary>
        /// 初始
        /// </summary>
        Origin = 0,

        /// <summary>
        /// 已接单
        /// </summary>
        Receipted=10,

        /// <summary>
        /// 已完成
        /// </summary>
        Completed=20,

        /// <summary>
        /// 已取消
        /// </summary>
        Canceled=30,

        /// <summary>
        /// 异常单
        /// </summary>
        Abnormaled=40

    }
    
}
