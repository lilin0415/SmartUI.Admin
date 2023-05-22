using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YiSha.Entity.OrganizationManage;
using YiSha.Enum.OrganizationManage;
using YiSha.Model;
using YiSha.Model.Result;
using YiSha.Model.Param.OrganizationManage;
using YiSha.Service.OrganizationManage;
using YiSha.Util;
using YiSha.Util.Model;
using YiSha.Util.Extension;
using YiSha.Web.Code;

using YiSha.Model.Param.TenantManage;

namespace YiSha.Business.OrganizationManage
{
    public class DepartmentBLL
    {
        private DepartmentService departmentService = new DepartmentService();
        private UserService userService = new UserService();

        #region 获取数据
        public async Task<TData<List<DepartmentEntity>>> GetList(DepartmentListParam param)
        {
            var items = await departmentService.GetList(param);

            OperatorInfo operatorInfo = await Operator.Instance.Current();

            //if (operatorInfo.IsSystem != 1 && operatorInfo.DepartmentId.HasValue)
            //{
            //    List<long> childrenDepartmentIdList = await departmentService.GetChildrenDepartmentIdList(obj.Result, operatorInfo.DepartmentId.Value);
            //    obj.Result = obj.Result.Where(p => childrenDepartmentIdList.Contains(p.Id.Value)).ToList();
            //}

             var userList = await userService.GetList(new UserListParam { UserIds = string.Join(",", items.Select(p => p.PrincipalId).ToArray()) });
            foreach (DepartmentEntity entity in items)
            {
                if (entity.PrincipalId > 0)
                {
                    entity.PrincipalName = userList.FirstOrDefault(p => p.Id == entity.PrincipalId)?.RealName;
                }
                else
                {
                    entity.PrincipalName = string.Empty;
                }
            }

            TData<List<DepartmentEntity>> obj = new TData<List<DepartmentEntity>>();
            obj.Result = items;
            obj.Status = true;
            return obj;
        }

        public async Task<TData<List<ZtreeInfo>>> GetZtreeDepartmentList(DepartmentListParam param)
        {
            var obj = new TData<List<ZtreeInfo>>();
            obj.Result = new List<ZtreeInfo>();
            List<DepartmentEntity> departmentList = await departmentService.GetList(param);
            //OperatorInfo operatorInfo = await Operator.Instance.Current();
            //if (operatorInfo.IsSystem != 1)
            //{
            //    List<long> childrenDepartmentIdList = await departmentService.GetChildrenDepartmentIdList(departmentList, operatorInfo.DepartmentId.Value);
            //    departmentList = departmentList.Where(p => childrenDepartmentIdList.Contains(p.Id.Value)).ToList();
            //}
            foreach (DepartmentEntity department in departmentList)
            {
                obj.Result.Add(new ZtreeInfo
                {
                    id = department.Id.ToString(),
                    pId = department.ParentId.ToString(),
                    name = department.DepartmentName
                });
            }
            obj.Status = true;
            return obj;
        }

        public async Task<TData<List<ZtreeInfo>>> GetZtreeUserList(DepartmentListParam param)
        {
            var obj = new TData<List<ZtreeInfo>>();
            obj.Result = new List<ZtreeInfo>();
            List<DepartmentEntity> departmentList = await departmentService.GetList(param);
            //OperatorInfo operatorInfo = await Operator.Instance.Current();
            //if (operatorInfo.IsSystem != 1)
            //{
            //    List<long> childrenDepartmentIdList = await departmentService.GetChildrenDepartmentIdList(departmentList, operatorInfo.DepartmentId.Value);
            //    departmentList = departmentList.Where(p => childrenDepartmentIdList.Contains(p.Id.Value)).ToList();
            //}

            var userList = await userService.GetList(null);
            foreach (DepartmentEntity department in departmentList)
            {
                obj.Result.Add(new ZtreeInfo
                {
                    id = department.Id.ToString(),
                    pId = department.ParentId.ToString(),
                    name = department.DepartmentName
                });
                List<long> userIdList = userList.Where(t => t.DepartmentId == department.Id).Select(t => t.Id.Value).ToList();
                foreach (var user in userList.Where(t => userIdList.Contains(t.Id.Value)))
                {
                    obj.Result.Add(new ZtreeInfo
                    {
                        id = user.Id.ToString(),
                        pId = department.Id.ToString(),
                        name = user.RealName
                    });
                }
            }
            obj.Status = true;
            return obj;
        }

        public async Task<TData<DepartmentEntity>> GetEntity(long id)
        {
            TData<DepartmentEntity> obj = new TData<DepartmentEntity>();
            obj.Result = await departmentService.GetEntity(id);
            obj.Status = true;
            return obj;
        }

        public async Task<TData<int>> GetMaxSort()
        {
            TData<int> obj = new TData<int>();
            obj.Result = await departmentService.GetMaxSort();
            obj.Status = true;
            return obj;
        }
        #endregion

        #region 提交数据
        public async Task<TData<string>> SaveForm(DepartmentEntity entity)
        {
            TData<string> obj = new TData<string>();
           
            await departmentService.SaveForm(entity);
            obj.Result = entity.Id.ParseToString();
            obj.Status = true;
            return obj;
        }

        public async Task<TData> DeleteForm(string ids)
        {
            TData obj = new TData();
            foreach (long id in TextHelper.SplitToArray<long>(ids, ','))
            {
                if (departmentService.ExistChildrenDepartment(id))
                {
                    obj.Message = "该部门下面有子部门！";
                    return obj;
                }
            }
            await departmentService.DeleteForm(ids);
            obj.Status = true;
            return obj;
        }
        #endregion

      
    }
}
