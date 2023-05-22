using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Koo.Utilities.Helpers;
using Quartz;
using Quartz.Impl.Triggers;
using YiSha.Entity.Exceptions;
using YiSha.Entity.SystemManage;
using YiSha.Entity.TestTaskManager;
using YiSha.Enum;
using YiSha.Service.SystemManage;
using YiSha.Service.TestTaskManager;
using YiSha.Util;
using YiSha.Util.Extension;
using YiSha.Util.Model;

namespace YiSha.Business.AutoJob.TestTaskJob
{
    public class TestTaskJobExecute : IJob
    {
        private TestTaskService autoJobService = new TestTaskService();
        //private AutoJobLogService autoJobLogService = new AutoJobLogService();

        private DateTime? GetDateTime(DateTimeOffset? v)
        {
            if (v == null)
            {
                return null;
            }
            return v.Value.DateTime.AddHours(8);
        }

        public Task Execute(IJobExecutionContext context)
        {
            return Task.Run(async () =>
            {

                TData obj = new TData();
                long jobId = 0;
                TestTaskEntity dbJobEntity = null;
                try
                {
                    var jobData = context.JobDetail.JobDataMap;
                    jobId = jobData["Id"].ParseToLong();

                    // 获取数据库中的任务
                    dbJobEntity = await autoJobService.GetEntity(jobId);
                }
                catch (Exception ex)
                {
                    obj.Message = ex.GetOriginalException().Message;
                    LogHelper.Error(ex);
                    return;
                }

                if (dbJobEntity == null)
                {
                    return;
                }

                if (dbJobEntity.IsEnable != StatusEnum.Yes.ParseToInt())
                {
                    return;
                }

                try
                {
                    if (dbJobEntity.MaxExecCount > 0 && dbJobEntity.ExecedCount >= dbJobEntity.MaxExecCount)
                    {
                        await autoJobService.DisableFromScheduler(dbJobEntity);
                        await new JobCenter().Delete(dbJobEntity);

                        return;
                    }
                }
                catch (Exception ex)
                {
                    obj.Message = ex.GetOriginalException().Message;
                    LogHelper.Error(ex);
                    return;
                }

                try
                {
                 
                    var str = DateTime.Now + " " + jobId + " " + dbJobEntity.Name
                    + "\r\n context.JobRunTime" + context.JobRunTime.ToString()
                    + "\r\n context.ScheduledFireTimeUtc" + GetDateTime(context.ScheduledFireTimeUtc)?.ToString()
                    + "\r\n context.PreviousFireTimeUtc" + GetDateTime(context.PreviousFireTimeUtc)?.ToString()
                    + "\r\n context.FireTimeUtc" + GetDateTime(context.FireTimeUtc)?.ToString()
                    + "\r\n context.NextFireTimeUtc" + GetDateTime(context.NextFireTimeUtc)?.ToString();
                    Debug.WriteLine(str);

                    CronTriggerImpl trigger = context.Trigger as CronTriggerImpl;
                    if (trigger != null)
                    {
                        if (trigger.CronExpressionString != dbJobEntity.CronExpression)
                        {
                            // 更新任务周期
                            trigger.CronExpressionString = dbJobEntity.CronExpression;
                            await JobScheduler.GetScheduler().RescheduleJob(trigger.Key, trigger);
                        }

                        try
                        {
                            var taskRecord = new TaskExecRecordService();
                            await taskRecord.AddAutoRecord(jobId);
                        }
                        catch (TaskItemIsEmptyException)
                        {

                        }
                        catch (Exception ex)
                        {
                            obj.Message = ex.GetOriginalException().Message;
                            LogHelper.Error(ex);
                        }
                    }
                }
                catch (Exception ex)
                {
                    obj.Message = ex.GetOriginalException().Message;
                    LogHelper.Error(ex);
                }

                try
                {
                    #region 更新下次运行时间

                    dbJobEntity.PrevRunTime = context.FireTimeUtc.DateTime.AddHours(8);
                    dbJobEntity.NextRunTime = context.NextFireTimeUtc.Value.DateTime.AddHours(8);
                    await autoJobService.UpdateFromScheduler(dbJobEntity);

                    #endregion

                    //#region 记录执行状态
                    //await autoJobLogService.SaveForm(new AutoJobLogEntity
                    //{
                    //    JobGroupName = context.JobDetail.Key.Group,
                    //    JobName = context.JobDetail.Key.Name,
                    //    LogStatus = obj.StatusCode,
                    //    Remark = obj.Message
                    //});
                    //#endregion
                }
                catch (Exception ex)
                {
                    obj.Message = ex.GetOriginalException().Message;
                    LogHelper.Error(ex);
                }
            });
        }
    }
}
