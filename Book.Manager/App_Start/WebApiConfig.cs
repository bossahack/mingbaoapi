using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace Book.Manager
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API 配置和服务

            // Web API 路由
            config.MapHttpAttributeRoutes();

            //config.Routes.MapHttpRoute(
            //    name: "DefaultApi",
            //    routeTemplate: "api/{controller}/{action}",
            //    defaults: new { id = RouteParameter.Optional }
            //);

            config.Routes.MapHttpRoute(
               name: "DefaultApi",
               routeTemplate: "api/{controller}/{action}",
               defaults: new { id = RouteParameter.Optional }
           );

            //调用前面的静态方法，将映射关系注册
            Book.Dal.Model.ColumnMapper.SetMapper();
        }
    }
}
