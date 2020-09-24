using Infrastructure.Dao;
using Infrastructure.DbModel;
using Infrastructure.Pub;
using Infrastructure.ViewModels;
using SyntacticSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SqlSugar;
using Infrastructure;
using Infrastructure.Tool;

namespace SugarSite.Controllers
{
    public class HomeController : BaseController
    {
        public HomeController(DbService s) : base(s) { }

        #region Page View
        public ActionResult Index()
        {
            ViewBag.IsMainPage = true;
            return View();
        }

        public ActionResult Doc(int masterId=1,int typeId = 0)
        {
            var model = new ResultModel<DocResult>();
            _service.Command<HomeOutsourcing, ResultModel<DocResult>>((o, api) =>
            {
                model = api.Get(Url.Action("GetDoc"), new { typeId = typeId, masterId = masterId });
            });
            ViewBag.IsAdmin = _userInfo != null && _userInfo.RoleId == 1;
            ViewBag.DocTitle = model.ResultInfo.Title;
            model.ResultInfo.MasterId = masterId;
            model.ResultInfo.TypeId = typeId;
            return View(model);
        }

        public ActionResult DocSearch(string title, int masterId = 1, int typeId = 0)
        {
            var model = new ResultModel<DocDetailsResult>();
            _service.Command<HomeOutsourcing, ResultModel<DocDetailsResult>>((o, api) =>
            {
                model = api.Get(Url.Action("GetDocDetails"), new { typeId = typeId,title=title, masterId = masterId });
            });
            ViewBag.IsAdmin = _userInfo != null && _userInfo.RoleId == 1;
            model.ResultInfo.MasterId = masterId;
            model.ResultInfo.TypeId = typeId;
            return View(model);
        }

        public ActionResult Reward()
        {
            return View();
        }

        public ActionResult Login()
        {
            if (IsLogin)
            {
                return Redirect("~/Ask");
            }
            ViewBag.ReturnUrl = Request.UrlReferrer.TryToString(Url.Content("/ASK"));
            return View();
        }

        public ActionResult HomeCallBack()
        {
            return View();
        }

        public ActionResult Logout()
        {
            var cm = CacheManager<UserInfo>.GetInstance();
            var cm2 = CookiesManager<string>.GetInstance();
            string uniqueKey = PubGet.GetUserKey;
            cm.Remove(uniqueKey);
            cm2.Remove(PubConst.UserUniqueKey);
            return Redirect("~/Ask?isLogout=true");
        }

        public ActionResult Register()
        {
            if (IsLogin)
            {
                return Redirect("~/Ask");
            }
            _service.Command<HomeOutsourcing, RestApi<ResultModel<string>>>((db, api) =>
             {

             });
            ViewBag.ReturnUrl = Request.UrlReferrer.TryToString(Url.Content("/ASK"));
            return View();
        }


        #endregion

        #region Public API
        public JsonResult LoginSubmit(UserInfo user, string vercode, string returnUrl)
        {
            var model = new ResultModel<string>();
            _service.Command<HomeOutsourcing>((db, o) =>
            {
                ExpCheck.Exception(user.UserName.IsNullOrEmpty() || user.Password.IsNullOrEmpty(), "用户名和密码不能为空！");
                user.Password = new EncryptSugar().MD5(user.Password);
                var loginUser = db.Queryable<UserInfo>().FirstOrDefault(it => it.UserName == user.UserName && it.Password == user.Password);
                var sm = SessionManager<string>.GetInstance();
                var severCode = sm[PubConst.SessionVerifyCode];
                if (loginUser == null)
                {
                    model.IsSuccess = false;
                    model.ResultInfo = "邮箱或者密码不正确！";
                }
                else if (vercode != severCode)
                {
                    model.IsSuccess = false;
                    model.ResultInfo = "验证码不正确！";
                }
                else
                {
                    var cm = CacheManager<UserInfo>.GetInstance();
                    string uniqueKey = PubGet.GetUserKey;
                    cm.Add(uniqueKey, loginUser, cm.Day * 365);//保存一年
                    LoginHistory lh = new LoginHistory()
                    {
                        CreateDate = DateTime.Now,
                        IsDeleted = false,
                        Uid = loginUser.Id,
                        UniqueKey = uniqueKey
                    };
                    db.Insert(lh);
                    model.IsSuccess = true;
                    model.ResultInfo = returnUrl;
                }

            });
            return Json(model);
        }

