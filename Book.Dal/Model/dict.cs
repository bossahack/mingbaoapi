//-----------------------------------------------------------------------
// <copyright file=" dict.cs" company="xxxx Enterprises">
// * Copyright (C) 2020 xxxx Enterprises All Rights Reserved
// * version : 4.0.30319.42000
// * author  : licun
// * FileName: dict.cs
// * history : Created by T4 09/26/2020 14:51:31 
// </copyright>
//-----------------------------------------------------------------------
using System;

namespace Book.Dal.Model
{
    /// <summary>
    /// dict Entity Model
    /// </summary>   
	[Dapper.Table("dict")]
    public class Dict
    {
        /// <summary>
        /// 
        /// </summary>
        public int Id { get; set; }
    

        /// <summary>
        /// 
        /// </summary>
		[Column(Name = "flag")]
        public string Flag { get; set; }
    

        /// <summary>
        /// 
        /// </summary>
		[Column(Name = "value")]
        public string Value { get; set; }
    }
}
