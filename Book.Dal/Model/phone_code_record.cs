//-----------------------------------------------------------------------
// <copyright file=" phone_code_record.cs" company="xxxx Enterprises">
// * Copyright (C) 2020 xxxx Enterprises All Rights Reserved
// * version : 4.0.30319.42000
// * author  : licun
// * FileName: phone_code_record.cs
// * history : Created by T4 09/08/2020 16:59:13 
// </copyright>
//-----------------------------------------------------------------------
using System;

namespace Book.Dal.Model
{
    /// <summary>
    /// phone_code_record Entity Model
    /// </summary>   
	[Dapper.Table("phone_code_record")]
    public class PhoneCodeRecord
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
		[Column(Name = "phone")]
        public string Phone { get; set; }
    

        /// <summary>
        /// 
        /// </summary>
		[Column(Name = "code")]
        public string Code { get; set; }
    

        /// <summary>
        /// 
        /// </summary>
		[Column(Name = "create_time")]
        public DateTime CreateTime { get; set; }
    }
}
