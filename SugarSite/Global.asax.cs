using Autofac;
using Autofac.Integration.Mvc;
using Infrastructure.Pub;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace SugarSite
{
     
    public class MvcApplication : System.Web.HttpApplication
    {
        public const string Version = "2.4";
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            #region 依赖注入
            var builder = new ContainerBuilder();
            //注册DomainServices
            var services = Assembly.Load("Infrastructure");
            builder.RegisterAssemblyTypes(services);
            builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly());
            var container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
            #endregion

        }
        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            //将不带www.的域名跳转到带www.的域名 (301跳转)
            string strUrl = Request.Url.ToString().Trim().ToLower();
            if (strUrl.Contains("codeisbug"))
            {
                var url = strUrl.Replace("codeisbug", "donet5");
                HttpContext.Current.Response.ContentType = "text/html;charset=utf-8";
                HttpContext.Current.Response.ContentEncoding = System.Text.Encoding.GetEncoding("utf-8"); //设置输出流为简体中文
                Response.Write("<html><title>孙凯旋个人主页</title> <body><p>孙凯旋个人主页</p> 该域名已停止使用 新地址  <a href='"+ url + "'> 点这里 </a> <p>当前网站备案号： 苏ICP备16056174号-1 </p> </html> </body>");
                Response.End();
            }
            if (strUrl.Contains(PubConst.SiteDomain.ToLower()) && !strUrl.Contains("www."))
            {
                Response.RedirectPermanent(strUrl.Replace(PubConst.SiteDomain, "www." + PubConst.SiteDomain));
            }
        }
    }
}
