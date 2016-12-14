using SyntacticSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Pub
{
    public class PubMethod
    {
        /// <summary>
        /// 写入日志文件
        /// </summary>
        /// <param name="ex"></param>
        public static void WirteExp(Exception ex)
        {
            try
            {
                var logPath = FileSugar.MergeUrl(
                       FileSugar.GetMapPath("~/"),
                       "log",
                       DateTime.Now.ToString("yyyy-MM-dd") + ".txt"
                       );
                if (FileSugar.IsExistFile(logPath).IsFalse())
                {
                    FileSugar.CreateFile(logPath);
                }
                FileSugar.AppendText(logPath, "***********{0}{1}***********".ToFormat("开始:", DateTime.Now));
                FileSugar.AppendText(logPath, ex.Message);
                FileSugar.AppendText(logPath, "***********{0}***********\r\n".ToFormat("结束"));
            }
            catch
            {

            }
        }

        public static string RemoveAllSpace(string str)
        {
            if (str.IsNullOrEmpty()) return str;
            return str.Trim().Replace(" ", "_");
        }

        /// <summary>
        /// 获取默认头像
        /// </summary>
        /// <param name="rootUrl"></param>
        /// <param name="avatar"></param>
        /// <returns></returns>
        public static string GetDefaultAvatar(string rootUrl, string avatar)
        {
            if (avatar.IsNullOrEmpty())
            {
                return rootUrl + "_theme/default.jpg";
            }
            else
            {
                if (avatar.ToLower().IsContainsIn("http"))
                {
                    return avatar;
                }
                else
                {
                    return rootUrl.TryToString().TrimEnd('/') +"/"+ avatar.TrimStart('/');
                }
            }
        }
    }
}
