using NLog.Fluent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YiSha.Entity;
using YiSha.Entity.ProductCategoryManager;
using YiSha.Model.Param.ProductCategoryManager;
using YiSha.Model.Result;
using YiSha.Service.ProductCategoryManager;
using YiSha.Util.Model;
using YiSha.Web.Code;

namespace YiSha.Service
{
    public class ZtreeHelper
    {
        public static List<ZtreeInfo> GetZtreeList(IEnumerable<ITreeNodeEntity> departmentList,long parentId,long? excludedId=null, bool addRoot=false)
        {
            
            var Result = new List<ZtreeInfo>();
            //如果指定了父节点，则从集体中筛选已指定父节点的树

            if (parentId > -1)
            {
                List<long> childrenDepartmentIdList = GetChildrenIdList(departmentList, parentId);
                departmentList = departmentList.Where(p => childrenDepartmentIdList.Contains(p.Id.Value)).ToList();
            }
            //如果指定了要排除的子树，则查找当前子树的所有子节点，然后从列表中排除
            if (excludedId.HasValue && excludedId.Value > 0)
            {
                List<long> childrenDepartmentIdList = GetChildrenIdList(departmentList, excludedId.Value);
                departmentList = departmentList.Where(p => !childrenDepartmentIdList.Contains(p.Id.Value)).ToList();
            }
            if (addRoot)
            {
                Result.Add(new ZtreeInfo
                {
                    id = 0.ToString(),
                    pId = "-1",
                    name = "根节点"
                });
            }
            foreach (ITreeNodeEntity department in departmentList)
            {
                Result.Add(new ZtreeInfo
                {
                    id = department.Id.ToString(),
                    pId = department.ParentId.ToString(),
                    name = department.DisplayName.ToString(),
                    tag = department,
                    Obj = department,
                });
            }
            return Result;
        }
        /// <summary>
        /// 获取当前部门及下面所有的部门
        /// </summary>
        /// <param name="departmentList"></param>
        /// <param name="departmentId"></param>
        /// <returns></returns>
        public static List<long> GetChildrenIdList(IEnumerable<ITreeNodeEntity> allItems, long nodeId)
        {
            List<long> departmentIdList = new List<long>();
            departmentIdList.Add(nodeId);
            GetChildrenIdListRecursive(allItems, nodeId, departmentIdList);
            return departmentIdList;
        }
        /// <summary>
        /// 获取该部门下面所有的子部门
        /// </summary>
        /// <param name="departmentList"></param>
        /// <param name="departmentId"></param>
        /// <param name="departmentIdList"></param>
        private static void GetChildrenIdListRecursive(IEnumerable<ITreeNodeEntity> allItems, long nodeId, List<long> childrenIdList)
        {
            var children = allItems.Where(p => p.ParentId == nodeId).Select(p => p.Id.Value).ToList();
            if (children.Count > 0)
            {
                childrenIdList.AddRange(children);
                foreach (long id in children)
                {
                    GetChildrenIdListRecursive(allItems, id, childrenIdList);
                }
            }
        }
    }
}
