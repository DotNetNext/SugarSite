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
            if (strUrl.Contains(PubConst.SiteDomain.ToLower())&&!strUrl.Contains("www.")){ 
                Response.RedirectPermanent(strUrl.Replace(PubConst.SiteDomain, "www." + PubConst.SiteDomain));
            }
        }
    }
}
