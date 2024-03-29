﻿using System;
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
        public IEnumerable<Product> ListAllPaging(long cateID, string searchString, int page, int pagesize, int isApproved = -1)
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
            if (isApproved == 0)
            {
                model = model.Where(x => x.Status == false);
            }
            else if (isApproved == 1)
            {
                model = model.Where(x => x.Status == true);
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

        public bool updateStatus(int id, bool status)
        {
            try
            {
                var product = DataProvider.Ins.DB.Products.Find(id);
                product.Status = status;

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
            entity.Status = true;
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
                product.Status = entity.Status;

                DataProvider.Ins.DB.SaveChanges();
                return true;

            }
            catch (Exception)
            {
                return false;

            }
        }
        public List<Product> ListAllProduct(int isApproved = -1)
        {
            if (isApproved == 0)
                return DataProvider.Ins.DB.Products.Where(x => x.Status == false).OrderByDescending(x => x.ID).ToList();
            else if (isApproved == 1)
                return DataProvider.Ins.DB.Products.Where(x => x.Status == true).OrderByDescending(x => x.ID).ToList();
            return DataProvider.Ins.DB.Products
                .OrderByDescending(x => x.ID)
                .Where(x => (bool)x.Status == true).ToList();


        }
        public List<Product> ListByCategoryID(
            string searchString, string searchTeacherName, long CategoryID,
            int page, int itemPerPage,
            string order,
            int minPrice,
            int maxPrice,
            int isapproved = -1
        )
        {

            if (page < 1) page = 1;
            IOrderedQueryable<Product> model = DataProvider.Ins.DB.Products;
            IOrderedQueryable<User> users = DataProvider.Ins.DB.Users;

            List<Product> result = new List<Product>();

            // lấy theo loại khóa học
            if (CategoryID > 0)
                result = model.Where(x => x.CategoryID == CategoryID).ToList();
            else
                result = model.ToList();

            // tìm kiếm theo chuổi
            if (!string.IsNullOrEmpty(searchString))
            {
                result = result.Where(x =>
                            // điều kiện theo tên khóa học
                            x.Name.ToLower().Contains(searchString.ToLower()) ||
                            // điều kiện theo tên người đăng
                            (users.Where(user => (
                                user.ID.ToString() == x.CreateBy &&
                                user.Name.Contains(searchString.ToLower())
                            )).Count() > 0)
                         ).ToList();
            }

            // sắp xếp mới nhất cũ nhất
            if (order == "1")
                result = result.OrderByDescending(x => x.CreateDate).ToList();
            else
                result = result.OrderBy(x => x.CreateDate).ToList();

            // lấy theo giá
            result = result.Where(x => x.Price >= minPrice).ToList();
            result = result.Where(x => x.Price <= maxPrice).ToList();

            // phân trang
            result = result.Skip((page - 1) * itemPerPage).Take(itemPerPage).ToList();

            /*
            if (isapproved == 0)
            {
                result = result.Where(x => (bool)x.Status == false).ToList();
            }
            else if (isapproved == 1)
            {
                result = result.Where(x => (bool)x.Status == true).ToList();
            }
            */

            result = result.Where(x => (bool)x.Status == true).ToList();

            return result;
        }

        public int CountByCategoryID(string searchString, long CategoryID)
        {
            IOrderedQueryable<Product> model = DataProvider.Ins.DB.Products;
            if (CategoryID == 0)
            {
                if (!string.IsNullOrEmpty(searchString))
                {
                    return model.Where(x => x.Name.Contains(searchString) || x.Description.Contains(searchString)).Where(x => (bool)x.Status).Count();
                }
                else
                {
                    return model.Where(x => (bool)x.Status).Count();
                }

            }
            else
            {
                if (!string.IsNullOrEmpty(searchString))
                {
                    return model.Where(x => x.Name.Contains(searchString) || x.Description.Contains(searchString)).Where(x => (bool)x.Status && x.CategoryID == CategoryID).Count();
                }
                else
                {
                    return model.Where(x => (bool)x.Status && x.CategoryID == CategoryID).Count();
                }
            }
        }

        public User GetCreatedByUser(int userId)
        {
            User user = DataProvider.Ins.DB.Users.Where(x => x.ID.Equals(userId)).FirstOrDefault();
            return user;
        }

        public int GetCountComment(long productId)
        {
            return DataProvider.Ins.DB.Comments.Where(x => x.ProductID == productId).ToList().Count();
        }

        public int GetCountLearner(long productId)
        {
            return DataProvider.Ins.DB.WishProducts.Where(x => x.ProductID == productId).ToList().Count();
        }

        public bool BuyProduct(int userId, int productId)
        {
            bool result = false;
            try
            {
                DataProvider.Ins.DB.WishProducts.Where(x => x.ProductID == productId && x.UserID == userId).SingleOrDefault().IsBought = true;
                DataProvider.Ins.DB.SaveChanges();
                return true;
            }
            catch
            {
                return result;
            }
        }

        public bool AddProductToCart(int userId, int productId)
        {
            try
            {
                WishProduct wishProduct = new WishProduct() { ProductID = productId, UserID = userId, IsBought = false };
                DataProvider.Ins.DB.WishProducts.Add(wishProduct);
                DataProvider.Ins.DB.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool DeleteProductFromCart(int userId, int productId)
        {
            try
            {
                WishProduct wishProduct = DataProvider.Ins.DB.WishProducts.Where(x => x.ProductID == productId && x.UserID == userId).SingleOrDefault();
                DataProvider.Ins.DB.WishProducts.Remove(wishProduct);
                DataProvider.Ins.DB.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }



        public Dictionary<string, bool> GetWishListProduct(int userId)
        {
            Dictionary<string, bool> lisId = new Dictionary<string, bool>();
            foreach (var item in DataProvider.Ins.DB.WishProducts.Where(x => x.UserID == userId))
            {
                if (item.IsBought == true)
                {
                    lisId.Add(item.ProductID.ToString(), true);
                }
                else
                {
                    lisId.Add(item.ProductID.ToString(), false);
                }
            }
            return lisId;
        }

        public List<Product> getByUserId(int userId)
        {
            IQueryable<Product> model = DataProvider.Ins.DB.Products;
            model = model.Where(x => x.CreateBy == userId.ToString());
            return model
                .OrderByDescending(x => x.CreateDate)
                .Where(x => (bool)x.Status == true).ToList();
        }

        public List<Product> getListRecommend(Product product)
        {
            IOrderedQueryable<Product> model = DataProvider.Ins.DB.Products;

            List<Product> result = new List<Product>();
            result = model.ToList();

            result = result
                .Where(x => (bool)x.Status == true)
                .Where(x => checkIsSuitable(x.Name, product.Name) || x.CategoryID == product.CategoryID)
                .OrderBy(x => Guid.NewGuid())
                .Take(4).ToList();

            // sắp xếp mới nhất cũ nhất
            result = result.OrderByDescending(x => x.CreateDate).ToList();

            return result;
        }

        private bool checkIsSuitable(String name1, string name2)
        {
            string[] list1 = name1.Split('#');
            foreach (var item in list1)
            {
                if (name2.Contains(item))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
