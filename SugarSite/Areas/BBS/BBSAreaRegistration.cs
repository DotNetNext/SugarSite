using System.Web.Mvc;

namespace SugarSite.Areas.BBS
{
    public class BBSAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "BBS";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "BBS_default_Index",
                "ask/{fid}",
                new { Controller = "Main", action = "Index", fid = UrlParameter.Optional }
            );
            context.MapRoute(
               "BBS_default_Detail",
               "ask/{fid}/{id}",
                new { Controller = "Main", action = "Detail", id = UrlParameter.Optional }
            );
            context.MapRoute(
              "BBS_default",
              "BBS/{controller}/{action}/{id}",
               new { action = "Index", id = UrlParameter.Optional }
        );
        }
    }
}