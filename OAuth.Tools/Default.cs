using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OAuth.Tools
{
    class Default
    {
            #region 集成登录
            //OAuth2Base ob = OAuth2Factory.Current;//获取当前的授权类型，如果成功，则会缓存到Session中。
            //if (ob != null) //说明用户点击了授权，并跳回登陆界面来
            //{
            //    OAuth2Account account = null;
            //    if (ob.Authorize(out account))//检测是否授权成功，并返回绑定的账号（具体是绑定ID还是用户名，你的选择）
            //    {
            //        if (account.BindAccount != null && !"".Equals(account.BindAccount))//已绑定账号，直接用该账号设置登陆。
            //        {
            //            //UserLogin ul = new UserLogin();
            //            //if (ul.Login(account))
            //            //{
            //            //    Response.Write("已绑定账号");
            //            //}
            //            //else
            //            //{
            //            //    Response.Write("人人人");
            //            //}
            //        }
            //        else // 未绑定账号，引导提示用户绑定账号。
            //        {
            //            Response.Write("========" + account.NickName);
            //        }
            //    }
            //    else
            //    {
            //        Response.Write("没获取授权");
            //    }
            //}
            //else // 读取授权失败。
            //{
            //    Response.Write("提示用户重试，或改用其它社区方法登陆。");
            //    //提示用户重试，或改用其它社区方法登陆。
            //}
            #endregion
    }
}
