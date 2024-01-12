using Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Dao
{
    public class VideoExamDao
    {
        public VideoExamDao() { }

        public VideoExam GetById(int examId)
        {
            return DataProvider.Ins.DB.VideoExams.Where(x=>x.ID == examId).FirstOrDefault();
        }

        public List<VideoExam> GetListVideoExamByProductId(int videoId)
        {
            return DataProvider.Ins.DB.VideoExams.Where(x => x.VideoID == videoId).ToList();
        }

        public bool Add(VideoExam entity)
        {
            var result = false;

            try
            {
                var x = DataProvider.Ins.DB.VideoExams.Add(entity);
                DataProvider.Ins.DB.SaveChanges();
                result = true;
            }
            catch
            {
                result = false;
            }

            return result;
        }

        public bool Add(VideoExam entity, List<int> listChosenQuestionId)
        {
            var result = false;

            try
            {
                var resultAddExam = DataProvider.Ins.DB.VideoExams.Add(entity);
                DataProvider.Ins.DB.SaveChanges();

                var exam = DataProvider.Ins.DB.VideoExams.Where(x => x.VideoID == entity.VideoID).FirstOrDefault();

                foreach (var chosenQuestionId in listChosenQuestionId)
                {
                    var exam_question = new Exam_Question() { ExamID = exam.ID, QuestionID = chosenQuestionId };

                    DataProvider.Ins.DB.Exam_Question.Add(exam_question);
                }

                DataProvider.Ins.DB.SaveChanges();
                result = true;
            }
            catch
            {
                result = false;
            }

            return result;
        }

        public Dictionary<ExamQuestion, List<QuestionAnswer>> GetListQuestion(int examId)
        {
            Dictionary<ExamQuestion, List<QuestionAnswer>> result = new Dictionary<ExamQuestion, List<QuestionAnswer>> ();

            var listQuestionId = new List<Exam_Question>();
            listQuestionId = DataProvider.Ins.DB.Exam_Question.Where(x => x.ExamID == examId).ToList();

            foreach (var questionId in listQuestionId) 
            {
                var listQuestion = new List<ExamQuestion>();
                listQuestion = DataProvider.Ins.DB.ExamQuestions.Where(x => x.ID == questionId.QuestionID).ToList();

                foreach (var question in listQuestion)
                {
                    var listAnswer = new List<QuestionAnswer>();
                    listAnswer = DataProvider.Ins.DB.QuestionAnswers.Where(x=>x.QuestionID == question.ID).ToList();
                    result.Add(question, listAnswer);
                }
            }

            return result;
        }
    }
}
