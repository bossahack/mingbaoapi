using System;
using System.Collections.Generic;

namespace Book.Model
{
    public class OrderResponse
    {
        public List<OrderVM> Orders { get; set; }
        public List<OrderItemVM> OrderItems { get; set; }

        public List<OrderShopModel> Shops { get; set; }
    }

    public class OrderVM
    {

        /// <summary>
        /// 
        /// </summary>
        public int Id { get; set; }


        /// <summary>
        /// 
        /// </summary>
        public int ShopId { get; set; }


        /// <summary>
        /// 
        /// </summary>
        public int UserId { get; set; }


        /// <summary>
        /// 
        /// </summary>
        public DateTime CreateDate { get; set; }


        /// <summary>
        /// 
        /// </summary>
        public int ArriveTimeType { get; set; }


        /// <summary>
        /// 
        /// </summary>
        public string Note { get; set; }


        /// <summary>
        /// 取餐码
        /// </summary>
        public string TakeCode { get; set; }


        /// <summary>
        /// 0:初始 10:已接单 20:已取单 30:异常未处理 40:异常已处理
        /// </summary>
        public int Status { get; set; }

    }

    public class OrderItemVM
    {

        /// <summary>
        /// 
        /// </summary>
        public int FoodId { get; set; }


        /// <summary>
        /// 
        /// </summary>
        public string FoodName { get; set; }


        /// <summary>
        /// 
        /// </summary>
        public decimal FoodPrice { get; set; }


        /// <summary>
        /// 
        /// </summary>
        public int Qty { get; set; }
    }

    public class BookOrderRequest
    {
        public int ShopId { get; set; }
        public int ArriveTimeType { get; set; }
        public string Note { get; set; }

        public List<BookOrderItem> Items {get;set;}

    }

    public class BookOrderItem
    {
        public int FoodId { get; set; }

        public int Qty { get; set; }
    }
}
