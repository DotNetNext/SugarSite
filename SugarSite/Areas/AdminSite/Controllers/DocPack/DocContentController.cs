using Infrastructure.Dao;
using Infrastructure.DbModel;
using Infrastructure.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SqlSugar;
using Infrastructure.Pub;

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

        public ActionResult PageDCAdd(int id = 0)
        {
            ViewBag.isAdd = id == 0;
            return View();
        }
        #endregion

        #region Admin Api
        public JsonResult Dc_GetList(int typeId = 13, int pageIndex = 1, int pageSize = PubConst.SitePageSize)
        {
            var model = new ResultModel<DocContentResult>();
            _service.Command<LoginOutsourcing>((db, o) =>
            {
                db.CurrentFilterKey = PubConst.FilterKeyFalseDelteJoin;
                int pageCount = 0;
                model.ResultInfo = new DocContentResult();
                model.ResultInfo.PageIndex = pageIndex;
                model.ResultInfo.PageSize = pageSize;
                model.ResultInfo.PageCount = 0;
                model.ResultInfo.DocList = db.Queryable<DocContent>()
                                            .JoinTable<DocType>((m, dt) => m.TypeId == dt.Id)
                                            .OrderBy(m => m.Id, OrderByType.Desc)
                                            .Select<DocType, V_DocContent>((m, dt) => new V_DocContent()
                                            {
                                                Title = m.Title,
                                                TypeName = dt.TypeName,
                                                CreateTime = m.CreateTime,
                                                Creator = m.Creator,
                                                Id= m.Id

                                            }).ToPageList(pageIndex, pageSize, ref pageCount);
            });
            return Json(model, JsonRequestBehavior.AllowGet);
        }

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
                    db.AddDisableUpdateColumns();
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
                db.FalseDelete<DocContent, int>("IsDeleted", ids);
            });
            return Json(model,JsonRequestBehavior.AllowGet);
        }

        public JsonResult DC_Single(int id)
        {
            var model = new ResultModel<DocContent>();
            _service.Command<LoginOutsourcing>((db, o) =>
            {
                model.ResultInfo = db.Queryable<DocContent>().InSingle(id);
            });
            return Json(model, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}