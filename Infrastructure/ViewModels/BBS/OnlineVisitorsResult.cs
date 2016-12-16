using Infrastructure.DbModel;
using Infrastructure.Pub;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SyntacticSugar;
namespace Infrastructure.ViewModels.BBS
{
    /// <summary>
    /// 在线游客
    /// </summary>
    public class OnlineVisitorsResult
    {
        public UserInfo User { get; set; }

        public string LastActionIp { get; set; }

        public DateTime LastActionTime { get; set; }

        public string TypeName { get { return ((PubEnum.RoleType)User.RoleId.Value).GetAttributeValue(); } }
    }
}
