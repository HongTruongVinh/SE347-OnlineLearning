using Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Dao
{
    public class ExamQuestionDao
    {
        public ExamQuestionDao()
        {

        }

        public bool Add(ExamQuestion examQuestion, string answer1, string answer2, string answer3, string answer4, string trueAnswer, int videoId)
        {
            bool result = false;

            int _trueAswer = int.Parse(trueAnswer);

            try
            {
                var courseVideo = DataProvider.Ins.DB.CourseVideos.Where(x => x.ID == videoId).FirstOrDefault();

                examQuestion.ProductID = courseVideo.productID.GetValueOrDefault();

                //if (courseVideo.productID == null)
                //    examQuestion.ProductID = default(int);
                //else
                //    examQuestion.ProductID = courseVideo.productID.Value;

                var resultQuestion = DataProvider.Ins.DB.ExamQuestions.Add(examQuestion);

                DataProvider.Ins.DB.SaveChanges();

                var question = DataProvider.Ins.DB.ExamQuestions.ToList().LastOrDefault();

                QuestionAnswer entityAnswer1 = new QuestionAnswer() { QuestionID = question.ID, Content = answer1, IsTrueAnswer = false };
                QuestionAnswer entityAnswer2 = new QuestionAnswer() { QuestionID = question.ID, Content = answer2, IsTrueAnswer = false };
                QuestionAnswer entityAnswer3 = new QuestionAnswer() { QuestionID = question.ID, Content = answer3, IsTrueAnswer = false };
                QuestionAnswer entityAnswer4 = new QuestionAnswer() { QuestionID = question.ID, Content = answer4, IsTrueAnswer = false };

                if (_trueAswer == 1)
                {
                    entityAnswer1.IsTrueAnswer = true;
                }
                else if(_trueAswer == 2)
                {
                    entityAnswer2.IsTrueAnswer = true;
                }
                else if (_trueAswer == 3)
                {
                    entityAnswer3.IsTrueAnswer = true;
                }
                else 
                {
                    entityAnswer4.IsTrueAnswer = true;
                }

                DataProvider.Ins.DB.QuestionAnswers.Add(entityAnswer1);
                DataProvider.Ins.DB.QuestionAnswers.Add(entityAnswer2);
                DataProvider.Ins.DB.QuestionAnswers.Add(entityAnswer3);
                DataProvider.Ins.DB.QuestionAnswers.Add(entityAnswer4);

                DataProvider.Ins.DB.SaveChanges();
                result = true;
            }
            catch
            {
                result = false;
            }
            return result;
        }

        public ExamQuestion GetByContent(string content)
        {
            return DataProvider.Ins.DB.ExamQuestions.Where(x => x.Content==content).FirstOrDefault();
        }

        public List<ExamQuestion> GetListQuetionByCourseId(int courseId)
        {
            return DataProvider.Ins.DB.ExamQuestions.Where(x => x.ProductID == courseId).ToList();
        }

        public Dictionary<ExamQuestion, List<QuestionAnswer>> GetDictionaryQuetionByVideoId(int videoId)
        {
            int courseId = DataProvider.Ins.DB.CourseVideos.Where(x => x.ID == videoId).FirstOrDefault().productID.GetValueOrDefault();

            Dictionary <ExamQuestion, List<QuestionAnswer>> result = new Dictionary<ExamQuestion, List<QuestionAnswer>> ();

            var listQuest = new List<ExamQuestion>();
            listQuest = DataProvider.Ins.DB.ExamQuestions.Where(x => x.ProductID == courseId).ToList();

            foreach ( var quest in listQuest )
            {
                var listAnswer = new List<QuestionAnswer>();
                listAnswer = DataProvider.Ins.DB.QuestionAnswers.Where(x => x.QuestionID == quest.ID).ToList();

                result.Add(quest, listAnswer);
            }

            return result;
        }

        public bool isTrueAnswer(int questionId, int answerId)
        {
            bool result = false;
            try
            {
                var answer = DataProvider.Ins.DB.QuestionAnswers.Where(x=>x.ID == answerId).FirstOrDefault();
                if ( answer.IsTrueAnswer == true )
                {
                    result = true;
                }
                else
                {
                    result = false;
                }

                return result;
            }
            catch
            {
                return result;
            }
        }
    }
}
