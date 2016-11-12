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
            var model = new ResultModel<Doc>();
            _service.Command<HomeOutsourcing, ResultModel<Doc>>((o, api) =>
            {
                model = api.Get(Url.Action("GetDoc"), new { typeId = typeId });
            });
            return View(model);
        }
        #endregion

        #region Public API
        /// <summary>
        /// 获取文档
        /// </summary>
        /// <param name="TypeId"></param>
        /// <param name="Id"></param>
        /// <returns></returns>
        public JsonResult GetDoc(int typeId) 
        {
            var model = new ResultModel<Doc>();
            _service.Command<HomeOutsourcing>((db, o) =>
            {
                model.ResultInfo = new Infrastructure.ViewModels.Doc();
                model.ResultInfo.DocType = db.Queryable<DocType>().ToList();
                if (typeId == 0)//如果没有文章ID取第一条
                {
                    typeId = model.ResultInfo.DocType.OrderByDescending(it => it.Level).ThenBy(it => it.Sort).First().Id;
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
            VerifyCodeSugar v = new VerifyCodeSugar();
            //是否随机字体颜色
            v.SetIsRandomColor = true;
            //随机码的旋转角度
            v.SetRandomAngle = 4;
            //文字大小
            v.SetFontSize = 15;
            var questionList = new Dictionary<string, string>()
           {
               {"Sugar群号是多少？"," 225982985"},
               {"作者名字叫什么？","孙凯旋"},
               {"作者QQ号码是多少？","610262374" },
               {"ASDF23花木成畦","123ADFA" }
           };
            var questionItem = v.GetQuestion(questionList);//不赋值为随机验证码 例如： 1*2=? 这种
            v.SetVerifyCodeText = questionItem.Key;
            string value = questionItem.Value;
            var sm = SessionManager<string>.GetInstance();
            sm.Add(PubConst.SessionVerifyCode, value);
            //输出图片
            v.OutputImage(System.Web.HttpContext.Current.Response);
        }
        #endregion
    }
}