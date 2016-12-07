using Infrastructure.DbModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.ViewModels.UserCenter
{
    public class UserMailResult
    {
        public string UserCode { get; set; }
        public UserInfo UserInfo { get; set; }
        public string Now { get; set; }
    }
}
