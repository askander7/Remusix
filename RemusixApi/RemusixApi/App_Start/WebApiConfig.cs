using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Web.Http;

namespace RemusixApi
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{action}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
    //        GlobalConfiguration.Configuration.Formatters.JsonFormatter.SerializerSettings
    //.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
    //        GlobalConfiguration.Configuration.Formatters
    //            .Remove(GlobalConfiguration.Configuration.Formatters.XmlFormatter);

            config.Formatters.JsonFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue("text/html"));
        }
    }
}
