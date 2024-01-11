using Model.Dao;
using Model.Models;
using OnlineCourse.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Results;
using System.Web.Mvc;

namespace OnlineCourse.Controllers
{
    public class ExamController : BaseController
    {
        static int _productId;
        static int _playingVideoId;

        //
        // GET: /Exam/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Category(string searchString, string Type)
        {
            ViewBag.Category = Type;
            ViewBag.CategoryID = new ProductCategoryDao().ListAll();
            var model = new ExamDao().ListByType(searchString, Type);
            return View(model);
        }
        public ActionResult Detail(long id)
        {
            try
            {
                var dao = new ExamDao().ViewDetail(Convert.ToInt16(id));
                ViewBag.ExamQuestion = new QuestionDao().ListExamQuestion(dao.QuestionList);
                var session = (UserLogin)Session[CommonConstants.USER_SESSION];
                ViewBag.Result = new ResultDao().GetByUserExamID(session.UserID, dao.ID);

                ViewBag.Msnv = session.UserName;
                ViewBag.UserID = session.UserID;

                //if(!dao.UserList.Contains("*" +session.UserID.ToString() + "*"))
                //{
                //    return Redirect("/trang-chu");
                //}
                return View(dao);

            }
            catch
            {
                return View();
            }
        }

        [HttpPost]
        public JsonResult AddResult(long examid,long userid)
        {
            try
            {
                var dao = new ResultDao();
                Result result = new Result();

                result.ExamID = examid;
                result.UserID = userid;
                result.Status = false;
                result.ResultQuiz = "";
                result.ResultEssay = "";
                result.StartDateQuiz = DateTime.Now.ToShortDateString();
                result.StartTimeQuiz = DateTime.Now.Hour.ToString() + ":" + DateTime.Now.Minute;


                Exam exam = new ExamDao().ViewDetail((int)examid);
                var x = exam.QuestionList.Split('*');
                int totalQuestion = 0;
                foreach (var item in x)
                {
                    if(item != "")
                    {
                        totalQuestion += 1;
                    }
                }
                Random random = new Random();
                int score = (100/totalQuestion)*random.Next(1, totalQuestion);
                result.Score = score.ToString();


                bool addresult = dao.Insert(result);
                if(addresult == true)
                {

                    return Json(new
                        {
                            status = true
                        });                       
                }
                else
                {
                    return Json(new
                    {
                        status = false
                    });        
                }
            }
            catch
            {
                return Json(new
                {
                    status = false
                });        
            }
        }

        [HttpPost]
        public JsonResult UpdateResult(long examid, long userid, string resultessay, string resultquiz)
        {
            try
            {
                var dao = new ResultDao();
                Result result = new Result();

                result.ExamID = examid;
                result.UserID = userid;
                result.Status = true;
                result.ResultQuiz = resultquiz;
                result.ResultEssay = resultessay;
                result.FinishTimeEssay = DateTime.Now.Hour.ToString() + ":" + DateTime.Now.Minute;
                result.FinishTimeQuiz = DateTime.Now.Hour.ToString() + ":" + DateTime.Now.Minute;
                 
                bool addresult = dao.Update(result);
                if (addresult == true)
                {

                    return Json(new
                    {
                        status = true
                    });
                }
                else
                {
                    return Json(new
                    {
                        status = false
                    });
                }
            }
            catch
            {
                return Json(new
                {
                    status = false
                });
            }
        }

        [HttpGet]
        public ActionResult DoExam(int examId, int productId, int playingIdVideo)
        {
            _productId = productId;
            _playingVideoId = playingIdVideo;

            ViewBag.ProductId = productId;
            ViewBag.PlayingIdVideo = playingIdVideo;

            try
            {
                var exam = new VideoExamDao().GetById(examId);
                ViewBag.ListExamQuestion = new VideoExamDao().GetListQuestion(examId);
                Exam = ViewBag.ListExamQuestion;

                var session = (UserLogin)Session[CommonConstants.USER_SESSION];
                ViewBag.Result = new ResultDao().GetByUserExamID(session.UserID, exam.ID);

                ViewBag.Msnv = session.UserName;
                ViewBag.UserID = session.UserID;

                return View(exam);

            }
            catch
            {
                return View();
            }
        }

        static Dictionary<ExamQuestion, List<QuestionAnswer>> Exam;

        static List<UserAnswer> UserAnswers = new List<UserAnswer>();

        [HttpPost]
        public ActionResult SaveAnswer(string chosenAnswer)
        {

            try
            {

                int questinIndex = int.Parse(chosenAnswer.Substring(0, 1)) - 1;//mảng của Exam bắt đầu từ 0 nhưng mảng từ view gửi về thì bắt
                                                                                // đầu từ 1 nên phải trừ đi 1 từ chỗ này.
                string answer = chosenAnswer.Substring(1, 1);

                //var questionId = ((Dictionary<ExamQuestion, List<QuestionAnswer>>)Exam).Keys.ToList()[questinIndex];

                var question = ((Dictionary<ExamQuestion, List<QuestionAnswer>>)Exam).ElementAt(questinIndex);

                int answerIndex = 0;
                if (answer == "A")
                {
                    answerIndex = 0;
                }
                else if (answer == "B")
                {
                    answerIndex = 1;
                }
                else if (answer == "C")
                {
                    answerIndex = 2;
                }
                else if (answer == "D")
                {
                    answerIndex = 3;
                }

                int chosenAnswerId = question.Value.ElementAt(answerIndex).ID;

                if (UserAnswers.Where(x => x.Question.ID == question.Key.ID).FirstOrDefault() == null)
                {
                    UserAnswers.Add(new UserAnswer(question.Key, question.Value, chosenAnswerId));
                }
                else
                {
                    UserAnswers.Remove(UserAnswers.Where(x => x.Question.ID == question.Key.ID).FirstOrDefault());
                    UserAnswers.Add(new UserAnswer(question.Key, question.Value, chosenAnswerId));
                }

                bool result = true;
                if (result == true)
                {

                    return Json(new
                    {
                        status = true
                    });
                }
                else
                {
                    return Json(new
                    {
                        status = false
                    });
                }
            }
            catch
            {
                return Json(new
                {
                    status = false
                });
            }
        }

        [HttpPost]
        public ActionResult GetResultExam(int examId)
        {
            try
            {
                int numberTrueQuestion = UserAnswers.Where(x=>x.IsTrueAnwser == true).ToList().Count();
                int score = (100 / Exam.Count) * numberTrueQuestion;

                ViewBag.ProductId = _productId;
                ViewBag.PlayingVideoId = _playingVideoId;

                bool addresult = true;
                if (addresult == true)
                {

                    return Json(new
                    {
                        status = true,
                        score = score,
                    });
                }
                else
                {
                    return Json(new
                    {
                        status = false
                    });
                }
            }
            catch
            {
                return Json(new
                {
                    status = false
                });
            }
        }

        public ActionResult ViewAnswer(int examId)
        {
            var exam = new VideoExamDao().GetById(examId);
            ViewBag.ListExamQuestion = new VideoExamDao().GetListQuestion(examId);
            Exam = ViewBag.ListExamQuestion;

            var session = (UserLogin)Session[CommonConstants.USER_SESSION];
            ViewBag.Result = new ResultDao().GetByUserExamID(session.UserID, exam.ID);

            ViewBag.Msnv = session.UserName;
            ViewBag.UserID = session.UserID;

            ViewBag.UserAnswers = UserAnswers;
            ViewBag.ProductId = _productId;
            ViewBag.PlayingVideoId = _playingVideoId;

            return View(exam);
        }
    }
}
