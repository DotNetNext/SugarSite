using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SqlSugar;
using SyntacticSugar;
using Infrastructure.Tool;

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
        ///服务命令
        /// </summary>
        /// <typeparam name="Outsourcing">外包服务类型</typeparam>
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
        ///服务命令
        /// </summary>
        /// <typeparam name="Outsourcing">外包服务类型</typeparam>
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
        ///服务命令
        /// </summary>
        /// <typeparam name="Outsourcing">外包服务类型</typeparam>
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
        ///服务命令
        /// </summary>
        /// <typeparam name="Outsourcing">外包服务类型</typeparam>
        /// <param name="func"></param>
        public void Command<Outsourcing, Api1, Api2>(Action<SqlSugarClient, Outsourcing, Tool.RestApi<Api1>, Tool.RestApi<Api2>> func) where Outsourcing : class, new() where Api1 : class, new() where Api2 : class, new()
        {
            try
            {
                var o = new Outsourcing();
                var api1 = new Tool.RestApi<Api1>();
                var api2 = new Tool.RestApi<Api2>();
                func(_db, o, api1, api2);
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
        private static void WriteExMessage(Exception ex)
        {
            var logPath = FileSugar.MergeUrl(
                FileSugar.GetMapPath("~/"),
                "log",
                DateTime.Now.ToString("yyyy-MM-dd.txt")
                );
            FileSugar.AppendText(logPath, "***********{0}{1}***********".ToFormat("开始:", DateTime.Now));
            FileSugar.AppendText(logPath, ex.Message);
            FileSugar.AppendText(logPath, "***********{0}***********\r\n".ToFormat("结束"));
        }


        public void Dispose()
        {
            if (_db != null)
            {
                _db.Dispose();
            }
        }
    }
}