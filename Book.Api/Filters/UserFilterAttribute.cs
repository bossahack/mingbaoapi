using Book.Model;
using Book.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace Book.Api.Filters
{
    public class UserFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            base.OnActionExecuting(actionContext);


            var autho = System.Web.HttpContext.Current.Request.Headers.GetValues("Authorization");
            if (autho == null || autho.Count() == 0)
            {
                actionContext.Response = new HttpResponseMessage(System.Net.HttpStatusCode.Forbidden);
                return;
            }
            //var loginInfo = Newtonsoft.Json.JsonConvert.DeserializeObject<UserInfoModel>(autho[0]);
            //if (loginInfo == null)
            //{
            //    actionContext.Response = new HttpResponseMessage(System.Net.HttpStatusCode.Forbidden);
            //    return;
            //}
            try
            {
                string str = SecurityUtil.GetInstance().DecryptString(autho[0]);
                var arr = str.Split('-');
                if (arr == null || arr.Length != 2)
                {
                throw new Exception("登录失效，请登录");
                }
            }
            catch
            {
                actionContext.Response = new HttpResponseMessage(System.Net.HttpStatusCode.Unauthorized);
                return;
            }


        }
    }
}