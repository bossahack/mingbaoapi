//-----------------------------------------------------------------------
// <copyright file=" user_info.cs" company="xxxx Enterprises">
// * Copyright (C) 2019 xxxx Enterprises All Rights Reserved
// * version : 4.0.30319.42000
// * author  : licun
// * FileName: user_info.cs
// * history : Created by T4 12/13/2019 15:44:49 
// </copyright>
//-----------------------------------------------------------------------
using System;

namespace Book.Dal.Model
{
    /// <summary>
    /// user_info Entity Model
    /// </summary>   
	[Dapper.Table("user_info")]
    public class UserInfo
    {
        /// <summary>
        /// 
        /// </summary>
        public int Id { get; set; }
    

        /// <summary>
        /// 
        /// </summary>
		[Column(Name = "wxid")]
        public string Wxid { get; set; }
    

        /// <summary>
        /// 
        /// </summary>
		[Column(Name = "wx_num")]
        public string WxNum { get; set; }
    

        /// <summary>
        /// 
        /// </summary>
		[Column(Name = "wx_name")]
        public string WxName { get; set; }
    

        /// <summary>
        /// 
        /// </summary>
		[Column(Name = "wx_photo")]
        public string WxPhoto { get; set; }
    

        /// <summary>
        /// 
        /// </summary>
		[Column(Name = "wx_phone")]
        public string WxPhone { get; set; }
    

        /// <summary>
        /// 
        /// </summary>
		[Column(Name = "has_shop")]
        public bool HasShop { get; set; }
    

        /// <summary>
        /// 1:推广者
        /// </summary>
		[Column(Name = "type")]
        public int Type { get; set; }
    

        /// <summary>
        /// 
        /// </summary>
		[Column(Name = "create_date")]
        public DateTime CreateDate { get; set; }
    

        /// <summary>
        /// 
        /// </summary>
		[Column(Name = "recommender")]
        public int Recommender { get; set; }
    

        /// <summary>
        /// 
        /// </summary>
		[Column(Name = "login_name")]
        public string LoginName { get; set; }
    

        /// <summary>
        /// 
        /// </summary>
		[Column(Name = "login_pwd")]
        public string LoginPwd { get; set; }
    }
}
