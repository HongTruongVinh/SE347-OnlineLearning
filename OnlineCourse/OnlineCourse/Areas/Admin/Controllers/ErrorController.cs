using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlineCourse.Areas.Admin.Controllers
{
    public class ErrorController : Controller
    {
        // GET: Admin/Error
        public ActionResult NotAllowedPage()
        {
            return View();
        }

        public ActionResult NotAllowed()
        {
            return PartialView("_NoPermission");
        }
    }
}