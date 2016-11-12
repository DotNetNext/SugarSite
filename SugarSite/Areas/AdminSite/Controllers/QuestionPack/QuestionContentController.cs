using Infrastructure.Dao;
using Infrastructure.DbModel;
using Infrastructure.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SugarSite.Areas.AdminSite.Controllers
{
    public class QuestionContentController : BaseController
    {
        public QuestionContentController(DbService s) : base(s) { }

        #region Admin Page
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult AddPage()
        {
            return View();
        } 
        #endregion

        #region Admin Api
        [HttpPost]
        public JsonResult SubmitQuestionContent(DocContent content)
        {
            var model = new ResultModel<bool>();
            _service.Command<LoginOutsourcing>((db, o) =>
            {
                db.Insert(o);
            });
            return Json(model);
        }
        public ActionResult Delete()
        {
            return View();
        } 
        #endregion
    }
}