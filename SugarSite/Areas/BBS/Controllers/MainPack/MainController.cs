﻿using Infrastructure.Dao;
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
        public ActionResult Index(int? fid, int? orderBy, int? pageIndex, string title = null)
        {
            MainResult model = new MainResult();
            ViewBag.NewUserList = base.GetNewUserList;
            ViewBag.ForList = base.GetForumsList;
            ViewBag.ForumsStatistics = base.GetForumsStatistics();
            ViewBag.IsLogin = base.IsLogin;
            ViewBag.BBS_Right = base.GetBBS_Right;
            _service.Command<MainOutsourcing, ResultModel<MainResult>>((db, o, api) =>
             {
                 model = api.Get(Url.Action("GetMainResult"), new {title = title, fid = fid, orderBy = orderBy, pageIndex = pageIndex }).ResultInfo;
                 model.ForumsList = ViewBag.ForList;
                 model.OrderBy = orderBy.TryToInt();
                 model.Fid = (short)fid.TryToInt();
                 model.OnlineList = ViewBag.OnlineVisitorsResult;
                 model.SiteInfo = ViewBag.SiteInfoResult;
                 var ps = new SyntacticSugar.PageString();
                 model.PageString = ps.ToPageString(model.PageCount, model.PageSize, model.PageIndex, Url.Content("/Ask?fid="+fid+"&"));
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
            ViewBag.BBS_Right = base.GetBBS_Right;
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

        public ActionResult AskItem(int postId)
        {
            if (IsLogin.IsFalse())
            {
                return this.Redirect("/Home/Login");
            }
            BBS_Posts model=new BBS_Posts();
            _service.Command<MainOutsourcing, ResultModel<BBS_Posts>>((db, o,api) =>
            {
                model = api.Get(Url.Action("GetPostByPid"), new { pid = postId }).ResultInfo;
            });
             return View(model);
        }
        #endregion

        #region api
        [HttpPost]
        public JsonResult AddFavorites(int tid, short fid)
        {
            ResultModel<string> model = new ResultModel<string>();
            if (base.IsLogin)
            {
                _service.Command<MainOutsourcing>((db, o) =>
                {
                    BBS_Favorites data = new BBS_Favorites()
                    {
                        Favtime = DateTime.Now,
                        Tid = tid,
                        Uid = _userInfo.Id,
                        Typeid = 0,
                        Viewtime=DateTime.Now
                    };
                    var isAny = (db.Queryable<BBS_Favorites>().Any(it => it.Tid == tid && it.Uid == _userInfo.Id));
                    if (isAny)
                    {
                        db.Delete<BBS_Favorites>(it => it.Tid == tid && it.Uid == _userInfo.Id);
                        model.ResultInfo = "已经取消收藏";
                    }
                    else
                    {
                        db.Insert(data);
                        model.ResultInfo = "收藏成功";
                        model.Status = "1";
                    }
                    model.IsSuccess = true;
                });
            }
            else {
                model.ResultInfo = "-1";
            }
           return  Json(model);
        }

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
                var myUser = db.Queryable<UserInfo>().InSingle(_userInfo.Id);
                Check.Exception(myUser.IsDeleted == true, "用户已被删除！");
                var sm = SessionManager<string>.GetInstance();
                var severCode = sm[PubConst.SessionVerifyCode];
                model.IsSuccess = false;
                if (vercode != severCode)
                {
                    model.ResultInfo = "验证码错误！";
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
                            Check.Exception(topics.Posterid != _userInfo.Id && _userInfo.RoleId == PubEnum.RoleType.User.TryToInt(), "您没有权限修改！");
                            db.Update<BBS_Topics>(new { Title = title, Rate = rate, Fid = fid }, it => it.Tid == id);
                            db.Update<BBS_Posts>(new { Title = title, Rate = rate, Fid = fid, Message = content }, it => it.Tid == id && it.Parentid == 0);
                        }
                        db.CommitTran();
                        model.IsSuccess = true;
                        base.RemoveForumsStatisticsCache();//清除统计缓存
                    }
                    catch (Exception ex)
                    {
                        model.ResultInfo = "发布失败！";
                        db.RollbackTran();
                        PubMethod.WirteExp(ex);
                    }
                }
            });
            return Json(model);
        }
        [HttpPost]
        [ValidateInput(false)]
        public JsonResult AskItemSubmit(short pid, string title, string content, string vercode)
        {
            ResultModel<string> model = new ResultModel<string>();
            _service.Command<MainOutsourcing>((db, o) =>
            {
                Check.Exception(pid == 0 ||  content.IsNullOrEmpty() || vercode.IsNullOrEmpty(), "参数不合法！");
                Check.Exception(content.Length < 5 || content.Length > 1000000, "参数不合法！");
                var myUser = db.Queryable<UserInfo>().InSingle(_userInfo.Id);
                Check.Exception(myUser.IsDeleted==true, "用户已被删除！");

                var sm = SessionManager<string>.GetInstance();
                var severCode = sm[PubConst.SessionVerifyCode];
                model.IsSuccess = false;
                if (vercode != severCode)
                {
                    model.ResultInfo = "验证码错误！";
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
                        var post= db.Queryable<BBS_Posts>().InSingle(pid);
                        if ((post.Postdatetime.AddDays(3)>DateTime.Now&&post.Posterid==base._userInfo.Id)||base._userInfo.RoleId==1) 
                        {
                            db.Update<BBS_Posts>(new { Message = content }, it => it.Pid == pid);
                        }
                        db.CommitTran();
                        model.IsSuccess = true;
                        base.RemoveForumsStatisticsCache();//清除统计缓存
                    }
                    catch (Exception ex)
                    {
                        model.ResultInfo = "发布失败！";
                        db.RollbackTran();
                        PubMethod.WirteExp(ex);
                    }
                }
            });
            return Json(model);
        }

        public JsonResult GetMainResult(int? fid, int? orderBy, int pageIndex = 1,string title=null)
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
                if (title.IsValuable()) {
                    qureyable = qureyable.Where(it => it.Title.Contains(title));
                }
                qureyable = o.GetMainQueryable(orderBy, db, qureyable, this);
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
                if (base.IsLogin)
                {
                    model.ResultInfo.IsFavorites = db.Queryable<BBS_Favorites>().Any(it => it.Tid == tid && it.Uid == _userInfo.Id);
                }
            });
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateInput(false)]
        public JsonResult RepliesSubmit(int tid, string content,int fid)
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
                        var prePost = db.Queryable<BBS_Posts>().OrderBy(it => it.Postdatetime, OrderByType.Desc).FirstOrDefault(it => it.Posterid == _userInfo.Id);
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
                            p.Fid = fid;
                            p.Tid = tid;
                            p.Posterid = base._userInfo.Id;
                            p.Poster = base._userInfo.NickName;
                            p.Lastedit = base._userInfo.NickName;
                            p.Postdatetime = DateTime.Now;
                            p.Ip = RequestInfo.UserAddress;
                            db.Insert(p);
                            db.Update<BBS_Topics>(" Replies=isnull([Replies],0)+1,Lastpost=@lp,Lastposter=@Lastposter,Lastposterid=@Lastposterid",
                                it => it.Tid == tid,
                                new { lp = DateTime.Now, Lastposter = _userInfo.NickName, Lastposterid = _userInfo.Id });//回复数加1
                            model.IsSuccess = true;
                            base.RemoveForumsStatisticsCache();
                            db.CommitTran();

                            //发送邮箱提醒
                            o.SendMail(base._userInfo,tid,p,db);
                            //发送站内信
                            o.SendPMS(base._userInfo, tid, p, db);
                        }
                    }
                    catch (Exception ex)
                    {
                        model.ResultInfo = "回复失败！";
                        PubMethod.WirteExp(ex);
                        db.RollbackTran();
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
                model.ResultInfo = db.Queryable<BBS_Posts>().Where(it => it.Tid == tid).Where(it => it.Parentid == 0).Single();
            });
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetPostByPid(int pid)
        {
            ResultModel<BBS_Posts> model = new ResultModel<BBS_Posts>();
            _service.Command<MainOutsourcing>((db, o) =>
            {
                model.ResultInfo = db.Queryable<BBS_Posts>().Where(it => it.Pid == pid).Single();
                model.ResultInfo.Title = db.Queryable<BBS_Topics>().Where(it => it.Tid == model.ResultInfo.Tid).Single().Title;
            });
            return Json(model, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}