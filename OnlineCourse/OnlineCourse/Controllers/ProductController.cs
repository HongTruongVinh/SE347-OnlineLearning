using Model.Dao;
using Model.Models;
using OnlineCourse.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlineCourse.Controllers
{
    public class ProductController : BaseController
    {
        public const int ITEMS_PER_PAGE = 120;
        public int currentPage { get; set; }
        public int countPages { get; set; }

        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Category(string searchString, long cateId)
        {
            return View();
        }

        public ActionResult Detaill(long id, long detailid)
        {
            return View();
        }

        public ActionResult Detail(int productId, int playingIdVideo)
        {
            return View();
        }

        [ChildActionOnly]
        public ActionResult _ChildComment(long parentid, long productid)
        {
            return PartialView("~/Views/Shared/_ChildComment.cshtml");
        }
        [HttpPost]
        public JsonResult AddNewComment(long productid, long userid, long parentid, string commentmsg, string rate)
        {
            return Json(new
            {
                status = true
            });
        }
        public ActionResult GetComment(long productid)
        {
            return PartialView("~/Views/Shared/_ChildComment.cshtml");
        }

        public FileResult LearnerDownloadDocument(string link)
        {
            //var memory = new MemoryStream();
            //using (var stream = new FileStream(link, FileMode.Open))
            //{
            //    stream.CopyTo(memory);
            //}
            //memory.Position = 0;

            string ext = Path.GetExtension(link).ToLowerInvariant();
            return File(link, GetMimeTypes()[ext]);
        }

        private Dictionary<string, string> GetMimeTypes()
        {
            return new Dictionary<string, string>
            {
                {".txt", "text/plain"},
                {".pdf", "application/pdf"},
                {".doc", "application/vnd.ms-word"},
                {".docx", "application/vnd.ms-word"},
                {".xls", "application/vnd.ms-excel"},
                {".xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"},
                {".png", "image/png"},
                {".jpg", "image/jpeg"},
                {".jpeg", "image/jpeg"},
                {".gif", "image/gif"},
                {".csv", "text/csv"}
            };
        }
    }
}
