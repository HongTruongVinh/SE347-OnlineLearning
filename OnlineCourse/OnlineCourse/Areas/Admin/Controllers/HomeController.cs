using Model.Dao;
using OnlineCourse.App_Start;
using OnlineCourse.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;

namespace OnlineCourse.Areas.Admin.Controllers
{
    public class HomeController : BaseController
    {
        //
        // GET: /Admin/Home/


        public ActionResult Index()
        {
            ViewBag.HomeInfor = new GetInforDao().GetHomeDashboardInfor();

            return View();
        }

        public ActionResult LogUot()
        {
            Session.Remove(CommonConstants.USER_SESSION);

            FormsAuthentication.SignOut();

            return new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Login", action = "Index", Area = "Admin" }));
        }
    }
}
