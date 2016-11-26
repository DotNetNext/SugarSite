using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Web;

namespace OAuth.Tools
{
    /// <summary>
    /// 授权基类
    /// </summary>
    public abstract class OAuth2Base
    {
        protected WebClient wc = new WebClient();
        public OAuth2Base()
        {
            wc = new WebClient();
            wc.Encoding = Encoding.UTF8;
            wc.Headers.Add("Pragma", "no-cache");
        }

        #region 基础属性
        /// <summary>
        /// 返回的开放ID。
        /// </summary>
        public string openID = string.Empty;
        /// <summary>
        /// 访问的Token
        /// </summary>
        public string token = string.Empty;
        /// <summary>
        /// 过期时间
        /// </summary>
        public DateTime expiresTime;

        /// <summary>
        /// 第三方账号昵称
        /// </summary>
        public string nickName = string.Empty;

        /// <summary>
        /// 第三方账号头像地址
        /// </summary>
        public string headUrl = string.Empty;
        /// <summary>
        /// 首次请求时返回的Code
        /// </summary>
        internal string code = string.Empty;
        internal abstract OAuthServer server
        {
            get;
        }

        #endregion

        #region 非公开的请求路径和Logo图片地址。

        internal abstract string OAuthUrl
        {
            get;
        }
        internal abstract string TokenUrl
        {
            get;
        }
        internal abstract string ImgUrl
        {
            get;
        }
        #endregion

        #region WebConfig对应的配置【AppKey、AppSercet、CallbackUrl】
        internal string AppKey
        {
            get
            {
                return Tool.GetConfig(server.ToString() + ".AppKey");
            }
        }
        internal string AppSercet
        {
            get
            {
                return Tool.GetConfig(server.ToString() + ".AppSercet");
            }
        }
        internal string CallbackUrl
        {
            get
            {
                return Tool.GetConfig(server.ToString() + ".CallbackUrl");
            }
        }
        #endregion

        #region 基础方法

        /// <summary>
        /// 获得Token
        /// </summary>
        /// <returns></returns>
        protected string GetToken(string method)
        {
            string result = string.Empty;
            try
            {
                string para = "grant_type=authorization_code&client_id=" + AppKey + "&client_secret=" + AppSercet + "&code=" + code + "&state=" + server;
                para += "&redirect_uri=" + HttpUtility.UrlEncode(CallbackUrl) + "&rnd=" + DateTime.Now.Second;
                if (method == "POST")
                {
                    if (string.IsNullOrEmpty(wc.Headers["Content-Type"]))
                    {
                        wc.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
                    }
                    result = wc.UploadString(TokenUrl, method, para);
                }
                else
                {
                    result = wc.DownloadString(TokenUrl + "?" + para);
                }
            }
            catch (Exception err)
            {
                throw err;
            }
            return result;
        }

        /// <summary>
        /// 获取是否通过授权。
        /// </summary>
        public abstract bool Authorize();
        /// <param name="bindAccount">返回绑定的账号（若未绑定返回空）</param>
        public bool Authorize(out OAuth2Account account)
        {
            account = null;
            if (Authorize())
            {
                account = GetBindAccount();
                return true;
            }
            return false;
        }

        #endregion

        #region 关联绑定账号

        /// <summary>
        /// 读取已经绑定的账号
        /// </summary>
        /// <returns></returns>
        public OAuth2Account GetBindAccount()
        {
            OAuth2Account oa = new OAuth2Account();
            oa.Token = token;
            oa.ExpireTime = expiresTime;
            oa.NickName = nickName;
            oa.HeadUrl = headUrl;
            oa.BindAccount="";
            return oa;
        }

        /// <summary>
        /// 添加绑定账号
        /// </summary>
        /// <param name="bindAccount"></param>
        /// <returns></returns>
        public bool SetBindAccount(string bindAccount)
        {
            bool result = false;
            if (!string.IsNullOrEmpty(openID) && !string.IsNullOrEmpty(token) && !string.IsNullOrEmpty(bindAccount))
            {
                OAuth2Account oa = new OAuth2Account();
                oa.OAuthServer = server.ToString();
                oa.Token = token;
                oa.OpenID = openID;
                oa.ExpireTime = expiresTime;
                oa.BindAccount = bindAccount;
                oa.NickName = nickName;
                oa.HeadUrl = headUrl;
                result = true;//oa.Insert 执行添加操作
            }
            return result;
        }
        #endregion
    }

    /// <summary>
    /// 提供授权的服务商
    /// </summary>
    public enum OAuthServer
    {
        /// <summary>
        /// 新浪微博
        /// </summary>
        SinaWeiBo,
        /// <summary>
        /// 腾讯QQ
        /// </summary>
        QQ,
        /// <summary>
        /// 淘宝网
        /// </summary>
        TaoBao,
        /// <summary>
        /// 人人网（未支持）
        /// </summary>
        RenRen,
        /// <summary>
        /// 腾讯微博（未支持）
        /// </summary>
        QQWeiBo,
        /// <summary>
        /// 开心网（未支持）
        /// </summary>
        KaiXin,
        /// <summary>
        /// 飞信（未支持）
        /// </summary>
        FeiXin,
        /// <summary>
        /// 
        /// </summary>
        None,
    }
}
