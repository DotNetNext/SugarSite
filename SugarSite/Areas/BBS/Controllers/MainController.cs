using Infrastructure.Dao;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SugarSite.Areas.BBS.Controllers
{
    public class MainController : BaseController
    {
        public MainController(DbService s) : base(s) { }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Ask()
        {
            return View();
        }
    }
}