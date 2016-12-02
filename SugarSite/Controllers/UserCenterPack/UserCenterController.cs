using Infrastructure.Dao;
using Infrastructure.DbModel;
using Infrastructure.ViewModels;
using Infrastructure.ViewModels.UserCenter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SqlSugar;
using SyntacticSugar;
namespace SugarSite.Controllers
{
    public class UserCenterController : BaseController
    {
        public UserCenterController(DbService s) : base(s) { }

        #region page
        public ActionResult Index()
        {
            if (base.IsLogin == false)
            {
                return this.Redirect("/UserCenter");
            }
            return View();
        }

        public ActionResult PublicInfo(int id = 0)
        {
            PubUserResult model = new PubUserResult();
            _service.Command<UserCenterOutsourcing, ResultModel<PubUserResult>>((o, api) =>
            {
                model = api.Get(Url.Action("GetPublicInfo"), new { id = id }).ResultInfo;
            });
            return View(model);
        }
        #endregion

        #region api
        public JsonResult GetPublicInfo(int id)
        {
            var model = new ResultModel<PubUserResult>();
            _service.Command<HomeOutsourcing>((db, o) =>
            {
                model.ResultInfo = new PubUserResult();
                model.ResultInfo.UserInfo = db.Queryable<UserInfo>().InSingle(id);
                //最新发布
                model.ResultInfo.RecentAsks = db.Queryable<BBS_Topics>()
                .Where(it => it.Posterid == id)
                .OrderBy(it => it.Postdatetime, OrderByType.Desc)
                .ToList();
                //最新回复信息
                model.ResultInfo.RecentReplies = db.Queryable<BBS_Posts>()
               .Where(it => it.Posterid == id)
               .Where(it=>it.Parentid>0)
               .OrderBy(it => it.Postdatetime, OrderByType.Desc)
               .ToList();
                if (model.ResultInfo.RecentReplies.IsValuable())
                {
                    var tids = model.ResultInfo.RecentReplies.Select(it => it.Tid).ToList();
                    if (tids.IsValuable())
                    {
                        model.ResultInfo.RecentRepliesTopics = db.Queryable<BBS_Topics>()
                       .In(it => it.Tid, tids)
                       .ToList();
                    }
                }
            });
            return Json(model, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}