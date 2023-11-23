using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Model.Models;
using PagedList;


namespace Model.Dao
{
    public class ProductCategoryDao
    {

        public ProductCategoryDao()
        {

        }

        public List<ProductCategory> ListAll()
        {
            return DataProvider.Ins.DB.ProductCategories.Where(x => x.Status == true).OrderBy(x => x.DisplayOrder).ToList();

        }
        public ProductCategory ViewDetail(long id)
        {

            return DataProvider.Ins.DB.ProductCategories.Find(id);
        }

        public int CountProduct()
        {
            return DataProvider.Ins.DB.ProductCategories.Where(x => x.Status == true).Count();
        }
    }
}
