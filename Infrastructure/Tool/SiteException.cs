using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Infrastructure.Tool
{
    /// <summary>
    /// ** 描述：Site自定义异常
    /// ** 创始时间：2015-7-13
    /// ** 修改时间：-
    /// ** 作者：sunkaixuan
    /// ** 使用说明：
    /// </summary>
    public class SiteException : Exception
    {
        /// <summary>
        /// Site异常
        /// </summary>
        /// <param name="message">错误信息</param>
        public SiteException(string message)
            : base(message)
        {

        }
        /// <summary>
        /// Site异常
        /// </summary>
        /// <param name="message">错误信息</param>
        /// <param name="sql">ORM生成的SQL</param>
        public SiteException(string message, string sql)
            : base(GetMessage(message, sql))
        {

        }
        /// <summary>
        /// Site异常
        /// </summary>
        /// <param name="message">错误信息</param>
        /// <param name="sql">ORM生成的SQL</param>
        /// <param name="pars">错误函数的参数</param>
        public SiteException(string message, string sql, object pars)
            : base(GetMessage(message, sql, pars))
        {

        }
        /// <summary>
        /// Site异常
        /// </summary>
        /// <param name="message">错误信息</param>
        /// <param name="pars">错误函数的参数</param>
        public SiteException(string message, object pars)
            : base(GetMessage(message, pars))
        {

        }

        private static string GetMessage(string message, object pars)
        {
            var parsStr = string.Empty; ;
            if (pars != null)
            {
                parsStr = JsonConverter.Serialize(pars);
            }
            var reval = GetLineMessage("错误信息", message) + GetLineMessage("函数参数", parsStr);
            return reval;

        }


        private static string GetMessage(string message, string sql, object pars)
        {
            if (pars == null)
            {
                return GetMessage(message, sql);
            }
            else
            {
                var reval = GetLineMessage("错误信息         ", message) + GetLineMessage("ORM生成的Sql", sql) + GetLineMessage("函数参数        ", JsonConverter.Serialize(pars));
                return reval;
            }
        }


        private static string GetMessage(string message, string sql)
        {
            var reval = GetLineMessage("错误信息         ", message) + GetLineMessage("ORM生成的Sql", sql);
            return reval;
        }

        private static string GetLineMessage(string key, string value)
        {
            return string.Format("{0} ： 【{1}】\r\n", key, value);
        }
    }
}
