using Microsoft.Ajax.Utilities;
using Model.Dao;
using Model.Models;
using Model.ViewModel;
using OnlineCourse.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Razor.Editor;
using System.Web.Routing;
using System.Web.UI.WebControls;

namespace OnlineCourse.Controllers
{
    public class ManagementCourseController : Controller
    {
        // GET: ManagementCourse

        List<Product> MyProducts;

        public ActionResult Index()
        {
            // data for nav bar product type
            ViewBag.CategoryID = new ProductCategoryDao().ListAll();

            var user = (OnlineCourse.Common.UserLogin)Session[OnlineCourse.Common.CommonConstants.USER_SESSION];
            if (user == null)
            {
                return new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Home", action = "Index" }));
            }

            GetListProductOfUser((int)user.UserID);
            ViewBag.MyProducts = ConvertToProductModels(MyProducts);

            return View();
        }

        public ActionResult ManagementCourseDetail(long productId)
        {
            // data for nav bar product type
            ViewBag.CategoryID = new ProductCategoryDao().ListAll();

            ViewBag.ListDocument = new CourseDocumentDao().GetListDocumentInfor((int)productId);
            ViewBag.ListVideo = new CourseVideoDao().GetListVideoInfor((int)productId);

            var user = (OnlineCourse.Common.UserLogin)Session[OnlineCourse.Common.CommonConstants.USER_SESSION];
            if (user == null)
            {
                return new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Home", action = "Index" }));
            }

            GetListProductOfUser((int)user.UserID);

            Product product = MyProducts.Where(x => x.ID == productId).FirstOrDefault();
            
            ProductModel model = new ProductModel() { ID = product.ID, Name = product.Name, Description = product.Description };

            return View(model);
        }

        public ActionResult ViewEditCourse(long productId)
        {
            // data for nav bar product type
            ViewBag.CategoryID = new ProductCategoryDao().ListAll();

            var user = (OnlineCourse.Common.UserLogin)Session[OnlineCourse.Common.CommonConstants.USER_SESSION];
            if (user == null)
            {
                return new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Home", action = "Index" }));
            }

            ViewBag.ListCategories = new ProductCategoryDao().ListAll();

            if (productId == -1)
            {
                return View(new Product());
            }

            var model = new ProductDao().ViewDetail(productId);

            return View(model);
        }

        public ActionResult ViewAddVieoToCourse(long productId)
        {
            // data for nav bar product type
            ViewBag.CategoryID = new ProductCategoryDao().ListAll();

            var user = (OnlineCourse.Common.UserLogin)Session[OnlineCourse.Common.CommonConstants.USER_SESSION];
            if (user == null)
            {
                return new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Home", action = "Index" }));
            }

            var product = new ProductDao().ViewDetail(productId);

            var model = new CourseVideo();

            ViewBag.product = new ProductModel() { CategoryID = productId, Name = product.Name };

            return View(model);
        }

        [System.Web.Http.HttpPost]
        public ActionResult AddVieoToCourse(CourseVideo video, HttpPostedFileBase file)
        {
            // data for nav bar product type
            ViewBag.CategoryID = new ProductCategoryDao().ListAll();

            var user = (OnlineCourse.Common.UserLogin)Session[OnlineCourse.Common.CommonConstants.USER_SESSION];
            if (user == null)
            {
                return new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Home", action = "Index" }));
            }

            if(file != null && video.Title != null)
            {
                video.Name = file.FileName;
                video.DateUpdate = DateTime.Now;
                video.Link = UploadVideo(file);

                var result = new CourseVideoDao().AddCourseVideo(video);
            }

            return RedirectToAction("ManagementCourseDetail", new { productId = video.productID });
        }


        public ActionResult ViewAddDocumentToCourse(long productId)
        {
            // data for nav bar product type
            ViewBag.CategoryID = new ProductCategoryDao().ListAll();

            var user = (OnlineCourse.Common.UserLogin)Session[OnlineCourse.Common.CommonConstants.USER_SESSION];
            if (user == null)
            {
                return new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Home", action = "Index" }));
            }

            var product = new ProductDao().ViewDetail(productId);

            var model = new CourseDocument();

            ViewBag.product = new ProductModel() { CategoryID = productId, Name = product.Name };

            return View(model);
        }

