﻿using Microsoft.AspNetCore.Mvc.Filters;
using Model.Dao;
using Model.Models;
using OnlineCourse.Common;
using System;
using System.Buffers.Text;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;

namespace OnlineCourse.Controllers
{
    public class ProductController : Controller
    {
        public const int ITEMS_PER_PAGE = 120;
        public int currentPage { get; set; }
        public int countPages { get; set; }

        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Category(
            string searchString, long cateId, 
            string searchTeacherName, string filterCategory, string order,
            string minPrice,
            string maxPrice
        )
        {   
            // Truyền giá trị từ url vào filter
            string valueSearchString = "";
            if (searchString != null)
            {
                valueSearchString = searchString;
            }
            ViewBag.ValueSearchString = valueSearchString;

            // Truyền giá trị từ url vào filter
            string valueFilterCategory = "0";
            if (filterCategory != null)
            {
                cateId = Int32.Parse(filterCategory);
                valueFilterCategory = filterCategory;
            }
            else
            {
                valueFilterCategory = cateId.ToString();
            }
            ViewBag.ValueFilterCategory = valueFilterCategory;


            // Truyền giá trị từ url vào filter
            string valueOrder = "1";
            if (order != null && order != "")
            {
                valueOrder = order;
                
            }
            ViewBag.ValueOrder = valueOrder;

            // Truyền giá trị từ url vào filter
            string valueMinPrice = "1000";
            if (minPrice != null && minPrice != "")
            {
                valueMinPrice = minPrice;
            }
            ViewBag.ValueMinPrice = valueMinPrice;

            // Truyền giá trị từ url vào filter
            string valueMaxPrice = "100000000";
            if (maxPrice != null && maxPrice != "")
            {
                valueMaxPrice = maxPrice;
            }
            ViewBag.ValueMaxPrice = valueMaxPrice;

            countPages = (int)Math.Ceiling((double)new ProductDao().CountByCategoryID(searchString, cateId) / ITEMS_PER_PAGE);

            if (currentPage < 1)
            {
                currentPage = 1;
            }
            if (currentPage > countPages)
            {
                currentPage = countPages;
            }

            ViewBag.currentPage = currentPage;
            ViewBag.countPages = countPages;

            var category = new ProductCategoryDao().ViewDetail(cateId);
            ViewBag.Category = category;
            ViewBag.CategoryID = new ProductCategoryDao().ListAll();
            var model = new ProductDao()
                .ListByCategoryID(
                    searchString, searchTeacherName, 
                    cateId, currentPage, ITEMS_PER_PAGE, 
                    valueOrder,
                    Int32.Parse(valueMinPrice),
                    Int32.Parse(valueMaxPrice)
                );

            List<Model.Models.User> users = new List<Model.Models.User>();

            var userDao = new UserDao();

            foreach (var product in model)
            {
                users.Add(userDao.GetByUserId(Int32.Parse(product.CreateBy)));
            }

            ViewBag.UserProducts = users;
            ViewBag.Products = model;

            return View();
        }

        public ActionResult Detaill(long id, long detailid)
        {
            // nếu chưa login thì out ra
            var checkLogin = CheckLogin();
            if (checkLogin != null) return checkLogin;

            var product = new ProductDao().ViewDetail(id);
            ViewBag.CategoryID = new ProductCategoryDao().ListAll();

            var sessionUser = (UserLogin)Session[CommonConstants.USER_SESSION];
            ViewBag.UserID = sessionUser.UserID;
            ViewBag.ListComment = new CommentDao().ListCommentViewModel(0, id);

            ViewBag.DetailID = detailid.ToString();

            int createrID = (int)Convert.ToDouble(product.CreateBy);
            ViewBag.CreatedBy = new ProductDao().GetCreatedByUser(createrID);



            return View(product);
        }

