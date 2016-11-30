using Infrastructure.Dao;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Infrastructure.DbModel;
using Infrastructure.ViewModels;
using SyntacticSugar;
using SqlSugar;
using Infrastructure.Pub;
using Infrastructure.ViewModels.BBS;

namespace SugarSite.Areas.BBS.Controllers
{
    public class MainController : BaseController
    {
        public MainController(DbService s) : base(s) { }

        #region page
        public ActionResult Index(int? fid, int? orderBy)
        {
            MainResult model = new MainResult();
            ViewBag.NewUserList = base.GetNewUserList;
            ViewBag.ForList = base.GetForumsList;
            ViewBag.ForumsStatistics = base.GetForumsStatistics();
            _service.Command<MainOutsourcing, ResultModel<MainResult>>((db, o, api) =>
             {
                 model = api.Get(Url.Action("GetMainResult"), new { fid = fid, orderBy = orderBy }).ResultInfo;
                 model.ForumsList = ViewBag.ForList;
             });
            return View(model);
        }

        public ActionResult Detail(int id = 0)
        {
            ViewBag.NewUserList = base.GetNewUserList;
            ViewBag.ForList = base.GetForumsList;
            ViewBag.ForumsStatistics = base.GetForumsStatistics();
            DetailResult model = new DetailResult();
            _service.Command<MainOutsourcing, ResultModel<DetailResult>>((db, o, api) =>
            {
                model = api.Get(Url.Action("GetItem"), new {tid=id}).ResultInfo;
                db.Update<BBS_Topics>(new { Views = model.TopItem.Views.TryToInt() + 1 },it=>it.Tid== model.TopItem.Tid);
            });
            return View(model);
        }

        public ActionResult Ask()
        {
            if (IsLogin.IsFalse())
            {
                return this.Redirect("/Home/Login");
            }
            ViewBag.ForList = base.GetForumsList;
            return View();
        }
        #endregion

        #region api
        [HttpPost]
        [ValidateInput(false)]
        public JsonResult AskSubmit(short fid, string title, string content, string vercode, int rate = 0)
        {
            ResultModel<string> model = new ResultModel<string>();
            _service.Command<MainOutsourcing>((db, o) =>
            {
                Check.Exception(fid == 0 || title.IsNullOrEmpty() || content.IsNullOrEmpty() || vercode.IsNullOrEmpty(), "参数不合法！");
                Check.Exception(title.Length < 5 && content.Length < 20 || content.Length > 10000, "参数不合法！");
                Check.Exception(base.GetForumsList != null && base.GetForumsList.Any(it => it.Fid == fid).IsFalse(), "参数不合法！");
                var sm = SessionManager<string>.GetInstance();
                var severCode = sm[PubConst.SessionVerifyCode];
                model.IsSuccess = false;
                if (vercode != severCode)
                {
                    model.ResultInfo = "参证码错误！";
                }
                else if (IsLogin.IsFalse())
                {
                    model.ResultInfo = "对不起您还没有登录！";
                }
                else
                {
                    try
                    {
                        db.BeginTran();
                        //插贴子标题
                        BBS_Topics t = o.GetTopics(fid, title, rate, _userInfo);
                        var tid = db.Insert(t).ObjToInt();
                        t.Tid = tid;
                        //插贴子主体
                        BBS_Posts p = o.GetPosts(fid, content, _userInfo, t);
                        db.Insert(p);
                        db.CommitTran();
                        model.IsSuccess = true;
                        base.RemoveForumsStatisticsCache();//清除统计缓存
                    }
                    catch (Exception ex)
                    {
                        model.ResultInfo = "发布失败！";
                        db.RollbackTran();
                    }
                }
            });
            return Json(model);
        }

        public JsonResult GetMainResult(int? fid, int? orderBy)
        {
            ResultModel<MainResult> model = new ResultModel<MainResult>();
            model.ResultInfo = new MainResult();
            model.ResultInfo.PageIndex = 1;
            model.ResultInfo.PageSize = PubConst.SitePageSize;
            _service.Command<MainOutsourcing>((db, o) =>
            {
                var qureyable = db.Queryable<BBS_Topics>();
                if (fid > 0)
                {
                    qureyable = qureyable.Where(it => it.Fid == fid);
                }
                model.ResultInfo.TopicsList = qureyable.OrderBy(it => it.Postdatetime, OrderByType.Desc).ToList();
                model.ResultInfo.PageCount = qureyable.Count();
            });
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetItem(int tid)
        {
            ResultModel<DetailResult> model = new ResultModel<DetailResult>();
            _service.Command<MainOutsourcing>((db, o) =>
            {
                model.ResultInfo = new DetailResult();
                model.ResultInfo.PosItem = db.Queryable<BBS_Posts>().Single(it=>it.Tid==tid&&it.Parentid==0);//主贴
                model.ResultInfo.TopItem = db.Queryable<BBS_Topics>().Single(it => it.Tid == tid);//主贴
            });
            return Json(model, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}