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

        public Dictionary<string, string> GetListResultExamOfUser(long userId)
        {
            Dictionary<string, string> listExamScore = new Dictionary<string, string>();

            List<Result> results = new List<Result> ();
            results = DataProvider.Ins.DB.Results.Where(x =>x.UserID == userId).ToList();
            
            foreach (Result result in results)
            {
                Exam exam = DataProvider.Ins.DB.Exams.Where(x => x.ID == result.ExamID).SingleOrDefault();
                listExamScore.Add(exam.Name, result.Score);
            }

            return listExamScore;
        } 
    }
}
