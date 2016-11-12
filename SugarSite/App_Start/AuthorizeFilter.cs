using Infrastructure.DbModel;
using Infrastructure.Pub;
using SyntacticSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SugarSite
{
    //受权过滤器
    public class AuthorizeFilter : AuthorizeAttribute
    {

        public AuthorizeFilter()
        {

        }
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            var cm = CookiesManager<string>.GetInstance();
            if (!cm.ContainsKey(PubConst.UserUniqueKey)||cm[PubConst.UserUniqueKey].IsNullOrEmpty())
            {
                cm.Add(PubConst.UserUniqueKey, Guid.NewGuid().ToString(), cm.Day * 365);
            }
            CheckAdmin(filterContext);
        }

        /// <summary>
        /// 后台管理员验证
        /// </summary>
        /// <param name="filterContext"></param>
        private static void CheckAdmin(AuthorizationContext filterContext)
        {
            List<SystemAuthorizeModel> smList = new List<SystemAuthorizeModel>()
                {

                    new SystemAuthorizeModel() { SystemAuthorizeType= SystemAuthorizeType.Area, AreaName="AdminSite",UserKeyArray=new dynamic[] {true} }

                };
            AuthorizeService.PubControllerNames = new List<string>() { "Login" };//无需验证的控制器
            SystemAuthorizeErrorRedirect sr = new SystemAuthorizeErrorRedirect();

            sr.DefaultUrl = "/AdminSite/Login/Index";//没有权限都跳转到DefaultUrl

            AuthorizeService.Start(filterContext, smList, sr, () =>
            {
                var cm = CacheManager<UserInfo>.GetInstance();
                string uniqueKey = PubGet.GetUserKey;
                var isLogin = cm.ContainsKey(uniqueKey);
                return isLogin;
            });
        }
    }
}