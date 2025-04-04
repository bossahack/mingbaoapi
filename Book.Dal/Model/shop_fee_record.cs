//-----------------------------------------------------------------------
// <copyright file=" shop_fee_record.cs" company="xxxx Enterprises">
// * Copyright (C) 2020 xxxx Enterprises All Rights Reserved
// * version : 4.0.30319.42000
// * author  : licun
// * FileName: shop_fee_record.cs
// * history : Created by T4 09/26/2020 14:51:31 
// </copyright>
//-----------------------------------------------------------------------
using System;

namespace Book.Dal.Model
{
    /// <summary>
    /// shop_fee_record Entity Model
    /// </summary>   
	[Dapper.Table("shop_fee_record")]
    public class ShopFeeRecord
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
		[Column(Name = "fee")]
        public decimal Fee { get; set; }
    

        /// <summary>
        /// 
        /// </summary>
		[Column(Name = "create_time")]
        public DateTime CreateTime { get; set; }
    

        /// <summary>
        /// 
        /// </summary>
		[Column(Name = "admin_id")]
        public int AdminId { get; set; }
    

        /// <summary>
        /// 
        /// </summary>
		[Column(Name = "note")]
        public string Note { get; set; }
    }
}
