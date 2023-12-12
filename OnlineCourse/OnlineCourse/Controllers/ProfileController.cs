using Model.Dao;
using Model.Models;
using OnlineCourse.Common;
using OnlineCourse.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace OnlineCourse.Controllers
{
    public class ProfileController : Controller
    {
        // GET: Profile
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult AcademicAchievement()
        {
            return View();
        }

        public ActionResult Exam()
        {
            return View();
        }

        public ActionResult CourseBought()
        {
            return View();
        }

        public ActionResult Cart()
        {
            return View();
        }

        public ActionResult MyCourse()
        {
            return RedirectToAction("Index", "ManagementCourse");
        }

        [System.Web.Http.HttpPost]
        public ActionResult UpdateProfile(UserLogin _user, HttpPostedFileBase imageFile)
        {
            return Json();
        }

        public string UploadImage(HttpPostedFileBase file)
        {
            string path = "-1";
            return path;
        }

        UserLogin SetUserSession(User user)
        {
            var usersession = new UserLogin();
            return usersession;
        }

        List<ProductModel> ConvertToProductModels(List<Product> products, bool isBought = false)
        {
            List<ProductModel> productModels = new List<ProductModel>();
            return productModels;
        }

        [System.Web.Http.HttpGet]
        public ActionResult BuyProduct(int userId, int productId)
        {
            return Json();
        }

        [System.Web.Mvc.HttpGet]
        public ActionResult AddProductToCart(int userId, int productId)
        {
            return Json();
        }

        [System.Web.Http.HttpPost]
        public ActionResult DeleteProduct(int userId, int productId)
        {
            return Json();
        }
    }
}