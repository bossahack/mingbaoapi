﻿using System;

namespace Book.Model
{
    public class ShopResponse
    {

        /// <summary>
        /// 
        /// </summary>
        public int Id { get; set; }
        

        /// <summary>
        /// 
        /// </summary>
        public string Name { get; set; }


        /// <summary>
        /// 
        /// </summary>
        public string Address { get; set; }


        /// <summary>
        /// 
        /// </summary>
        public string Logo { get; set; }


        /// <summary>
        /// 
        /// </summary>
        public string Recommender { get; set; }


        /// <summary>
        /// 0:正常 10:不营业 20:欠费
        /// </summary>
        public int Status { get; set; }


        /// <summary>
        /// 
        /// </summary>
        public DateTime CreateDate { get; set; }
    }

    public class OrderShopModel
    {
        /// <summary>
        /// 
        /// </summary>
        public int Id { get; set; }


        /// <summary>
        /// 
        /// </summary>
        public string Name { get; set; }


        /// <summary>
        /// 
        /// </summary>
        public string Address { get; set; }
    }

    public class ShopModel
    {
        /// <summary>
        /// 
        /// </summary>
        public int Id { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        public string Name { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        public string Address { get; set; }


        /// <summary>
        /// 
        /// </summary>
        public string Logo { get; set; }

        
        /// <summary>
        /// 0:正常 10:不营业 20:欠费
        /// </summary>
        public int Status { get; set; }
        
    }

    public class ShopCreateModel
    {
        public string Phone { get; set; }

        public string Pwd { get; set; }
    }
}
