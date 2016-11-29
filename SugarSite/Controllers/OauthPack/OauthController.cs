using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SqlSugar;
namespace SugarSite.Controllers
{
    public class OauthController : Controller
    {
        // GET: Oauth
        public ActionResult CallBack(string state)
        {
            if (state == "QQ")
            {
               
            }
            else {

            }
            foreach (var item in SqlSugarTool.GetParameterDictionary())
            {
                Response.Write(item.Key+"&nbsp;"+item.Value+"<br>");
            }
            return View();
        }
    }
}