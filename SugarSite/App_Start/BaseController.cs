using Infrastructure.Dao;
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
        protected BaseController(DbService s)
        {
            _service = s;
        }
    }
}