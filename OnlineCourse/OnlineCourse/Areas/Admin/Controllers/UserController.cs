using Model.Dao;
using Model.Models;
using OnlineCourse.App_Start;
using OnlineCourse.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Helpers;
using System.Web.Http.Controllers;
using System.Web.Mvc;
using System.Xml.Linq;

namespace OnlineCourse.Areas.Admin.Controllers
{
    public class UserController : BaseController
    {
        //
        // GET: /Admin/User/

        [AdminAuthorize(PermissionName = "User_View", IsAccessPage = true)]
        public ActionResult Index(string searchString, int page = 1, int pageSize = 200)
        {
            var dao = new UserDao();
            var model = dao.ListAllPaging(searchString, page, pageSize);
            ViewBag.SearchString = searchString;
            return View(model);
        }

        [HttpPost]
        public JsonResult AddUserAjax(string username, string name, string password, string address, string email, string phone)
        {
           try
           {
               var dao = new UserDao();
               User user = new User();

               var encryptedMd5Pas = Encryptor.MD5Hash(password);
               user.Password = encryptedMd5Pas;
               user.CreateDate = DateTime.Now;
               user.UserName = username;
               user.Name = name;
               user.Address = address;
               user.Email = email;
               user.Phone = phone;
               user.Status = true;

               long id = dao.Insert(user);
               if(id > 0)
               {
                   return Json(new {status = true});
               }
               else
               { 
                   return Json(new {status = false});
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

        //[HttpPost]
        //[AdminAuthorize(PermissionName = "User_Update")]
        //public JsonResult UpdateUserAjax(string userid, string name,string address, string email, string phone)
        //{
        //    try
        //    {
        //        var dao = new UserDao();
        //        User user = new User();

        //        user = dao.ViewDetail(Convert.ToInt16(userid));

        //        user.Name = name;
        //        user.Address = address;
        //        user.Email = email;
        //        user.Phone = phone;


        //        bool editresult = dao.Update(user);

        //        if (editresult == true)
        //        {
        //            return Json(new { status = true });
        //        }
        //        else
        //        {
        //            return Json(new { status = false });
        //        }
        //    }
        //    catch
        //    {
        //        return Json(new
        //        {
        //            status = false
        //        });
        //    }
        //}

        //[HttpDelete]
        //[AdminAuthorize(PermissionName = "User_Delete")]
        //public ActionResult Delete(int id)
        //{
        //    new UserDao().Delete(id);
        //    return RedirectToAction("Index");
        //}


        [HttpGet]
        [AdminAuthorize(PermissionName = "User_Update")]
        public ActionResult UpdateUser(int id)
        {
            User user = new UserDao().ViewDetail(id);

            ViewBag.Role = new RoleDao().GetRoleUser(id);

            return PartialView("_UpdateUserPartial", user);
        }

        [HttpPost]
        [AdminAuthorize(PermissionName = "User_Update")]
        public ActionResult UpdateUser(User _user)
        {
            try
            {
                var dao = new UserDao();
                User user = new User();

                user = dao.ViewDetail(Convert.ToInt16(_user.ID));

                user.Name = _user.Name;
                user.Address = _user.Address;
                user.Email = _user.Email;
                user.Phone = _user.Phone;


                bool editresult = dao.Update(user);

                if (editresult == true)
                {
                    return Json(new { status = true });
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

        [HttpGet]
        [AdminAuthorize(PermissionName = "User_Delete")]
        public ActionResult Delete(int id)
        {
            User user = new UserDao().ViewDetail(id);

            return PartialView("_ConfirmDeleteModelPartial", user);
        }

        [HttpPost]
        [AdminAuthorize(PermissionName = "User_Delete")]
        public ActionResult Delete(User user)
        {
            bool success = new UserDao().Delete((int)user.ID);

            return RedirectToAction("Index", new { searchString = "", page = 1 , pageSize = 200 });
        }


        //
        // GET: /Admin/User/Student

        [AdminAuthorize(PermissionName = "User_View", IsAccessPage = true)]
        public ActionResult Student(string searchString, int page = 1, int pageSize = 200)
        {
            var dao = new UserDao();
            var model = dao.ListAllPaging(searchString, page, pageSize);

        
            IEnumerable<User>  list = model.Where(item => new RoleDao().GetRoleUser(Convert.ToInt32(item.ID)).Contains("Learners"));

            
            ViewBag.SearchString = searchString;
   

            return View(list);
        }

        //
        // GET: /Admin/User/Teacher

        [AdminAuthorize(PermissionName = "User_View", IsAccessPage = true)]
        public ActionResult Teacher(string searchString, int page = 1, int pageSize = 200)
        {
            var dao = new UserDao();
            var model = dao.ListAllPaging(searchString, page, pageSize);


            IEnumerable<User> list = model.Where(item => new RoleDao().GetRoleUser(Convert.ToInt32(item.ID)).Contains("Teachers"));


            ViewBag.SearchString = searchString;
         

            return View(list);
        }

        //
        // GET: /Admin/User/Poster

        [AdminAuthorize(PermissionName = "User_View", IsAccessPage = true)]
        public ActionResult Poster(string searchString, int page = 1, int pageSize = 200)
        {
            var dao = new UserDao();
            var model = dao.ListAllPagingPoster(searchString, page, pageSize);

            ViewBag.SearchString = searchString;
            
            return View(model);
        }
    }   
}
