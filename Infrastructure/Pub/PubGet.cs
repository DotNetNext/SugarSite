using SyntacticSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Pub
{
    public class PubGet
    {
        public static string GetUserKey {
            get {
                 return CookiesManager<string>.GetInstance()[PubConst.UserUniqueKey] + "--" + PubConst.UserUniqueKey; ;
            }
        }
    }
}
