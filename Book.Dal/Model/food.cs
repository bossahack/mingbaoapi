//-----------------------------------------------------------------------
// <copyright file=" food.cs" company="xxxx Enterprises">
// * Copyright (C) 2019 xxxx Enterprises All Rights Reserved
// * version : 4.0.30319.42000
// * author  : licun
// * FileName: food.cs
// * history : Created by T4 11/21/2019 14:08:23 
// </copyright>
//-----------------------------------------------------------------------
using System;

namespace Book.Dal.Model
{
    /// <summary>
    /// food Entity Model
    /// </summary>   
	[Dapper.Table("food")]
    public class Food
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
		[Column(Name = "type")]
        public int Type { get; set; }
    

        /// <summary>
        /// 
        /// </summary>
		[Column(Name = "name")]
        public string Name { get; set; }
    

        /// <summary>
        /// 
        /// </summary>
		[Column(Name = "price")]
        public decimal Price { get; set; }
    

        /// <summary>
        /// 
        /// </summary>
		[Column(Name = "intro")]
        public string Intro { get; set; }
    

        /// <summary>
        /// 
        /// </summary>
		[Column(Name = "img")]
        public string Img { get; set; }
    

        /// <summary>
        /// 0:正常 1:下架
        /// </summary>
		[Column(Name = "status")]
        public int Status { get; set; }
    

        /// <summary>
        /// 
        /// </summary>
		[Column(Name = "create_date")]
        public DateTime CreateDate { get; set; }
    

        /// <summary>
        /// 
        /// </summary>
		[Column(Name = "level")]
        public int Level { get; set; }
    }
}
