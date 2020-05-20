//-----------------------------------------------------------------------
// <copyright file=" user_shop.cs" company="xxxx Enterprises">
// * Copyright (C) 2020 xxxx Enterprises All Rights Reserved
// * version : 4.0.30319.42000
// * author  : licun
// * FileName: user_shop.cs
// * history : Created by T4 05/20/2020 13:48:45 
// </copyright>
//-----------------------------------------------------------------------
using System;

namespace Book.Dal.Model
{
    /// <summary>
    /// user_shop Entity Model
    /// </summary>   
	[Dapper.Table("user_shop")]
    public class UserShop
    {
        /// <summary>
        /// 
        /// </summary>
        public int Id { get; set; }
    

        /// <summary>
        /// 
        /// </summary>
		[Column(Name = "user_id")]
        public int UserId { get; set; }
    

        /// <summary>
        /// 
        /// </summary>
		[Column(Name = "shop_id")]
        public int ShopId { get; set; }
    

        /// <summary>
        /// 
        /// </summary>
		[Column(Name = "create_date")]
        public DateTime CreateDate { get; set; }
    

        /// <summary>
        /// 
        /// </summary>
		[Column(Name = "lasted_date")]
        public DateTime LastedDate { get; set; }
    

        /// <summary>
        /// 预约的次数
        /// </summary>
		[Column(Name = "total")]
        public int Total { get; set; }
    }
}
