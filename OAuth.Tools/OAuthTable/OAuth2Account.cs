using System;
using System.Collections.Generic;
using System.Text;

namespace OAuth.Tools
{
    public class OAuth2Account
    {
        public OAuth2Account()
        {
        }
        private int _ID;

        public int ID
        {
            get
            {
                return _ID;
            }
            set
            {
                _ID = value;
            }
        }
        private string _OAuthServer;
        /// <summary>
        /// 授权的服务类型
        /// </summary>
        public string OAuthServer
        {
            get
            {
                return _OAuthServer;
            }
            set
            {
                _OAuthServer = value;
            }
        }
        private string _Token;
        /// <summary>
        /// 保存的Token
        /// </summary>
        public string Token
        {
            get
            {
                return _Token;
            }
            set
            {
                _Token = value;
            }
        }
        private string _OpenID;
        /// <summary>
        /// 保存对应的ID
        /// </summary>
        public string OpenID
        {
            get
            {
                return _OpenID;
            }
            set
            {
                _OpenID = value;
            }
        }
        private string _BindAccount;

        private DateTime _ExpireTime;
        /// <summary>
        /// 过期时间
        /// </summary>
        public DateTime ExpireTime
        {
            get
            {
                return _ExpireTime;
            }
            set
            {
                _ExpireTime = value;
            }
        }

        private string _NickName;
        /// <summary>
        /// 返回的第三方昵称
        /// </summary>
        public string NickName
        {
            get
            {
                return _NickName;
            }
            set
            {
                _NickName = value;
            }
        }
        private string _HeadUrl;
        /// <summary>
        /// 返回的第三方账号对应的头像地址。
        /// </summary>
        public string HeadUrl
        {
            get
            {
                return _HeadUrl;
            }
            set
            {
                _HeadUrl = value;
            }
        }


        /// <summary>
        /// 绑定的账号
        /// </summary>
        public string BindAccount
        {
            get
            {
                return _BindAccount;
            }
            set
            {
                _BindAccount = value;
            }
        }
    }
}