        [System.Web.Http.HttpPost]
        public ActionResult AddDocumentToCourse(CourseDocument document, HttpPostedFileBase file)
        {
            // data for nav bar product type
            ViewBag.CategoryID = new ProductCategoryDao().ListAll();

            var user = (OnlineCourse.Common.UserLogin)Session[OnlineCourse.Common.CommonConstants.USER_SESSION];
            if (user == null)
            {
                return new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Home", action = "Index" }));
            }

            if (document.Title != null && file != null)
            {
                document.Name = file.FileName;
                document.DateUpdate = DateTime.Now;
                document.Link = UploadDocument(file);
                var result = new CourseDocumentDao().AddCourseDocument(document);
            }

            return RedirectToAction("ManagementCourseDetail", new { productId = document.productID });
        }

        public ActionResult DeleteDocumentOfCourse(int documentId, int productId)
        {
            // data for nav bar product type
            ViewBag.CategoryID = new ProductCategoryDao().ListAll();

            var user = (OnlineCourse.Common.UserLogin)Session[OnlineCourse.Common.CommonConstants.USER_SESSION];
            if (user == null)
            {
                return new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Home", action = "Index" }));
            }

            var result = new CourseDocumentDao().DeleteCourseDocument(documentId);

            return RedirectToAction("ManagementCourseDetail", new { productId = productId });
        }

        public ActionResult DeleteVideoOfCourse(int videoId, int productId)
        {
            // data for nav bar product type
            ViewBag.CategoryID = new ProductCategoryDao().ListAll();

            var user = (OnlineCourse.Common.UserLogin)Session[OnlineCourse.Common.CommonConstants.USER_SESSION];
            if (user == null)
            {
                return new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Home", action = "Index" }));
            }

            var result = new CourseVideoDao().DeleteCourseVideo(videoId);

            return RedirectToAction("ManagementCourseDetail", new { productId = productId });
        }
        //public ActionResult AddCourse(Product product)
        //{

        //    var user = (OnlineCourse.Common.UserLogin)Session[OnlineCourse.Common.CommonConstants.USER_SESSION];
        //    if (user == null)
        //    {
        //        return new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Home", action = "Index" }));
        //    }

        //    long id = new ProductDao().Insert(product);

        //    return View(id);
        //}

        [System.Web.Http.HttpPost]
        public ActionResult UpdateCourse(Product product, HttpPostedFileBase imageFile)
        {
            // data for nav bar product type
            ViewBag.CategoryID = new ProductCategoryDao().ListAll();

            var user = (OnlineCourse.Common.UserLogin)Session[OnlineCourse.Common.CommonConstants.USER_SESSION];
            if (user == null)
            {
                return new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Home", action = "Index" }));
            }

            product.ModifiDate = DateTime.Now;


            if(imageFile != null)
            {
                string path = UploadImage(imageFile);

                if (!path.Equals("-1"))
                {
                    product.Image = path;
                }
            }

            bool result = false;

            if (product.ID != 0)
            {
                Product _product = new ProductDao().ViewDetail(product.ID);
                _product.ModifiDate = DateTime.Now;
                _product.Name = product.Name;
                _product.Description = product.Description;
                _product.Detail = product.Detail;
                _product.Price = product.Price;

                if (product.Image != null)
                {
                    _product.Image = product.Image;
                }

                result = new ProductDao().Update(_product);
            }
            else
            {
                product.CreateBy = user.UserID.ToString();
                product.Status = true;
                product.ListFile = " Bài 1: Giới thiệu khóa học " + product.Name + "* Bài 2: bai 2* Bài 3: bai 3* Bài 4: bai 4*Bài 5: bai 5";
                product.ListType = "0,0,0,0,0";
                product.CreateDate = DateTime.Now;

                if (product.Image == null)
                {
                    product.Image = "/assets/client/images/course/4by3/01.jpg";
                }


                long _id = new ProductDao().Insert(product);

                var _product = new ProductDao().ViewDetail(_id);
                _product.MetaTitle = _product.ID.ToString();

                result = new ProductDao().Update(_product);

            }

            if (result == true)
            {
                return RedirectToAction("Index");
            }
            else
            {
                return Json(new { status = false });
            }

        }

