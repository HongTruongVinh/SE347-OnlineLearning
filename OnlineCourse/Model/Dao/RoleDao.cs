using Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Dao
{
    public class RoleDao
    {
        public RoleDao() { }

        public List<Role> Roles() 
        {
            return DataProvider.Ins.DB.Roles.ToList();
        }

        public string GetRoleUser(int userId)
        {
            List<string> roles = new List<string>();

            //lấy danh sách các role của user dựa trên userID
            List<int> listRoleId = DataProvider.Ins.DB.User_Role.Where(x => x.idUser == userId).Select(x => x.idRole).ToList();

            foreach (Role role in DataProvider.Ins.DB.Roles)
            {
                if(!roles.Contains(role.Name) && listRoleId.Contains(role.ID))
                {
                    roles.Add(role.Name.Trim());
                }
            }

            string stringRole = "";

            foreach (string roleName in roles)
            {
                stringRole += roleName + ", ";
            }

            return stringRole;
        }

        //public List<string> GetListPer_IHave(int roleId) 
        //{
        //    List<string> list = new List<string>();

        //    foreach (var role_per in DataProvider.Ins.DB.Role_Per)
        //    {
        //        if (role_per.idRole == roleId)
        //        {
        //            list.Add(DataProvider.Ins.DB.Permissions.Where(x => x.ID == role_per.idPer).SingleOrDefault().Name.Trim());
        //        }
        //    }

        //    return list;
        //}

        //public List<string> GetListPer_IHaveNo(List<string> listPer_IHave)
        //{
        //    List<string> list = new List<string>();

        //    foreach (var perName in (DataProvider.Ins.DB.Permissions.Select(x => x.Name.Trim())))
        //    {
        //        if (!listPer_IHave.Contains(perName))
        //        {
        //            list.Add(perName.Trim());
        //        }
        //    }

        //    return list;
        //}

        public Dictionary<string, string> GetListPer_IHave(int roleId)
        {
            Dictionary<string, string> list = new Dictionary<string, string>();

            foreach (var role_per in DataProvider.Ins.DB.Role_Per)
            {
                if (role_per.idRole == roleId)
                {
                    Permission per = DataProvider.Ins.DB.Permissions.Where(x => x.ID == role_per.idPer).SingleOrDefault();
                    list.Add(per.Name.Trim(), per.Detail);
                }
            }

            return list;
        }

        public Dictionary<string, string> GetListPer_IHaveNo(Dictionary<string, string> listPer_IHave)
        {
            Dictionary<string, string> list = new Dictionary<string, string>();

            foreach (var per in (DataProvider.Ins.DB.Permissions))
            {
                if (!listPer_IHave.Keys.Contains(per.Name.Trim()))
                {
                    list.Add(per.Name.Trim(), per.Detail);
                }
            }

            return list;
        }


        public bool UpdateMyPermission(int roleId, List<string> listPermissionName)
        {
            try
            {
                //xóa các quyền có sẵn của role chuyền vào
                foreach (var per in (DataProvider.Ins.DB.Role_Per))
                {
                    if (per.idRole == roleId)
                    {
                        DataProvider.Ins.DB.Role_Per.Remove(per);
                    }
                }

                //nếu listPermissionName có item thêm list quyền mới theo list mới chuyền vào 
                if (listPermissionName != null)
                {
                    foreach (var perName in listPermissionName)
                    {
                        foreach (var per in (DataProvider.Ins.DB.Permissions))
                        {
                            if (per.Name.Trim() == perName)
                            {
                                Role_Per role_Per = new Role_Per() { idPer = per.ID, idRole = roleId };
                                DataProvider.Ins.DB.Role_Per.Add(role_Per);
                            }
                        }
                    }
                }
                DataProvider.Ins.DB.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
            
        }
    }
}
