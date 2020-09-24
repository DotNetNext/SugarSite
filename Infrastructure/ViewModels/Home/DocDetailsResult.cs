using Infrastructure.DbModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.ViewModels 
{
    public class DocDetailsResult
    {
        public List<DocType> DocType { get; set; }
        public List<DocType> SearchList { get; set; }
        public int MasterId { get; set; }
        public int TypeId { get; set; }
    }
}
