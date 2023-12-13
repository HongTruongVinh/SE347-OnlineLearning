using Model.Dao;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlineCourse.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/

        public ActionResult Index()
        {
            var dao = new ProductCategoryDao();
            ViewBag.CategoryID = dao.ListAll();
            var productdao = new ProductDao();
            ViewBag.HomeProducts = productdao.ListAllProduct();
            var examDao = new ExamDao();
            ViewBag.HomeExams = examDao.ListAllExam();
            HomeInfor homeInfor = new GetInforDao().GetHomeInfor();
            ViewBag.HomeInfor = homeInfor;
            return View();
        }

    }
}
