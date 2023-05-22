using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Quartz;
using YiSha.Entity.SystemManage;
using YiSha.Service.SystemManage;
using YiSha.Util;
using YiSha.Util.Model;
using YiSha.Util.Extension;
using YiSha.Enum;
using YiSha.Business.SystemManage;
using YiSha.Service.TestTaskManager;
using YiSha.Entity.TestTaskManager;
using YiSha.Business.AutoJob.TestTaskJob;
using Koo.Utilities.Helpers;

namespace YiSha.Business.AutoJob
{
    public class JobCenter
    {
        public void Start()
        {
            Task.Run(async () =>
            {
                var scheduler = JobScheduler.GetScheduler();
                await scheduler.Start();

                await AddTestTask();

                //TData<List<AutoJobEntity>> obj = await new AutoJobBLL().GetList(null);
                //if (obj.Status)
                //{
                //    AddScheduleJob(obj.Result);
                //}
            });
        }

        #region 添加任务计划
        private void AddScheduleJob(List<AutoJobEntity> entityList)
        {
            try
            {
                foreach (AutoJobEntity entity in entityList)
                {
                    if (entity.StartTime == null)
                    {
                        entity.StartTime = DateTime.Now;
                    }
                    DateTimeOffset starRunTime = DateBuilder.NextGivenSecondDate(entity.StartTime, 1);
                    if (entity.EndTime == null)
                    {
                        entity.EndTime = DateTime.MaxValue.AddDays(-1);
                    }
                    DateTimeOffset endRunTime = DateBuilder.NextGivenSecondDate(entity.EndTime, 1);

                    var scheduler = JobScheduler.GetScheduler();
                    IJobDetail job = JobBuilder.Create<JobExecute>().WithIdentity(entity.JobName, entity.JobGroupName).Build();
                    job.JobDataMap.Add("Id", entity.Id);

                    ICronTrigger trigger = (ICronTrigger)TriggerBuilder.Create()
                                                 .StartAt(starRunTime)
                                                 .EndAt(endRunTime)
                                                 .WithIdentity(entity.JobName, entity.JobGroupName)
                                                 .WithCronSchedule(entity.CronExpression)
                                                 .Build();

                    scheduler.ScheduleJob(job, trigger);
                    scheduler.Start();
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
        }
        #endregion

        #region 清除任务计划
        public void ClearScheduleJob()
        {
            try
            {
                JobScheduler.GetScheduler().Clear();
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
        }
        #endregion

        public async Task Delete(TestTaskEntity entity)
        {
            var scheduler = JobScheduler.GetScheduler();
            var jobKey = new JobKey(entity.Name);

            var existed = scheduler.GetJobDetail(jobKey);
            if (existed != null)
            {
                await scheduler.PauseTrigger(new TriggerKey(entity.Name));
                await scheduler.UnscheduleJob(new TriggerKey(entity.Name));
                await scheduler.DeleteJob(jobKey);
            }
        }
        public async Task AddOrUpdate(TestTaskEntity entity)
        {
            try
            {
                await AddOrUpdateInternal(entity);
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
        }
        private async Task AddOrUpdateInternal(TestTaskEntity entity)
        {
            if (entity.IsEnable == 0 || entity.CronExpression==null || string.IsNullOrEmpty(entity.CronExpression.Trim()))
            {
                await Delete(entity);
                return;
            }
            ////每次编辑的时候，继续上次运行时间
            //if (DateTimeHelper.IsEmpty(entity.PrevRunTime))
            //{
            //    entity.FromTime = entity.PrevRunTime;
            //}

         


            IJobDetail job = JobBuilder.Create<TestTaskJobExecute>().WithIdentity(entity.Name).Build();
            job.JobDataMap.Add("Id", entity.Id);


            if (DateTimeHelper.IsEmpty(entity.FromTime))
            {
                entity.FromTime = DateTime.Now;
            }

            if (DateTimeHelper.IsEmpty(entity.ToTime))
            {
                entity.ToTime = DateTime.MaxValue.AddDays(-1);
            }

            DateTimeOffset starRunTime = DateBuilder.NextGivenSecondDate(entity.FromTime, 1);
            DateTimeOffset endRunTime = DateBuilder.NextGivenSecondDate(entity.ToTime, 1);

            var trigger = TriggerBuilder.Create()
                .StartAt(starRunTime)
                .EndAt(endRunTime)
                .WithIdentity(entity.Name)
                .WithCronSchedule(entity.CronExpression, x => x.WithMisfireHandlingInstructionDoNothing())
                .Build();

            //ICronTrigger trigger = (ICronTrigger)triggerBuilder.Build();
            

            var scheduler = JobScheduler.GetScheduler();
            var jobKey = new JobKey(entity.Name);

            var existed =scheduler.GetJobDetail(jobKey);
            if (existed != null)
            {
                var triggerKey = new TriggerKey(entity.Name);
                
                await scheduler.PauseTrigger(triggerKey);
                await scheduler.UnscheduleJob(triggerKey);
                await scheduler.DeleteJob(jobKey);
            }
            
            await scheduler.ScheduleJob(job, trigger);
        }

        #region 添加任务计划
        private async Task AddTestTask()
        {
            try
            {
                var taskService = new TestTaskService();
                var entityList = await taskService.GetList(null);

                foreach (TestTaskEntity entity in entityList)
                {
                    await AddOrUpdate(entity);
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
        }
        #endregion
    }
}
