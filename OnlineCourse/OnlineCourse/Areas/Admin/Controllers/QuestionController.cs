using Model.Dao;
using Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlineCourse.Areas.Admin.Controllers
{
    public class QuestionController : BaseController
    {
        //
        // GET: /Admin/Question/

        public ActionResult Index(string searchString, int page = 1, int pageSize = 200)
        {
            var dao = new QuestionDao();
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
            new QuestionDao().Delete(id);
            return RedirectToAction("Index");
        }
        [HttpPost]
        public JsonResult AddQuestionAjax(string name, string content, string answer, string productid)
        {
            try
            {
                var dao = new QuestionDao();
                Question question = new Question();


                question.Name = name;
                question.Content = content;
                question.Answer = answer;
                question.ProductID = Convert.ToInt16(productid);
                question.Type = "1";              
                question.Status = true;

                long id = dao.Insert(question);
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
        public JsonResult UpdateQuestionAjax(string id,string name, string content, string answer, string productid)
        {
            try
            {
                var dao = new QuestionDao();
                Question question = new Question();
                question = dao.ViewDetail(Convert.ToInt16(id));
                question.Name = name;
                question.Content = content;
                question.Answer = answer;
                question.ProductID = Convert.ToInt16(productid);               
                question.Status = true;

                bool editquestion = dao.Update(question);
                if (editquestion == true)
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
