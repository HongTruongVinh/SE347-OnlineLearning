using Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Dao
{
    public class GetInforDao
    {
        public GetInforDao() { }

        public HomeInfor GetHomeInfor()
        {
            return new HomeInfor() { 
                CountProduct = DataProvider.Ins.DB.Products.Count(), 
                CountStudent = DataProvider.Ins.DB.WishProducts.Where(x => x.IsBought == true).GroupBy(x => x.UserID).Count(), //user are students
                CountTeacher = DataProvider.Ins.DB.Products.GroupBy(x => x.CreateBy).Count(), //user are teachers
                CountCertification = new Random().Next(DataProvider.Ins.DB.Products.Count())
            };
        }

        public HomeDashboardInfor GetHomeDashboardInfor()
        {
            return new HomeDashboardInfor()
            {
                CountProduct = DataProvider.Ins.DB.Products.Count(),
                CountLearner = DataProvider.Ins.DB.WishProducts.Where(x => x.IsBought == true).GroupBy(x => x.UserID).Count(), //user are students
                CountUser = DataProvider.Ins.DB.Users.Count()
            };
        }
    }

    public class HomeInfor
    {
        public int CountTeacher { get; set; }
        public int CountProduct { get; set; }
        public int CountStudent { get; set; }
        public int CountCertification { get; set; }
        public HomeInfor() { }
    }

    public class HomeDashboardInfor
    {
        public int CountProduct { get; set; }
        public int CountLearner { get; set; }
        public int CountUser { get; set; }
        public HomeDashboardInfor() { }
    }
}
