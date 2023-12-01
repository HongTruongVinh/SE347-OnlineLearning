using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Model.Models;
using PagedList;

namespace Model.Dao
{
    public class ResultDao
    {
       
        public ResultDao()
        {
            
        }
        public Result GetByUserExamID(long UserID, long ExamID)
        {
            return DataProvider.Ins.DB.Results.SingleOrDefault(x=>x.UserID == UserID && x.ExamID == ExamID);
        }
        public bool Insert(Result entity)
        {
            DataProvider.Ins.DB.Results.Add(entity);
            DataProvider.Ins.DB.SaveChanges();
            return true;
        }
        public bool Update(Result entity)
        {
            try
            {
                var result = GetByUserExamID(entity.UserID,entity.ExamID);
                result.ResultQuiz = entity.ResultQuiz;
                result.ResultEssay = entity.ResultEssay;
                result.FinishTimeEssay = entity.FinishTimeEssay;
                result.FinishTimeQuiz = entity.FinishTimeQuiz;

                DataProvider.Ins.DB.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;

            }
        }

        
    }
}
