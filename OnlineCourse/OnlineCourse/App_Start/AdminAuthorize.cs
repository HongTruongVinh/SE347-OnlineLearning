using Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace OnlineCourse.App_Start
{
    public class AdminAuthorize : AuthorizeAttribute
    {
        public string PermissionName { get; set; }

        public bool IsAccessPage = false;

        private List<Permission> ListPermission;

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            //1. Check session : Đã đăng nhập => cho thực hiện hiện Filter
            // Ngược lại thì cho trở lại => trang đăng nhập
            var userSession = (OnlineCourse.Common.UserLogin)HttpContext.Current.Session[OnlineCourse.Common.CommonConstants.USER_SESSION];

            if(userSession==null)
            {
                var returnUrl = filterContext.RequestContext.HttpContext.Request.RawUrl;
                filterContext.Result = new RedirectToRouteResult(new
                RouteValueDictionary(
                new
                {
                    controller = "Login",
                    action = "Index",
                    area = "Admin",
                    returnUrl = returnUrl.ToString()
                }));
                return;
            }

            GetListPermission((int)userSession.UserID);

            if (userSession != null)
            {
                //2.Check quyền: Có quyền thì => cho thực hiện hiện Filter
                //Ngược lại thì cho trở lại trang => Trang báo lỗi quyền truy cập
                
                if (ListPermission.Select(x => x.Name.Trim()).Contains(PermissionName))
                {
                    return;
                }
                else
                {
                    if (!IsAccessPage)
                    {
                        var returnUrl = filterContext.RequestContext.HttpContext.Request.RawUrl;
                        filterContext.Result = new RedirectToRouteResult(new
                        RouteValueDictionary(
                        new
                        {
                            controller = "Error",
                            action = "NotAllowed",
                            area = "Admin",
                            returnUrl = returnUrl.ToString()
                        }));
                    }
                    else
                    {
                        var returnUrl = filterContext.RequestContext.HttpContext.Request.RawUrl;
                        filterContext.Result = new RedirectToRouteResult(new
                        RouteValueDictionary(
                        new
                        {
                            controller = "Error",
                            action = "NotAllowedPage",
                            area = "Admin",
                            returnUrl = returnUrl.ToString()
                        }));
                        IsAccessPage = false;
                    }
                }

                return;
            }
            else
            {
                var returnUrl = filterContext.RequestContext.HttpContext.Request.RawUrl;
                filterContext.Result = new RedirectToRouteResult(new
                RouteValueDictionary(
                new
                {
                    controller = "Login",
                    action = "Login",
                    area = "Admin",
                    returnUrl = returnUrl.ToString()
                }));
            }



        }

        public void GetListPermission(int userID)
        {
            ListPermission = new List<Permission>();

            //lấy danh sách các role của user dựa trên userID
            List<int> listRoleId = DataProvider.Ins.DB.User_Role.Where(x => x.idUser == userID).Select(x => x.idRole).ToList();

            //với mỗi roleId của user sẽ có 1 list các permission
            //nên tôi sẽ cho lặp danh sách listRoleId
            //sau đó lặp danh sách các role_per 
            //nếu một row nào đó cửa role_per xuất hiện roleId cần kiểm tra và row này chưa được thêm vào list permission thì add vào  list permission
            foreach (var roleID in listRoleId)
            {
                foreach (var role_per in DataProvider.Ins.DB.Role_Per)
                {
                    if(role_per.idRole == roleID && !ListPermission.Select(x => x.ID).Contains(role_per.idPer))
                    {
                        ListPermission.Add(DataProvider.Ins.DB.Permissions.Where(x => x.ID == role_per.idPer).SingleOrDefault());
                    }
                }

            }

        }

    }
}