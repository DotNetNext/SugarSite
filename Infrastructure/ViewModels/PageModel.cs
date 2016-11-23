using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.ViewModels
{
    public class PageModel
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public int PageCount { get; set; }

        public int Pages
        {
            get
            {
                if (PageCount % PageSize == 0)
                {
                    return PageCount / PageSize;
                }
                else {
                    return PageCount /PageSize+1;
                }
            }
        }
    }
}
