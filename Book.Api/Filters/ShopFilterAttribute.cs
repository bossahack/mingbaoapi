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
    public class ShopFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            base.OnActionExecuting(actionContext);

            var versionStr = System.Web.HttpContext.Current.Request.Headers.GetValues("version");
            if (versionStr != null || versionStr.Count() > 0)
            {
                var version = versionStr[0];
                if (!string.IsNullOrEmpty(version))
                {
                    string a_v = "app_version";
                    var cacheVersion = CacheHelper.GetCache(a_v);
                    if (cacheVersion != null)
                    {
                        if (version != cacheVersion.ToString())
                        {
                            actionContext.Response = new HttpResponseMessage(System.Net.HttpStatusCode.PreconditionFailed);
                            return;
                        }
                    }else
                    {
                        var dict = Service.DictService.GetInstance().Get("appVersion");
                        CacheHelper.SetCache(a_v, dict.Value);
                        if (version != dict.Value)
                        {
                            actionContext.Response = new HttpResponseMessage(System.Net.HttpStatusCode.PreconditionFailed);
                            return;
                        }
                    }
                }
            }

            var autho = System.Web.HttpContext.Current.Request.Headers.GetValues("Authorization");
            if (autho == null || autho.Count() == 0)
            {
                actionContext.Response = new HttpResponseMessage(System.Net.HttpStatusCode.Forbidden);
                return;
            }
            try
            {
                string str = SecurityUtil.GetInstance().DecryptString(autho[0]);
                var arr = str.Split('-');
                if (arr == null || arr.Length != 2)
                {
                    throw new Exception("登录失效，请登录");
                }
            }catch
            {
                actionContext.Response = new HttpResponseMessage(System.Net.HttpStatusCode.Forbidden);
                return;
            }
        }
    }
}