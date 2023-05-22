using Koo.Utilities.Helpers;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using NPOI.SS.Formula.Functions;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YiSha.Data.Repository;
using YiSha.Entity.ProjectManager;
using YiSha.Entity.TestCaseManager;
using YiSha.Entity.TestTaskManager;
using YiSha.Model.Param.TestTaskManager;
using YiSha.Model.StatsModels;

namespace YiSha.Service
{
    public class StatsService: RepositoryFactory
    {
        public async Task<List<BaseStatsModel>> GetAllStatsModel()
        {
            var ret = new List<BaseStatsModel>();
            ret.Add(await GetProjectStatsModel());
            ret.Add(await GetTestCaseStatsModel());
            ret.Add(await GetTestTaskStatsModel());
            ret.Add(await GetTaskExecStatsModel());
            ret.Add(await GetCaseExecStatsModel());
            
            return ret;
        }

        public async Task<ProjectStatsModel> GetProjectStatsModel()
        {
            var ret = new ProjectStatsModel();

            await QueryCountData(PublishedProjectEntity.TblName, ret);
            return ret;
        }

        /// <summary>
        /// 用例
        /// </summary>
        /// <returns></returns>
        public async Task<TestCaseStatsModel> GetTestCaseStatsModel()
        {
            var ret = new TestCaseStatsModel();

            await QueryCountData(TestCaseEntity.TblName, ret);
            return ret;
        }

        /// <summary>
        /// 任务计划
        /// </summary>
        /// <returns></returns>
        public async Task<TestTaskStatsModel> GetTestTaskStatsModel()
        {
            var ret = new TestTaskStatsModel();

            await QueryCountData(TestTaskEntity.TblName, ret);
            return ret;
        }
      
        public async Task<TaskExecStatsModel> GetTaskExecStatsModel()
        {
            var ret = new TaskExecStatsModel();

            await QueryCountData(TaskExecRecordEntity.TblName, ret);
            return ret;
        }
        public async Task<CaseExecStatsModel> GetCaseExecStatsModel()
        {
            var ret = new CaseExecStatsModel();

            await QueryCountData(CaseExecRecordEntity.TblName, ret);
            return ret;
        }

        private async Task QueryCountData(string tableName,BaseStatsModel statsModel)
        {
            var fieldName = nameof(TestTaskEntity.BaseCreateTime);

            var sql = $" select count(1) from {tableName}";
            var sqlToday = $" select count(1) from {tableName} where {SqlHelperInternal.GetDatediffWhere(SqlHelperInternal.QueryTimePeriodEnumType.Today, fieldName, fieldName)}";
            var sqlYesterday = $" select count(1) from {tableName} where {SqlHelperInternal.GetDatediffWhere(SqlHelperInternal.QueryTimePeriodEnumType.Yesterday, fieldName, fieldName)}";
            var sqlThisWeek = $" select count(1) from {tableName} where {SqlHelperInternal.GetDatediffWhere(SqlHelperInternal.QueryTimePeriodEnumType.ThisWeek, fieldName, fieldName)}";

            var results = await this.BaseRepository().GetTables(sql, sqlToday, sqlYesterday, sqlThisWeek);

            statsModel.Total = DataTableHelper.ToList<int?>(results[0]).FirstOrDefault().GetValueOrDefault();
            statsModel.IncreasedToday = DataTableHelper.ToList<int?>(results[1]).FirstOrDefault().GetValueOrDefault();
            statsModel.IncreasedYesterday = DataTableHelper.ToList<int?>(results[2]).FirstOrDefault().GetValueOrDefault();
            statsModel.IncreasedThisWeek = DataTableHelper.ToList<int?>(results[3]).FirstOrDefault().GetValueOrDefault();
        }

        internal class SqlHelperInternal
        {
            public enum QueryTimePeriodEnumType
            {
                [Description("今天")]
                Today,
                [Description("昨天")]
                Yesterday,
                [Description("本周")]
                ThisWeek,
                [Description("上周")]
                LastWeek,
                [Description("本月")]
                ThisMonth,
                [Description("上月")]
                LastMonth,
                [Description("本季度")]
                ThisQuarter,
            }
            public static string GetDatediffWhere(QueryTimePeriodEnumType period, string fieldName, string inputdate)
            {
                if (period == QueryTimePeriodEnumType.Today)
                {
                    return $"to_days({fieldName}) = to_days(now())";
                }
                if (period == QueryTimePeriodEnumType.Yesterday)
                {
                    return $"TO_DAYS( NOW( ) ) - TO_DAYS({fieldName}) = 1";
                }
                if (period == QueryTimePeriodEnumType.ThisWeek)
                {
                    return $"YEARWEEK(date_format({fieldName},'%Y-%m-%d')) = YEARWEEK(now())";
                }
                if (period == QueryTimePeriodEnumType.LastWeek)
                {
                    return $"YEARWEEK(date_format({fieldName},'%Y-%m-%d')) = YEARWEEK(now())-1";
                }
                if (period == QueryTimePeriodEnumType.ThisMonth)
                {
                    return $"date_format({fieldName},'%Y-%m')=date_format(now(),'%Y-%m')";
                }
                if (period == QueryTimePeriodEnumType.LastMonth)
                {
                    return $"date_format({fieldName},'%Y-%m')=date_format(DATE_SUB(curdate(), INTERVAL 1 MONTH),'%Y-%m')";
                }
                if (period == QueryTimePeriodEnumType.ThisQuarter)
                {
                    return $"DATEDIFF(QQ, {inputdate}, GETDATE())=0";
                }

                return string.Empty;
            }

        }
    }

}
