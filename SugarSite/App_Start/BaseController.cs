using Infrastructure.Dao;
using Infrastructure.DbModel;
using Infrastructure.Pub;
using SyntacticSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SugarSite.Controllers
{

    public class BaseController : Controller
    {
        protected DbService _service;
        protected UserInfo _userInfo;
        protected BaseController(DbService s)
        {
            _service = s;
            string uniqueKey = PubGet.GetUserKey;
            _userInfo = CacheManager<UserInfo>.GetInstance()[uniqueKey];
        }
    }
}