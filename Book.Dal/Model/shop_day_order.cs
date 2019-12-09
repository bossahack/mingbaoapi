//-----------------------------------------------------------------------
// <copyright file=" shop_day_order.cs" company="xxxx Enterprises">
// * Copyright (C) 2019 xxxx Enterprises All Rights Reserved
// * version : 4.0.30319.42000
// * author  : licun
// * FileName: shop_day_order.cs
// * history : Created by T4 12/09/2019 16:55:39 
// </copyright>
//-----------------------------------------------------------------------
using System;

namespace Book.Dal.Model
{
    /// <summary>
    /// shop_day_order Entity Model
    /// </summary>   
	[Dapper.Table("shop_day_order")]
    public class ShopDayOrder
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
		[Column(Name = "date")]
        public DateTime Date { get; set; }
    

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
    }
}
