using Book.Model;
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
            if(autho==null||autho.Count()==0)
            {
                actionContext.Response = new HttpResponseMessage(System.Net.HttpStatusCode.Forbidden);
                return;
            }
            var loginInfo = Newtonsoft.Json.JsonConvert.DeserializeObject<UserInfoModel>(autho[0]);
            if (loginInfo == null)
            {
                actionContext.Response =new HttpResponseMessage(System.Net.HttpStatusCode.Forbidden);

                return;
            }
            if(loginInfo.Id<=0)
            {
                actionContext.Response = new HttpResponseMessage(System.Net.HttpStatusCode.Forbidden);
                return;
            }
        }
    }
}