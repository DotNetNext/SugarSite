using Infrastructure.DbModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.ViewModels.Views
{
    public class V_VisitorList : VisitorList
    {
        public string VisName { get; set; }

        public string VisAvatar { get; set; }
    }
}
