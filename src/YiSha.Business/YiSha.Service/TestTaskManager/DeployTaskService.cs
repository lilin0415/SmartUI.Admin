using System;
using System.Linq;
using System.Text;
using System.Data.Common;
using System.Linq.Expressions;
using System.Collections.Generic;
using System.Threading.Tasks;
using YiSha.Util;
using YiSha.Util.Extension;
using YiSha.Util.Model;
using YiSha.Data;
using YiSha.Data.Repository;
using YiSha.Entity.TestTaskManager;
using YiSha.Model.Param.TestTaskManager;
using YiSha.Entity.TestCaseManager;
using YiSha.Model.TestTaskManager;

namespace YiSha.Service.TestTaskManager
{
    /// <summary>
    /// 创 建：admin
    /// 日 期：2022-10-12 21:21
    /// 描 述：服务类
    /// </summary>
    public class DeployTaskService : BaseRepositoryService
    {
        #region 获取数据
        public async Task<List<DeployTaskEntity>> GetListByTaskId(long taskId)
        {
            var expression = CreateFilter<DeployTaskEntity>();
            expression = expression.And(x => x.TaskId == taskId);

            var list = await this.BaseRepository().FindList(expression);
            return list.ToList();
        }

        public async Task<List<DeployTaskEntity>> GetList(DeployTaskListParam param)
        {
            var expression = ListFilter(param);
            var list = await this.BaseRepository().FindList(expression);
            return list.ToList();
        }

        public async Task<List<DeployTaskEntity>> GetPageList(DeployTaskListParam param, Pagination pagination)
        {
            var expression = ListFilter(param);
            var list= await this.BaseRepository().FindList(expression, pagination);
            return list.ToList();
        }

        public async Task<DeployTaskEntity> GetEntity(long id)
        {
            return await this.BaseRepository().FindEntity<DeployTaskEntity>(id);
        }
        #endregion

        #region 提交数据
        public async Task<long> SaveForm(DeployTaskEntity entity)
        {
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
        {this.VerifyIsMyDataOnDelete<DeployTaskEntity>(ids);
            long[] idArr = TextHelper.SplitToArray<long>(ids, ',');
            await this.BaseRepository().Delete<DeployTaskEntity>(idArr);
        }
        public async Task SaveList(long taskId, TenantDeviceModel[] deviceList)
        {
            var deployItems = new List<DeployTaskEntity>();
            foreach (var x in deviceList)
            {
                var deployItem = new DeployTaskEntity();
             
                deployItem.TaskId = taskId;
                deployItem.AppToken = x.AppToken;
                deployItem.DeviceGuid = x.DeviceGuid;
                deployItem.UserId = x.UserId;

                deployItems.Add(deployItem);
            }


            await this.BaseRepository().Delete<DeployTaskEntity>(x => x.TaskId == taskId);

            foreach (var entity in deployItems)
            {
                entity.Create();
                this.VerifyIsMyDataOnCreate<DeployTaskEntity>(entity);
                await this.BaseRepository().Insert(entity);
            }
        }
        #endregion

        #region 私有方法
        private Expression<Func<DeployTaskEntity, bool>> ListFilter(DeployTaskListParam param)
        {
            var expression = CreateFilter<DeployTaskEntity>();
            if (param != null)
            {
            }
            return expression;
        }
        #endregion
    }
}