        public ActionResult DeleteCourse(int productId)
        {
            // data for nav bar product type
            ViewBag.CategoryID = new ProductCategoryDao().ListAll();

            var user = (OnlineCourse.Common.UserLogin)Session[OnlineCourse.Common.CommonConstants.USER_SESSION];
            if (user == null)
            {
                return new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Home", action = "Index" }));
            }

            bool result = new ProductDao().Delete(productId);

            if (result == true)
            {
                return Json(new { status = true });
            }
            else
            {
                return Json(new { status = false });
            }
        }

        public ActionResult UpdateVideoDetail(int videoId)
        {
            // data for nav bar product type
            ViewBag.CategoryID = new ProductCategoryDao().ListAll();

            var video = new CourseVideoDao().GetVideo(videoId);

            var listVideoExam = new VideoExamDao().GetListVideoExamByProductId(videoId);

            ViewBag.ListVideoExam = listVideoExam!=null?listVideoExam:new List<VideoExam>();

            return View(video);
        }

        public ActionResult AddExamView(int videoId)
        {
            // data for nav bar product type
            ViewBag.CategoryID = new ProductCategoryDao().ListAll();

            var newExamViewModel = new ExamViewModel();
            newExamViewModel.Exam = new VideoExam();
            newExamViewModel.ChosenQuestion = new Dictionary<int, bool>();
            newExamViewModel.Exam.VideoID = videoId;

            ViewBag.courseVideo = new CourseVideoDao().GetVideo(videoId);

            var listCourseQuestion = new ExamQuestionDao().GetDictionaryQuetionByVideoId(videoId);
            ViewBag.ListCourseQuestion = listCourseQuestion;

            return View(newExamViewModel);
        }

