using Book.Model;
using System;
using System.Linq;

namespace Book.Utils
{
    public class UserUtil
    {
        public static UserInfoModel CurrentUser()
        {
            return new UserInfoModel() {
                Id=1,
                ShopId=1
            };
            var autho = System.Web.HttpContext.Current.Request.Headers.GetValues("Authorization");
            if (autho == null || autho.Count() == 0)
            {
                throw new Exception("登录失效，请登录");
            }
            var loginInfo = Newtonsoft.Json.JsonConvert.DeserializeObject<UserInfoModel>(autho[0]);
            if (loginInfo == null)
            {
                throw new Exception("登录失效，请登录");
            }
            return loginInfo;
        }
    }
}
