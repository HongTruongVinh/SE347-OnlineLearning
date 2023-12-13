using Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Dao
{
    public class AdminDao
    {
        public AdminDao() { }

        public int Login(string username, string password)
        {
            var result = DataProvider.Ins.DB.Users.SingleOrDefault(x => x.UserName == username);
            if (result == null)
                return 0;
            else
            {
                if (result.Status == false)
                {
                    return -1;
                }
                else
                {
                    if (result.Password == password)
                        return 1;
                    else
                        return -2;
                }
            }
        }
    }
}