        static List<int> listChosenQuestionId = new List<int>();
        [HttpPost]
        public JsonResult UpdateCheckboxes(string checkboxId, string isChecked)
        {
            // data for nav bar product type
            ViewBag.CategoryID = new ProductCategoryDao().ListAll();

            if (bool.Parse(isChecked) == true && !listChosenQuestionId.Contains(int.Parse(checkboxId)))
            {
                listChosenQuestionId.Add(int.Parse(checkboxId));
            }
            else if(bool.Parse(isChecked) == false && listChosenQuestionId.Contains(int.Parse(checkboxId)))
            {
                listChosenQuestionId.Remove(int.Parse(checkboxId));
            }

            return Json(new { success = true });
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult AddExam(ExamViewModel examViewModel)
        {
            // data for nav bar product type
            ViewBag.CategoryID = new ProductCategoryDao().ListAll();

            var result = new VideoExamDao().Add(examViewModel.Exam, listChosenQuestionId);

            return RedirectToAction("UpdateVideoDetail", new { videoId = examViewModel.Exam.VideoID });
        }

        public ActionResult AddQuestionView(int videoId)
        {
            // data for nav bar product type
            ViewBag.CategoryID = new ProductCategoryDao().ListAll();

            ViewBag.videoID = videoId;
            return View();
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult AddQuestion(int videoId, ExamQuestion examQuestion, string answer1, string answer2, string answer3, string answer4, string trueAnswer)
        {
            // data for nav bar product type
            ViewBag.CategoryID = new ProductCategoryDao().ListAll();

            var result = new ExamQuestionDao().Add(examQuestion, answer1, answer2, answer3, answer4, trueAnswer, videoId);

            return RedirectToAction("AddExamView", new { videoId = videoId } );
        }

        void GetListProductOfUser(int userId)
        {

            MyProducts = new ManagementCourseDao().GetProductOfUser(userId);
        }

        List<ProductModel> ConvertToProductModels(List<Product> products)
        {
            List<ProductModel> productModels = new List<ProductModel>();

            foreach (Product product in products)
            {
                ProductModel model = new ProductModel();
                model.ID = product.ID;
                model.Name = product.Name;
                model.Description = product.Description;
                model.ModifiDate = product.ModifiDate;
                model.Detail = product.Detail;
                model.Image = product.Image;
                
                model.Price = product.Price;
                model.MetaTitle = product.MetaTitle;

                int createrID = (int)Convert.ToDouble(product.CreateBy);
                model.CreateBy = new ProductDao().GetCreatedByUser(createrID).Name;

                model.CountVideo = product.ListFile.Split('*').Length;

                model.CountComment = new ProductDao().GetCountComment(product.ID);

                model.CountLearner = new ProductDao().GetCountLearner(product.ID);

                productModels.Add(model);
            }

            return productModels;
        }

        public string UploadImage(HttpPostedFileBase file)
        {
            Random r = new Random();
            string path = "-1";
            int random = r.Next();

            if (file != null && file.ContentLength > 0)
            {
                string extension = Path.GetExtension(file.FileName);
                if (extension.ToLower().Equals(".jpg") || extension.ToLower().Equals(".img") || extension.ToLower().Equals(".png"))
                {
                    try
                    {
                        path = Path.Combine(Server.MapPath("~/assets/client/images/courses"), random + Path.GetFileName(file.FileName));
                        file.SaveAs(path);
                        path = "/assets/client/images/courses/" + random + Path.GetFileName(file.FileName);

                    }
                    catch
                    {
                        path = "-1";
                    }
                }
                else
                {
                    Response.Write("<script>alert('Only jpg, png or img formats are acceptable....'); </script>");
                }
            }
            else
            {
                Response.Write("<script>alert('Please select a file'); </script>");
                path = "-1";
            }

            return path;
        }

        public string UploadDocument(HttpPostedFileBase file)
        {
            Random r = new Random();
            string path = "-1";
            int random = r.Next();

            if (file != null && file.ContentLength > 0)
            {
                string extension = Path.GetExtension(file.FileName);
                if (extension.ToLower().Equals(".txt") || extension.ToLower().Equals(".pdf") || extension.ToLower().Equals(".docx") || extension.ToLower().Equals(".zip"))
                {
                    try
                    {
                        path = Path.Combine(Server.MapPath("~/assets/client/documents"), random + Path.GetFileName(file.FileName));
                        file.SaveAs(path);
                        path = "/assets/client/documents/" + random + Path.GetFileName(file.FileName);

                    }
                    catch
                    {
                        path = "-1";
                    }
                }
                else
                {
                    Response.Write("<script>alert('Only pdf, txt, doc or zip formats are acceptable....'); </script>");
                }
            }
            else
            {
                Response.Write("<script>alert('Please select a file'); </script>");
                path = "-1";
            }

            return path;
        }

        public string UploadVideo(HttpPostedFileBase file)
        {
            Random r = new Random();
            string path = "-1";
            int random = r.Next();

            if (file != null && file.ContentLength > 0)
            {
                string extension = Path.GetExtension(file.FileName);
                if (extension.ToLower().Equals(".mp4"))
                {
                    try
                    {
                        path = Path.Combine(Server.MapPath("~/assets/client/videos"), random + Path.GetFileName(file.FileName));
                        file.SaveAs(path);
                        path = "/assets/client/videos/" + random + Path.GetFileName(file.FileName);

                    }
                    catch
                    {
                        path = "-1";
                    }
                }
                else
                {
                    Response.Write("<script>alert('Only mp4 formats are acceptable....'); </script>");
                }
            }
            else
            {
                Response.Write("<script>alert('Please select a file'); </script>");
                path = "-1";
            }

            return path;
        }
    }
}