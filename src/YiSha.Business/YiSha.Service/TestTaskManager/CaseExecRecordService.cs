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
using YiSha.Model.TestTaskManager;
using System.Diagnostics;
using YiSha.Enum;
using YiSha.Model.WebApis;
using YiSha.Entity.TestCaseManager;
using Koo.Utilities.Exceptions;
using Koo.Utilities.Helpers;
using YiSha.Data.EF;
using YiSha.Web.Code;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using YiSha.Model.StatsModels;
using YiSha.Entity.DeviceManager;

namespace YiSha.Service.TestTaskManager
{
    /// <summary>
    /// 创 建：admin
    /// 日 期：2022-10-16 18:04
    /// 描 述：服务类
    /// </summary>
    public class CaseExecRecordService : BaseRepositoryService
    {
        #region 查询作业数据
        public async Task<List<CaseExecRecordModel>> GetListByTaskExecId(long taskExecId)
        {
          
            var sql = $" select a.*,b.MD5 as ProjectMD5 from caseexecrecord a " +
                $"          left join publishedproject b on a.ProjectGuid = b.ProjectGuid and a.ProjectVersion = b.Version " +
                $" where TaskExecId={taskExecId}";
            var list = await this.BaseRepository().FindList<CaseExecRecordModel>(sql);

            return list.ToList();
        }

        public async Task<List<CaseExecRecordEntity>> GetList(CaseExecRecordListParam param)
        {
            var expression = ListFilter(param);
            var list = await this.BaseRepository().FindList(expression);
            return list.ToList();
        }

        /// <summary>
        /// 获取首页未完成的作业
        /// </summary>
        /// <param name="param"></param>
        /// <param name="pagination"></param>
        /// <returns></returns>
        public async Task<List<CaseExecRecordModel>> GetUnfinishedPageListJson(CaseExecRecordListParam param, Pagination pagination)
        {
            //var expression = ListFilter(param);
            //var list = await this.BaseRepository().FindList(expression, pagination);
            //var items = list.ToList();
            //return items.MapListTo<CaseExecRecordModel>();
            var expression = ListFilterSql(param);
            expression += $" and a.FinishStatus={(int)FinishStatusEnumType.None} ";

            pagination.TotalCount = await this.BaseRepository().GetCount(expression);
            var list = await this.BaseRepository().FindList<CaseExecRecordModel>(expression, pagination);
            var items = list.list.ToList();
            return items;
        }
        public async Task<List<CaseExecRecordModel>> GetPageList(CaseExecRecordListParam param, Pagination pagination)
        {
            //var expression = ListFilter(param);
            //var list = await this.BaseRepository().FindList(expression, pagination);
            //var items = list.ToList();
            //return items.MapListTo<CaseExecRecordModel>();

            var expression = ListFilterSql(param);

            pagination.TotalCount = await this.BaseRepository().GetCount(expression);
            var list = await this.BaseRepository().FindList<CaseExecRecordModel>(expression, pagination);
            var items = list.list.ToList();
            return items;
        }
        
