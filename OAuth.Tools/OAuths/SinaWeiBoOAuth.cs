using System;
using System.Collections.Generic;
using System.Text;
using System.Web.Script.Serialization;

namespace OAuth.Tools
{
    public class SinaWeiBoOAuth : OAuth2Base
    {
        internal override OAuthServer server
        {
            get
            {
                return OAuthServer.SinaWeiBo;
            }
        }
        internal override string ImgUrl
        {
            get
            {
                return "<img align='absmiddle' src=\"/template/v1/img/oauth_sinaweibo.png\" /> 微博";
            }
        }
        internal override string OAuthUrl
        {
            get
            {
                return "https://api.weibo.com/oauth2/authorize?response_type=code&client_id={0}&redirect_uri={1}&state={2}";
            }
        }
        internal override string TokenUrl
        {
            get
            {
                return "https://api.weibo.com/oauth2/access_token";
            }
        }
        internal string UserInfoUrl = "https://api.weibo.com/2/users/show.json?access_token={0}&uid={1}";
        public override bool Authorize()
        {
            if (!string.IsNullOrEmpty(code))
            {
                string result = GetToken("POST");//一次性返回数据。
                //分解result;
                if (!string.IsNullOrEmpty(result))
                {
                    try
                    {
                        JavaScriptSerializer serializer = new JavaScriptSerializer();//引用 System.Web.Extensions
                        Dictionary<string, object> dictionaryObject = (Dictionary<string, object>)serializer.DeserializeObject(result);
                        if (dictionaryObject != null && dictionaryObject.Count > 0)
                        {
                            foreach (KeyValuePair<string, object> item in dictionaryObject)
                            {
                                switch (item.Key)
                                {
                                    case "access_token":
                                        token = item.Value == null ? "" : item.Value.ToString();
                                        break;
                                    case "expires_in":
                                        double d = 0;
                                        if (double.TryParse(item.Value == null ? "" : item.Value.ToString(), out d) && d > 0)
                                        {
                                            expiresTime = DateTime.Now.AddSeconds(d);
                                        }
                                        break;
                                    case "uid":
                                        openID = item.Value == null ? "" : item.Value.ToString();
                                        break;
                                }
                            }
                            if (!string.IsNullOrEmpty(token) && !string.IsNullOrEmpty(openID))
                            {
                                //获取微博昵称和头像
                                result = wc.DownloadString(string.Format(UserInfoUrl, token, openID));
                                if (!string.IsNullOrEmpty(result)) //返回：callback( {"client_id":"YOUR_APPID","openid":"YOUR_OPENID"} ); 
                                {
                                    nickName = Tool.GetJosnValue(result, "screen_name");
                                    headUrl = Tool.GetJosnValue(result, "profile_image_url");
                                    return true;
                                }
                            }
                            else
                            {
                                //WriteLogToTxt
                            }
                        }
                    }
                    catch (Exception err)
                    {
                        throw err;
                    }
                }
            }
            return false;
        }
    }
}
