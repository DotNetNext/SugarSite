using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SqlSugar;
using SyntacticSugar;
using Infrastructure.Tool;
using Infrastructure.Pub;
using System.Data.SqlClient;

namespace Infrastructure.Dao
{
    /// <summary>
    ///数据访问层
    /// </summary>
    public class DbService : IDisposable
    {
        private SqlSugarClient _db;
        public DbService()
        {
            _db = DbConfig.GetDbInstance();
        }

        /// <summary>
        /// 服务命令
        /// </summary>
        /// <typeparam name="Outsourcing">外包对象</typeparam>
        /// <param name="func"></param>
        public void Command<Outsourcing>(Action<Outsourcing> func) where Outsourcing : class, new() 
        {
            try
            {
                var o = new Outsourcing();
                func(o);
                o = null;//及时释放对象 
                //_db 会在http请求结束前执行 dispose 
            }
            catch (Exception ex)
            {
                //在这里可以处理所有controller的异常
                //获错误写入日志
                WriteExMessage(ex);
                throw ex;
            }
        }

        /// <summary>
        /// 服务命令
        /// </summary>
        /// <typeparam name="Outsourcing">外包对象</typeparam>
        /// <typeparam name="Api">接口</typeparam>
        /// <param name="func"></param>
        public void Command<Outsourcing, Api>(Action<Outsourcing, RestApi<Api>> func) where Outsourcing : class, new() where Api : class, new()
        {
            try
            {
                var o = new Outsourcing();
                var api = new Tool.RestApi<Api>();
                func(o, api);
                o = null;//及时释放对象 
                //_db 会在http请求结束前执行 dispose 
            }
            catch (Exception ex)
            {
                //在这里可以处理所有controller的异常
                //获错误写入日志
                WriteExMessage(ex);
                throw ex;
            }
        }

        /// <summary>
        /// 服务命令
        /// </summary>
        /// <typeparam name="Outsourcing">外包对象</typeparam>
        /// <param name="func"></param>
        public void Command<Outsourcing>(Action<SqlSugarClient, Outsourcing> func) where Outsourcing : class, new() 
        {
            try
            {
                var o = new Outsourcing();
                func(_db, o);
                o = null;//及时释放对象 
                //_db 会在http请求结束前执行 dispose 
            }
            catch (Exception ex)
            {
                //在这里可以处理所有controller的异常
                //获错误写入日志
                WriteExMessage(ex);
                throw ex;
            }
        }

        /// <summary>
        /// 服务命令
        /// </summary>
        /// <typeparam name="Outsourcing">外包对象</typeparam>
        /// <typeparam name="Api">接口</typeparam>
        /// <param name="func"></param>
        public void Command<Outsourcing, Api>(Action<SqlSugarClient, Outsourcing, RestApi<Api>> func) where Outsourcing : class, new() where Api : class, new()
        {
            try
            {
                var o = new Outsourcing();
                var api = new Tool.RestApi<Api>();
                func(_db, o, api);
                o = null;//及时释放对象 
                //_db 会在http请求结束前执行 dispose 
            }
            catch (Exception ex)
            {
                //在这里可以处理所有controller的异常
                //获错误写入日志
                WriteExMessage(ex);
                throw ex;
            }
        }

        /// <summary>
        /// 将错误信息写入日志
        /// </summary>
        /// <param name="ex"></param>
        private void WriteExMessage(Exception ex)
        {
            Dispose();
            PubMethod.WirteExp(ex);

            var context = System.Web.HttpContext.Current;
            var url=context.Request.Url;

            var errorUrl = FileSugar.GetMapPath("~/error_url/log.txt");
            if (FileSugar.IsExistFile(errorUrl).IsFalse())
            {
                FileSugar.CreateFile(errorUrl);
            }

            FileSugar.AppendText(errorUrl, DateTime.Now.ToString()); 
            FileSugar.AppendText(errorUrl, url.ToString());
            if (context.Request.QueryString.Count > 0)
            {
                StringBuilder sb = new StringBuilder();
                foreach (var item in context.Request.QueryString.Keys)
                {
                    sb.Append(item.ToString() + "_" + context.Request.QueryString[item.ToString()] + "|");
                }
                FileSugar.AppendText(errorUrl, sb.ToString());
            }
            if (context.Request.Form.Count > 0)
            {
                StringBuilder sb = new StringBuilder();
                foreach (var item in context.Request.Form.Keys)
                {
                    sb.Append(item.ToString() + "_" + context.Request.Form[item.ToString()] + "|");
                }
                FileSugar.AppendText(errorUrl, sb.ToString());
            }
            FileSugar.AppendText(errorUrl, context.Request.ServerVariables.Get("Remote_Addr").ToString());
            FileSugar.AppendText(errorUrl, context.Request.ServerVariables.Get("Remote_Host").ToString());
            FileSugar.AppendText(errorUrl, context.Request.Browser.Platform);
            FileSugar.AppendText(errorUrl, "");
            SqlConnection.ClearAllPools();
            GC.Collect();
        }

        public void Dispose()
        {
            if (_db != null)
            {
                _db.Dispose();
                _db = null;
            }
        }
    }
}