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

    }
}
