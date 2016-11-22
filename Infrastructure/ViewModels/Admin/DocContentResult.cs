using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.ViewModels
{
    public class DocContentResult : PageModel
    {
        public List<V_DocContent> DocList { get; set; }
    }
}
