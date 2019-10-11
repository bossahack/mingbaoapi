using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Http.Filters;

namespace Book.Api.Filters
{
    public class GlobalErrorFilterAttribute: ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext actionExecutedContext)
        {
            base.OnException(actionExecutedContext);
            
            try
            {
                Trace.WriteLine(actionExecutedContext.Exception.ToString());//FileLogTraceListener
            }
            catch(Exception ex)
            {

            }
        }
    }
}