        public JsonResult RegisterSubmit(UserInfo user, string vercode, string confirmPassword, string returnUrl)
        {
            string oldPassword = user.Password;
            var model = new ResultModel<string>();
            _service.Command<HomeOutsourcing>((db, o) =>
            {
                ExpCheck.Exception(user.UserName.IsNullOrEmpty() || user.Password.IsNullOrEmpty(), "用户名和密码不能为空！");
                ExpCheck.Exception(user.UserName.IsEamil().IsFalse(), "不是有效邮箱！");
                ExpCheck.Exception(user.NickName.IsNullOrEmpty(), "妮称不能为空！");
                ExpCheck.Exception(user.Password != confirmPassword, "两次密码不一致！");
                user.Password = new EncryptSugar().MD5(user.Password);
                var sm = SessionManager<string>.GetInstance();
                var severCode = sm[PubConst.SessionVerifyCode];
                try
                {
                    if (oldPassword.Length < 6 || oldPassword.Length > 16)
                    {
                        model.IsSuccess = false;
                        model.ResultInfo = "密码长度为6-16个字符";
                    }
                    else if (severCode != vercode)
                    {
                        model.IsSuccess = false;
                        model.ResultInfo = "验证码不正确！";
                    }
                    else
                    {
                        var isAny = db.Queryable<UserInfo>().Any(it => it.UserName == user.UserName);
                        if (isAny)
                        {
                            model.IsSuccess = false;
                            model.ResultInfo = "邮箱已经被注册！";
                        }
                        else
                        {
                            user.RoleId = PubEnum.RoleType.User.TryToInt();
                            user.CreateTime = DateTime.Now;
                            var id = db.Insert(user).ObjToInt();
                            var loginUser = db.Queryable<UserInfo>().InSingle(id);
                            var cm = CacheManager<UserInfo>.GetInstance();
                            string uniqueKey = PubGet.GetUserKey;
                            cm.Add(uniqueKey, loginUser, cm.Day * 365);//保存一年
                            model.IsSuccess = true;
                            model.ResultInfo = returnUrl;
                            RemoveNewUserListCache();
                        }
                    }
                }
                catch (Exception ex)
                {
                    PubMethod.WirteExp(ex);
                    model.IsSuccess = false;
                    model.ResultInfo = "用户注册失败,请联系我们！";
                }
            });
            return Json(model);
        }

        /// <summary>
        /// 获取文档
        /// </summary>
        /// <param name="TypeId"></param>
        /// <param name="Id"></param>
        /// <returns></returns>
        [OutputCache(Duration = 0)]
        public JsonResult GetDoc(int typeId, int masterId = 1)
        {
            var model = new ResultModel<DocResult>();
            _service.Command<HomeOutsourcing>((db, o) =>
            {
                model.ResultInfo = new Infrastructure.ViewModels.DocResult();
                model.ResultInfo.DocType = db.Queryable<DocType>().Where(it => it.MasterId == masterId).ToList();
                model.ResultInfo.Title = model.ResultInfo.DocType?.FirstOrDefault()?.TypeName;
                if (typeId == 0)//如果没有文章ID取第一条
                {
                    typeId = model.ResultInfo.DocType.Where(it => it.MasterId == masterId).Where(it => it.Level == 2).OrderBy(it => it.Level).ThenBy(it => it.Sort).First().Id;
                }
                var list = db.Queryable<DocContent>().Where(it => it.TypeId == typeId).ToList();
                model.ResultInfo.DocContent = list;
                model.ResultInfo.CurrentType = model.ResultInfo.DocType.Single(it => it.Id == typeId);
                model.ResultInfo.Master = db.Queryable<DocMaster>().Single(it => it.Id == model.ResultInfo.CurrentType.MasterId);
                model.IsSuccess = true;
            });
            return Json(model, JsonRequestBehavior.AllowGet);

        }
        /// <summary>
        /// 获取文档搜索
        /// </summary>
        /// <param name="TypeId"></param>
        /// <param name="Id"></param>
        /// <returns></returns>
        [OutputCache(Duration = 0)]
        public JsonResult GetDocDetails(int typeId,string title, int masterId = 1)
        {
            var model = new ResultModel<DocDetailsResult>();
            _service.Command<HomeOutsourcing>((db, o) =>
            {
                model.ResultInfo = new Infrastructure.ViewModels.DocDetailsResult();
                model.ResultInfo.DocType = db.Queryable<DocType>().Where(it => it.MasterId == masterId).ToList();
                model.ResultInfo.SearchList =new List<DocType>();
                var level2Types = db.SqlQuery<DocType>(@"select top 20 * from DocType t  where t.[Level]=2 and 
                (
                t.id in(
                       select  TypeId from DocContent   where Content like '%'+@title+'%' 
                )
                or 
                 TypeName like  '%'+@title+'%' 
                )", new { title =title});
                if (level2Types != null && level2Types.Count > 0)
                {
                    foreach (var item in level2Types)
                    {
                        model.ResultInfo.SearchList.Add(item);
                    }
                }
                model.IsSuccess = true;
            });
            return Json(model, JsonRequestBehavior.AllowGet);

        }
        
        /// <summary>
        /// 获取验证码图片
        /// </summary>
        public void OutPutVerifyCode()
        {
            _service.Command<HomeOutsourcing>(o =>
            {
                o.OutPutVerifyCode(this.GetVerifyCode);
            });

        }
        #endregion
    }
}