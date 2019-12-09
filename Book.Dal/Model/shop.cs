//-----------------------------------------------------------------------
// <copyright file=" shop.cs" company="xxxx Enterprises">
// * Copyright (C) 2019 xxxx Enterprises All Rights Reserved
// * version : 4.0.30319.42000
// * author  : licun
// * FileName: shop.cs
// * history : Created by T4 12/09/2019 18:20:37 
// </copyright>
//-----------------------------------------------------------------------
using System;

namespace Book.Dal.Model
{
    /// <summary>
    /// shop Entity Model
    /// </summary>   
	[Dapper.Table("shop")]
    public class Shop
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
		[Column(Name = "name")]
        public string Name { get; set; }
    

        /// <summary>
        /// 
        /// </summary>
		[Column(Name = "address")]
        public string Address { get; set; }
    

        /// <summary>
        /// 
        /// </summary>
		[Column(Name = "logo")]
        public string Logo { get; set; }
    

        /// <summary>
        /// 
        /// </summary>
		[Column(Name = "recommender")]
        public int Recommender { get; set; }
    

        /// <summary>
        /// 0:正常 10:不营业 20:欠费
        /// </summary>
		[Column(Name = "status")]
        public int Status { get; set; }
    

        /// <summary>
        /// 
        /// </summary>
		[Column(Name = "create_date")]
        public DateTime CreateDate { get; set; }
    }
}
