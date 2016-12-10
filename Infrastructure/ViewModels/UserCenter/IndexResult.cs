using Infrastructure.ViewModels.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.ViewModels.UserCenter
{
    public class IndexResult
    {
        /// <summary>
        /// 访客
        /// </summary>
        public List<V_VisitorList> VList { get; set; }
    }
}
