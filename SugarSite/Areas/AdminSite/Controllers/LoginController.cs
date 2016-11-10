using Infrastructure.Dao;
using Infrastructure.Pub;
using Infrastructure.ViewModels;
using SqlSugar;
using SugarSite.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Infrastructure.DbModel;
using SyntacticSugar;
namespace SugarSite.Areas.AdminSite.Controllers
{
    public class LoginController : BaseController
    {
        public LoginController(DbService s) : base(s) { }
        // GET: AdminSite/Login
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult Submit(string userName, string password, string code)
        {
            var model = new ResultModel<string>();
            _service.Command<Outsourcing>((db, o) =>
            {
                var sm = SessionManager<string>.GetInstance();
                var severCode = sm[PubConst.SessionVerifyCode];
                if (severCode == code)
                {
                    password = new EncryptSugar().MD5(password);
                    var isLogin= db.Queryable<UserInfo>().FirstOrDefault(it => it.UserName == userName && it.Password == password)!=null;
                    model.Status = isLogin ? "1" : "3";
                    if (model.Status == "3") {
                        model.ResultInfo = "用户名密码不正确！";
                    }
                }
                else
                {
                    model.Status = "2";
                    model.ResultInfo = "验证码不正确!";
                }
            });
            return Json(model);
        }
    }
}