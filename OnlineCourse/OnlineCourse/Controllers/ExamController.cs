using Model.Dao;
using Model.Models;
using OnlineCourse.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlineCourse.Controllers
{
    public class ExamController : BaseController
    {
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



    
    }
}
