using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace OnlineCourse
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
            name: "Home",
            url: "trang-chu",
            defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
            namespaces: new[] { "OnlineCourse.Controllers" });

            routes.MapRoute(
            name: "Product Category",
            url: "khoa-hoc/{metatitle}-{cateId}",
            defaults: new { controller = "Product", action = "Category", id = UrlParameter.Optional },
            namespaces: new[] { "OnlineCourse.Controllers" });

            routes.MapRoute(
            name: "Exam Category",
            url: "kiem-tra/{metatitle}-{Type}",
            defaults: new { controller = "Exam", action = "Category", id = UrlParameter.Optional },
            namespaces: new[] { "OnlineCourse.Controllers" });

            routes.MapRoute(
            name: "Exam Detail",
            url: "chi-tiet-kiem-tra/{metatitle}-{id}",
            defaults: new { controller = "Exam", action = "Detail", id = UrlParameter.Optional },
            namespaces: new[] { "OnlineCourse.Controllers" });

            routes.MapRoute(
            name: "Product Detail",
            url: "chi-tiet/{metatitle}-{id}-{detailid}",
            defaults: new { controller = "Product", action = "Detail", id = UrlParameter.Optional },
            namespaces: new[] { "OnlineCourse.Controllers" });

            routes.MapRoute(
            name: "Login",
            url: "dang-nhap",
            defaults: new { controller = "User", action = "Login", id = UrlParameter.Optional },
            namespaces: new[] { "OnlineCourse.Controllers" });

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional},
                namespaces: new[] { "OnlineCourse.Controllers"});
        }
    }
}