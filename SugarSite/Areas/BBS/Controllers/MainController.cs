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

        public ActionResult Index(int? fid)
        {
            ViewBag.NewUserList = base.GetNewUserList;
            ViewBag.ForList = base.GetForumsList;
            return View();
        }

        public ActionResult Ask()
        {
            ViewBag.ForList = base.GetForumsList;
            return View();
        }

        [HttpPost]
        public JsonResult AskSubmit()
        {
            return null;
        }
    }
}