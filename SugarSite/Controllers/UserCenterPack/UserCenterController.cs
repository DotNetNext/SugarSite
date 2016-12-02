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
            return View();
        }

        public JsonResult GetPublicInfo(int uid)
        {
            var model = new ResultModel<PubUserResult>();
            _service.Command<HomeOutsourcing>((db, o) =>
            {
                model.ResultInfo = new PubUserResult();
                model.ResultInfo.UserInfo = db.Queryable<UserInfo>().InSingle(uid);
            });
            return Json(model);
        }
    }
}