using Infrastructure.ViewModels.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infrastructure.DbModel;
namespace Infrastructure.ViewModels.UserCenter
{
    public class IndexResult
    {
        /// <summary>
        /// 访客
        /// </summary>
        public List<V_VisitorList> VList { get; set; }
        /// <summary>
        /// 站内信 未读
        /// </summary>
        public List<BBS_PMS> PmsListNew { get; set; }
        /// <summary>
        /// 站内信 已读
        /// </summary>
        public List<BBS_PMS> PmsListOld { get; set; }
    }
}
