//-----------------------------------------------------------------------
// <copyright file=" b_order.cs" company="xxxx Enterprises">
// * Copyright (C) 2019 xxxx Enterprises All Rights Reserved
// * version : 4.0.30319.42000
// * author  : licun
// * FileName: b_order.cs
// * history : Created by T4 08/05/2019 22:07:15 
// </copyright>
//-----------------------------------------------------------------------
using System;

namespace Book.Dal.Model
{
    /// <summary>
    /// b_order Entity Model
    /// </summary>   
	[Dapper.Table("b_order")]
    public class BOrder
    {
        /// <summary>
        /// 
        /// </summary>
        public int Id { get; set; }
    

        /// <summary>
        /// 
        /// </summary>
		[Column(Name = "shop_id")]
        public int ShopId { get; set; }
    

        /// <summary>
        /// 
        /// </summary>
		[Column(Name = "user_id")]
        public int UserId { get; set; }
    

        /// <summary>
        /// 
        /// </summary>
		[Column(Name = "create_date")]
        public DateTime CreateDate { get; set; }
    

        /// <summary>
        /// 
        /// </summary>
		[Column(Name = "arrive_time_type")]
        public int ArriveTimeType { get; set; }
    

        /// <summary>
        /// 
        /// </summary>
		[Column(Name = "note")]
        public string Note { get; set; }
    

        /// <summary>
        /// 取餐码
        /// </summary>
		[Column(Name = "take_code")]
        public string TakeCode { get; set; }
    

        /// <summary>
        /// 0:初始 10:已接单 20:已取单 30:异常未处理 40:异常已处理
        /// </summary>
		[Column(Name = "status")]
        public int Status { get; set; }
    }
}
