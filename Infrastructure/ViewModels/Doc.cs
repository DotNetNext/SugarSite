using Infrastructure.DbModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.ViewModels
{
    public class Doc
    {
        public List<DocType> DocType { get; set; }

        public List<DocContent> DocContent { get; set; }
        public DocType CurrentType { get; set; }
    }
}
