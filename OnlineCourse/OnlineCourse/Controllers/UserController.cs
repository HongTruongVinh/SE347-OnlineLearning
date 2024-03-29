﻿using Model.Dao;
using Model.Models;
using OnlineCourse.Common;
using OnlineCourse.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Razor.Editor;
using System.Web.Routing;
using System.Web.Security;

namespace OnlineCourse.Controllers
{
    public class UserController : Controller
    {
        //
        // GET: /User/

        public ActionResult Login()
        {
            HomeInfor homeInfor = new GetInforDao().GetHomeInfor();
            ViewBag.countLearner = homeInfor.CountStudent;
            return View();
        }
        [System.Web.Mvc.HttpPost]
        public async Task<ActionResult> Login(LoginModel model)
        {

            if (ModelState.IsValid)
            {
                var dao = new UserDao();
                var result = dao.Login(model.UserName, Encryptor.MD5Hash(model.Password), true);

                if (result == 1)
                {
                    var user = dao.GetByUserName(model.UserName);
                    var usersession = SetUserSession(user);
                    usersession.Address = user.Address;

                    var wishProducts = new ProductDao().GetWishListProduct((int)user.ID);

                    
                    
                    // lấy giỏ hàng nạp vào data
                    var strProductCart = OnlineCourse.Common.CommonConstants.PRODUCTS_CART;
                    var userNotLoginSession = (OnlineCourse.Common.UserNotLogin)Session[strProductCart];

                    var products = userNotLoginSession.productsInCart;
                    var productDao = new ProductDao();
                    foreach(var item in products)
                    {
                        // khóa học này chưa trong giỏ hàng hoặc đã mua thì add
                        if (wishProducts.ContainsKey(item.ID.ToString()) == false)
                        {
                            productDao.AddProductToCart((int)user.ID, (int)item.ID);
                        }
                    }
                    

                    // xóa dữ liệu trong session
                    userNotLoginSession.productsInCart.Clear();

                    Session.Remove(strProductCart);
                    Session.Add(strProductCart, userNotLoginSession);

                    await Task.Delay(2000);


                    wishProducts = new ProductDao().GetWishListProduct((int)user.ID);
                    usersession.WishListIdProduct = wishProducts;
                    Session.Add(CommonConstants.USER_SESSION, usersession);

                    Session["WishProuducts"] = new WishProductDao().GetListWishProduct((int)user.ID);


                    return RedirectToAction("Index", "Home");
                }
                else if (result == 0)
                {
                    ModelState.AddModelError("", "Tài khoản không tồn tai");
                }
                else if (result == -1)
                {
                    ModelState.AddModelError("", "Tài khoản đang bị khóa");
                }
                else if (result == -2)
                {
                    ModelState.AddModelError("", "Mật khẩu không đúng");
                }
                else if (result == -3)
                {
                    ModelState.AddModelError("", "Tài khoản không có quyền đăng nhập");
                }


            }
            return View("model");
        }

        public ActionResult LogUot()
        {
            Session.Remove(CommonConstants.USER_SESSION);

            FormsAuthentication.SignOut();

            return new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Home", action = "Index"}));
        }


        public ActionResult ProfileUser()
        {
            var user = (OnlineCourse.Common.UserLogin)Session[OnlineCourse.Common.CommonConstants.USER_SESSION];
            if (user == null)
            {
                return new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Home", action = "Index" }));
            }

            //ViewBag.ListResultExam = new ResultDao().GetListResultExamOfUser(user.UserID);

            //ViewBag.ListOwnProducts = ConvertToProductModels(new OwnProductDao().GetListOwnProduct(user.UserID), true);

            //ViewBag.CartProducts = ConvertToProductModels(new OwnProductDao().GetListCartProduct(user.UserID), false);

            return View(user);
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

        public ActionResult ProductAuthor(int userId)
        {
            // data for nav bar product type
            ViewBag.CategoryID = new ProductCategoryDao().ListAll();

    
            ViewBag.User = new UserDao().GetByUserId(userId);
            ViewBag.UserProducts = new ProductDao().getByUserId(userId);
            ViewBag.CountStudent = new UserDao().countStudentByID(userId);
            ViewBag.countComment = new UserDao().countCommentByID(userId);


            return View();
        }
    }
}
