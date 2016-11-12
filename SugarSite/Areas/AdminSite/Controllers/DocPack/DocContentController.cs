using Infrastructure.Dao;
using Infrastructure.DbModel;
using Infrastructure.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SqlSugar;
namespace SugarSite.Areas.AdminSite.Controllers
{
    public class DocContentController : BaseController
    {
        //DC表示 DocContent(文章主表)
        public DocContentController(DbService s) : base(s) { }

        #region Admin Page
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult PageDCAdd(int id=0)
        {
            ViewBag.isAdd = id == 0;
            return View();
        }
        #endregion

        #region Admin Api
        [HttpPost]
        [ValidateInput(false)]
        public JsonResult DC_Submit(DocContent obj)
        {
            var model = new ResultModel<object>();
            _service.Command<LoginOutsourcing>((db, o) =>
            {
                var isAdd = obj.Id == 0;
                if (isAdd)
                {
                    model.ResultInfo = new { id = db.Insert(obj) };
                }
                else
                {
                    db.AddDisableUpdateColumn();
                    db.Update(obj);
                    model.ResultInfo = new { id = obj.Id };
                }
            });
            return Json(model);
        }
        public JsonResult DC_Delete(int[] ids)
        {
            var model = new ResultModel<bool>();
            _service.Command<LoginOutsourcing>((db, o) =>
            {
                db.FalseDelete<DocContent, int>("id", ids);
            });
            return Json(model);
        }

        public JsonResult DC_Single(int id)
        {
            var model = new ResultModel<DocContent>();
            _service.Command<LoginOutsourcing>((db, o) =>
            {
                model.ResultInfo=db.Queryable<DocContent>().InSingle(id);
            });
            return Json(model,JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}