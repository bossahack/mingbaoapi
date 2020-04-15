using Book.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace Book.Manager.Filters
{
    public class AdminFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            base.OnActionExecuting(actionContext);


            var autho = System.Web.HttpContext.Current.Request.Headers.GetValues("token");
            if (autho == null || autho.Count() == 0)
            {
                actionContext.Response = new HttpResponseMessage(System.Net.HttpStatusCode.Forbidden);
                return;
            }
            try
            {
                string str = SecurityUtil.GetInstance().DecryptString(autho[0]);
                int id = 0;
                if(!Int32.TryParse(str,out id))
                {
                    actionContext.Response = new HttpResponseMessage(System.Net.HttpStatusCode.Forbidden);
                    return;
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