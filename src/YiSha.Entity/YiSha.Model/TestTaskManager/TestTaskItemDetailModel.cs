using Koo.Utilities.Helpers;
using Newtonsoft.Json;
using NPOI.SS.Formula.Functions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using YiSha.Model.TestCaseManager;
using YiSha.Util;

namespace YiSha.Model.TestTaskManager
{
    public class TestTaskItemDetailModel
    {
        public TestTaskItemDetailModel()
        {
            this.Items = new List<TestTaskCaseItemModel>();
            this.Groups = new List<TestTaskCaseGroupItemModel>();
        }
        public List<TestTaskCaseItemModel> Items
        {
            get;set;
        }
        public List<TestTaskCaseGroupItemModel> Groups
        {
            get;set;
        }
    }

    /// <summary>
    /// 执行用例接口
    /// 单个执行用例、用例组
    /// </summary>
    public interface ITaskItemCaseBase
    {
        [JsonConverter(typeof(StringJsonConverter))]
        long? TaskId
        {
            get; set;
        }
        [JsonConverter(typeof(StringJsonConverter))]
        long? TaskItemId
        {
            get; set;
        }
        [JsonConverter(typeof(StringJsonConverter))]
        long? GroupId
        {
            get; set;
        }
        [JsonConverter(typeof(StringJsonConverter))]
        long? CaseId
        {
            get; set;
        }


        string Code
        {
            get; set;
        }

        string Name
        {
            get; set;
        }

        byte? ExecStatus
        {
            get; set;
        }
        [JsonConverter(typeof(DateTimeJsonConverter))]
        DateTime? StartTime
        {
            get; set;
        }
        [JsonConverter(typeof(DateTimeJsonConverter))]
        DateTime? EndTime
        {
            get; set;
        }
        byte? FinishStatus
        {
            get; set;
        }

        string Reason
        {
            get; set;
        }
        int SortNum
        {
            get;set;
        }
        DateTime? TaskItemCreateTime
        {
            get; set;
        }
        /// <summary>
        /// 断言数量
        /// </summary>
        /// <returns></returns>
      
        int? AssertionCount
        {
            get; set;
        }

        /// <summary>
        /// 支持并行,在生成作业的时候通过sql查询值
        /// </summary>
        /// <returns></returns>
      
        byte? SupportParallel
        {
            get; set;
        }
        int IsEnable
        {
            get;set;
        }
    }
    /// <summary>
    /// 测试明细中的 执行用例
    /// </summary>
    public class TestTaskCaseItemModel : TestCaseModel, ITaskItemCaseBase
    {
      
        [JsonConverter(typeof(StringJsonConverter))]

        public long? TaskItemId
        {
            get; set;
        }
        [JsonConverter(typeof(StringJsonConverter))]

        public long? TaskId
        {
            get; set;
        }

        [JsonConverter(typeof(StringJsonConverter))]

        public long? GroupId
        {
            get; set;
        }

        [JsonConverter(typeof(StringJsonConverter))]

        public long? CaseId
        {
            get
            {
                return this.Id;
            }
            set
            {
                this.Id = value;
            }
        }
        public int SortNum
        {
            get;set;
        }
        public DateTime? TaskItemCreateTime
        {
            get;set;
        }
        public int IsEnable
        {
            get; set;
        }
        //[JsonConverter(typeof(StringJsonConverter))]
        //long ITaskItemCaseScene.TaskId
        //{
        //    get
        //    {
        //        return this.TaskId.HasValue ? this.TaskId.Value : 0;
        //    }
        //    set
        //    {
        //        this.TaskId = value;
        //    }
        //}
        //[JsonConverter(typeof(StringJsonConverter))]
        //long ITaskItemCaseScene.TaskItemId
        //{
        //    get
        //    {
        //        return this.TaskItemId.HasValue ? this.TaskItemId.Value : 0;
        //    }
        //    set
        //    {
        //        this.TaskItemId = value;
        //    }
        //}
        //[JsonConverter(typeof(StringJsonConverter))]
        //long ITaskItemCaseScene.GroupId
        //{
        //    get
        //    {
        //        return this.GroupId.HasValue ? this.GroupId.Value : 0;
        //    }
        //    set
        //    {
        //        this.GroupId = value;
        //    }
        //}
        //[JsonConverter(typeof(StringJsonConverter))]
        //long ITaskItemCaseScene.CaseId
        //{
        //    get
        //    {
        //        return this.Id.HasValue ? this.Id.Value : 0;
        //    }
        //    set
        //    {
        //        this.Id = value;
        //    }
        //}
        //int ITaskItemCaseScene.ExecStatus
        //{
        //    get
        //    {
        //        return this.ExecStatus.HasValue ? this.ExecStatus.Value : 0;
        //    }
        //    set
        //    {
        //        this.ExecStatus = (byte)value;
        //    }
        //}
        //[JsonConverter(typeof(DateTimeJsonConverter))]
        //DateTime ITaskItemCaseScene.StartTime
        //{
        //    get
        //    {
        //        return this.StartTime.HasValue ? this.StartTime.Value : DateTimeHelper.Empty;
        //    }
        //    set
        //    {
        //        this.StartTime = value;
        //    }
        //}
        //[JsonConverter(typeof(DateTimeJsonConverter))]
        //DateTime ITaskItemCaseScene.EndTime
        //{
        //    get
        //    {
        //        return this.EndTime.HasValue ? this.EndTime.Value : DateTimeHelper.Empty;
        //    }
        //    set
        //    {
        //        this.EndTime = value;
        //    }
        //}
        //int ITaskItemCaseScene.FinishStatus
        //{
        //    get
        //    {
        //        return this.FinishStatus.HasValue ? this.FinishStatus.Value : 0;
        //    }
        //    set
        //    {
        //        this.FinishStatus = (byte)value;
        //    }
        //}

    }

