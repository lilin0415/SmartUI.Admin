using Koo.Utilities.Exceptions;
using Quartz.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using YiSha.Data.Repository;
using YiSha.Entity.OrganizationManage;
using YiSha.Model;
using YiSha.Model.Param.OrganizationManage;
using YiSha.Model.TestTaskManager;
using YiSha.Util;
using YiSha.Util.Extension;
using YiSha.Util.Model;
using YiSha.Web.Code;

namespace YiSha.Service.OrganizationManage
{
    public class DepartmentService : BaseRepositoryService
    {
        #region 获取数据
        #region 获取当前部门及下面所有的部门
        /// <summary>
        /// 获取当前部门及下面所有的部门
        /// </summary>
        /// <param name="departmentList"></param>
        /// <param name="departmentId"></param>
        /// <returns></returns>
        public async Task<List<long>> GetChildrenDepartmentIdList(List<DepartmentEntity> departmentList, long departmentId)
        {
            if (departmentList == null)
            {
                departmentList = await this.GetList(null);
            }
            List<long> departmentIdList = new List<long>();
            departmentIdList.Add(departmentId);
            GetChildrenDepartmentIdList(departmentList, departmentId, departmentIdList);
            return departmentIdList;
        }
        /// <summary>
        /// 获取该部门下面所有的子部门
        /// </summary>
        /// <param name="departmentList"></param>
        /// <param name="departmentId"></param>
        /// <param name="departmentIdList"></param>
        private void GetChildrenDepartmentIdList(List<DepartmentEntity> departmentList, long departmentId, List<long> departmentIdList)
        {
            var children = departmentList.Where(p => p.ParentId == departmentId).Select(p => p.Id.Value).ToList();
            if (children.Count > 0)
            {
                departmentIdList.AddRange(children);
                foreach (long id in children)
                {
                    GetChildrenDepartmentIdList(departmentList, id, departmentIdList);
                }
            }
        }
        #endregion

        public async Task<List<DepartmentEntity>> GetList(DepartmentListParam param)
        {
            var expression = ListFilter(param);
            var list = await this.BaseRepository().FindList(expression);
            return list.OrderBy(p => p.DepartmentSort).ToList();
        }

        public async Task<DepartmentEntity> GetEntity(long id)
        {
            return await this.BaseRepository().FindEntity<DepartmentEntity>(id);
        }

        public async Task<int> GetMaxSort()
        {
            object result = await this.BaseRepository().FindObject("SELECT MAX(DepartmentSort) FROM sysdepartment");
            int sort = result.ParseToInt();
            sort++;
            return sort;
        }


        /// <summary>
        /// 是否存在子部门
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool ExistChildrenDepartment(long id)
        {
            var expression = LinqExtensions.True<DepartmentEntity>();
            expression = expression.And(t => t.ParentId == id);
            return this.BaseRepository().IQueryable(expression).Count() > 0 ? true : false;
        }
        public async Task<List<DepartmentEntity>> GetAllChildren(long? id)
        {
            var expression = ListFilter(new DepartmentListParam());
            var list = await this.BaseRepository().FindList(expression);
            var items =  list.ToList();

            var ret = new List<DepartmentEntity>();

            GetAllChildrenRecursive(items, id, ret);

            return ret;
        }
        private void  GetAllChildrenRecursive(List<DepartmentEntity> items,long? id, List<DepartmentEntity> ret)
        {
            
            var subItems = items.Where(x => x.ParentId == id);
            if (subItems.Any())
            {
                ret.AddRange(subItems);

                foreach (var item in subItems)
                {
                    GetAllChildrenRecursive(items, item.Id, ret);
                }
            }
        }

        #endregion

        #region 提交数据

        /// <summary>
        /// 部门名称是否存在
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool ExistDepartmentName(DepartmentEntity entity)
        {
            var expression = CreateFilter<DepartmentEntity>();
            expression = expression.And(t => t.BaseIsDelete == 0);
            if (entity.Id.IsNullOrZero())
            {
                expression = expression.And(t => t.DepartmentName == entity.DepartmentName && t.ParentId == entity.ParentId);
            }
            else
            {
                expression = expression.And(t => t.DepartmentName == entity.DepartmentName && t.ParentId == entity.ParentId && t.Id != entity.Id);
            }
            return this.BaseRepository().IQueryable(expression).Count() > 0 ? true : false;
        }
        public async Task<long> SaveForm(DepartmentEntity entity)
        {
            if (string.IsNullOrWhiteSpace(entity.DepartmentName))
            {
                throw new CanNotBeEmptyException("名称不能为空");
            }
            entity.DepartmentName= entity.DepartmentName.Trim();

            if (this.ExistDepartmentName(entity))
            {
                throw new DuplicationDataExection("部门名称已经存在");
            }

            //如果修改数据，验证父节点
            if (entity.Id.GetValueOrDefault()!=0 && entity.ParentId.GetValueOrDefault() != 0)
            {
                if (entity.ParentId== entity.Id)
                {
                    throw new BizException("不能选择自己作为上级部门");
                }

                var children = await GetAllChildren(entity.Id);
                if (children.Any(x => x.Id == entity.ParentId))
                {
                    throw new BizException("不能选择下级数据作为上级部门");
                }
            }
          
          

            if (entity.Id.IsNullOrZero())
            {
                entity.Create();

                return await VerifyIsMyDataAndInsert(entity);
            }
            else
            {
                entity.Modify();

                return await VerifyIsMyDataAndUpdate(entity);
            }
        }

        public async Task DeleteForm(string ids)
        {
            var db = await this.BaseRepository().BeginTrans();
            try
            {
                this.VerifyIsMyDataOnDelete<DepartmentEntity>(ids);

                long[] idArr = TextHelper.SplitToArray<long>(ids, ',');
                await db.Delete<DepartmentEntity>(idArr);
                await db.CommitTrans();
            }
            catch
            {
                await db.RollbackTrans();
                throw;
            }
        }
        #endregion

        #region 私有方法
        private Expression<Func<DepartmentEntity, bool>> ListFilter(DepartmentListParam param)
        {
            var expression = CreateFilter<DepartmentEntity>();

            if (param != null)
            {
                if (!param.DepartmentName.IsEmpty())
                {
                    expression = expression.And(t => t.DepartmentName.Contains(param.DepartmentName));
                }
            }
            return expression;
        }
        #endregion
    }
}
