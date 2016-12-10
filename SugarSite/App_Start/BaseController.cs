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
using Infrastructure.ViewModels.BBS;

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
            if (IsLogin == false)
            {
                var request = System.Web.HttpContext.Current.Request;
                var appPath = request.AppRelativeCurrentExecutionFilePath;
                var isAgainLogin = appPath.TryToString().ToLower().IsContainsIn("~/ask", "~/bbs");
                string[] keys = request.QueryString.AllKeys;
                var isLogout = keys.IsValuable() && keys.Contains("isLogout");
                if (isAgainLogin && isLogout.IsFalse())
                {
                    _service.Command<BaseOutsourcing>((db, o) =>
                    {
                        //近一个月
                        var userHistory = db.Queryable<LoginHistory>()
                            .Where(it => it.CreateDate > DateTime.Now.AddDays(-30))
                            .Where(it => it.UniqueKey == uniqueKey)
                            .OrderBy(it => it.CreateDate, OrderByType.Desc)
                            .SingleOrDefault();
                        if (userHistory != null)
                        {
                            var user = db.Queryable<UserInfo>().Single(it => it.Id == userHistory.Uid);
                            _userInfo = user;
                            //重新将登录信息加到缓存
                            CacheManager<UserInfo>.GetInstance().Add(uniqueKey, user, int.MaxValue);
                            ViewBag.IsLogin = IsLogin;
                        }
                    });
                }
            }
            ViewBag.User = _userInfo;
        }

        public bool IsLogin
        {
            get
            {
                return (_userInfo != null && _userInfo.Id > 0);
            }
        }

        public void AddUpdateMailCache(string updateKey)
        {
            string key = PubConst.SessionUpdateUserMail;
            var cm = CacheManager<List<string>>.GetInstance();
            var list = new List<string>();
            if (cm.ContainsKey(key))
            {
                list = cm[key];
            }
            if (list.Contains(updateKey).IsFalse())
            {
                list.Add(updateKey);
            }
            cm.Add(key, list, cm.Day * 365);
        }
        public void RestCurrentUserCache() {
            string uniqueKey = PubGet.GetUserKey;
            var cm = CacheManager<UserInfo>.GetInstance();
            if (cm.ContainsKey(uniqueKey)) {
                _service.Command<BaseOutsourcing>((db, o) =>
                {
                    var user = db.Queryable<UserInfo>().InSingle(_userInfo.Id);
                    cm.Add(uniqueKey, user, cm.Day);
                });
            }
        }
        public void UpdateMailCache(int userId,string mail)
        {
            var cm = CacheManager<List<string>>.GetInstance();
            string key = PubConst.SessionUpdateUserMail;
            if (cm.ContainsKey(key))
            {
                var listKey=cm[key];
                if (listKey.IsValuable()) {
                    var cmUser = CacheManager<UserInfo>.GetInstance();
                    string uniqueKey = PubGet.GetUserKey;
                    foreach (var item in listKey)
                    {
                        if (cmUser.ContainsKey(item)) {
                            var userInfo = cmUser[item];
                            if (userInfo.Id == userId) {
                                userInfo.Email = mail;
                                cmUser.Add(item,userInfo,cm.Day*365);
                            }
                        }
                    }
                }
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
                    filtre = db.CurrentFilterKey;
                    db.CurrentFilterKey = null;
                    var date = DateTime.Now.ToString("yyyy-MM-dd").TryToDate();
                    string sql = @"select fid,count(1) as c from(
                                select fid from  BBS_Topics where fid>0 and [Postdatetime]>@d AND  (isdeleted=0  or isdeleted is null )
                                union all
                                select fid  from [dbo].[BBS_Posts] where fid>0 and [Postdatetime]>@d AND  (isdeleted=0  or isdeleted is null ))t  group by fid ";
                    reval = db.SqlQuery<KeyValuePair<string, string>>(sql, new { d = date }).ToDictionary(it => Convert.ToInt32(it.Key), it => it.Value);
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

        /// <summary>
        /// 论坛右边数据
        /// </summary>
        public RightRelust GetBBS_Right
        {
            get
            {
                var key = PubConst.SessionBBSRight;
                var cm = CacheManager<RightRelust>.GetInstance();
                if (cm.ContainsKey(key)) return cm[key];
                else
                {
                    RightRelust reval = new RightRelust();
                    _service.Command<BaseOutsourcing>((db, o) =>
                    {
                        reval.DocMasterList = db.Queryable<DocMaster>().OrderBy(it => it.Sort).ToList();
                        reval.UserRepliesList = db.Queryable<V_UserStatisticsInfo>()
                        .OrderBy(it => it.RepliesCount, OrderByType.Desc)
                        .Take(8).ToList();
                        reval.UserTopicsInfoList = db.Queryable<V_UserStatisticsInfo>()
                        .OrderBy(it => it.TopicsCount, OrderByType.Desc)
                        .Take(8).ToList();
                    });
                    cm.Add(key, reval, cm.Minutes * 8);//8分钟
                    return reval;
                }
            }
        }
    }

    public class BaseOutsourcing
    {
    }
}