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
            var user = (OnlineCourse.Common.UserLogin)Session[OnlineCourse.Common.CommonConstants.USER_SESSION];
            if (user == null)
            {
                return new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Home", action = "Index" }));
            }
            return View(user);
        }

        public ActionResult AcademicAchievement()
        {
            var user = (OnlineCourse.Common.UserLogin)Session[OnlineCourse.Common.CommonConstants.USER_SESSION];
            if (user == null)
            {
                return new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Home", action = "Index" }));
            }

            ViewBag.ListResultExam = new ResultDao().GetListResultExamOfUser(user.UserID);

            return View(user);
        }

        public ActionResult Exam()
        {
            var user = (OnlineCourse.Common.UserLogin)Session[OnlineCourse.Common.CommonConstants.USER_SESSION];
            if (user == null)
            {
                return new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Home", action = "Index" }));
            }

            ViewBag.ListOwnProducts = ConvertToProductModels(new WishProductDao().GetListWishProduct(user.UserID), true);

            ViewBag.Exams = (List<Model.Models.Exam>)new ExamDao().ListExamOfUser((int)user.UserID);

            return View(user);
        }

        public ActionResult CourseBought()
        {
            var user = (OnlineCourse.Common.UserLogin)Session[OnlineCourse.Common.CommonConstants.USER_SESSION];
            if (user == null)
            {
                return new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Home", action = "Index" }));
            }

            ViewBag.ListOwnProducts = ConvertToProductModels(new WishProductDao().GetListWishProduct(user.UserID), true);

            return View(user);
        }

        public ActionResult Cart()
        {
            var user = (OnlineCourse.Common.UserLogin)Session[OnlineCourse.Common.CommonConstants.USER_SESSION];
            if (user == null)
            {
                return new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Home", action = "Index" }));
            }

            ViewBag.CartProducts = ConvertToProductModels(new WishProductDao().GetListCartProduct(user.UserID), false);

            return View(user);
        }

        public ActionResult MyCourse()
        {
            var user = (OnlineCourse.Common.UserLogin)Session[OnlineCourse.Common.CommonConstants.USER_SESSION];
            if (user == null)
            {
                return new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Home", action = "Index" }));
            }
            return RedirectToAction("Index", "ManagementCourse");
        }

        [System.Web.Http.HttpPost]
        public ActionResult UpdateProfile(UserLogin _user, HttpPostedFileBase imageFile)
        {
            try
            {
                var dao = new UserDao();
                User user = new User();

                user = dao.ViewDetail(Convert.ToInt16(_user.UserID));

                user.Name = _user.FullName;
                user.Address = _user.Address;
                user.Email = _user.Email;
                user.Phone = _user.Phone;

                string path = UploadImage(imageFile);
                if (!path.Equals("-1"))
                {
                    user.LinkImage = path;
                }

                if (user.LinkImage == null)
                {
                    user.LinkImage = "/assets/client/images/avatar/00.jpg";
                }

                bool editresult = dao.Update(user);

                if (editresult == true)
                {
                    user = dao.ViewDetail(Convert.ToInt16(_user.UserID));
                    var usersession = SetUserSession(user);
                    Session.Remove(CommonConstants.USER_SESSION);
                    Session.Add(CommonConstants.USER_SESSION, usersession);
                    return RedirectToAction("Index");
                }
                else
                {
                    return Json(new { status = false });
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
                        path = Path.Combine(Server.MapPath("~/assets/client/images/avatar"), random + Path.GetFileName(file.FileName));
                        file.SaveAs(path);
                        path = "/assets/client/images/avatar/" + random + Path.GetFileName(file.FileName);

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

        UserLogin SetUserSession(User user)
        {
            var usersession = new UserLogin();
            usersession.UserID = user.ID;
            usersession.UserName = user.UserName;
            usersession.FullName = user.Name;
            usersession.Email = user.Email;

            if (user.LinkImage == null)
            {
                usersession.Image = "/assets/client/images/avatar/00.jpg";
            }
            else
            {
                usersession.Image = user.LinkImage;
            }

            usersession.Role = "Học viên";
            usersession.Phone = user.Phone;
            usersession.Address = user.Address;
            usersession.WishListIdProduct = new ProductDao().GetWishListProduct((int)user.ID);
            return usersession;
        }

        List<ProductModel> ConvertToProductModels(List<Product> products, bool isBought = false)
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
                model.IsBought = isBought;
                model.Price = product.Price;
                model.MetaTitle = product.MetaTitle;

                int createrID = (int)Convert.ToDouble(product.CreateBy);
                model.CreateBy = new ProductDao().GetCreatedByUser(createrID).Name;

                model.CountVideo = product.ListFile.Split('*').Length;

                model.CountComment = new ProductDao().GetCountComment(product.ID);

                productModels.Add(model);
            }

            return productModels;
        }

        [System.Web.Http.HttpGet]
        public ActionResult BuyProduct(int userId, int productId)
        {
            bool status = new ProductDao().BuyProduct(userId, productId);

            if (status == true)
            {
                return RedirectToAction("Cart");
            }
            else
            {
                return Json(new { status = false });
            }
        }

        [System.Web.Mvc.HttpGet]
        public ActionResult AddProductToCart(int userId, int productId)
        {
            bool status = new ProductDao().AddProductToCart(userId, productId);

            if (status == true)
            {
                var usersession = (OnlineCourse.Common.UserLogin)Session[OnlineCourse.Common.CommonConstants.USER_SESSION];
                Session.Remove(CommonConstants.USER_SESSION);
                usersession.WishListIdProduct.Add(productId.ToString(), false);
                Session.Add(CommonConstants.USER_SESSION, usersession);

                return Json(new { status = true }); ;
            }
            else
            {
                return Json(new { status = false });
            }
        }

        [System.Web.Http.HttpPost]
        public ActionResult DeleteProduct(int userId, int productId)
        {
            bool status = new ProductDao().DeleteProductFromCart(userId, productId);

            if (status == true)
            {
                var usersession = (OnlineCourse.Common.UserLogin)Session[OnlineCourse.Common.CommonConstants.USER_SESSION];
                Session.Remove(CommonConstants.USER_SESSION);
                usersession.WishListIdProduct.Remove(usersession.WishListIdProduct.Where(x => x.Key == productId.ToString()).SingleOrDefault().Key);
                Session.Add(CommonConstants.USER_SESSION, usersession);

                return RedirectToAction("Cart");
            }
            else
            {
                return Json(new { status = false });
            }
        }
    }
}