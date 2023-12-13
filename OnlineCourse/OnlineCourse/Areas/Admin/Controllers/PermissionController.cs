using Model.Dao;
using Model.Models;
using OnlineCourse.App_Start;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlineCourse.Areas.Admin.Controllers
{
    public class PermissionController : Controller
    {
        // GET: Admin/Permission

        [AdminAuthorize(PermissionName = "Permission_Update", IsAccessPage = true)]
        public ActionResult Index()
        {
            return View(new RoleDao().Roles());
        }


        [HttpGet]
        [AdminAuthorize(PermissionName = "Permission_Update")]
        public ActionResult ViewPermissionDetail(int roleId)
        {
            Dictionary<string, string> listPer_Ihave = new RoleDao().GetListPer_IHave(roleId + 1);// Role' ID start from 1
            Dictionary<string, string> listPer_IhaveNo = new RoleDao().GetListPer_IHaveNo(listPer_Ihave);

            return Json(new
            {
                listPer_IHave = listPer_Ihave,
                listPer_IHaveNo = listPer_IhaveNo
            }
            , JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [AdminAuthorize(PermissionName = "Permission_Update")]
        public ActionResult UpdatePermissionDetail(int roleId, List<string> listPermissionName)
        {
            bool success = new RoleDao().UpdateMyPermission(roleId + 1, listPermissionName);// Role' ID start from 1

            return Json(new
            {
                result = success
            }
            , JsonRequestBehavior.AllowGet);
        }
    }
}