//-----------------------------------------------------------------------
// <copyright file=" shop_online.cs" company="xxxx Enterprises">
// * Copyright (C) 2019 xxxx Enterprises All Rights Reserved
// * version : 4.0.30319.42000
// * author  : licun
// * FileName: shop_online.cs
// * history : Created by T4 11/07/2019 10:21:03 
// </copyright>
//-----------------------------------------------------------------------
using System;

namespace Book.Dal.Model
{
    /// <summary>
    /// shop_online Entity Model
    /// </summary>   
	[Dapper.Table("shop_online")]
    public class ShopOnline
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
		[Column(Name = "ip")]
        public string Ip { get; set; }
    

        /// <summary>
        /// 
        /// </summary>
		[Column(Name = "port")]
        public int Port { get; set; }
    

        /// <summary>
        /// 
        /// </summary>
		[Column(Name = "last_keep_time")]
        public DateTime LastKeepTime { get; set; }
    }
}
