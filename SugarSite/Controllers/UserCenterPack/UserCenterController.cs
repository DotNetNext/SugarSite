using Infrastructure.Dao;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SugarSite.Controllers
{
    public class UserCenterController : BaseController
    {
        public UserCenterController(DbService s) : base(s) { }

        public ActionResult Index()
        {
            if (base.IsLogin == false)
            {
                return this.Redirect("/UserCenter");
            }
            return View();
        }
    }
}