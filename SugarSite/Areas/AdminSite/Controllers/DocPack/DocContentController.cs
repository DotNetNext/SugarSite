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
    public class DocContentController : BaseController
    {
        //DC表示 DocContent(文章主表)
        public DocContentController(DbService s) : base(s) { }

        #region Admin Page
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult PageDCAdd()
        {
            return View();
        } 
        #endregion

        #region Admin Api
        [HttpPost]
        public JsonResult DC_Submit(DocContent content)
        {
            var model = new ResultModel<bool>();
            _service.Command<LoginOutsourcing>((db, o) =>
            {
                db.Insert(o);
            });
            return Json(model);
        }
        public JsonResult DC_Delete(int [] ids)
        {
            var model = new ResultModel<bool>();
            _service.Command<LoginOutsourcing>((db, o) =>
            {
                db.FalseDelete<DocContent,int>("id",ids);
            });
            return Json(model);
        } 
        #endregion
    }
}