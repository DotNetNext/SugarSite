using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Pub
{
    public class PubConst
    {
        public const string SessionVerifyCode = "SessionVerifyCode";
        public const string SessionVerifyCodeDetails = "SessionVerifyCodeDetails";
        public const string SessionNewUserList = "SessionnNewUserList";
        public const string SessionGetForumsStatistics = "SessionGetForumsStatistics";
        public const string SessionForumsList = "SessionnForumsList";
        public const string SessionBBSRight = "SessionBBSRight";
        public const string UserUniqueKey = "CookeUniqueKey";
        public const string UrlAdminIndex = "AdminSite/DocContent";
        public const string SitePrefix = "-文档园";
        public const string SiteDomain = "codeisbug.com";
        public const int SitePageSize = 20;
        public const string FilterKeyFalseDelteJoin = "FalseDelteJoin";
        public static List<int> SiteSeoFidList = new List<int>() { 9,14,11,17 };//希望收录的版块ID让文章更容易让搜索引擎识别
    }
}
