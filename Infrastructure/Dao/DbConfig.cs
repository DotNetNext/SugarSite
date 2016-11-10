using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SqlSugar;
using SyntacticSugar;
namespace Infrastructure.Dao
{
    public class DbConfig
    {
        public static string ConnectionString = ConfigSugar.GetConfigString("DefaultConnection");
        public static SqlSugarClient GetDbInstance()
        {
            try
            {
                //这里可以动态根据cookies或session实现多库切换
                return new SqlSugarClient(ConnectionString);
            }
            catch (Exception ex)
            {
                throw new Exception("连接数据库出错，请检查您的连接字符串，和网络。 ex:".AppendString(ex.Message));
            }
        }
    }
}