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
using Infrastructure.Pub;
using Infrastructure.ViewModels.Views;

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
            IndexResult model = new IndexResult();
            _service.Command<IndexResult, ResultModel<IndexResult>>((db, api) =>
            {
                model = api.Get(Url.Action("GetIndexModel"), null).ResultInfo;
            });
            return View(model);
        }

        public ActionResult ChangeAvatar()
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

        public ActionResult ActivateMailSubmitSuccess(string key, string userId, string mail)
        {
            var model = new ResultModel<string>();
            _service.Command<UserCenterOutsourcing, ResultModel<string>>((o, api) =>
            {
                model = api.Get(Url.Action("ActivateMailSubmit"), new { key = key, userId = userId, mail = mail });
                ViewBag.Message = model.ResultInfo;
            });
            return View();
        }
        #endregion

        #region api
        public JsonResult ActivateMailSubmit(string key, string userId, string mail)
        {
            Check.Exception(mail.IsEamil().IsFalse(), "参数不合法");
            var model = new ResultModel<string>();
            _service.Command<HomeOutsourcing>((db, o) =>
            {
                var userIdInt = EncryptSugar.GetInstance().Decrypto(key).ObjToInt();
                var date = EncryptSugar.GetInstance().Decrypto(userId).ObjToDate();
                var isAny = db.Queryable<UserInfo>().Any(it => userIdInt == it.Id);
                var isOkDate = ((DateTime.Now - date).TotalDays <= 3);
                if (isAny && isOkDate)
                {
                    model.ResultInfo = "激活成功,请刷新你的页面！";
                    model.IsSuccess = true;
                    mail = mail.ToLower();
                    db.Update<UserInfo>(new { Email = mail }, it => it.Id == userIdInt);
                    UpdateMailCache(userIdInt, mail);
                }
                else
                {
                    model.ResultInfo = "激活失败，请重新发送邮箱验证或者联系管理员 610262374@qq.com 。";
                }
            });
            return Json(model, JsonRequestBehavior.AllowGet);
        }
        public JsonResult ActivateMailSend(string key, string userId, string mail)   //命名反的误导黑客
        {

            Check.Exception(mail.IsEamil().IsFalse(), "参数不合法");
            var userIdInt = EncryptSugar.GetInstance().Decrypto(key).ObjToInt();
            var date = EncryptSugar.GetInstance().Decrypto(userId).ObjToDate();
            var model = new ResultModel<string>();
            if (base.IsLogin == false)
            {
                model.ResultInfo = "登录超时请刷新页面重新登录";
                return Json(model, JsonRequestBehavior.AllowGet);
            }
            var cm = CacheManager<DateTime>.GetInstance();
            string mailTimeKey = PubConst.SessionMailTime + key.ToLower();
            if (cm.ContainsKey(mailTimeKey))
            {
                var mins = (DateTime.Now - cm[mailTimeKey]).TotalSeconds;
                if (mins < 60)
                {
                    model.ResultInfo = "您刚才已经发送成功，如果还没有收到邮件，请等待{0}秒后重新发送。".ToFormat(Convert.ToUInt32(60 - mins));
                    return Json(model, JsonRequestBehavior.AllowGet);
                }
            }
            _service.Command<HomeOutsourcing>((db, o) =>
            {
                var isAnyUser = db.Queryable<UserInfo>().Any(it => userIdInt == it.Id);
                var isOkDate = ((DateTime.Now - date).TotalDays <= 3);
                var isAnyMail = db.Queryable<UserInfo>().Any(it => mail.ToLower() == it.Email);
                if (isAnyUser && isOkDate && isAnyMail.IsFalse())
                {
                    var html = FileSugar.FileToString(FileSugar.GetMapPath("~/Template/mail/Validate.html")).Replace('\r', ' ').Replace('\n', ' ');
                    string userName = _userInfo.NickName;
                    string aHtml = "<a href=\"{0}\">{1}</a>".ToFormat(RequestInfo.HttpDomain + "" + Url.Action("ActivateMailSubmitSuccess", "UserCenter", new { key = key, userId = userId, mail }), "请点击这儿完成激活");
                    string dateString = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    MailSmtp ms = new MailSmtp(PubGet.GetEmailSmtp, PubGet.GetEmailUserName, PubGet.GetEmailPassword);
                    html = html.ToFormat(userName, aHtml, dateString);
                    ms.Send(PubGet.GetEmailUserName, PubConst.SiteMailUserName, mail, userName + "邮箱激活通知", html);
                    model.ResultInfo = "发送成功，请打开邮箱完成激活！";
                    string uniqueKey = PubGet.GetUserKey;
                    base.AddUpdateMailCache(uniqueKey);
                    model.IsSuccess = true;
                    Check.Exception(ms.Result.IsValuable(), "邮件激活失败！" + ms.Result);
                    cm.Add(mailTimeKey, DateTime.Now, cm.Minutes);
                }
                else if (isAnyMail)
                {
                    model.ResultInfo = "发送失败，该邮箱已经被激活，或已经存在。";
                }
                else
                {
                    model.ResultInfo = "发送失败";
                }
            });
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        public JsonResult UploadAvatar()
        {
            ResultModel<string> model = new ResultModel<string>();
            if (base.IsLogin.IsFalse())
            {
                model.ResultInfo = "您还没有登录！";
                return Json(model, JsonRequestBehavior.AllowGet);
            }
            try
            {
                _service.Command<UserCenterOutsourcing>((db, o) =>
                {
                    var imgObj = new UploadImage();
                    var file = Request.Files[0];
                    if (file.ContentLength / 1024.0 / 1024.0 > 1)
                    {
                        model.ResultInfo = "上传图片不能超过一M！";
                    }
                    else if (file.ContentType.IsIn("image/png", "image/gif", "image/jpg", "image/jpeg").IsFalse())
                    {
                        model.ResultInfo = "上传失败，您的图片不是PNG GIF JPG 格式！";
                    }
                    else
                    {
                        model.IsSuccess = true;
                        string webPath = "/_theme/img/avatar" + _userInfo.Id + ".jpg";
                        string savePath = FileSugar.GetMapPath("~" + webPath);
                        string smallPath = FileSugar.GetMapPath("~/_theme/img/_avatar" + _userInfo.Id + ".jpg");
                        file.SaveAs(savePath);
                        model.ResultInfo = Url.Content("~/" + webPath) + "?r=" + Guid.NewGuid();
                        imgObj.CreateSmallPhoto(savePath, 100, 100, smallPath);//生成缩略图
                        FileSugar.DeleteFile(savePath);
                        FileSugar.Move(smallPath, savePath);
                        db.Update<UserInfo>(new { avatar = webPath + "?r=" + RandomSugar.GetRandomNum(5) }, it => it.Id == _userInfo.Id);
                        RestCurrentUserCache();
                    }
                });
                return Json(model);
            }
            catch (Exception ex)
            {
                PubMethod.WirteExp(ex);
                model.ResultInfo = "上传失败！";
            }
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetPublicInfo(int id)
        {
            var model = new ResultModel<PubUserResult>();
            _service.Command<HomeOutsourcing>((db, o) =>
            {
                o.InsertVisitor(base.IsLogin, id, db, _userInfo);
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

        public JsonResult GetIndexModel()
        {
            var model = new ResultModel<IndexResult>();
            _service.Command<IndexResult>((db, o) =>
            {
                db.CurrentFilterKey = null;//无假删除不需要走过滤器
                IndexResult res = new IndexResult();
                res.VList = db.Queryable<VisitorList>()
                .JoinTable<UserInfo>((v, u) => v.VisitorId == u.Id)
                .OrderBy(v => v.VisitorId).Take(12)
                .Where(v => v.Uid == _userInfo.Id)
                .Select<UserInfo, V_VisitorList>((v, u) => new V_VisitorList()
                {
                    VisitorId = v.VisitorId,
                    VisAvatar = u.Avatar,
                    VisName = u.NickName,
                    CreateDate = v.CreateDate,
                    Uid = v.Uid
                }).ToList();
                model.ResultInfo = res;
                //获取未读
                res.PmsListNew = db.Queryable<BBS_PMS>()
                .Where(it => it.Msgtoid == _userInfo.Id && it.New == 1)
                .OrderBy(it => it.Postdatetime, OrderByType.Desc).Take(100).ToList();
                //获取已读
                res.PmsListOld = db.Queryable<BBS_PMS>()
                .Where(it => it.Msgtoid == _userInfo.Id && it.New == 0)
                .OrderBy(it => it.Postdatetime, OrderByType.Desc).Take(100).ToList();
            });
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        public JsonResult DeletePmsById(int id)
        {
            var model = new ResultModel<string>();
            if (base.IsLogin.IsFalse())
            {
                model.ResultInfo = "您还没有登录！";
                return Json(model,JsonRequestBehavior.AllowGet);
            }
            else
            {
                _service.Command<IndexResult>((db, o) =>
                {
                    db.Delete<BBS_PMS>(it => it.Pmid == id&&it.Msgtoid==_userInfo.Id);
                });
                return Json(model, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion
    }
}