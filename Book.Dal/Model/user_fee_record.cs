//-----------------------------------------------------------------------
// <copyright file=" user_fee_record.cs" company="xxxx Enterprises">
// * Copyright (C) 2020 xxxx Enterprises All Rights Reserved
// * version : 4.0.30319.42000
// * author  : licun
// * FileName: user_fee_record.cs
// * history : Created by T4 05/20/2020 13:48:45 
// </copyright>
//-----------------------------------------------------------------------
using System;

namespace Book.Dal.Model
{
    /// <summary>
    /// user_fee_record Entity Model
    /// </summary>   
	[Dapper.Table("user_fee_record")]
    public class UserFeeRecord
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
		[Column(Name = "fee")]
        public decimal Fee { get; set; }
    

        /// <summary>
        /// 
        /// </summary>
		[Column(Name = "create_time")]
        public DateTime CreateTime { get; set; }
    

        /// <summary>
        /// 1:店铺提成 2:推广提成 3:提现
        /// </summary>
		[Column(Name = "type")]
        public int Type { get; set; }
    }
}
