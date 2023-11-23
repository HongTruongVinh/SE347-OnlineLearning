using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Model.Models;
using PagedList;


namespace Model.Dao
{
    public class ExamDao
    {
        
        public ExamDao()
        {
            
        }
        public long Insert(Exam entity)
        {
            DataProvider.Ins.DB.Exams.Add(entity);
            DataProvider.Ins.DB.SaveChanges();
            return entity.ID;
        }
        public bool Delete(int id)
        {
            try
            {
                var exam = DataProvider.Ins.DB.Exams.Find(id);
                DataProvider.Ins.DB.Exams.Remove(exam);
                DataProvider.Ins.DB.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public IEnumerable<Exam> ListAllPaging(string searchString, int page, int pagesize)
        {
            IQueryable<Exam> model = DataProvider.Ins.DB.Exams;
            if (!string.IsNullOrEmpty(searchString))
            {
                model = model.Where(x => x.ProductID.ToString().Contains(searchString) || x.Name.Contains(searchString));
            }
            return model.OrderByDescending(x => x.ID).ToPagedList(page, pagesize);
        }
    }
}
