using Infrastructure.DbModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.ViewModels 
{
    public class HomeIndexResult
    {
        /// <summary>
        /// 新贴
        /// </summary>
        public List<BBS_Topics> NewList { get; set; }
        /// <summary>
        /// 最后回复
        /// </summary>
        public List<BBS_Topics> LastList { get; set; }
        /// <summary>
        /// 文档
        /// </summary>
        public List<DocMaster> Master { get; set; }
    }
}
