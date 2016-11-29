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
        public List<BBS_Topics> TopicsList { get; set; }
        public List<BBS_Forums> ForumsList { get; set; }
    }
}
