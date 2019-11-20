//-----------------------------------------------------------------------
// <copyright file=" shop_month_order.cs" company="xxxx Enterprises">
// * Copyright (C) 2019 xxxx Enterprises All Rights Reserved
// * version : 4.0.30319.42000
// * author  : licun
// * FileName: shop_month_order.cs
// * history : Created by T4 11/20/2019 09:43:12 
// </copyright>
//-----------------------------------------------------------------------
using System;

namespace Book.Dal.Model
{
    /// <summary>
    /// shop_month_order Entity Model
    /// </summary>   
	[Dapper.Table("shop_month_order")]
    public class ShopMonthOrder
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
		[Column(Name = "year")]
        public int Year { get; set; }
    

        /// <summary>
        /// 
        /// </summary>
		[Column(Name = "month")]
        public int Month { get; set; }
    

        /// <summary>
        /// 
        /// </summary>
		[Column(Name = "qty")]
        public int Qty { get; set; }
    

        /// <summary>
        /// 
        /// </summary>
		[Column(Name = "effect_qty")]
        public int EffectQty { get; set; }
    

        /// <summary>
        /// 
        /// </summary>
		[Column(Name = "should_pay")]
        public decimal ShouldPay { get; set; }
    

        /// <summary>
        /// 0:待统计，10:待结算，20:已付款
        /// </summary>
		[Column(Name = "status")]
        public int Status { get; set; }
    }
}
