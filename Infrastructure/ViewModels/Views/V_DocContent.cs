using Infrastructure.DbModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.ViewModels
{
    /// <summary>
    /// 文章列表视图
    /// </summary>
    public class V_DocContent: DocContent
    {
        /// <summary>
        /// 文章分类名称
        /// </summary>
        public string TypeName { get; set; }
    }
}
