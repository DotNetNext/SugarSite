using Infrastructure.DbModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SyntacticSugar;
namespace SugarSite.Areas.AdminSite.Controllers
{
    public class DocOutsourcing
    {
        /// <summary>
        /// 获取所有子ID
        /// </summary>
        /// <param name="allTypeList"></param>
        /// <param name="typeId"></param>
        /// <returns></returns>
        internal List<int> GetChildrenTypeIds(List<DocType> allTypeList, int? typeId)
        {
            List<int> reval = new List<int>();
            reval.Add((int)typeId);
            var firsts = allTypeList.Where(it => it.ParentId.TryToInt() == typeId.TryToInt()).ToList();
            if (firsts.IsValuable())
            {
                foreach (var item in firsts)
                {
                    GetChildrenTypeIds_Part(allTypeList, reval, item.Id);
                }
            }
            return reval;
        }
        private void GetChildrenTypeIds_Part(List<DocType> allTypeList, List<int> reval, int currentTypeId)
        {
            reval.Add((int)currentTypeId);
            var firsts = allTypeList.Where(it => it.ParentId.TryToInt() == currentTypeId.TryToInt()).ToList();
            if (firsts.IsValuable())
            {
                foreach (var item in firsts)
                {
                    GetChildrenTypeIds_Part(allTypeList, reval, item.Id);
                }
            }
        }
    }
}