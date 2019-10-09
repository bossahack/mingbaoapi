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

            var s = typeof(LogTraceListener).AssemblyQualifiedName;

            try
            {
                Trace.WriteLine(actionExecutedContext.Exception.ToString());
            }
            catch(Exception ex)
            {

            }
        }
    }
}