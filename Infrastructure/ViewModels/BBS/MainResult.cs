using Infrastructure.DbModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.ViewModels.BBS
{
    public class MainResult : PageModel
    {
        public short Fid { get; set; }

        /// <summary>
        /// 贴子列表
        /// </summary>
        public List<BBS_Topics> TopicsList { get; set; }

        /// <summary>
        /// 版块列表
        /// </summary>
        public List<BBS_Forums> ForumsList { get; set; }

        /// <summary>
        /// 排序方式
        /// </summary>
        public int OrderBy { get; set; }

        /// <summary>
        /// 在线用户
        /// </summary>
        public List<OnlineVisitorsResult> OnlineList { get; set; }

        /// <summary>
        /// 获取网站信息
        /// </summary>
        public SiteInfoResult SiteInfo { get; set; }
    }
}