        public async Task<long?> GetIdByGuid(string guid)
        {
            if (SqlStringHelper.IsSafeSqWhere(guid))
            {
                var sql = $"select Id from {CaseExecRecordEntity.TblName} where Guid ='{guid}' ";
                return await this.BaseRepository().GetValue<long?>(sql);
            }
            return null;
        }
        public async Task<(long? CaseExecId,long?TaskExecId,long?GroupId)> GetIdAndTaskExecIdByGuid(string guid)
        {
            
            if (SqlStringHelper.IsSafeSqWhere(guid))
            {
                var sql = $"select Id,TaskExecId,GroupId from {CaseExecRecordEntity.TblName} where Guid ='{guid}' ";
                var dt = await this.BaseRepository().FindTable(sql);
                if (dt.Rows.Count == 1)
                {
                    var caseExecId = DataConverter.ToLong(dt.Rows[0]["Id"]);
                    var taskExecId = DataConverter.ToLong(dt.Rows[0]["TaskExecId"]);
                    var groupId = DataConverter.ToLong(dt.Rows[0]["GroupId"]);

                    return (caseExecId, taskExecId, groupId);
                }
            }
            return (0,0,0);
        }
        public async Task<CaseExecRecordEntity> GetEntity(long id)
        {
            return await this.BaseRepository().FindEntity<CaseExecRecordEntity>(id);
        }
        private Expression<Func<CaseExecRecordEntity, bool>> ListFilter(CaseExecRecordListParam param)
        {
            var expression = CreateFilter<CaseExecRecordEntity>();
            if (param == null)
            {
                return expression;
            }
            if (SqlStringHelper.IsSafeSqWhere(param.Guid))
            {
                expression = expression.And(x => x.Guid == param.Guid);
            }

            //if (param.TaskExecId.HasValue)
            //{
            //    expression = expression.And(x => x.TaskExecId == param.TaskExecId);
            //}
            //if (SecurityHelper.IsSafeSqlParam(param.TaskName))
            //{
            //    expression = expression.And(x => x.Name.Contains(param.TaskName));
            //}
            if (SecurityHelper.IsSafeSqlParam(param.CaseCode))
            {
                expression = expression.And(x => x.Code.Contains(param.CaseCode));
            }
            if (SecurityHelper.IsSafeSqlParam(param.CaseName))
            {
                expression = expression.And(x => x.Name.Contains(param.CaseName));
            }
            return expression;
        }
        private string ListFilterSql(CaseExecRecordListParam param)
        {
            var sql = "select a.*,b.Name as TaskExecName,b.ConsumeMode,b.ConsumerId from caseexecrecord a";
            sql += " inner join taskexecrecord b on a.TaskExecId = b.Id ";
            sql += " where 1=1 ";
            if (param == null)
            {
                return sql;
            }
            if (SqlStringHelper.IsSafeSqWhere(param.Guid))
            {
                sql += $" and a.Guid='{param.Guid}'";
            }
            if (SqlStringHelper.IsSafeSqWhere(param.TaskExecGuid))
            {
                sql += $" and b.Guid ='{param.TaskExecGuid}'";
            }
            //if (param.TaskId.HasValue)
            //{
            //    expression = expression.And(x => x.TaskId == param.TaskId);
            //}
            if (SecurityHelper.IsSafeSqlParam(param.TaskExecName))
            {
                sql += $" and b.Name like '%{param.TaskExecName}%'";
            }
            if (SecurityHelper.IsSafeSqlParam(param.CaseCode))
            {
                sql += $" and a.Code like '%{param.CaseCode}%'";
            }
            if (SecurityHelper.IsSafeSqlParam(param.CaseName))
            {
                sql += $" and a.Name like '%{param.CaseName}%'";
            }
            return sql;
        }
        #endregion

