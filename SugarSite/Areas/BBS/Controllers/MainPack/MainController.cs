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

namespace SugarSite.Areas.BBS.Controllers
{
    public class MainController : BaseController
    {
        public MainController(DbService s) : base(s) { }

        public ActionResult Index(int? fid)
        {
            ViewBag.NewUserList = base.GetNewUserList;
            ViewBag.ForList = base.GetForumsList;
            return View();
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
                        BBS_Topics t = new BBS_Topics()
                        {
                            Fid = fid,
                            Title = title,
                            Posterid = _userInfo.Id,
                            Poster = _userInfo.NickName,
                            Postdatetime = DateTime.Now,
                            Rate = rate,
                            PosterAvatar=_userInfo.Avatar
                        };
                        var tid = db.Insert(t).ObjToInt();
                        //插贴子主体
                        BBS_Posts p = new BBS_Posts()
                        {
                            Fid = fid,
                            Ip = RequestInfo.UserAddress,
                            Posterid = _userInfo.Id,
                            Poster = _userInfo.NickName,
                            Message = content
                        };
                        db.CommitTran();
                        model.IsSuccess = true;
                    }
                    catch
                    {
                        model.ResultInfo = "发布失败！";
                        db.RollbackTran();
                    }
                }
            });
            return Json(model);
        }
    }
}