using Model.Dao;
using Model.Models;
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
            var homeProducts = productdao.ListAllProduct();
            ViewBag.HomeProducts = homeProducts;
            var examDao = new ExamDao();
            ViewBag.HomeExams = examDao.ListAllExam();
            HomeInfor homeInfor = new GetInforDao().GetHomeInfor();
            ViewBag.HomeInfor = homeInfor;


           
            List<Model.Models.User> users = new List<Model.Models.User>();

            var userDao = new UserDao();

            foreach (var item in homeProducts)
            {
                users.Add(userDao.GetByUserId(Int32.Parse(item.CreateBy)));
            }

            ViewBag.UserProducts = users;
     
            return View();
        }

        public ActionResult About()
        {
            var dao = new ProductCategoryDao();
            ViewBag.CategoryID = dao.ListAll();

            return View();
        }
    }
}
