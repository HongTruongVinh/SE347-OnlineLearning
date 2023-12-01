using Model.Dao;
using Model.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlineCourse.Controllers
{
    public class ManagementCourseController : Controller
    {
        // GET: ManagementCourse
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ManagementCourseDetail(long productId)
        {
            return View();
        }

        public ActionResult ViewEditCourse(long productId)
        {
            return View();
        }

        public ActionResult ViewAddVieoToCourse(long productId)
        {
            return View();
        }

        [System.Web.Http.HttpPost]
        public ActionResult AddVieoToCourse(CourseVideo video, HttpPostedFileBase file)
        {
            return View();
        }

        [System.Web.Http.HttpPost]
        public ActionResult AddDocumentToCourse(CourseDocument document, HttpPostedFileBase file)
        {
            return View();
        }

        public ActionResult DeleteDocumentOfCourse(int documentId, int productId)
        {
            return View();
        }

        public ActionResult DeleteVideoOfCourse(int videoId, int productId)
        {
            return View();
        }

        [System.Web.Http.HttpPost]
        public ActionResult UpdateCourse(Product product, HttpPostedFileBase imageFile)
        {
            return View();

        }

        public ActionResult DeleteCourse(int productId)
        {

            return View();
        }


        void GetListProductOfUser(int userId)
        {
        }


        public string UploadImage(HttpPostedFileBase file)
        {
            return "";
        }

        public string UploadDocument(HttpPostedFileBase file)
        {
            return "";
        }

        public string UploadVideo(HttpPostedFileBase file)
        {
            return "";
        }

    }

}