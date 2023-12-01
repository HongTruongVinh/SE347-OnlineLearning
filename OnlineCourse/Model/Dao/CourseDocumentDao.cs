using Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Dao
{
    public class CourseDocumentDao
    {
        public CourseDocumentDao() { }

        public List<CourseDocument> GetListDocumentInfor(int productId)
        {
            List<CourseDocument> coursedocuments = new List<CourseDocument>();

            coursedocuments = DataProvider.Ins.DB.CourseDocuments.Where(x => x.productID == productId).ToList();

            return coursedocuments;
        }

        public bool AddCourseDocument(CourseDocument courseDocument)
        {
            try
            {
                DataProvider.Ins.DB.CourseDocuments.Add(courseDocument);
                DataProvider.Ins.DB.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool UpdateCourseDocument(CourseDocument entity)
        {
            try
            {
                var Document = DataProvider.Ins.DB.CourseDocuments.Find(entity.ID);
                Document.Title = entity.Title;
                Document.DateUpdate = DateTime.Now;
                Document.Link = entity.Link;

                DataProvider.Ins.DB.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool DeleteCourseDocument(int courseDocumentId)
        {
            try
            {
                DataProvider.Ins.DB.CourseDocuments.Remove(DataProvider.Ins.DB.CourseDocuments.Where(x => x.ID == courseDocumentId).SingleOrDefault());
                DataProvider.Ins.DB.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
