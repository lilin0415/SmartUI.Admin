using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using YiSha.Util;
using YiSha.Util.Extension;
using YiSha.Util.Model;
using YiSha.Entity.TestTaskManager;
using YiSha.Model.Param.TestTaskManager;
using YiSha.Service.TestTaskManager;
using YiSha.Model.TestTaskManager;
using Koo.Utilities.Helpers;
using YiSha.Enum;
using YiSha.Model.Param.TestCaseManager;
using YiSha.Model.TestCaseManager;
using YiSha.Service.TestCaseManager;

namespace YiSha.Business.TestTaskManager
{
    /// <summary>
    /// 创 建：admin
    /// 日 期：2022-10-02 16:21
    /// 描 述：业务类
    /// </summary>
    public class TestTaskItemBLL
    {
        private TestTaskItemService testTaskItemService = new TestTaskItemService();

        #region 获取数据
        public async Task<TData<List<TestTaskItemEntity>>> GetList(TestTaskItemListParam param)
        {
            TData<List<TestTaskItemEntity>> obj = new TData<List<TestTaskItemEntity>>();
            obj.Result = await testTaskItemService.GetList(param);
            obj.Total = obj.Result.Count;
            obj.Status = true;
            return obj;
        }

        public async Task<TData<List<TestTaskItemModel>>> GetPageList(TestTaskItemListParam param, Pagination pagination)
        {
            TData<List<TestTaskItemModel>> obj = new TData<List<TestTaskItemModel>>();

            var models = await testTaskItemService.GetPageList(param, pagination);

            var envSerice = new ExecEnvironmentService();
            var envList = await envSerice.GetList(new ExecEnvironmentListParam());

            //foreach (var model in models)
            //{
            //    model.EnvDisplayName = envList.FirstOrDefault(x => x.Id == model.EnvId)?.Name;
            //    var usingVersionEnum = DataConverter.ToEnum<UsingVersionEnumType>(model.UsingVersion);
            //    model.UsingVersionDisplayName = DescriptionHelper.GetDescription(usingVersionEnum);
            //}

            obj.Result = models;
            obj.Total = pagination.TotalCount;
            obj.Status = true;

            return obj;
        }

        public async Task<TData<TestTaskItemEntity>> GetEntity(long id)
        {
            TData<TestTaskItemEntity> obj = new TData<TestTaskItemEntity>();
            obj.Result = await testTaskItemService.GetEntity(id);
            if (obj.Result != null)
            {
                obj.Status = true;
            }
            return obj;
        }
        #endregion

        #region 提交数据
        public async Task<TData<string>> SaveForm(TestTaskItemEntity entity)
        {
            TData<string> obj = new TData<string>();
            await testTaskItemService.SaveForm(entity);
            obj.Result = entity.Id.ParseToString();
            obj.Status = true;
            return obj;
        }

        public async Task<TData> DeleteForm(string ids)
        {
            TData obj = new TData();
            await testTaskItemService.DeleteForm(ids);
            obj.Status = true;
            return obj;
        }
        #endregion

        #region 私有方法
        #endregion
    }
}
