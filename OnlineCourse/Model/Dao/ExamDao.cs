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
        public Exam ViewDetail(int id)
        {

            return DataProvider.Ins.DB.Exams.Find(id);
        }
        public bool Update(Exam entity)
        {
            try
            {
                var exam = DataProvider.Ins.DB.Exams.Find(entity.ID);
                exam.Name = entity.Name;
                exam.Code = entity.Code;
                exam.MetaTitle = entity.MetaTitle;
                exam.QuestionList = entity.QuestionList ;
                exam.AnswerList = entity.AnswerList;
                exam.ProductID = entity.ProductID;
                exam.StartDate = entity.StartDate;
                exam.EndDate = entity.EndDate;
                exam.TotalScore = entity.TotalScore;
                exam.Time = entity.Time;
                exam.TotalQuestion = entity.TotalQuestion;
                exam.QuestionEssay = entity.QuestionEssay;
                exam.UserList = entity.UserList;
                exam.ScoreList = entity.ScoreList;

                DataProvider.Ins.DB.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;

            }
        }
        public List<Exam> ListAllExam()
        {
            return DataProvider.Ins.DB.Exams.Where(x => x.Status == true).OrderByDescending(x => x.ID).ToList();
        }
        public List<Exam> ListByType(string searchString, string Type)
        {
            IOrderedQueryable<Exam> model = DataProvider.Ins.DB.Exams;
            if (Type == "0")
            {
                if (!string.IsNullOrEmpty(searchString))
                    return model.Where(x => x.Name.ToString().Contains(searchString)).Where(x => (bool)x.Status).OrderByDescending(x => x.StartDate).ToList();
                else
                {
                    return model.Where(x => (bool)x.Status).OrderByDescending(x => x.StartDate).ToList();
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(searchString))
                {
                    model =  model.Where(x => x.Name.ToString().Contains(searchString)).Where(x => (bool)x.Status).OrderByDescending(x => x.StartDate);
                    return model.Where(x=>x.Type == Type).ToList();
                }
                return DataProvider.Ins.DB.Exams.Where(x => x.Type == Type).ToList();
            }
             
        }

        public List<Exam> ListExamOfUser(int userId)
        {
            List<Exam> exams = new List<Exam>();

            WishProductDao ownProductDao = new WishProductDao();

            var userCourses = ownProductDao.GetListWishProduct(userId);

            foreach(var exam  in DataProvider.Ins.DB.Exams)
            {
                foreach (var course in userCourses)
                {
                    if (exam.ProductID == course.ID)
                    {
                        exams.Add(exam);
                    }
                }
            }

            return exams;
        }

    }
}
