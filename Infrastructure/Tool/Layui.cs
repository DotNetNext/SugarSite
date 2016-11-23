using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SyntacticSugar;
namespace Infrastructure.Tool
{
    public static class LayuiTree
    {
        /// <summary>
        /// 递归组装tree对象
        /// </summary>
        /// <param name="thisValue"></param>
        /// <returns></returns>
        public static List<LayuiTreeModel>  ToLayuiTree(this List<LayuiTreeModel> thisValue)
        {
            if (thisValue == null) { return thisValue; }
            List<LayuiTreeModel> reval = new List<LayuiTreeModel>();
            var firsts = thisValue.Where(it => it.level == 0).ToList();
            foreach (var item in firsts)
            {
                var newItem = new LayuiTreeModel();
                ToLayuiTree_Part(item, newItem, thisValue);
                reval.Add(newItem);
            }
            return reval;
        }

        private static void ToLayuiTree_Part(LayuiTreeModel oldItem,LayuiTreeModel newItem, List<LayuiTreeModel> allTypeList)
        {
            newItem.id = oldItem.id;
            newItem.name = oldItem.name;
            newItem.spread = oldItem.spread==null?true: oldItem.spread;
            newItem.alias = oldItem.alias;
            newItem.level = oldItem.level;
            var childs = allTypeList.Where(it => it.parentId.TryToInt() == newItem.id.TryToInt()).ToList();
            var isAny = childs.IsValuable();
            if (isAny) {
                newItem.children = new List<LayuiTreeModel>();
                foreach (var child in childs)
                {
                    LayuiTreeModel childNewItem = new LayuiTreeModel();
                    ToLayuiTree_Part(child,childNewItem,allTypeList);
                    newItem.children.Add(childNewItem);
                }
            }
        }

    }
     
    public class LayuiTreeModel
    {
        public bool? spread { get; set; }

        public object name { get; set; }

        public object id { get; set; }

        public object parentId { get; set; }

        //1是展开
        public int alias { get; set; }

        public List<LayuiTreeModel> children { get; set; }

        public int? level { get; set; }
    }
}
