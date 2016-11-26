using System;
using System.Collections.Generic;
using System.Text;

namespace OAuth.Tools
{
    class TaoBaoAuth : OAuth2Base
    {
        internal override OAuthServer server
        {
            get
            {
                return OAuthServer.TaoBao;
            }
        }
        internal override string ImgUrl
        {
            get
            {
                return "<img align='absmiddle' src=\"/template/v1/img/oauth_taobao.png\" /> 淘宝";
            }
        }
        internal override string OAuthUrl
        {
            get
            {
                return "https://oauth.taobao.com/authorize?response_type=code&client_id={0}&redirect_uri={1}&state={2}";
            }
        }
        internal override string TokenUrl
        {
            get
            {
                return "https://oauth.taobao.com/token";
            }
        }
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
                        token = Tool.GetJosnValue(result, "access_token");
                        if (!string.IsNullOrEmpty(token))
                        {
                            double d = 0;
                            if (double.TryParse(Tool.GetJosnValue(result, "expires_in"), out d))
                            {
                                expiresTime = DateTime.Now.AddSeconds(d);
                            }
                            //读取OpenID
                            openID = Tool.GetJosnValue(result, "taobao_user_id");
                            nickName = Tool.GetJosnValue(result, "taobao_user_nick");
                            return true;

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
