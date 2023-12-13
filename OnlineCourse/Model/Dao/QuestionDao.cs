using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Model.Models;
using PagedList;

namespace Model.Dao
{
    public class QuestionDao
    {
        
        public QuestionDao()
        {
            
        }
        public long Insert(Question entity)
        {
            DataProvider.Ins.DB.Questions.Add(entity);
            DataProvider.Ins.DB.SaveChanges();
            return entity.ID;
        }
        public bool Delete(int id)
        {
            try
            {
                var question = DataProvider.Ins.DB.Questions.Find(id);
                DataProvider.Ins.DB.Questions.Remove(question);
                DataProvider.Ins.DB.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public IEnumerable<Question> ListAllPaging(string searchString, int page, int pagesize)
        {
            IQueryable<Question> model = DataProvider.Ins.DB.Questions;           
            if (!string.IsNullOrEmpty(searchString))
            {
                model = model.Where(x => x.Content.Contains(searchString) || x.Name.Contains(searchString));
            }
            return model.OrderByDescending(x => x.ID).ToPagedList(page, pagesize);
        }
        public Question ViewDetail(int id)
        {

            return DataProvider.Ins.DB.Questions.Find(id);
        }
        public bool Update(Question entity)
        {
            try
            {
                var question = DataProvider.Ins.DB.Questions.Find(entity.ID);
                question.Name = entity.Name;
                question.Content = entity.Content;
                question.Answer = entity.Answer;
                question.ProductID = entity.ProductID;          
                DataProvider.Ins.DB.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;

            }
        }
        public List<Question> ListExamQuestion(string pList)
        {
            return DataProvider.Ins.DB.Questions.Where(x => pList.Contains("*" + x.ID.ToString() + "*")).ToList();
        }
    }

}
