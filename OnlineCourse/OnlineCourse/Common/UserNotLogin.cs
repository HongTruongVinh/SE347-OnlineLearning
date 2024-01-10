using Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlineCourse.Common
{
    [Serializable]
    public class UserNotLogin
    {
        public List<Product> productsInCart { get; set; }
    }
}