using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SyntacticSugar;
namespace Infrastructure.Pub
{
    public class PubEnum
    {
        public enum RoleType
        {
            [Desc("管理员")]
            Admin = 1,
            [Desc("会员")]
            User = 2,
            [Desc("版主")]
            Moderator=3,
            [Desc("游客")]
            Tourist =4
        }

    }
}
