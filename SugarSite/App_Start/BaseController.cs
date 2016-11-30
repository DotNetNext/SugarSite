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
        protected List<BBS_Forums> GetForumsList
        {
            get
            {
                var key = PubConst.SessionForumsList;
                var cm = CacheManager<List<BBS_Forums>>.GetInstance();
                if (cm.ContainsKey(key))
                {
                    return cm[key];
                }
                else
                {
                    List<BBS_Forums> reval = null;
                    _service.Command<BaseOutsourcing>((db, o) =>
                    {
                        reval = db.Queryable<BBS_Forums>().OrderBy(it => it.Displayorder, OrderByType.Asc).ToList();
                    });
                    cm.Add(key, reval, cm.Day);
                    return reval;
                }
            }
        }
        /// <summary>
        /// 移除新贴统计缓存
        /// </summary>
        /// <returns></returns>
        protected void RemoveForumsStatisticsCache()
        {
            var cm = CacheManager<Dictionary<int, string>>.GetInstance();
            var key = PubConst.SessionGetForumsStatistics;
            if (cm.ContainsKey(key))
            {
                cm.Remove(key);
            }
        }
        /// <summary>
        /// 获取新贴统计
        /// </summary>
        /// <returns></returns>
        protected Dictionary<int, string> GetForumsStatistics()
        {
            Dictionary<int, string> reval = null;
            var key = PubConst.SessionGetForumsStatistics;
            var cm = CacheManager<Dictionary<int, string>>.GetInstance();
            if (cm.ContainsKey(key)) return cm[key];
            else
            {
                _service.Command<BaseOutsourcing>((db, o) =>
                {
                    reval = db.Queryable<BBS_Topics>()
                    .GroupBy(it => it.Fid)
                     .Select<KeyValuePair<string, string>>("Fid,count(1) as Count ")
                     .ToList().ToDictionary(it =>Convert.ToInt32(it.Key), it => it.Value);
                    cm.Add(key, reval, cm.Day);
                });
            }
            return reval;
        }
        /// <summary>
        /// 删除最新用户缓存
        /// </summary>
        protected void RemoveNewUserListCache()
        {
            var key = PubConst.SessionNewUserList;
            var cm = CacheManager<List<UserInfo>>.GetInstance();
            if (cm.ContainsKey(key))
            {
                cm.Remove(key);
            }
        }
        /// <summary>
        /// 获取最新用户
        /// </summary>
        protected List<UserInfo> GetNewUserList
        {
            get
            {
                var key = PubConst.SessionNewUserList;
                var cm = CacheManager<List<UserInfo>>.GetInstance();
                if (cm.ContainsKey(key))
                {
                    return cm[key];
                }
                else
                {
                    List<UserInfo> reval = null;
                    _service.Command<BaseOutsourcing>((db, o) =>
                    {
                        reval = db.Queryable<UserInfo>().OrderBy(it => it.CreateTime, OrderByType.Desc).Take(8).ToList();
                    });
                    cm.Add(key, reval, cm.Day);
                    return reval;
                }
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