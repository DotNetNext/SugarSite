using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace SugarSite
{
    /// <summary>
    /// 系统授权服务
    /// 作者:sunkaixuan
    /// 时间:2015-10-25
    /// </summary>
    public class AuthorizeService
    {


        /// <summary>
        /// 无需验证的控制器名称
        /// </summary>
        public static  List<string> PubControllerNames = null;

        /// <summary>
        /// 启动系统授权
        /// </summary>
        /// <param name="filterContext"></param>
        /// <param name="SystemAuthorizeList">所有验证项</param>
        /// <param name="errorRediect">没有权限跳转地址</param>
        /// <param name="GetCurrentUserId">获取当前用户ID</param>
        public static void Start(AuthorizationContext filterContext, List<SystemAuthorizeModel> systemAuthorizeList, SystemAuthorizeErrorRedirect errorRediect, Func<object> GetCurrentUserKey)
        {


            if (errorRediect == null)
            {
                throw new ArgumentNullException("SystemAuthorizeService.Start.errorRediect");
            }
            if (systemAuthorizeList == null)
            {
                throw new ArgumentNullException("SystemAuthorizeService.Start.systemAuthorizeList");
            }

            //全部小写
            foreach (var it in systemAuthorizeList)
            {
                if (it.ControllerName != null)
                    it.ControllerName = it.ControllerName.ToLower();
                if (it.ActionName != null)
                    it.ActionName = it.ActionName.ToLower();
                if (it.AreaName != null)
                    it.AreaName = it.AreaName.ToLower();
            }


       

            //声名变量
            var context = filterContext.HttpContext;
            var request = context.Request;
            var response = context.Response;
            string actionName = filterContext.ActionDescriptor.ActionName.ToLower();
            string controllerName = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName.ToLower();
            string areaName = null;
            bool isArea = filterContext.RouteData.DataTokens["area"] != null;

            //是否有无需验证的控制器
            if (PubControllerNames != null && PubControllerNames.Count > 0)
            {
                //无需验证跳过
                if (PubControllerNames.Contains(controllerName)) {
                    return;
                }

            }


            //变量赋值
            if (isArea)
                areaName = filterContext.RouteData.DataTokens["area"].ToString().ToLower();


            //函数方法
            #region 函数方法
            Action<string, string, string> Redirect = (action, controller, area) =>
            {
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = controller, action = action, area = area }));
            };
            Action<string> RedirectUrl = url =>
            {
                filterContext.Result = new RedirectResult(url);
            };
            #endregion


            Func<SystemAuthorizeErrorRedirectItemList, bool> redirectActionExpression = it => it.SystemAuthorizeType == SystemAuthorizeType.Action && it.Area == areaName && it.Controller == controllerName && it.Action == actionName;
            Func<SystemAuthorizeErrorRedirectItemList, bool> redirectControllerExpression = it => it.SystemAuthorizeType == SystemAuthorizeType.Action && it.Area == areaName && it.Controller == controllerName;
            Func<SystemAuthorizeErrorRedirectItemList, bool> redirectAreaExpression = it => it.SystemAuthorizeType == SystemAuthorizeType.Action && it.Area == areaName;


            Func<SystemAuthorizeModel, bool> actionExpression = it => it.SystemAuthorizeType == SystemAuthorizeType.Action && it.AreaName == areaName && it.ControllerName == controllerName && it.ActionName == actionName;
            Func<SystemAuthorizeModel, bool> controllerExpression = it => it.SystemAuthorizeType == SystemAuthorizeType.Controller && it.AreaName == areaName && it.ControllerName == controllerName;
            Func<SystemAuthorizeModel, bool> areaExpression = it => it.SystemAuthorizeType == SystemAuthorizeType.Area && it.AreaName == areaName;

            dynamic userId = GetCurrentUserKey();

            //所有权限
            bool isAllByUserKey = IsAllByUserKey(systemAuthorizeList, userId);
            bool isAreaByUserKey = IsAreaByUserKey(systemAuthorizeList, areaName, userId);
            bool isControllerByUserKey = IsControllerByUserKey(systemAuthorizeList, areaName, controllerName, userId);
            bool isActionByUserKey = IsActionByUserKey(systemAuthorizeList, areaName, controllerName, actionName, userId);
            //有权限
            var hasPower = (isAllByUserKey || isActionByUserKey || isControllerByUserKey || isAreaByUserKey);
            //需要验证
            var mustValidate = systemAuthorizeList.Any(actionExpression) || systemAuthorizeList.Any(controllerExpression) || systemAuthorizeList.Any(areaExpression);

            if (!hasPower && mustValidate)
            {
                ErrorRediect(errorRediect, RedirectUrl, redirectActionExpression, redirectControllerExpression, redirectAreaExpression);
            }

        }

        private static void ErrorRediect(SystemAuthorizeErrorRedirect errorRediect, Action<string> RedirectUrl, Func<SystemAuthorizeErrorRedirectItemList, bool> actionExpression, Func<SystemAuthorizeErrorRedirectItemList, bool> controllerExpression, Func<SystemAuthorizeErrorRedirectItemList, bool> areaExpression)
        {
            if (errorRediect.ItemList == null)
            {//返回默认错误地址
                RedirectUrl(errorRediect.DefaultUrl);
            }
            else if (errorRediect.ItemList.Any(actionExpression))
            {
                var red = errorRediect.ItemList.Single(actionExpression);
                RedirectUrl(red.ErrorUrl);
            }
            else if (errorRediect.ItemList.Any(controllerExpression))
            {
                var red = errorRediect.ItemList.Single(controllerExpression);
                RedirectUrl(red.ErrorUrl);
            }
            else if (errorRediect.ItemList.Any(areaExpression))
            {
                var red = errorRediect.ItemList.Single(areaExpression);
                RedirectUrl(red.ErrorUrl);
            }
            else if (errorRediect.ItemList.Any(it => it.SystemAuthorizeType == SystemAuthorizeType.All))
            {
                var red = errorRediect.ItemList.Single(it => it.SystemAuthorizeType == SystemAuthorizeType.All);
                RedirectUrl(red.ErrorUrl);
            }
            else
            {
                RedirectUrl(errorRediect.DefaultUrl);
            }
        }

        private static bool IsAllByUserKey(List<SystemAuthorizeModel> systemAuthorizeList, object userKey)
        {
            var hasAll = systemAuthorizeList.Any(it => it.SystemAuthorizeType == SystemAuthorizeType.All);
            if (hasAll)
            {
                if (systemAuthorizeList.Any(it => it.UserKeyArray != null && it.UserKeyArray.Contains(userKey)))
                {
                    return true;
                }
            }

            return false;
        }
        private static bool IsAreaByUserKey(List<SystemAuthorizeModel> systemAuthorizeList, string area, object userKey)
        {

            if (systemAuthorizeList.Any(it => it.AreaName == area && it.SystemAuthorizeType == SystemAuthorizeType.Area)) //是否存在验证级别为Area的验证
            {
                var isContains = systemAuthorizeList.Any(it => it.AreaName == area && it.SystemAuthorizeType == SystemAuthorizeType.Area && it.UserKeyArray.Any(uk => uk.ToString() == userKey.ToString()));
                return isContains;
            }
            return false;
        }


        private static bool IsControllerByUserKey(List<SystemAuthorizeModel> systemAuthorizeList, string area, string controller, object userKey)
        {
            if (systemAuthorizeList.Any(it => it.AreaName == area && it.ControllerName == controller && it.SystemAuthorizeType == SystemAuthorizeType.Controller)) //是否存在验证级别为Controller的验证
            {
                var isContains = systemAuthorizeList.Any(it => it.AreaName == area && it.ControllerName == controller && it.SystemAuthorizeType == SystemAuthorizeType.Controller && it.UserKeyArray.Any(uk => uk.ToString() == userKey.ToString()));
                return isContains;
            }
            return false;
        }




        private static bool IsActionByUserKey(List<SystemAuthorizeModel> systemAuthorizeList, string area, string controller, string action, dynamic userKey)
        {

            if (systemAuthorizeList.Any(it => it.AreaName == area && it.ControllerName == controller && it.ActionName == action && it.SystemAuthorizeType == SystemAuthorizeType.Action)) //是否存在验证级别为action的验证
            {
                return systemAuthorizeList.Any(it => it.AreaName == area && it.ControllerName == controller && it.ActionName == action && it.SystemAuthorizeType == SystemAuthorizeType.Action && it.UserKeyArray.Any(uk=>uk.ToString()==userKey.ToString()));
            }

            return false;
        }
    }






    /// <summary>
    /// 用户访问需要授权的项
    /// </summary>
    public class SystemAuthorizeModel
    {
        /// <summary>
        /// 验证类型
        /// </summary>
        public SystemAuthorizeType SystemAuthorizeType { get; set; }
        /// <summary>
        /// 用户拥有权限访问的Area
        /// </summary>
        public string AreaName { get; set; }
        /// <summary>
        /// 用户拥有权限访问的Controller
        /// </summary>
        public string ControllerName { get; set; }
        /// <summary>
        /// 用户拥有权限访问的Actioin
        /// </summary>
        public string ActionName { get; set; }
        /// <summary>
        /// 用户ID
        /// </summary>
        public dynamic[] UserKeyArray { get; set; }

    }

    /// <summary>
    /// 如果没有权限返回地址
    /// </summary>
    public class SystemAuthorizeErrorRedirect
    {
        /// <summary>
        /// 默认值
        /// </summary>
        public string DefaultUrl { get; set; }

        public List<SystemAuthorizeErrorRedirectItemList> ItemList { get; set; }
    }

    public class SystemAuthorizeErrorRedirectItemList
    {
        /// <summary>
        /// 验证类型
        /// </summary>
        public SystemAuthorizeType SystemAuthorizeType { get; set; }
        public string Controller { get; set; }
        public string Action { get; set; }
        public string Area { get; set; }

        public string ErrorUrl { get; set; }

    }

    /// <summary>
    /// 验证类型
    /// </summary>
    public enum SystemAuthorizeType
    {
        /// <summary>
        /// 所有权限
        /// </summary>
        All = 0,
        /// <summary>
        ///验证Area
        /// </summary>
        Area = 1,
        /// <summary>
        /// 验证Area和Controller
        /// </summary>
        Controller = 2,
        /// <summary>
        /// 验证Area和Controller和Action
        /// </summary>
        Action = 3,
        /// <summary>
        /// 没有权限
        /// </summary>
        No = 4

    }
}