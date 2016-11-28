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

        public ActionResult Doc(int typeId = 0)
        {
            var model = new ResultModel<DocResult>();
            _service.Command<HomeOutsourcing, ResultModel<DocResult>>((o, api) =>
            {
                model = api.Get(Url.Action("GetDoc"), new { typeId = typeId });
            });
            ViewBag.IsAdmin = _userInfo != null && _userInfo.RoleId == 1;
            return View(model);
        }

        public ActionResult Reward()
        {
            return View();
        }

        public ActionResult Login()
        {
            return View();
        }

        public ActionResult Register()
        {
            return View();
        }


        #endregion

        #region Public API
        /// <summary>
        /// 获取文档
        /// </summary>
        /// <param name="TypeId"></param>
        /// <param name="Id"></param>
        /// <returns></returns>
        [OutputCache(Duration = 0)]
        public JsonResult GetDoc(int typeId)
        {
            var model = new ResultModel<DocResult>();
            _service.Command<HomeOutsourcing>((db, o) =>
            {
                model.ResultInfo = new Infrastructure.ViewModels.DocResult();
                model.ResultInfo.DocType = db.Queryable<DocType>().ToList();
                if (typeId == 0)//如果没有文章ID取第一条
                {
                    typeId = model.ResultInfo.DocType.Where(it => it.Level == 2).OrderBy(it => it.Level).ThenBy(it => it.Sort).First().Id;
                }
                var list = db.Queryable<DocContent>().Where(it => it.TypeId == typeId).ToList();
                model.ResultInfo.DocContent = list;
                model.ResultInfo.CurrentType = model.ResultInfo.DocType.Single(it => it.Id == typeId);
                model.IsSuccess = true;
            });
            return Json(model, JsonRequestBehavior.AllowGet);

        }
        /// <summary>
        /// 获取验证码图片
        /// </summary>
        public void VerifyCode()
        {
            _service.Command<HomeOutsourcing>(o =>
            {
                o.OutPutVerifyCode();
            });

        }
        #endregion
    }
}