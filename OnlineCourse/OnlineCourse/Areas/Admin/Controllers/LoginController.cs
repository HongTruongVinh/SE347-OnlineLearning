using Model.Dao;
using OnlineCourse.App_Start;
using OnlineCourse.Areas.Admin.Models;
using OnlineCourse.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlineCourse.Areas.Admin.Controllers
{
    public class LoginController : Controller
    {
        //
        // GET: /Admin/Login/

        
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Login(LoginModel model)
        {
            if(ModelState.IsValid)
            {
                var result = new AdminDao().Login(model.UserName, Encryptor.MD5Hash(model.Password));
                if(result == 1)
                {
                    var dao = new UserDao();
                    var user = dao.GetByUserName(model.UserName);
                    if (dao.isAdminRole((int)user.ID))
                    {
                        var usersession = new UserLogin();
                        usersession.UserName = user.UserName;
                        usersession.UserID = user.ID;
                        Session.Add(CommonConstants.USER_SESSION, usersession);
                        
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Tài khoản không có quyền đăng nhập");
                    }
                }
                else if(result == 0)
                    {
                        ModelState.AddModelError("","Tài khoản không tồn tại");
                    }
                else if(result ==-1)
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
            return View("Index");
        }



    }
}
