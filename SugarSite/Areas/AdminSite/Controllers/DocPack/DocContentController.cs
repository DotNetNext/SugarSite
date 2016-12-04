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
using Infrastructure.Tool;

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
        public JsonResult Dc_GetList(int? typeId, int pageIndex = 1, int pageSize = PubConst.SitePageSize)
        {
            var model = new ResultModel<DocContentResult>();
            _service.Command<DocOutsourcing>((db, o) =>
            {
                var typeList = db.Queryable<DocType>().ToList();
                int pageCount = 0;
                model.ResultInfo = new DocContentResult();
                model.ResultInfo.PageIndex = pageIndex;
                model.ResultInfo.PageSize = pageSize;
                model.ResultInfo.PageCount = 0;
                db.CurrentFilterKey = PubConst.FilterKeyFalseDelteJoin;
                var queryable = db.Queryable<DocContent>()
                            .JoinTable<DocType>((m, dt) => m.TypeId == dt.Id);
                if (typeId != null)
                {
                    queryable.In("m.typeId", o.GetChildrenTypeIds(typeList, typeId));
                }
                model.ResultInfo.DocList = queryable.OrderBy(m => m.Id, OrderByType.Desc).Select<DocType, V_DocContent>((m, dt) => new V_DocContent()
                {
                    Title = m.Title,
                    TypeName = dt.TypeName,
                    CreateTime = m.CreateTime,
                    Creator = m.Creator,
                    Id = m.Id,
                    TypeId = m.TypeId

                }).ToPageList(pageIndex, pageSize, ref pageCount);
                model.ResultInfo.PageCount = pageCount;
            });

            return Json(model, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Dc_GetTypeList()
        {
            var model = new ResultModel<List<LayuiTreeModel>>();
            _service.Command<DocOutsourcing>((db, o) =>
            {
                var list = db.Queryable<DocType>().Select(it => new LayuiTreeModel()
                {
                    id = it.Id,
                    name = it.TypeName,
                    parentId = it.ParentId,
                    alias = 1,
                    level = it.Level,
                    sort=it.Sort,
                    masterId=it.MasterId.ObjToInt()
                }).ToList();
                model.ResultInfo = list.ToLayuiTree();
            });
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateInput(false)]
        public JsonResult DC_Submit(DocContent obj)
        {
            var model = new ResultModel<object>();
            _service.Command<DocOutsourcing>((db, o) =>
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
            _service.Command<DocOutsourcing>((db, o) =>
            {
                db.FalseDelete<DocContent, int>("IsDeleted", ids);
            });
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        public JsonResult DC_Single(int id)
        {
            var model = new ResultModel<DocContent>();
            _service.Command<DocOutsourcing>((db, o) =>
            {
                model.ResultInfo = db.Queryable<DocContent>().InSingle(id);
            });
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult DcType_Submit(DocType type)
        {
            var model = new ResultModel<DocContent>();
            _service.Command<DocOutsourcing>((db, o) =>
            {
                DocType parent = new DocType();
                if (type.ParentId > 0) {
                    parent = db.Queryable<DocType>().InSingle(type.ParentId);
                }
                if (type.Id == 0)
                {
                    type.Level = parent.Level.ObjToInt() + 1;
                    db.Insert(type);
                }
                else
                {
                    db.Update<DocType>(new { typeName = type.TypeName,Sort=type.Sort, MasterId = type.MasterId }, it => it.Id == type.Id);
                }
                model.IsSuccess = true;
            });
            return Json(model);
        }
        public JsonResult DCType_Delete(int id)
        {
            var model = new ResultModel<bool>();
            _service.Command<DocOutsourcing>((db, o) =>
            {
                db.FalseDelete<DocType, int>("IsDeleted", id);
            });
            return Json(model, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}