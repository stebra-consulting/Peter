using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using testPeterWeb.App_Start;

namespace testPeterWeb
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Newsitem",
                url: "Home/Read/{title}",
                defaults: new { controller = "Home", action = "Item", title = "" }
                );

            routes.MapRoute(
                name: "News",
                url: "Home/News",
                defaults: new { controller = "Home", action = "About" }
                );

            routes.MapRoute(
                    name: "Default",
                    url: "{controller}/{action}/{id}",
                    defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
                );
        }
    }
}
