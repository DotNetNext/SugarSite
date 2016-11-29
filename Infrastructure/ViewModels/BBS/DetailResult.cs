using Infrastructure.DbModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.ViewModels.BBS
{
    public class DetailResult
    {
        public BBS_Topics TopItem { get; set; }

        public BBS_Posts PosItem { get; set; }
    }
}
