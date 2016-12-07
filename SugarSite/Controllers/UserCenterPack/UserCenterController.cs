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
                return this.Redirect("~/Ask");
            }
            return View();
        }

        public ActionResult ActivateMail()
        {
            if (base.IsLogin == false)
            {
                return this.Redirect("~/Ask");
            }
            UserMailResult model = new UserMailResult();
            model.UserInfo = _userInfo;
            model.UserCode = EncryptSugar.GetInstance().Encrypto(model.UserInfo.Id.ToString());
            string dateStr = DateTime.Now.ToString("yyyy-MM-dd");
            model.Now = EncryptSugar.GetInstance().Encrypto(dateStr);
            return View(model);
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

        public ActionResult ActivateMailSubmitSuccess(string key ,string userId)
        {
            return View();
        }
        #endregion

        #region api
        public JsonResult ActivateMailSubmit(string key, string userId,string mail)
        {
            var model = new ResultModel<string>();
            _service.Command<HomeOutsourcing>((db, o) =>
            {
                var userIdInt = EncryptSugar.GetInstance().Decrypto(key).ObjToInt();
                var date = EncryptSugar.GetInstance().Decrypto(userId).ObjToDate();
                var isAny = db.Queryable<UserInfo>().Any(it => userIdInt == it.Id);
                var isOkDate = ((DateTime.Now - date).TotalDays <= 3);
                if (isAny && isOkDate)
                {
                    model.ResultInfo = "激活成功！";
                    model.IsSuccess = true;
                    db.Update<UserInfo>(new { Email = mail }, it => it.Id == userIdInt);
                }
                else
                {
                    model.ResultInfo = "激活失败，请重新发送邮箱验证或者联系管理员 610262374@qq.com 。";
                }
            });
            return Json(model,JsonRequestBehavior.AllowGet);
        }
        public JsonResult ActivateMailSend(string key, string userId)
        {
            //命名反的防止误导黑客
            var userIdInt = EncryptSugar.GetInstance().Decrypto(key).ObjToInt();
            var date =EncryptSugar.GetInstance().Decrypto(userId).ObjToDate();
            var model = new ResultModel<string>();
            _service.Command<HomeOutsourcing>((db, o) =>
            {
                var isAny=db.Queryable<UserInfo>().Any(it => userIdInt == it.Id);
                var isOkDate = ((DateTime.Now - date).TotalDays<=3);
                if (isAny && isOkDate)
                {
                    model.ResultInfo = "发送成功，请打开邮箱完成激活！";
                    model.IsSuccess = true;
                }
                else {
                    model.ResultInfo = "发送失败";
                }
            });
            return Json(model,JsonRequestBehavior.AllowGet);
        }

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
               .Where(it => it.Parentid > 0)
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