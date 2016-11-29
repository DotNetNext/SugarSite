using Infrastructure.DbModel;
using SyntacticSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SugarSite.Areas.BBS.Controllers
{
    public class MainOutsourcing
    {
        public BBS_Posts GetPosts(short fid, string content,UserInfo _userInfo)
        {
            return new BBS_Posts()
            {
                Fid = fid,
                Ip = RequestInfo.UserAddress,
                Posterid = _userInfo.Id,
                Poster = _userInfo.NickName,
                Message = content
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
    }
}