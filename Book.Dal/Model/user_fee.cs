//-----------------------------------------------------------------------
// <copyright file=" user_fee.cs" company="xxxx Enterprises">
// * Copyright (C) 2020 xxxx Enterprises All Rights Reserved
// * version : 4.0.30319.42000
// * author  : licun
// * FileName: user_fee.cs
// * history : Created by T4 09/26/2020 14:51:31 
// </copyright>
//-----------------------------------------------------------------------
using System;

namespace Book.Dal.Model
{
    /// <summary>
    /// user_fee Entity Model
    /// </summary>   
	[Dapper.Table("user_fee")]
    public class UserFee
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
		[Column(Name = "total")]
        public decimal Total { get; set; }
    

        /// <summary>
        /// 
        /// </summary>
		[Column(Name = "create_time")]
        public DateTime CreateTime { get; set; }
    }
}
