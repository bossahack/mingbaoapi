using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Book.Model
{
    public class BillModel
    {

        /// <summary>
        /// 
        /// </summary>
        public int Id { get; set; }
        

        /// <summary>
        /// 
        /// </summary>
        public int Year { get; set; }


        /// <summary>
        /// 
        /// </summary>
        public int Month { get; set; }


        /// <summary>
        /// 
        /// </summary>
        public int Qty { get; set; }


        /// <summary>
        /// 
        /// </summary>
        public int EffectQty { get; set; }


        /// <summary>
        /// 
        /// </summary>
        public decimal ShouldPay { get; set; }


        /// <summary>
        /// 0:待统计，10:待结算，20:已付款
        /// </summary>
        public int Status { get; set; }
    }
}
