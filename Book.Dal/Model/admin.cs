//-----------------------------------------------------------------------
// <copyright file=" admin.cs" company="xxxx Enterprises">
// * Copyright (C) 2020 xxxx Enterprises All Rights Reserved
// * version : 4.0.30319.42000
// * author  : licun
// * FileName: admin.cs
// * history : Created by T4 05/20/2020 13:48:45 
// </copyright>
//-----------------------------------------------------------------------
using System;

namespace Book.Dal.Model
{
    /// <summary>
    /// admin Entity Model
    /// </summary>   
	[Dapper.Table("admin")]
    public class Admin
    {
        /// <summary>
        /// 
        /// </summary>
        public int Id { get; set; }
    

        /// <summary>
        /// 
        /// </summary>
		[Column(Name = "name")]
        public string Name { get; set; }
    

        /// <summary>
        /// 
        /// </summary>
		[Column(Name = "pwd")]
        public string Pwd { get; set; }
    

        /// <summary>
        /// 
        /// </summary>
		[Column(Name = "create_time")]
        public DateTime CreateTime { get; set; }
    }
}
