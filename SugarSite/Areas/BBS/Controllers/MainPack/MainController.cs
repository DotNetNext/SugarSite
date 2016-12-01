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
        public ActionResult Index(int? fid, int? orderBy,int? pageIndex)
        {
            MainResult model = new MainResult();
            ViewBag.NewUserList = base.GetNewUserList;
            ViewBag.ForList = base.GetForumsList;
            ViewBag.ForumsStatistics = base.GetForumsStatistics();
            ViewBag.IsLogin = base.IsLogin;
            _service.Command<MainOutsourcing, ResultModel<MainResult>>((db, o, api) =>
             {
                 model = api.Get(Url.Action("GetMainResult"), new { fid = fid, orderBy = orderBy, pageIndex= pageIndex }).ResultInfo;
                 model.ForumsList = ViewBag.ForList;
                 model.OrderBy = orderBy.TryToInt();
                 model.Fid = (short)fid.TryToInt();
                 var ps = new SyntacticSugar.PageString();
                 model.PageString =ps.ToPageString(model.PageCount,model.PageSize,model.PageIndex,Url.Content("/Ask?"));
             });
            return View(model);
        }

        public ActionResult Detail(int id = 0)
        {
            ViewBag.NewUserList = base.GetNewUserList;
            ViewBag.ForList = base.GetForumsList;
            ViewBag.ForumsStatistics = base.GetForumsStatistics();
            ViewBag.IsLogin = base.IsLogin;
            ViewBag.User = base._userInfo;
            DetailResult model = new DetailResult();
            _service.Command<MainOutsourcing, ResultModel<DetailResult>>((db, o, api) =>
            {
                model = api.Get(Url.Action("GetItem"), new { tid = id }).ResultInfo;
                db.Update<BBS_Topics>(new { Views = model.TopItem.Views.TryToInt() + 1 }, it => it.Tid == model.TopItem.Tid);
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
        public JsonResult AskSubmit(short fid, string title, string content, string vercode, int rate = 0, int id = 0)
        {
            ResultModel<string> model = new ResultModel<string>();
            _service.Command<MainOutsourcing>((db, o) =>
            {
                Check.Exception(fid == 0 || title.IsNullOrEmpty() || content.IsNullOrEmpty() || vercode.IsNullOrEmpty(), "参数不合法！");
                Check.Exception(title.Length < 5 && content.Length < 20 || content.Length > 1000000, "参数不合法！");
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
                        var isAdd = id == 0;
                        if (isAdd)
                        {
                            //插贴子标题
                            BBS_Topics t = o.GetTopics(fid, title, rate, _userInfo);
                            var tid = db.Insert(t).ObjToInt();
                            t.Tid = tid;
                            //插贴子主体
                            BBS_Posts p = o.GetPosts(fid, content, _userInfo, t);
                            db.Insert(p);
                        }
                        else
                        {
                            var topics = db.Queryable<BBS_Topics>().InSingle(id);
                            Check.Exception(topics.Posterid!=_userInfo.Id&&_userInfo.RoleId==PubEnum.RoleType.User.TryToInt(), "您没有权限修改！");
                            db.Update<BBS_Topics>(new  { Title = title, Rate = rate, Fid = fid }, it => it.Tid == id);
                            db.Update<BBS_Posts>(new  { Title = title, Rate = rate, Fid = fid, Message = content }, it => it.Tid == id && it.Parentid == 0);
                        }
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

        public JsonResult GetMainResult(int? fid, int? orderBy,int pageIndex=1)
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
                qureyable = o.GetMainQueryable(orderBy, db, qureyable,this);
                int pageCount = 0;
                model.ResultInfo.TopicsList = qureyable.ToPageList(pageIndex, PubConst.SitePageSize, ref pageCount);
                model.ResultInfo.PageCount = pageCount;
                model.ResultInfo.PageIndex = pageIndex;

            });
            return Json(model, JsonRequestBehavior.AllowGet);
        }

    

        public JsonResult GetItem(int tid)
        {
            ResultModel<DetailResult> model = new ResultModel<DetailResult>();
            _service.Command<MainOutsourcing>((db, o) =>
            {
                model.ResultInfo = new DetailResult();
                model.ResultInfo.PosItem = db.Queryable<BBS_Posts>().Single(it => it.Tid == tid && it.Parentid == 0);//主贴
                model.ResultInfo.TopItem = db.Queryable<BBS_Topics>().Single(it => it.Tid == tid);//主贴
                model.ResultInfo.PostsChildren = db.Queryable<V_BBS_Posts>().Where(it => it.Tid == tid && it.Parentid > 0)
                .OrderBy(it => it.Postdatetime).ToList();
            });
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateInput(false)]
        public JsonResult RepliesSubmit(int tid, string content)
        {
            ResultModel<string> model = new ResultModel<string>();
            _service.Command<MainOutsourcing>((db, o) =>
            {
                model.IsSuccess = false;
                if (base.IsLogin.IsFalse())
                {
                    model.ResultInfo = "-1";
                }
                else
                {
                    db.BeginTran();
                    try
                    {
                        var prePost = db.Queryable<BBS_Posts>().OrderBy(it => it.Postdatetime, OrderByType.Desc).SingleOrDefault(it => it.Parentid == _userInfo.Id);
                        if (prePost != null && (DateTime.Now - prePost.Postdatetime.TryToDate()).Seconds <= 6)
                        {
                            model.ResultInfo = "你回复的太快了，请在等"
                            + (5 - (DateTime.Now - prePost.Postdatetime.TryToDate()).Seconds) + "秒！";
                        }
                        else
                        {
                            BBS_Posts p = new BBS_Posts();
                            p.Title = "";
                            p.Parentid = tid;
                            p.Message = content;
                            p.Tid = tid;
                            p.Posterid = base._userInfo.Id;
                            p.Poster = base._userInfo.NickName;
                            p.Lastedit = base._userInfo.NickName;
                            p.Postdatetime = DateTime.Now;
                            p.Ip = RequestInfo.UserAddress;
                            db.Insert(p);
                            db.Update<BBS_Topics>(" Replies=isnull([Replies],0)+1,Lastpost=@lp,Lastposter=@Lastposter,Lastposterid=@Lastposterid",
                                it => it.Tid == tid, 
                                new { lp = DateTime.Now, Lastposter = _userInfo.NickName, Lastposterid=_userInfo.Id });//回复数加1
                            model.IsSuccess = true;
                            base.RemoveForumsStatisticsCache();
                        }
                    }
                    catch (Exception ex)
                    {
                        model.ResultInfo = "回复失败！";
                    }

                }
            });
            return Json(model);
        }

        public JsonResult GetPost(int tid)
        {
            ResultModel<BBS_Posts> model = new ResultModel<BBS_Posts>();
            _service.Command<MainOutsourcing>((db, o) =>
            {
                model.ResultInfo= db.Queryable<BBS_Posts>().Where(it => it.Tid == tid).Where(it => it.Parentid == 0).Single();
            });
            return Json(model, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}