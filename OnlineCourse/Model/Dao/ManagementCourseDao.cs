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

            foreach (Product product in DataProvider.Ins.DB.Products)
            {
                if (int.Parse(product.CreateBy) == userId)
                {
                    products.Add(product);
                }
            }

            return products;
        }


    }
}
