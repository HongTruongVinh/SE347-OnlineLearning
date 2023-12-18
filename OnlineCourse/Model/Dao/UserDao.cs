using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Model.Models;
using PagedList;


namespace Model.Dao
{
    public class UserDao
    {
       
        public UserDao()
        {
            
        }
        public long Insert(User entity)
        {
            DataProvider.Ins.DB.Users.Add(entity);
            DataProvider.Ins.DB.SaveChanges();
            return entity.ID;
        }
        public bool Update(User entity)
        {
            try
            {
                var user = DataProvider.Ins.DB.Users.Find(entity.ID);
                user.Name = entity.Name;
                if(!string.IsNullOrEmpty(entity.Password))
                {
                    user.Password = entity.Password;
                }
                user.Address = entity.Address;
                user.Email = entity.Email;
                user.ModifiedBy = entity.ModifiedBy;
                user.ModifiedDate = entity.ModifiedDate;
                user.Status = entity.Status;
                user.Phone = entity.Phone;
                DataProvider.Ins.DB.SaveChanges();
                return true;

            }
            catch(Exception)
            {
                return false;

            }
        }
        public bool Delete(int id)
        {
            try
            {
                var user = DataProvider.Ins.DB.Users.Find(id);
                DataProvider.Ins.DB.Users.Remove(user);
                DataProvider.Ins.DB.SaveChanges();
                return true;
            }
            catch(Exception)
            {
                return false;
            }
        }
        public User GetByUserName(string username)
        {
            return DataProvider.Ins.DB.Users.SingleOrDefault(x => x.UserName == username);
        }
        public IEnumerable<User> ListAllPaging(string searchString, int page, int pagesize)
        {
            IQueryable<User> model = DataProvider.Ins.DB.Users;
            model = model.Where(x=>x.UserName != "admin");
            if (!string.IsNullOrEmpty(searchString))
            {
                model = model.Where(x => x.UserName.Contains(searchString) || x.Name.Contains(searchString));
            }
            return model.OrderByDescending(x => x.CreateDate).ToPagedList(page, pagesize);
        }
        public User ViewDetail(int id)
        {

             return DataProvider.Ins.DB.Users.Find(id);                
        }
        public int Login(string userName, string passWord,bool isLoginAdmin = false)
        {
            var result = DataProvider.Ins.DB.Users.SingleOrDefault(x => x.UserName == userName);
            if (result == null)
                return 0;
            else
            {
                if(isLoginAdmin == true)
                {
                    if(result.Status == false)
                    {
                        return -1;
                    }
                    else
                    {
                        if (result.Password == passWord)
                            return 1;
                        else
                            return -2;
                    }
                }
                else
                {
                    if(result.Status == false)
                    {
                        return -1;
                    }
                    else
                    {
                        if (result.Password == passWord)
                            return 1;
                        else
                            return -2;
                    }
                }
            }
        }

        public bool isAdminRole(int UserId)
        {
            foreach (var item in DataProvider.Ins.DB.User_Role)
            {
                if (item.idUser == UserId && (item.idRole == 5 || item.idRole == 6 || item.idRole == 7))
                {
                    return true;
                }
            }

            return false;
        }

        public bool isTeacherRole(int UserId)
        {
            foreach (var item in DataProvider.Ins.DB.User_Role)
            {
                if (item.idUser == UserId && (item.idRole == 3 || item.idRole == 4))
                {
                    return true;
                }
            }

            return false;
        }

        public bool isLearnerRole(int UserId)
        {
            foreach (var item in DataProvider.Ins.DB.User_Role)
            {
                if (item.idUser == UserId && (item.idRole == 1 || item.idRole == 2))
                {
                    return true;
                }
            }

            return false;
        }

        public IEnumerable<User> ListAllPagingPoster(string searchString, int page, int pagesize)
        {
            IQueryable<User> model = DataProvider.Ins.DB.Users;
            IQueryable<Product> products = DataProvider.Ins.DB.Products;
            model = model.Where(x => x.UserName != "admin");
            if (!string.IsNullOrEmpty(searchString))
            {
                model = model.Where(x => x.UserName.Contains(searchString) || x.Name.Contains(searchString));
            }

            model = model.Where(item =>
                products.Where(product => (product.CreateBy == item.ID.ToString())).Count() > 0
            );

            return model.OrderByDescending(x => x.CreateDate).ToPagedList(page, pagesize);
        }

    
    }
}
