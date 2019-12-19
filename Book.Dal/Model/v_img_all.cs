//-----------------------------------------------------------------------
// <copyright file=" v_img_all.cs" company="xxxx Enterprises">
// * Copyright (C) 2019 xxxx Enterprises All Rights Reserved
// * version : 4.0.30319.42000
// * author  : licun
// * FileName: v_img_all.cs
// * history : Created by T4 12/19/2019 18:04:24 
// </copyright>
//-----------------------------------------------------------------------
using System;

namespace Book.Dal.Model
{
    /// <summary>
    /// v_img_all Entity Model
    /// </summary>   
	[Dapper.Table("v_img_all")]
    public class VImgAll
    {
        /// <summary>
        /// 
        /// </summary>
        public string Img { get; set; }
    }
}
