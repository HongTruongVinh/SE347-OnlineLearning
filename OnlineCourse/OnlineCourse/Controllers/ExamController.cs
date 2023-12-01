using Model.Dao;
using Model.Models;
using OnlineCourse.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlineCourse.Controllers
{
    public class ExamController : BaseController
    {

        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Category(string searchString, string Type)
        {
            return View();
        }
        public ActionResult Detail(long id)
        {
            return View();
        }

    }
    [HttpPost]
    public JsonResult AddResult(long examid, long userid)
    {

    }

    [HttpPost]
    public JsonResult UpdateResult(long examid, long userid, string resultessay, string resultquiz)
    {

    }
}
