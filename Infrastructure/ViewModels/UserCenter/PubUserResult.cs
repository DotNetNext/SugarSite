using Infrastructure.DbModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.ViewModels.UserCenter
{
    public class PubUserResult
    {
        public UserInfo UserInfo { get; set; }
        public List<BBS_Posts> RecentReplies { get; set; }
        public List<BBS_Topics> RecentRepliesTopics { get; set; }
        public List<BBS_Topics> RecentAsks { get; set; }
        public bool IsAdmin { get; set; }
    }
}
