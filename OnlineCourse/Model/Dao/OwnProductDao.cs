using Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Dao
{
    public class WishProductDao
    {
        public WishProductDao() { }

        public List<Product> GetListWishProduct(long userId)
        {
            List<Product> products = new List<Product>();

            List<WishProduct> wishProduct = new List<WishProduct>();

            wishProduct = DataProvider.Ins.DB.WishProducts.Where(x => x.UserID == userId && x.IsBought == true).ToList();

            foreach (var item in wishProduct)
            {
                Product p = DataProvider.Ins.DB.Products.Where(x => x.ID == item.ProductID).SingleOrDefault();
                if (p != null)
                {
                    products.Add(p);
                }
            }

            return products;
        }

        public List<Product> GetListCartProduct(long userId)
        {
            List<Product> products = new List<Product>();

            List<WishProduct> cartProducts = new List<WishProduct>();

            cartProducts = DataProvider.Ins.DB.WishProducts.Where(x => x.UserID == userId && x.IsBought == false).ToList();

            foreach (var item in cartProducts)
            {
                Product p = DataProvider.Ins.DB.Products.Where(x => x.ID == item.ProductID).SingleOrDefault();
                if (p != null)
                {
                    products.Add(p);
                }
            }

            return products;
        }
    }
}
