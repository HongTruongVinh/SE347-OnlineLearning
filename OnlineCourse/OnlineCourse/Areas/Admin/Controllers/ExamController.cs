using Model.Dao;
using Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlineCourse.Areas.Admin.Controllers
{
    public class ExamController : BaseController
    {
        public ActionResult Index(string searchString, int page = 1, int pageSize = 200)
        {
            var dao = new ExamDao();
            var model = dao.ListAllPaging(searchString, page, pageSize);
            ViewBag.SearchString = searchString;
            SetViewBag();
            return View(model);
        }
        public void SetViewBag(long? selectedId = null)
        {
            var dao = new ProductDao();
            ViewBag.ProductList = dao.ListAllProduct();
        }
        [HttpDelete]
        public ActionResult Delete(int id)
        {
            new ExamDao().Delete(id);
            return RedirectToAction("Index");
        }
        [HttpPost]
        public JsonResult AddExamAjax(string name,string metatitle,  string code, string questionlist, string answerlist,string productid,
            string startdate, string enddate,string totalscore,string time,string totalquestion,string questionessay, string userlist,string scorelist)
        {
            try
            {
                var dao = new ExamDao();
                Exam exam = new Exam();

                exam.Name = name;

                exam.MetaTitle = metatitle;
                exam.Code = code;
                exam.QuestionList = questionlist;
                exam.AnswerList = answerlist;
                exam.ProductID = Convert.ToInt16(productid);
                exam.StartDate = Convert.ToDateTime(startdate);
                exam.EndDate = Convert.ToDateTime(enddate);
                exam.TotalScore = Convert.ToInt16(totalscore);
                exam.Time = Convert.ToInt16(time);
                exam.TotalQuestion = Convert.ToInt16(totalquestion);
                exam.QuestionEssay = questionessay;
                exam.UserList = userlist;
                exam.ScoreList = scorelist;
                exam.Type = "1";
                exam.Status = true;

                long id = dao.Insert(exam);
                if (id > 0)
                {
                    return Json(new { status = true });
                }
                else
                {
                    return Json(new { status = false });
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
        public JsonResult UpdateExamAjax(string id, string name, string code, string metatitle, string questionlist, string answerlist, string productid,
            string startdate, string enddate, string totalscore, string time, string totalquestion, string questionessay, string userlist, string scorelist)
        {
            try
            {
                var dao = new ExamDao();
                Exam exam = new Exam();

                exam = dao.ViewDetail(Convert.ToInt16(id));


                exam.Name = name;

                exam.MetaTitle = metatitle;
                exam.Code = code;
                exam.QuestionList = questionlist;
                exam.AnswerList = answerlist;
                exam.ProductID = Convert.ToInt16(productid);
                exam.StartDate = Convert.ToDateTime(startdate);
                exam.EndDate = Convert.ToDateTime(enddate);
                exam.TotalScore = Convert.ToInt16(totalscore);
                exam.Time = Convert.ToInt16(time);
                exam.TotalQuestion = Convert.ToInt16(totalquestion);
                exam.QuestionEssay = questionessay;
                exam.UserList = userlist;
                exam.ScoreList = scorelist;                             

                bool editexam = dao.Update(exam);
                if (editexam == true)
                {
                    return Json(new { status = true });
                }
                else
                {
                    return Json(new { status = false });
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
