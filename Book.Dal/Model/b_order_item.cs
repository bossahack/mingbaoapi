//-----------------------------------------------------------------------
// <copyright file=" b_order_item.cs" company="xxxx Enterprises">
// * Copyright (C) 2019 xxxx Enterprises All Rights Reserved
// * version : 4.0.30319.42000
// * author  : licun
// * FileName: b_order_item.cs
// * history : Created by T4 12/09/2019 16:55:39 
// </copyright>
//-----------------------------------------------------------------------
using System;

namespace Book.Dal.Model
{
    /// <summary>
    /// b_order_item Entity Model
    /// </summary>   
	[Dapper.Table("b_order_item")]
    public class BOrderItem
    {
        /// <summary>
        /// 
        /// </summary>
        public int Id { get; set; }
    

        /// <summary>
        /// 
        /// </summary>
		[Column(Name = "order_id")]
        public int OrderId { get; set; }
    

        /// <summary>
        /// 
        /// </summary>
		[Column(Name = "food_id")]
        public int FoodId { get; set; }
    

        /// <summary>
        /// 
        /// </summary>
		[Column(Name = "food_name")]
        public string FoodName { get; set; }
    

        /// <summary>
        /// 
        /// </summary>
		[Column(Name = "food_price")]
        public decimal FoodPrice { get; set; }
    

        /// <summary>
        /// 
        /// </summary>
		[Column(Name = "qty")]
        public int Qty { get; set; }
    }
}
