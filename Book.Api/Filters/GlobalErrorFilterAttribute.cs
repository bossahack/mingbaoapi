using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Http.Filters;

namespace Book.Api.Filters
{
    public class GlobalErrorFilterAttribute: ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext actionExecutedContext)
        {
            try
            {
                Trace.WriteLine(actionExecutedContext.Exception.ToString());//FileLogTraceListener
            }
            catch { }
            actionExecutedContext.Response = new System.Net.Http.HttpResponseMessage() {
                StatusCode=System.Net.HttpStatusCode.InternalServerError,
                Content = new StringContent(actionExecutedContext.Exception.Message)
            };

            base.OnException(actionExecutedContext);
        }
    }
}