using Infrastructure.DbModel;
using OAuth.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Infrastructure.Pub;
using SyntacticSugar;
using System.Net;
using System.Drawing.Imaging;
using System.IO;

namespace SugarSite.Controllers
{
    public class OauthOutsourcing
    {
        public UserInfo GetUser(OAuth2Base current, string pwd)
        {
            return new UserInfo()
            {
                Avatar = current.headUrl.TryToString().Replace(@"\/", @"/"),
                UserName = current.nickName,
                NickName = current.nickName,
                CreateTime = DateTime.Now,
                RoleId = PubEnum.RoleType.User.TryToInt(),
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

        internal void SaveAvatar(UserInfo user)
        {
            //将远程图片下载到本地
            if (user.Avatar.IsValuable() && user.Avatar.Contains("http://"))
            {
                string path = "/_theme/img/avatar{0}.jpg".ToFormat(user.Id);
                string savePath = FileSugar.GetMapPath("~" + path);
                WebClient my = new WebClient();
                byte[] mybyte;
                mybyte = my.DownloadData(user.Avatar);
                using (MemoryStream ms = new MemoryStream(mybyte))
                {
                    System.Drawing.Image img;
                    img = System.Drawing.Image.FromStream(ms);
                    img.Save(savePath, ImageFormat.Jpeg);//保存
                }
                user.Avatar = path;
            }
        }
    }
}