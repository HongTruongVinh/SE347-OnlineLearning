using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlineCourse.Common
{
    [Serializable]
    public class UserLogin
    {
        public long UserID { set; get; }
        public string UserName { set; get; }
        public string GroupID { set; get; }
        public string Email { set; get; }
        public string FullName { set; get; }
        public string Image { set; get; }
        public string Phone { set; get; }
        public DateTime CreateDate { set; get; }
        public string ProductList { set; get; }
        public string Address { set; get; }
        public string Role { set; get; }
        public string Password { set; get; }

        public Dictionary<string, bool> WishListIdProduct { set; get; }
    }
}