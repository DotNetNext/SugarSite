using Infrastructure.Dao;
using Infrastructure.DbModel;
using Infrastructure.Pub;
using SyntacticSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SqlSugar;
namespace SugarSite
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
            ViewBag.IsLogin = IsLogin;
            ViewBag.User = _userInfo;
        }

        protected bool IsLogin
        {
            get
            {
                return (_userInfo != null && _userInfo.Id > 0);
            }
        }

        protected Dictionary<string, string> GetVerifyCode
        {
            get
            {
                var key = PubConst.SessionVerifyCodeDetails;
                var cm = CacheManager<Dictionary<string, string>>.GetInstance();
                if (cm.ContainsKey(key))
                {
                    return cm[key];
                }
                else
                {
                    Dictionary<string, string> reval = null;
                    _service.Command<BaseOutsourcing>((db, o) =>
                    {
                        reval = db.Queryable<VerifyCode>().Select<KeyValuePair<string, string>>("Problem,Answer").ToList().ToDictionary(it => it.Key, it => it.Value);
                    });
                    cm.Add(key, reval, cm.Day);
                    return reval;
                }
            }
        }
    }

    public class BaseOutsourcing
    {
    }
}