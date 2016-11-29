using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Linq;
namespace OAuth.Tools
{
    public class UI
    {
        public static string GetHtml()
        {
            string link = "<a href=\"{0}\" target=\"_blank\">{1}</a> ";
            StringBuilder sb = new StringBuilder();
            foreach (KeyValuePair<string, OAuth2Base> ob in OAuth2Factory.ServerList)
            {
                if (!string.IsNullOrEmpty(ob.Value.AppKey))
                {
                    sb.AppendFormat(link, string.Format(ob.Value.OAuthUrl, ob.Value.AppKey, HttpUtility.UrlEncode(ob.Value.CallbackUrl), ob.Key), ob.Value.ImgUrl);
                }
            }
            return sb.ToString();
        }
        public static List<string> GetUrls()
        {
            List<string> reval = new List<string>();
            StringBuilder sb = new StringBuilder();
            foreach (KeyValuePair<string, OAuth2Base> ob in OAuth2Factory.ServerList)
            {
                reval.Add(string.Format(ob.Value.OAuthUrl, ob.Value.AppKey, HttpUtility.UrlEncode(ob.Value.CallbackUrl), ob.Key));
            }
            return reval;
        }
        public static string GetQQUrl()
        {
            return GetUrls().First(it => it.ToLower().Contains(OAuthServer.QQ.ToString().ToLower()));
        }
        public static string GetWbUrl()
        {
            return GetUrls().First(it => it.ToLower().Contains(OAuthServer.SinaWeiBo.ToString().ToLower()));
        }
    }

}