    /// <summary>
    /// 测试明细中的 执行用例组
    /// </summary>
    public class TestTaskCaseGroupItemModel : TestCaseGroupModel, ITaskItemCaseBase
    {
        public TestTaskCaseGroupItemModel()
        {
            this.Items = new List<TestTaskCaseItemModel>();
        }
        public List<TestTaskCaseItemModel> Items
        {
            get; set;
        }

        [JsonConverter(typeof(StringJsonConverter))]

        public long? TaskItemId
        {
            get; set;
        }

        [JsonConverter(typeof(StringJsonConverter))]
        public long? TaskId
        {
            get;set;
        }
        [JsonConverter(typeof(StringJsonConverter))]
        public long? GroupId
        {
            get
            {
                return this.Id.HasValue ? this.Id.Value : 0;
            }
            set
            {
                this.Id = value;
            }
        }
        [JsonConverter(typeof(StringJsonConverter))]
        public long? CaseId
        {
            get; set;
        } = 0;
        public int SortNum
        {
            get; set;
        }
        public DateTime? TaskItemCreateTime
        {
            get; set;
        }
        /// <summary>
        /// 断言数量
        /// </summary>
        /// <returns></returns>
     
        public int? AssertionCount
        {
            get; set;
        }

        /// <summary>
        /// 支持并行,在生成作业的时候通过sql查询值
        /// </summary>
        /// <returns></returns>
      
        public byte? SupportParallel
        {
            get; set;
        }
        public int IsEnable
        {
            get; set;
        }
        //long? ITaskItemCaseScene.TaskId
        //{
        //    get => throw new NotImplementedException();
        //    set => throw new NotImplementedException();
        //}
        //long? ITaskItemCaseScene.GroupId
        //{
        //    get => throw new NotImplementedException();
        //    set => throw new NotImplementedException();
        //}
        //long? ITaskItemCaseScene.CaseId
        //{
        //    get => throw new NotImplementedException();
        //    set => throw new NotImplementedException();
        //}

        //[JsonConverter(typeof(StringJsonConverter))]
        //long ITaskItemCaseScene.TaskItemId
        //{
        //    get
        //    {
        //        return this.TaskItemId.HasValue ? this.TaskItemId.Value : 0;
        //    }
        //    set
        //    {
        //        this.TaskItemId = value;
        //    }
        //}
        //int ITaskItemCaseScene.ExecStatus
        //{
        //    get
        //    {
        //        return this.ExecStatus.HasValue ? this.ExecStatus.Value : 0;
        //    }
        //    set
        //    {
        //        this.ExecStatus = (byte)value;
        //    }
        //}
        //[JsonConverter(typeof(DateTimeJsonConverter))]
        //DateTime ITaskItemCaseScene.StartTime
        //{
        //    get
        //    {
        //        return this.StartTime.HasValue ? this.StartTime.Value : DateTimeHelper.Empty;
        //    }
        //    set
        //    {
        //        this.StartTime = value;
        //    }
        //}
        //[JsonConverter(typeof(DateTimeJsonConverter))]
        //DateTime ITaskItemCaseScene.EndTime
        //{
        //    get
        //    {
        //        return this.EndTime.HasValue ? this.EndTime.Value : DateTimeHelper.Empty;
        //    }
        //    set
        //    {
        //        this.EndTime = value;
        //    }
        //}
        //int ITaskItemCaseScene.FinishStatus
        //{
        //    get
        //    {
        //        return this.FinishStatus.HasValue ? this.FinishStatus.Value : 0;
        //    }
        //    set
        //    {
        //        this.FinishStatus = (byte)value;
        //    }
        //}
    }
}
