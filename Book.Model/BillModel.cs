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

    public class BillShopParam:PageSearch
    {

        /// <summary>
        /// 
        /// </summary>
        public int? ShopId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int? BillStatus { get; set; }
    }

    public class BillShopModel
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

        /// <summary>
        /// 
        /// </summary>
        public int ShopId { get; set; }


        /// <summary>
        /// 
        /// </summary>
        public string ShopName { get; set; }


        /// <summary>
        /// 
        /// </summary>
        public string ShopAddress { get; set; }

        /// <summary>
        /// 0:正常 10:不营业 20:欠费
        /// </summary>
        public int ShopStatus { get; set; }


        /// <summary>
        /// 
        /// </summary>
        public DateTime ShopCreateDate { get; set; }
    }
}
