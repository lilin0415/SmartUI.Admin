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
using YiSha.Entity.DeviceManager;
using YiSha.Model.Param.DeviceManager;
using NPOI.HSSF.Record;
using YiSha.Model.DeviceManager;
using YiSha.Entity.TestTaskManager;

namespace YiSha.Service.DeviceManager
{
    /// <summary>
    /// 创 建：admin
    /// 日 期：2023-04-22 09:42
    /// 描 述：服务类
    /// </summary>
    public class DeviceGroupDetailService : BaseRepositoryService
    {
        #region 获取数据
        public async Task<List<DeviceGroupDetailModel>> GetList(DeviceGroupDetailListParam param)
        {
            var sql = "select a.Id as DetailId, a.GroupId, b.* from devicegroupdetail a inner join device b on a.DeviceId = b.Id where 1=1 ";
            if (param != null)
            {
                if (SqlStringHelper.IsSafeSqWhere(param.GroupId))
                {
                    sql += $" and a.GroupId = {param.GroupId} ";
                }
            }
           
            var list = await this.BaseRepository().FindList<DeviceGroupDetailModel>(sql);
            return list.OrderByDescending(x=>x.LastActiveTime).ToList();
        }

        public async Task<List<DeviceGroupDetailModel>> GetPageList(DeviceGroupDetailListParam param, Pagination pagination)
        {
            var sql = "select a.Id as DetailId, a.GroupId, b.* from devicegroupdetail a inner join device b on a.DeviceId = b.Id where 1=1 ";
            if (param != null)
            {
                if (SqlStringHelper.IsSafeSqWhere(param.GroupId))
                {
                    sql += $" and a.GroupId = {param.GroupId} ";
                }
            }
            var count = await this.BaseRepository().GetCount(sql);
            pagination.TotalCount = count;
            var list= await this.BaseRepository().FindList<DeviceGroupDetailModel>(sql, pagination);
            return list.list.OrderByDescending(x=>x.LastActiveTime).ToList();
        }

        public async Task<DeviceGroupDetailEntity> GetEntity(long id)
        {
            return await this.BaseRepository().FindEntity<DeviceGroupDetailEntity>(id);
        }
        #endregion

        #region 提交数据
        public async Task SaveForm(DeviceGroupDetailEntity entity)
        {
            if (entity.Id.IsNullOrZero())
            {
                entity.Create();
                await this.BaseRepository().Insert(entity);
            }
            else
            {
                entity.Modify();
                await this.BaseRepository().Update(entity);
            }
        }

        public async Task DeleteForm(string ids)
        {
            long[] idArr = TextHelper.SplitToArray<long>(ids, ',');
            await this.BaseRepository().Delete<DeviceGroupDetailEntity>(idArr);
        }
        #endregion

        #region 私有方法
        private Expression<Func<DeviceGroupDetailEntity, bool>> ListFilter(DeviceGroupDetailListParam param)
        {
            var expression = LinqExtensions.True<DeviceGroupDetailEntity>();
            if (param != null)
            {
            }
            return expression;
        }
        #endregion

        private async Task<bool> ExistDevice(long groupId, long deviceId)
        {
            var item = await this.BaseRepository().FindEntity<DeviceGroupDetailEntity>(x => x.GroupId == groupId && x.DeviceId == deviceId);
            return item != null;    
        }

        public async Task AddList(long groupId, List<DeviceEntity> devices)
        {
            var items = new List<DeviceGroupDetailEntity>();
            foreach (var device in devices)
            {
                var exist = await ExistDevice(groupId, device.Id.GetValueOrDefault());
                if (!exist)
                {
                    var entity = new DeviceGroupDetailEntity();
                    entity.GroupId = groupId;
                    entity.DeviceId = device.Id;
                    items.Add(entity);
                }
               
            }

            if (items.Any())
            {
                var trans = await this.BaseRepository().BeginTrans();
                try
                {
                    foreach (var entity in items)
                    {
                        entity.Create();
                        this.VerifyIsMyDataOnCreate(entity);
                        await trans.Insert(entity);
                    }
                    await trans.CommitTrans();
                }
                catch
                {
                    await trans.RollbackTrans();
                    throw;
                }
            }
        }
    }
}
