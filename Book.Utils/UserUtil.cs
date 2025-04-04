﻿using Book.Model;
using System;
using System.Linq;

namespace Book.Utils
{
    public class UserUtil
    {
        public static UserInfoModel CurrentUser()
        {
            var autho = System.Web.HttpContext.Current.Request.Headers.GetValues("Authorization");
            if (autho == null || autho.Count() == 0)
            {
                throw new Exception("登录失效，请登录");
            }
            //var userinfo=Newtonsoft.Json.JsonConvert.DeserializeObject<UserInfoModel>(autho[0]);
            //if (userinfo == null)
            //    throw new Exception("登录失效");

            string str = SecurityUtil.GetInstance().DecryptString(autho[0]);
            var arr = str.Split('-');
            if (arr == null || arr.Length != 2)
            {
                throw new Exception("登录失效，请登录");
            }
           
            return new UserInfoModel()
            {
                Id = int.Parse(arr[0]),
                ShopId = int.Parse(arr[1])
            };
        }
    }


    public class AdminUtil
    {
        public static UserInfoModel CurrentUser()
        {
            var autho = System.Web.HttpContext.Current.Request.Headers.GetValues("token");
            if (autho == null || autho.Count() == 0)
            {
                throw new Exception("登录失效，请登录");
            }
            //var userinfo=Newtonsoft.Json.JsonConvert.DeserializeObject<UserInfoModel>(autho[0]);
            //if (userinfo == null)
            //    throw new Exception("登录失效");

            string str = SecurityUtil.GetInstance().DecryptString(autho[0]);
            int id = 0;
            if (!Int32.TryParse(str, out id))
            {
                throw new Exception("登录失效，请登录");
            }          

            return new UserInfoModel()
            {
                Id = id
            };
        }
    }
}
