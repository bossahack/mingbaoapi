using Book.Model;
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
}