        public ActionResult Detail(int productId, int playingIdVideo)
        {
            // nếu chưa login thì out ra
            //var checkLogin = CheckLogin();
            //if (checkLogin != null) return checkLogin;

            var product = new ProductDao().ViewDetail(productId);
            ViewBag.CategoryID = new ProductCategoryDao().ListAll();

            var sessionUser = (UserLogin)Session[CommonConstants.USER_SESSION];

            // đã đăng nhập
            if (sessionUser != null)
            {
                ViewBag.UserID = sessionUser.UserID;
                ViewBag.isProductOfUserSession = new ProductDao().IsProductOfUserSession(productId, (int)sessionUser.UserID);
            }

            ViewBag.ListComment = new CommentDao().ListCommentViewModel(0, productId);

            int createrID = (int)Convert.ToDouble(product.CreateBy);
            ViewBag.CreatedBy = new ProductDao().GetCreatedByUser(createrID);

            List<CourseVideo> productVideos = new CourseVideoDao().GetListVideoInfor(productId);
            ViewBag.productVideos = productVideos;

            ViewBag.productDocuments = new CourseDocumentDao().GetListDocumentInfor(productId);

            if (playingIdVideo == -1 && productVideos.Count > 0)
            {
                playingIdVideo = new CourseVideoDao().GetListVideoInfor(productId).OrderByDescending(o => o.DateUpdate).ToList().FirstOrDefault().ID;
            }

            ViewBag.playingVideo = new CourseVideoDao().GetVideo(playingIdVideo);

            var productdao = new ProductDao();
            ViewBag.HomeProducts = productdao.ListAllProduct();

            var lisExam = new List<VideoExam>();
            lisExam = new VideoExamDao().GetListVideoExamByProductId(playingIdVideo);
            ViewBag.ListVideoExam = lisExam;

            ViewBag.currentCategoryId = (int)product.CategoryID;


            var model = new ProductDao().getListRecommend(product);
            

            List<Model.Models.User> users = new List<Model.Models.User>();

            var userDao = new UserDao();

            foreach (var item in model)
            {
                users.Add(userDao.GetByUserId(Int32.Parse(item.CreateBy)));
            }

            ViewBag.UserProducts = users;
            ViewBag.RecommendProducts = model;

            return View(product);
        }

        [ChildActionOnly]
        public ActionResult _ChildComment(long parentid, long productid)
        {
            // nếu chưa login thì out ra
            var checkLogin = CheckLogin();
            if (checkLogin != null) return checkLogin;

            var data = new CommentDao().ListCommentViewModel(parentid, productid);
            var sessionuser = (UserLogin)Session[CommonConstants.USER_SESSION];
            for (int k = 0; k < data.Count; k++)
            {
                data[k].UserID = sessionuser.UserID;
            }
            return PartialView("~/Views/Shared/_ChildComment.cshtml", data);
        }
        [HttpPost]
        public JsonResult AddNewComment(long productid, long userid, long parentid, string commentmsg, string rate)
        {
            try
            {
                var dao = new CommentDao();
                Comment comment = new Comment();

                comment.CommentMsg = commentmsg;
                comment.ProductID = productid;
                comment.UserID = userid;
                comment.ParentID = parentid;
                comment.Rate = Convert.ToInt16(rate);
                comment.CommentDate = DateTime.Now;

                bool addcomment = dao.Insert(comment);
                if (addcomment == true)
                {
                    return Json(new
                    {
                        status = true
                    });
                }
                else
                {
                    return Json(new
                    {
                        status = false
                    });
                }
            }
            catch
            {
                return Json(new
                {
                    status = false
                });
            }
        }
        public ActionResult GetComment(long productid)
        {
            var data = new CommentDao().ListCommentViewModel(0, productid);
            return PartialView("~/Views/Shared/_ChildComment.cshtml", data);
        }

        public FileResult LearnerDownloadDocument(string link)
        {
            if (link == "-1") return null;

            byte[] fileBytes = System.IO.File.ReadAllBytes(Server.MapPath(link));

            string fileName = new CourseDocumentDao().GetByLink(link).Name;

            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
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

        private ActionResult CheckLogin()
        {
            var session = (UserLogin)Session[CommonConstants.USER_SESSION];
            if (session == null)
            {
                return new RedirectToRouteResult(new RouteValueDictionary(new { controller = "User", action = "Login" }));
            }
            return null;           
        }
    }
}
