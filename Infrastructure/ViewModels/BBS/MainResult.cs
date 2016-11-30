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


        public int OrderBy { get; set; }

    }
}
