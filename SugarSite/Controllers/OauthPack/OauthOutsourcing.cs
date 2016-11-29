using Infrastructure.DbModel;
using OAuth.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Infrastructure.Pub;
using SyntacticSugar;
namespace SugarSite.Controllers
{
    public class OauthOutsourcing
    {
        public  UserInfo GetUser(OAuth2Base current, string pwd)
        {
            return new UserInfo()
            {
                Avatar = current.headUrl.TryToString().Replace(@"\/",@"/"),
                UserName = current.nickName,
                NickName = current.nickName,
                CreateTime = DateTime.Now,
                RoleId=PubEnum.RoleType.User.TryToInt(),
                Password = pwd
            };
        }
        public UserOAuthMapping GetUserOauthMapping(OAuth2Base current, int id)
        {
            return new UserOAuthMapping()
            {
                UserId = id,
                AppId = current.openID,
                IsDeleted = false
            };
        }
    }
}