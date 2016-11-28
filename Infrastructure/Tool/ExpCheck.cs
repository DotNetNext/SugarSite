using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Tool
{
    /// <summary>
    /// ** 描述：验证失败，则抛出异常
    /// ** 创始时间：2015-7-19
    /// ** 修改时间：-
    /// ** 作者：sunkaixuan
    /// ** 修改人：sunkaixuan
    /// ** 使用说明：
    /// </summary>
    public class ExpCheck
    {
        /// <summary>
        /// 使用指定的错误消息初始化 System.Exception 类的新实例。
        /// </summary>
        /// <param name="isException">true则引发异常</param>
        /// <param name="message">错误信息</param>
        /// <param name="args">参数</param>
        public static void Exception(bool isException, string message, params string[] args)
        {
            if (isException)
                throw new SiteException(string.Format(message, args));
        }
    }
}
