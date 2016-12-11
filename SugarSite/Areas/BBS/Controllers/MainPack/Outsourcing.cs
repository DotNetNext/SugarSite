using Infrastructure.DbModel;
using Infrastructure.Pub;
using SqlSugar;
using SyntacticSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SugarSite.Areas.BBS.Controllers
{
    public class MainOutsourcing
    {
        public BBS_Posts GetPosts(short fid, string content, UserInfo _userInfo, BBS_Topics top)
        {
            return new BBS_Posts()
            {
                Fid = fid,
                Ip = RequestInfo.UserAddress,
                Posterid = _userInfo.Id,
                Poster = _userInfo.NickName,
                Message = content,
                Postdatetime = DateTime.Now,
                Title = top.Title,
                Tid = top.Tid,
                Lastedit = _userInfo.NickName
            };
        }

        public BBS_Topics GetTopics(short fid, string title, int rate, UserInfo _userInfo)
        {
            return new BBS_Topics()
            {
                Fid = fid,
                Title = title,
                Posterid = _userInfo.Id,
                Poster = _userInfo.NickName,
                Postdatetime = DateTime.Now,
                Rate = rate,
                PosterAvatar = _userInfo.Avatar
            };
        }

        /// <summary>
        /// 贴子查询分类
        /// </summary>
        public enum TopicsSearchType
        {
            最新 = 1,
            最新回复 = 2,
            我发 = 3,
            我回 = 4,
            收藏 = 5,
            精华 = 6,
            置顶 = 7
        }

        /// <summary>
        /// 获取贴子根据条件
        /// 1最新 2最新回复 3我发 4我回 5收藏 6精华
        /// </summary>
        /// <param name="orderBy"></param>
        /// <param name="db"></param>
        /// <param name="qureyable"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public Queryable<BBS_Topics> GetMainQueryable(int? orderBy, SqlSugarClient db, Queryable<BBS_Topics> qureyable, BaseController b)
        {
            if (b.IsLogin)
            {
                if (orderBy == TopicsSearchType.我发.TryToInt())
                {
                    qureyable = qureyable.Where(it => it.Posterid == b._userInfo.Id);
                }
                if (orderBy == TopicsSearchType.我回.TryToInt())
                {
                    var tidList = db.Queryable<BBS_Posts>().Where(it => it.Parentid > 0 && it.Posterid == b._userInfo.Id).Select<int>("distinct Tid").ToList();
                    if (tidList.IsValuable())
                    {
                        qureyable = qureyable.In(it => it.Tid, tidList);
                    }
                    else
                    {
                        qureyable = qureyable.In(it => it.Tid, "-1");
                    }
                }
                if (orderBy == TopicsSearchType.收藏.TryToInt())
                {
                    var tidList = db.Queryable<BBS_Favorites>().Where(it => it.Uid == b._userInfo.Id).Select(it => it.Tid).ToList();
                    if (tidList.IsValuable())
                    {
                        qureyable = qureyable.In(it => it.Tid, tidList);
                    }
                    else
                    {
                        qureyable = qureyable.In(it => it.Tid, "-1");
                    }
                }
            }
            if (orderBy == TopicsSearchType.最新.TryToInt())
            {
                qureyable = qureyable.OrderBy(it => it.Postdatetime, OrderByType.Desc);
            }
            else if (orderBy == TopicsSearchType.最新回复.TryToInt() || orderBy == TopicsSearchType.我回.TryToInt())
            {
                qureyable = qureyable.Where(it => it.Replies > 0).OrderBy(it => it.Lastpost, OrderByType.Desc);
            }
            else
            {
                qureyable = qureyable.OrderBy(it => it.Postdatetime, OrderByType.Desc);
            }

            return qureyable;
        }
        /// <summary>
        /// 发送邮件通知
        /// </summary>
        /// <param name="isLogin"></param>
        /// <param name="_userInfo"></param>
        /// <param name="tid"></param>
        /// <param name="p"></param>
        /// <param name="db"></param>
        internal void SendMail(UserInfo currentUser, int tid, BBS_Posts p, SqlSugarClient db)
        {
            var topic = db.Queryable<BBS_Topics>().Single(it => it.Tid == tid);
            var isOneUser = currentUser.Id == topic.Posterid;
            //发贴和回贴不是同一个人
            if (isOneUser.IsFalse())
            {
                var toUser = db.Queryable<UserInfo>().Single(it => it.Id == topic.Posterid);
                if (toUser.Email.IsEamil())
                {
                    var html = FileSugar.FileToString(FileSugar.GetMapPath("~/Template/mail/Replies.html")).Replace('\r', ' ').Replace('\n', ' ');
                    string toUserName = toUser.NickName;
                    string fromUserName = currentUser.NickName;
                    string toMail = toUser.Email;
                    MailSmtp ms = new MailSmtp(PubGet.GetEmailSmtp, PubGet.GetEmailUserName, PubGet.GetEmailPassword);
                    string url =RequestInfo.HttpDomain+ "/Ask/{0}/{1}#btnSubmit".ToFormat(topic.Fid,topic.Tid);
                    html = html.ToFormat(toUserName,fromUserName,topic.Title,DateTime.Now,url);
                    ms.Send(PubGet.GetEmailUserName, PubConst.SiteMailUserName, toMail, fromUserName + "回复了您的贴子："+topic.Title.Trim(), html);
                }
            }
            //处理@
        }
    }

}