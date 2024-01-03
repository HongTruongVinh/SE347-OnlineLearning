using Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Dao
{
    public class ManagementCourseDao
    {
        public ManagementCourseDao() { }

        public List<Product> GetProductOfUser(int userId)
        {
            List<Product> products = new List<Product>();

            foreach(Product product in DataProvider.Ins.DB.Products)
            {
                if (int.Parse(product.CreateBy) == userId) 
                {
                    products.Add(product);
                }
            }

            return products;
        }

        public IEnumerable<Product> GetListCoursesByTeacherName(string teacherName)
        {
            IQueryable<Product> products = DataProvider.Ins.DB.Products;
            IQueryable<User> users = DataProvider.Ins.DB.Users;
            users = users.Where(x => x.UserName != "admin" && x.Name.Contains(teacherName));

            products.Where(p => users.Where(u => p.CreateBy == u.ID.ToString()).Count() > 0);

            return products;
        }

    }
}
