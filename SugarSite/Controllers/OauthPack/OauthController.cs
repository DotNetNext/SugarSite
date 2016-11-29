using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SqlSugar;
using OAuth.Tools;

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
            var current = OAuth2Factory.Current;
            Response.Write(current.headUrl += " " + current.openID);
            return View();
        }
    }
}