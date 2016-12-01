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
        public DbService _service;
        public UserInfo _userInfo;
        public BaseController(DbService s)
        {
            _service = s;
            string uniqueKey = PubGet.GetUserKey;
            _userInfo = CacheManager<UserInfo>.GetInstance()[uniqueKey];
            ViewBag.IsLogin = IsLogin;
            ViewBag.User = _userInfo;
        }

        public bool IsLogin
        {
            get
            {
                return (_userInfo != null && _userInfo.Id > 0);
            }
        }
        public List<BBS_Forums> GetForumsList
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
        public void RemoveForumsStatisticsCache()
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
        public Dictionary<int, string> GetForumsStatistics()
        {
            Dictionary<int, string> reval = null;
            var key = PubConst.SessionGetForumsStatistics;
            var cm = CacheManager<Dictionary<int, string>>.GetInstance();
            if (cm.ContainsKey(key)) return cm[key];
            else
            {
                _service.Command<BaseOutsourcing>((db, o) =>
                {
                    string filtre = null;
                    filtre=db.CurrentFilterKey;
                    db.CurrentFilterKey = null;
                    var date = DateTime.Now.ToString("yyyy-MM-dd").TryToDate();
                    string sql = @"select fid,count(1) as c from(
                                select fid from  BBS_Topics where fid>0 and [Postdatetime]>@d AND  (isdeleted=0  or isdeleted is null )
                                union all
                                select fid  from [dbo].[BBS_Posts] where fid>0 and [Postdatetime]>@d AND  (isdeleted=0  or isdeleted is null ))t  group by fid ";
                    reval=db.SqlQuery<KeyValuePair<string, string>>(sql,new {d=date }).ToDictionary(it =>Convert.ToInt32(it.Key), it => it.Value);
                    db.CurrentFilterKey = filtre;
                    cm.Add(key, reval, cm.Day);
                });
            }
            return reval;
        }
        /// <summary>
        /// 删除最新用户缓存
        /// </summary>
        public void RemoveNewUserListCache()
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
        public List<UserInfo> GetNewUserList
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
        public Dictionary<string, string> GetVerifyCode
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