        #region 保存作业数据
        public async Task<long> SaveForm(CaseExecRecordEntity entity)
        {
            throw new ForbidUpdateExection();
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

        #endregion

        #region 删除作业
        /// <summary>
        /// 删除作业
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        /// <exception cref="ForbidDeleteExection"></exception>
        public async Task DeleteForm(string ids)
        {
            throw new ForbidDeleteExection();
            this.VerifyIsMyDataOnDelete<CaseExecRecordEntity>(ids);
            long[] idArr = TextHelper.SplitToArray<long>(ids, ',');
            await this.BaseRepository().Delete<CaseExecRecordEntity>(idArr);
        }
        #endregion

        #region 取消作业
        /// <summary>
        /// 取消作业
        /// </summary>
        /// <param name="caseExecId"></param>
        /// <returns></returns>
        public async Task CancelCaseExec(long caseExecId)
        {
            var entity = await GetEntity(caseExecId);

            UpdateCaseExecStatusBody body = new UpdateCaseExecStatusBody();
          
            body.CaseExecGuid = entity.Guid;

            body.ExecStatus = ExecStatusEnumType.Finished;
            body.FinishStatus = FinishStatusEnumType.Cancelled;
            body.Reason = "从控制台取消";
            body.StartTime = DateTimeHelper.Now;
            body.EndTime = DateTimeHelper.Now;

            await UpdateCaseExecStatus(body,true);
        }
        #endregion

        #region 拉取任务，更新任务的状态
        /// <summary>
        /// 拉取任务
        /// </summary>
        /// <param name="user"></param>
        /// <param name="appToken"></param>
        /// <param name="appVersion"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public async Task<List<CaseExecRecordModel>> PullCaseList(OperatorInfo user, string appToken, string appVersion, int size)
        {
            List<CaseExecRecordModel> ret = new List<CaseExecRecordModel>();

            var deviceTokenInfo = AppTokenInfo.FromToken(appToken);
            if (size <= 0)
            {
                size = 1;
            }

            var sql = $"select * from device where UserId = {user.UserId.Value} and Guid ='{deviceTokenInfo.DeviceGuid}' ";
            var deviceList = await this.BaseRepository().FindList<DeviceEntity>(sql);
            var device = deviceList.FirstOrDefault();
            if (device == null)
            {
                throw new Exception("未找到当前客户端的注册信息");
            }

            var hasData = false;

            var db = this.BaseRepository().db as MySqlDatabase;
            await db.BeginTrans(async (x) =>
            {
                //1、查询推送至单个客户端的
                sql = $"select a.*, b.MD5 as ProjectMD5,c.TotalCaseCount, c.Name as TaskExecRecordName, d.Name as GroupName, c.Guid as TaskExecGuid  ";
                sql += $"from {CaseExecRecordEntity.TblName} a ";
                sql += $" inner join publishedproject b on a.ProjectGuid = b.ProjectGuid and a.ProjectVersion = b.Version ";//查询用例模板
                sql += $" inner join {TaskExecRecordEntity.TblName} c on a.TaskExecId= c.Id ";//查询任务
                sql += $"left join {TestCaseGroupEntity.TblName} d on a.GroupId =d.Id ";//查询用例组

                //sql += $"  where  a.ConsumeStatus=1 and a.IsConsumed =0  and a.FinishStatus ={(int)FinishStatusEnumType.None} ";
                sql += $"  where  a.ConsumeStatus={(int)ConsumeStatusEnumType.Ready} and c.ConsumerId = {device.Id} ";//待推送且全部 消费状态加索引，用于for update行锁
                //当有并发时，同时多个任务创建作业，不同任务之间的作业会交叉，直接按钮作业的创建时间排序
                //sql += $"  order by a.BaseCreateTime asc ";//按照创建时间排序或者 任务执行记录id（a.TaskExecId asc, ），然后同一个任务里面再按照SortNum排序，//暂时用创建时间，
                sql += $" order by a.Id asc ";
                sql += $"  limit {size}   ";//使用乐观锁 ，取消for update

                var list = await x.FindList<CaseExecRecordModel>(System.Data.CommandType.Text, sql);
                if (!list.Any())
                {
                    //查询当前客户端所属的组
                    sql = $"select GroupId from devicegroupdetail where DeviceId={device.Id} ";
                    var clientGroupIds = await x.FindList<long>(System.Data.CommandType.Text, sql);

                    if (clientGroupIds.Any())
                    {   
                        //查询待推送至客户端组的
                        sql = $"select a.*, b.MD5 as ProjectMD5,c.TotalCaseCount, c.Name as TaskExecRecordName, d.Name as GroupName, c.Guid as TaskExecGuid  ";
                        sql += $"from {CaseExecRecordEntity.TblName} a ";
                        sql += $" inner join publishedproject b on a.ProjectGuid = b.ProjectGuid and a.ProjectVersion = b.Version ";//查询用例模板
                        sql += $" inner join {TaskExecRecordEntity.TblName} c on a.TaskExecId= c.Id ";//查询任务
                        sql += $"left join {TestCaseGroupEntity.TblName} d on a.GroupId =d.Id ";//查询用例组

                        //sql += $"  where  a.ConsumeStatus=1 and a.IsConsumed =0  and a.FinishStatus ={(int)FinishStatusEnumType.None} ";
                        sql += $"  where  a.ConsumeStatus={(int)ConsumeStatusEnumType.Ready} and c.ConsumerId in( {string.Join(",", clientGroupIds.Distinct())} )";//待推送且全部 消费状态加索引，用于for update行锁
                        sql += $"  order by a.Id asc";//按照创建时间排序或者 任务执行记录id，然后同一个任务里面再按照SortNum排序，//暂时用创建时间，
                        sql += $"  limit {size}   ";//使用乐观锁 ，取消for update

                        list = await x.FindList<CaseExecRecordModel>(System.Data.CommandType.Text, sql);
                    }
                 
                }

                if (!list.Any())
                {
                    //查询待推送(Ready)且推送全部的作业。未消费并且未取消的作业，因为对于取消的用例消费状态为已取消
                    sql = $"select a.*, b.MD5 as ProjectMD5,c.TotalCaseCount, c.Name as TaskExecRecordName, d.Name as GroupName, c.Guid as TaskExecGuid  ";
                    sql += $"from {CaseExecRecordEntity.TblName} a ";
                    sql += $" inner join publishedproject b on a.ProjectGuid = b.ProjectGuid and a.ProjectVersion = b.Version ";//查询用例模板
                    sql += $" inner join {TaskExecRecordEntity.TblName} c on a.TaskExecId= c.Id ";//查询任务
                    sql += $"left join {TestCaseGroupEntity.TblName} d on a.GroupId =d.Id ";//查询用例组

                    //sql += $"  where  a.ConsumeStatus=1 and a.IsConsumed =0  and a.FinishStatus ={(int)FinishStatusEnumType.None} ";
                    sql += $"  where  a.ConsumeStatus={(int)ConsumeStatusEnumType.Ready} and c.ConsumerId = 0 ";//待推送且全部 消费状态加索引，用于for update行锁
                    sql += $"  order by a.Id asc ";//按照创建时间排序或者 任务执行记录id，然后同一个任务里面再按照SortNum排序，//暂时用创建时间，
                    sql += $"  limit {size}   ";//使用乐观锁 ，取消for update

                    list = await x.FindList<CaseExecRecordModel>(System.Data.CommandType.Text, sql);
                  
                }

                  
                if (!list.Any())
                {
                    return;
                }

                hasData = true;
                foreach (var item in list)
                {
                    //item.ConsumeStatus = (int)ConsumeStatusEnumType.Consumed;
                    //item.ConsumedTime = DateTimeHelper.Now;

                    //item.UserId = user.UserId;
                    //item.UserName = user.UserName;

                    //item.AppToken = appToken;
                    //item.AppVersion = appVersion;
                    //item.DeviceGuid = device.Guid;
                    //item.DeviceName = device.Name;
                    //item.DeviceIP = device.IP;
                    //item.DeviceMAC = device.MAC;
                    //item.DeviceLoginName = device.LoginName;

                    //消费时候需要返回给客户端
                    item.ConsumeStatus = (int)ConsumeStatusEnumType.Consumed;
                    item.ConsumedTime = DateTimeHelper.Now;
                    item.Modify();

                    sql = $@" update {CaseExecRecordEntity.TblName} 
                            set ConsumeStatus ={item.ConsumeStatus}, ConsumedTime='{DateTimeHelper.ToSqlString(item.ConsumedTime)}'
                            , UserId={user.UserId}, UserName='{user.UserName}'
                            , AppToken='{appToken}', DeviceGuid='{device.Guid}'
                            , DeviceName='{device.Name}', DeviceIP='{device.IP}'
                            , DeviceMAC='{device.MAC}', DeviceLoginName='{device.LoginName}'
                            , AppVersion='{appVersion}' 
                            , BaseModifyTime='{DateTimeHelper.ToSqlString(item.ConsumedTime)}', BaseModifierId={user.UserId}  
";
                    sql += $" where Id = {item.Id} and ConsumeStatus={(int)ConsumeStatusEnumType.Ready} ";
                    var succeed =await x.ExecuteNonQueryAsync(System.Data.CommandType.Text, sql);
                    if (succeed > 0)
                    {
                        ret.Add(item);
                    }
                }
              
            });

            if (hasData)
            {
                //从数据库中查询到数据，但由于乐观锁的原因，没有锁住，返回一个空的数据
                //告诉客户端，可以继续拉取作业
                if (ret.Count == 0)
                {
                    ret.Add(new CaseExecRecordModel() { Id=-1});
                }
            }
          
            return ret;
        }

        /// <summary>
        /// 仅拉取分配给指定客户端的、或者已经推送给客户端但客户端由于各种原因没有收到执行，需要重新拉取一遍
        /// </summary>
        /// <param name="user"></param>
        /// <param name="appToken"></param>
        /// <param name="appVersion"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public async Task<List<CaseExecRecordModel>> PullMyCaseList(OperatorInfo user, string appToken, string appVersion, int size)
        {
            List<CaseExecRecordModel> ret = new List<CaseExecRecordModel>();

            var deviceTokenInfo = AppTokenInfo.FromToken(appToken);
            if (size <= 0)
            {
                size = 1;
            }

            /*
             * 
             *   var sql = $" select a.*,b.MD5 as ProjectMD5 from caseexecrecord a " +
                $"          left join publishedproject b on a.ProjectGuid = a.ProjectGuid and a.ProjectVersion = b.Version " +
                $" where TaskExecId={taskExecId}";
            var list = await this.BaseRepository().FindList<CaseExecRecordModel>(sql);

            return list.ToList();
             */


            var db = this.BaseRepository().db as MySqlDatabase;
            await db.BeginTrans(async (x) =>
            {
                //查询未消费并且未取消的作业，因为对于取消的用例isConsumed还是0
                var sql = $"select a.*, b.MD5 as ProjectMD5,c.TotalCaseCount, c.Name as TaskExecRecordName, d.Name as GroupName  ";
                sql += $"from {CaseExecRecordEntity.TblName} a ";
                sql += $" inner join publishedproject b on a.ProjectGuid = b.ProjectGuid and a.ProjectVersion = b.Version ";//查询用例模板
                sql += $" inner join {TaskExecRecordEntity.TblName} c on a.TaskExecId= c.Id ";//查询任务
                sql += $"left join {TestCaseGroupEntity.TblName} d on a.GroupId =d.Id ";//查询用例组

                //sql += $"  where  a.ConsumeStatus=1 and a.IsConsumed =0  and a.FinishStatus ={(int)FinishStatusEnumType.None} ";
                sql += $"  where  a.AppToken='{appToken}' ";
                sql += @$"    and (a.ConsumeStatus={(int)ConsumeStatusEnumType.Ready}  "; //待推送
                sql += $"           or a.ConsumeStatus={(int)ConsumeStatusEnumType.Consumed} " +
                $"and a.ExecStatus={(int)ExecStatusEnumType.Ready} " +
                $"and a.FinishStatus={(int)FinishStatusEnumType.None}" +
                //推送过去后1个小时还未开始，重新拉取一次 currenttime-ConsumedTime>2，即ConsumedTime<currenttime-2
                $"and a.ConsumedTime<='{DateTimeHelper.Now.AddHours(-2).ToString("yyyy-MM-dd HH:mm:ss.fff")}'" +
                $")";//已推送并且未执行且时间超过1小时，

                sql += $"  order by a.TaskExecId asc, a.BaseCreateTime asc ";//按照创建时间排序或者 任务执行记录id，然后同一个任务里面再按照SortNum排序，//暂时用创建时间，
                sql += $"  limit {size} ";//这里不需要for update，因为查询的是自己用数据
                
                var list = await x.FindList<CaseExecRecordModel>(System.Data.CommandType.Text, sql);
                ret = list.ToList();
                if (!ret.Any())
                {
                    return;
                }


                sql = $"select * from device where UserId = {user.UserId.Value} and Guid ='{deviceTokenInfo.DeviceGuid}' ";
                var deviceList = await x.FindList<DeviceEntity>(System.Data.CommandType.Text, sql);
                var device = deviceList.FirstOrDefault();
                if (device == null)
                {
                    device = new DeviceEntity();
                    device.Guid = deviceTokenInfo.DeviceGuid;
                }

                foreach (var item in ret)
                {
                    //item.ConsumeStatus = (int)ConsumeStatusEnumType.Consumed;
                    //item.ConsumedTime = DateTimeHelper.Now;

                    //item.UserId = user.UserId;
                    //item.UserName = user.UserName;

                    //item.AppToken = appToken;
                    //item.AppVersion = appVersion;
                    //item.DeviceGuid = device.Guid;
                    //item.DeviceName = device.Name;
                    //item.DeviceIP = device.IP;
                    //item.DeviceMAC = device.MAC;
                    //item.DeviceLoginName = device.LoginName;


                    item.Modify();

                    sql = $@" update {CaseExecRecordEntity.TblName} 
                            set ConsumeStatus ={(int)ConsumeStatusEnumType.Consumed}, ConsumedTime='{DateTimeHelper.ToSqlString()}'
                            , UserId={user.UserId}, UserName='{user.UserName}'
                            , AppToken='{appToken}', DeviceGuid='{device.Guid}'
                            , DeviceName='{device.Name}', DeviceIP='{device.IP}'
                            , DeviceMAC='{device.MAC}', DeviceLoginName='{device.LoginName}'
                            , AppVersion='{appVersion}' 
                            , BaseModifyTime='{DateTimeHelper.ToSqlString()}', BaseModifierId={user.UserId}  
";
                    sql += $" where Id = {item.Id}";
                    await x.ExecuteNonQueryAsync(System.Data.CommandType.Text, sql);
                }

            });

            return ret;
        }

        /// <summary>
        /// 更改用例的运行状态、结果
        /// </summary>
        /// <param name="body"></param>
        /// <returns></returns>
        public async Task<bool> UpdateCaseExecStatus(UpdateCaseExecStatusBody body,bool cancelFromWeb=false)
        {
            var sql = string.Empty;
            var idTable = await new CaseExecRecordService().GetIdAndTaskExecIdByGuid(body.CaseExecGuid);
            var caseExecId = idTable.CaseExecId;
            var taskExecId = idTable.TaskExecId;
            var groupid = idTable.GroupId;

            if (caseExecId == 0)
            {
                throw new BizException("未找到作业:" + body.CaseExecGuid);
            }

            //对于并发来说，有的执行快，有的执行慢，
            //所以不能判断第一个是最快的，
            //只要收到作业消息，就可以认为任务已经开始，加
            if (/*body.SortNum == 1 && */body.ExecStatus == ExecStatusEnumType.Initing)
            {
                if (!body.StartTime.HasValue)
                {
                    body.StartTime = DateTimeHelper.Now;
                }

                //设置任务开始运行
                sql = $"update  {TaskExecRecordEntity.TblName} ";
                sql += $"  set {nameof(TaskExecRecordEntity.ExecStatus)}={(int)ExecStatusEnumType.Running} ";
                sql += $", {nameof(TaskExecRecordEntity.StartTime)}='{body.StartTime}' ";
                sql += $" where {nameof(TaskExecRecordEntity.Id)}={taskExecId} " +
                $" and {nameof(TaskExecRecordEntity.ExecStatus)}={(int)ExecStatusEnumType.Ready}";
                await this.BaseRepository().ExecuteBySql(sql);

                //设置作业为初始化状态？
            }
            else if (body.ExecStatus == ExecStatusEnumType.Running)
            {
                if (!body.StartTime.HasValue)
                {
                    body.StartTime = DateTimeHelper.Now;
                }

                var db = this.BaseRepository().db as MySqlDatabase;
                await db.BeginTrans(async trans => {

                    //更新就绪状态的作业为运行状态，并设置开始时间
                    //如果作业为客户端重启重置执行的呢？
                    //因为客户端只有初始化，但未运行的任务才会重新开始
                    sql = $"update  caseexecrecord ";
                    sql += $"  set {nameof(CaseExecRecordEntity.ExecStatus)}={(int)ExecStatusEnumType.Running} ";
                    sql += $", {nameof(CaseExecRecordEntity.StartTime)}='{body.StartTime}' ";
                    sql += $" where {nameof(CaseExecRecordEntity.Id)}={caseExecId} ";
                    sql += $"   and {nameof(CaseExecRecordEntity.ExecStatus)}={(int)ExecStatusEnumType.Ready} ";
                    await trans.ExecuteNonQueryAsync(System.Data.CommandType.Text, sql);
                });
            }
            else if (body.ExecStatus == ExecStatusEnumType.Finished)
            {
                Debug.Assert(body.FinishStatus.HasValue);
                //如果任务直接从后台取消，下发状态、下放时间；开始时间都没有
                var db = this.BaseRepository().db as MySqlDatabase;
                await db.BeginTrans(async trans =>
                { 
                    //一般情况下，结束状态是不带开始时间的。
                    //但是，如果任务还没有执行就出错了，会带上开始时间，一般等于结束时间
                    if (body.StartTime == null)
                    {
                        body.StartTime = body.EndTime;
                    }

                    var startTime = body.StartTime.ToLongString();
                    var endTime = body.EndTime.ToLongString();

                    #region 更新作业为结束状态、及结束时间、结果、原因

                    //设置用例结束时间、结束状态
                    var sql = $"update  caseexecrecord " +
                    $"  set {nameof(CaseExecRecordEntity.ExecStatus)}={(int)ExecStatusEnumType.Finished} ";


                    //对于完成的作业，设置消费状态
                    //正常消费结束的作业，消费状态肯定是已消费
                    sql += $", ConsumeStatus= (case ConsumeStatus ";
                    sql += $"                   when {(int)ConsumeStatusEnumType.Pending} then {(int)ConsumeStatusEnumType.Cancelled} ";
                    sql += $"                   when {(int)ConsumeStatusEnumType.Ready} then {(int)ConsumeStatusEnumType.Cancelled} ";
                    sql += $"                   else ConsumeStatus  end ) ";

                    sql += $", StartTime= (case StartTime when '1970-01-01 00:00:00' then '{startTime}' else StartTime  end ) ";
                    sql += $", {nameof(CaseExecRecordEntity.EndTime)}='{endTime}' ";
                    sql += $", {nameof(CaseExecRecordEntity.FinishStatus)}={(int)body.FinishStatus}";
                    sql += $", {nameof(CaseExecRecordEntity.Reason)}='{body.Reason}'";
                    sql += $", {nameof(CaseExecRecordEntity.SucceedAssertionCount)}={body.SucceedAssertionCount}";
                    sql += $", {nameof(CaseExecRecordEntity.FailedAssertionCount)}={body.FailedAssertionCount}";

                    sql += $" where {nameof(CaseExecRecordEntity.Id)}={caseExecId} ";
                    sql += $"   and {nameof(CaseExecRecordEntity.FinishStatus)}={(int)FinishStatusEnumType.None} ";
                    var updateFinished = await trans.ExecuteNonQueryAsync(System.Data.CommandType.Text, sql);
                    #endregion

                    //某些情况，一个作业可能会发送多次结果，因此加个执行结果条件
                    if (updateFinished > 0)
                    {
                        #region 检查是否为任务中最后一个作业 并查询作业的最近一个错误原因，用来设置任务的错误原因
                        //查询还剩余几个用例没有执行完成
                        //如果还有1个没有执行完成，则当前是最后一个，需要设置结束时间、结果等
                        sql = $"select (TotalCaseCount-SucceedCaseCount-FailedCaseCount-CancelledCaseCount) as Unfinished ";
                        sql += $"from {TaskExecRecordEntity.TblName} ";
                        sql += $" where {nameof(TaskExecRecordEntity.Id)}={taskExecId} for update ";
                        var unfinishedCaseCountObj = await trans.ExecuteScalarAsync(System.Data.CommandType.Text, sql);
                        var unfinishedCaseCount = DataConverter.ToInt(unfinishedCaseCountObj).GetValueOrDefault();


                        var finalFinishStatus = body.FinishStatus;
                        var finalReason = body.Reason;

                        //如果还剩余一个用例，即当前是最后一个
                        //查询所有用例的结果
                        if (unfinishedCaseCount == 1)
                        {
                            //当前是通过后台主动取消的任务
                            if (cancelFromWeb)
                            {
                                //完成原因、完成状态使用原来的
                            }
                            //如果当前已经是错误了，直接设置任务的错误信息
                            //否则查询最近的一条错误信息
                            else if (body.FinishStatus == FinishStatusEnumType.Failed)
                            {

                            }
                            else
                            {
                                sql = $"select Reason ";
                                sql += $"from {CaseExecRecordEntity.TblName} ";
                                sql += $" where {nameof(CaseExecRecordEntity.TaskExecId)} ={taskExecId} ";
                                sql += $" and {nameof(CaseExecRecordEntity.FinishStatus)}={(int)FinishStatusEnumType.Failed} ";
                                sql += $"order by BaseModifyTime desc ";
                                sql += $"limit 1 ";
                                var list = await trans.FindList<string>(System.Data.CommandType.Text, sql);
                                var reasons = list.ToList();
                                if (reasons.Any())
                                {
                                    finalFinishStatus = FinishStatusEnumType.Failed;
                                    finalReason = body.Reason;
                                }
                                else
                                {
                                    finalFinishStatus = FinishStatusEnumType.Succeeded;
                                    finalReason = "";
                                }
                            }
                        }
                        #endregion

                        #region 更新任务中用例的进度，开始时间、结束时间、结束状态
                        sql = $"update  {TaskExecRecordEntity.TblName} set ";

                        //设置任务中用例的成功、失败、取消个数
                        if (body.FinishStatus == FinishStatusEnumType.Succeeded)
                        {
                            sql += " SucceedCaseCount=SucceedCaseCount+1 ";
                        }
                        else if (body.FinishStatus == FinishStatusEnumType.Failed)
                        {
                            sql += " FailedCaseCount=FailedCaseCount+1 ";
                        }
                        else if (body.FinishStatus == FinishStatusEnumType.Cancelled)
                        {
                            sql += " CancelledCaseCount=CancelledCaseCount+1 ";
                        }

                        sql += $", StartTime= (case StartTime when '1970-01-01 00:00:00' then '{startTime}' else StartTime  end ) ";

                        //并发时，有的执行快，有的执行慢，不能依赖于是否是最后一个用例，来结束任务
                        //如果为最后一个用例，设置任务的结束状态
                        //if (body.SortNum == body.TotalCaseCount)
                        if(unfinishedCaseCount==1)
                        {
                            sql += $", {nameof(TaskExecRecordEntity.ExecStatus)}={(int)ExecStatusEnumType.Finished} ";
                            sql += $", {nameof(TaskExecRecordEntity.EndTime)}='{endTime}' ";
                            sql += $", {nameof(TaskExecRecordEntity.FinishStatus)}={(int)finalFinishStatus}";
                            sql += $", {nameof(TaskExecRecordEntity.Reason)}='{finalReason}'";
                        }

                        sql += $" where {nameof(TaskExecRecordEntity.Id)}={taskExecId} ";
                        await trans.ExecuteNonQueryAsync(System.Data.CommandType.Text, sql);
                        #endregion

                        #region 启用下一个作业的可消费状态
                        //不同任务之间的作业是并发的，因此是判断，同一个任务中的下一个作业
                        //如果为最后一个用例，设置任务的结束状态
                        if (unfinishedCaseCount==1)
                        {

                        }
                        else
                        {
                            /*
                             * 
                             * 只有在串行任务的时候才需要在当前作业处理完成后根据序号查找下一个需要运行的作业
                             * 
                             * 如果没有找到下一个需要的运行的作业，
                             *      1、当前已经是最后一个条件（这个说明作业已经处理完毕），
                             *      2、下一个状态已经处理运行中（这个说明作业是并行的）
                             */
                         
                            //查找下一个待消费的用例 
                            sql = $"select a.Id, a.GroupId, a.ConsumeStatus   ";
                            sql += $"from {CaseExecRecordEntity.TblName} a ";
                            sql += $"where  {nameof(CaseExecRecordEntity.TaskExecId)}={taskExecId} ";
                            sql += $"   and ExecStatus ={(int)ExecStatusEnumType.Ready} ";
                            //sql += $"   and SortNum ={body.SortNum + 1} ";//这个有可能会查到组，
                            sql += $"   and ConsumeStatus ={(int)ConsumeStatusEnumType.Pending} ";//组的状态是Invalid，
                            sql += $" order by SortNum asc ";//升序查找下一下待消费的
                            sql += $"limit 1 ";

                            var list = await trans.FindList<CaseExecRecordModel>(System.Data.CommandType.Text, sql);
                            var next = list.FirstOrDefault();
                            if (next == null)
                            {
                                //如果未找到，返回
                                return;
                            }
                            else
                            {
                                //如果这两条记录的组id都为0，则说明此任务为串行任务，并且下一个记录为用例
                                //如果这两条记录都是同一个组里面的，则说明这个组是串行的，
                                //或者下一个记录为单个用例
                                if (groupid == next.GroupId.GetValueOrDefault() || next.GroupId.GetValueOrDefault() == 0)
                                {
                                    sql = $"update {CaseExecRecordEntity.TblName} set ConsumeStatus = {(int)ConsumeStatusEnumType.Ready} ";
                                    sql += $" where {nameof(CaseExecRecordEntity.Id)}={next.Id} ";
                                    await trans.ExecuteNonQueryAsync(System.Data.CommandType.Text, sql);
                                }
                                else
                                {
                                    //开始设置一个新的组数据

                                    //查询组的运行模式
                                    sql = $"select {nameof(TestCaseGroupEntity.IsParallelMode)} ";
                                    sql += $"from {TestCaseGroupEntity.TblName} ";
                                    sql += $"where id={next.GroupId} ";
                                    var caseGroupList = await trans.FindList<TestCaseGroupEntity>(System.Data.CommandType.Text, sql);
                                    var caseGroup = caseGroupList.FirstOrDefault();
                                    if (caseGroup.IsParallelMode == 1)
                                    {
                                        //设置组中都可消费
                                        sql = $"update {CaseExecRecordEntity.TblName} set ConsumeStatus = {(int)ConsumeStatusEnumType.Ready} ";
                                        sql += $" where {nameof(CaseExecRecordEntity.TaskExecId)}={taskExecId} ";
                                        sql += $" and {nameof(CaseExecRecordEntity.GroupId)}={next.GroupId} ";
                                        sql += $" and {nameof(CaseExecRecordEntity.CaseId)}!=0 ";//不包含组
                                        await trans.ExecuteNonQueryAsync(System.Data.CommandType.Text, sql);
                                    }
                                    else
                                    {
                                        //只设置组中的第一个可以消费，即组为串行的
                                        sql = $"update {CaseExecRecordEntity.TblName} set ConsumeStatus = {(int)ConsumeStatusEnumType.Ready} ";
                                        sql += $" where {nameof(CaseExecRecordEntity.Id)}={next.Id} ";
                                        await trans.ExecuteNonQueryAsync(System.Data.CommandType.Text, sql);
                                    }
                                }
                            }
                        }
                        #endregion

                    }

                });
                 
            }
           
            return true;

        }


        #endregion
    }
}
