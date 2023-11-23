using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Model.Models;
using PagedList;

namespace Model.Dao
{
    public class ProductDao
    {

        public ProductDao()
        {

        }
        public IEnumerable<Product> ListAllPaging(long cateID, string searchString, int page, int pagesize)
        {
            IQueryable<Product> model = DataProvider.Ins.DB.Products;
            if (cateID != -1)
            {
                model = model.Where(x => x.CategoryID == cateID);
            }
            if (!string.IsNullOrEmpty(searchString))
            {
                model = model.Where(x => x.Name.Contains(searchString) || x.MetaTitle.Contains(searchString));
            }
            return model.OrderByDescending(x => x.CreateDate).ToPagedList(page, pagesize);
        }

        public bool IsProductOfUserSession(int productId, int userId)
        {

            Product product = DataProvider.Ins.DB.Products.Where(x => x.ID == productId).SingleOrDefault();

            if (product != null && int.Parse(product.CreateBy) == userId)
            {
                return true;
            }
            else { return false; }
        }

        public bool Delete(int id)
        {
            try
            {
                var product = DataProvider.Ins.DB.Products.Find(id);
                DataProvider.Ins.DB.Products.Remove(product);
                DataProvider.Ins.DB.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public long Insert(Product entity)
        {
            DataProvider.Ins.DB.Products.Add(entity);
            DataProvider.Ins.DB.SaveChanges();
            return entity.ID;
        }
        public Product ViewDetail(long id)
        {

            return DataProvider.Ins.DB.Products.Find(id);
        }
        public bool Update(Product entity)
        {
            try
            {
                var product = DataProvider.Ins.DB.Products.Find(entity.ID);
                product.Name = entity.Name;
                product.Code = entity.Code;
                product.MetaTitle = entity.MetaTitle;
                product.Description = entity.Description;
                product.Detail = entity.Detail;
                product.Image = entity.Image;
                product.ListType = entity.ListType;
                product.ListFile = entity.ListFile;
                product.CategoryID = entity.CategoryID;

                DataProvider.Ins.DB.SaveChanges();
                return true;

            }
            catch (Exception)
            {
                return false;

            }
        }
        public List<Product> ListAllProduct()
        {
            return DataProvider.Ins.DB.Products.Where(x => x.Status == true).OrderByDescending(x => x.ID).ToList();
        }

    }
}
