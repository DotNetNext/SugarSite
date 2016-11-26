using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SugarSite.Areas.BBS.Controllers
{
    public class MainController : Controller
    {
        // GET: BBS/Main
        public ActionResult Index()
        {
            return View();
        }
    }
}