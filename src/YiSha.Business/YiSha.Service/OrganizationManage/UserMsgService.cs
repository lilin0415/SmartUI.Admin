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
using YiSha.Entity.OrganizationManage;
using YiSha.Model.Param.OrganizationManage;
using YiSha.Model.OrganizationManage;
using YiSha.Enum;
using Koo.Utilities.Exceptions;

using Koo.Utilities.Helpers;
using Koo.Utilities.FileFormaKoo.RSSHelper;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;

namespace YiSha.Service.OrganizationManage
{
    /// <summary>
    /// 创 建：admin
    /// 日 期：2022-11-24 10:41
    /// 描 述：服务类
    /// </summary>
    public class UserMsgService :  BaseRepositoryService
    {
        #region 获取数据
        public async Task<List<UserMsgEntity>> GetList(UserMsgListParam param)
        {
            var expression = ListFilter(param);
            var list = await this.BaseRepository().FindList(expression);
            return list.ToList();
        }

        public async Task<List<UserMsgModel>> GetPageList(UserMsgListParam param, Pagination pagination)
        {
            //var expression = ListFilter(param);

            var user = this.GetCurrentUser();

            var sql = string.Empty;
            //我发出的消息
            if (param.MsgDirection == (int)UserMsgDirectionEnumType.Sended)
            {
                //发送给某个用户
                //查询接收方的用户名称
                sql = $"(select a.*, b.RealName TargetName from usermsg a inner join sysuser b on a.ToId = b.Id " +
                    $" where  FromId={user.UserId} and FromIsDelete = 0 " +
                    $")";
                sql += "union all ";


                //发送给某个组织
                //查询接收方的租户名称，如我加入某个组织
                sql += $"(select a.*, b.Name TargetName from usermsg a inner join tenant b on a.ToId = b.Id " +
                    $" where  FromId={user.UserId} and FromIsDelete = 0 " +
                    $")";
            }
            else
            {
                //我申请加入组织，是给组织发的信息

                //我收到的消息
                //查询发送方用户的名称
                sql = $"select a.*, b.RealName TargetName from usermsg a inner join sysuser b on a.FromId = b.Id " +
                  $" where   ";

                sql += $"  ToId={user.UserId} ";
                sql += " and ToIsDelete = 0 ";

                //var tenantIds = user.MyTenantList.Select(x => x.TenantId);
                //if (tenantIds.Any())
                //{
                //    sql += $" or ToId in ({string.Join(",", tenantIds)})";
                //}
            }
            pagination.TotalCount = await this.BaseRepository().GetCount(sql);
            var list = await this.BaseRepository().FindList<UserMsgModel>(sql, pagination);
            var items = list.list.ToList();
            foreach (var item in items)
            {
                //如果当前显示的为我发出的消息
                if (item.FromId == this.GetCurrentUserId())
                {
                    item.ShowOk = 0;
                }
                else
                {
                    item.ShowOk = 1;
                }
            }

            return items;
        }

        public async Task<UserMsgEntity> GetEntity(long id)
        {
            return await this.BaseRepository().FindEntity<UserMsgEntity>(id);
        }
        #endregion

        #region 回复消息
        public async Task AckForm(UserMsgEntity entity)
        {
            var entityInDb = await this.GetEntity(entity.Id.GetValueOrDefault());
            if (entityInDb == null)
            {
                throw new DataNotExistedException();
            }
            
            if (entityInDb.AckStatus == 1)
            {
                throw new DuplicationDataExection("您已处理过此消息了");
            }

          
          
        }
        #endregion

        #region 提交数据
        public async Task SaveForm(UserMsgEntity entity)
        {
            if (entity.Id.IsNullOrZero())
            {
                entity.Create();
                this.VerifyIsMyDataOnCreate(entity);
                await this.BaseRepository().Insert(entity);
            }
            else
            {
                entity.Modify();
                this.VerifyIsMyDataOnModify(entity);
                await this.BaseRepository().Update(entity);
            }
        }

        public async Task DeleteForm(string ids)
        {
            //this.VerifyIsMyDataOnDelete<UserMsgEntity>(ids, false);
            long[] idArr = TextHelper.SplitToArray<long>(ids, ',');
            var items = await this.BaseRepository().FindList<UserMsgEntity>(x => idArr.Contains(x.Id.Value));

            var itemsToUpdate = new List<UserMsgEntity>();
            var itemsToDelete = new List<long>();

            foreach (var item in items)
            {
                //删除的是 我发出的信息
                if (item.FromId == this.GetCurrentUserId())
                {
                    if (item.ToIsDelete == 1)
                    {
                        itemsToDelete.Add(item.Id.Value);
                    }
                    else
                    {
                        item.FromIsDelete = 1;
                        item.FromDeleteTime = DateTimeHelper.Now;
                        item.Modify();
                        itemsToUpdate.Add(item);
                    }
                }
                else
                {
                    if (item.FromIsDelete == 1)
                    {
                        itemsToDelete.Add(item.Id.Value);
                    }
                    else
                    {
                        item.ToIsDelete = 1;
                        item.ToDeleteTime = DateTimeHelper.Now;
                        item.Modify();
                        itemsToUpdate.Add(item);
                    }
                }
            }

            var trans = await this.BaseRepository().BeginTrans();
            try
            {
                if (itemsToDelete.Any())
                {
                    await trans.Delete<UserMsgEntity>(itemsToDelete.ToArray());
                }
                if (itemsToUpdate.Any())
                {
                    await trans.Update(itemsToUpdate);
                }
                await trans.CommitTrans();
            }
            catch
            {
                await trans.RollbackTrans();
                throw;
            }
           
            
        }
        #endregion

        #region 私有方法
        private Expression<Func<UserMsgEntity, bool>> ListFilter(UserMsgListParam param)
        {
            var expression = LinqExtensions.True<UserMsgEntity>();
            if (param != null)
            {
            }
            return expression;
        }
        #endregion
    }
}
