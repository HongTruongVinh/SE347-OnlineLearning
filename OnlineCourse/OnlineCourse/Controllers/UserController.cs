using Model.Dao;
using Model.Models;
using OnlineCourse.Common;
using OnlineCourse.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Razor.Editor;
using System.Web.Routing;
using System.Web.Security;

namespace OnlineCourse.Controllers
{
    public class UserController : Controller
    {
        //
        // GET: /User/

        public ActionResult Login()
        {
            return View();
        }
        [System.Web.Mvc.HttpPost]
        public ActionResult Login(LoginModel model)
        {
            return View("model");
        }

        public ActionResult LogUot()
        {
            return new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Home", action = "Index" }));
        }


        public ActionResult ProfileUser()
        {
            return View();
        }

        UserLogin SetUserSession(User user)
        {
            var usersession = new UserLogin();
            return usersession;
        }
    }
}
