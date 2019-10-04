using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using Book.Api.Filters;

namespace Book.Api
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API 配置和服务
            GlobalConfiguration.Configuration.Formatters.JsonFormatter.MediaTypeMappings.Add(
               new System.Net.Http.Formatting.QueryStringMapping("datatype", "json", "application/json"));
            // Web API 路由
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "{controller}/{action}",
                defaults: new { id = RouteParameter.Optional }
            );


            //调用前面的静态方法，将映射关系注册
            Book.Dal.Model.ColumnMapper.SetMapper();

            config.Filters.Add(new GlobalErrorFilterAttribute());
        }
    }
}
