using System;
using System.Collections.Generic;
using System.Text;
using System.Web;

namespace OAuth.Tools
{
    public class QQOAuth : OAuth2Base
    {
        internal override OAuthServer server
        {
            get
            {
                return OAuthServer.QQ;
            }
        }
        internal override string ImgUrl
        {
            get
            {
                return "<img align='absmiddle' src=\"/template/v1/img/qq_logo.png\" alt='QQ登录' />&nbsp;&nbsp;<strong>QQ登录</strong>";
            }
        }
        internal override string OAuthUrl
        {
            get
            {
                return "https://graph.qq.com/oauth2.0/authorize?response_type=code&client_id={0}&redirect_uri={1}&state={2}";
            }
        }
        internal override string TokenUrl
        {
            get
            {
                return "https://graph.qq.com/oauth2.0/token";
            }
        }
        internal string OpenIDUrl = "https://graph.qq.com/oauth2.0/me?access_token={0}";
        internal string UserInfoUrl = "https://graph.qq.com/user/get_user_info?access_token={0}&oauth_consumer_key={1}&openid={2}";
        public override bool Authorize()
        {
            if (!string.IsNullOrEmpty(code))
            {
                string result = GetToken("GET");//一次性返回数据，QQ仅返回Token，还需要一次请求获取OpenID。//access_token=A5E175586196173434374BD3DBBAA5E8A3&expires_in=7776000
                //分解result;
                if (!string.IsNullOrEmpty(result))
                {
                    try
                    {
                        token = Tool.QueryString(result, "access_token");
                        if (!string.IsNullOrEmpty(token))
                        {
                            double d = 0;
                            if (double.TryParse(Tool.QueryString(result, "expires_in"), out d))
                            {
                                expiresTime = DateTime.Now.AddSeconds(d);
                            }
                            //读取OpenID
                            result = wc.DownloadString(string.Format(OpenIDUrl, token));
                            if (!string.IsNullOrEmpty(result)) //返回：callback( {"client_id":"YOUR_APPID","openid":"YOUR_OPENID"} ); 
                            {
                                openID = Tool.GetJosnValue(result, "openid");
                            }
                            if (!string.IsNullOrEmpty(openID))
                            {
                                //读取QQ账号和头像
                                result = wc.DownloadString(string.Format(UserInfoUrl, token, AppKey, openID));
                                if (!string.IsNullOrEmpty(result)) //返回：callback( {"client_id":"YOUR_APPID","openid":"YOUR_OPENID"} ); 
                                {
                                    nickName = Tool.GetJosnValue(result, "nickname");
                                    headUrl = Tool.GetJosnValue(result, "figureurl");
                                    return true;
                                }
                            }
                        }
                        else
                        {
                            //WriteLogToTxt
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
