using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infrastructure.DbModel;
namespace Infrastructure.ViewModels.BBS
{
    public class RightRelust
    {
        public List<DocMaster> DocMasterList { get; set; }

        public List<V_UserStatisticsInfo> UserTopicsInfoList { get; set; }

        public List<V_UserStatisticsInfo> UserRepliesList { get; set; }
    }
}
