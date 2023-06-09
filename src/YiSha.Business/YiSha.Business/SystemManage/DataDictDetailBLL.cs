﻿using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using YiSha.Util;
using YiSha.Util.Extension;
using YiSha.Util.Model;
using YiSha.Entity.SystemManage;
using YiSha.Model.Param.SystemManage;
using YiSha.Service.SystemManage;
using YiSha.Service.Cache;

namespace YiSha.Business.SystemManage
{
    public class DataDictDetailBLL
    {
        private DataDictDetailService dataDictDetailService = new DataDictDetailService();

        private DataDictDetailCache dataDictDetailCache = new DataDictDetailCache();

        #region  获取数据
        public async Task<TData<List<DataDictDetailEntity>>> GetList(DataDictDetailListParam param)
        {
            TData<List<DataDictDetailEntity>> obj = new TData<List<DataDictDetailEntity>>();
            obj.Result = await dataDictDetailService.GetList(param);
            obj.Total = obj.Result.Count;
            obj.Status = true;
            return obj;
        }

        public async Task<TData<List<DataDictDetailEntity>>> GetPageList(DataDictDetailListParam param, Pagination pagination)
        {
            TData<List<DataDictDetailEntity>> obj = new TData<List<DataDictDetailEntity>>();
            obj.Result = await dataDictDetailService.GetPageList(param, pagination);
            obj.Total = pagination.TotalCount;
            obj.Status = true;
            return obj;
        }

        public async Task<TData<DataDictDetailEntity>> GetEntity(long id)
        {
            TData<DataDictDetailEntity> obj = new TData<DataDictDetailEntity>();
            obj.Result = await dataDictDetailService.GetEntity(id);
            obj.Status = true;
            return obj;
        }

        public async Task<TData<int>> GetMaxSort()
        {
            TData<int> obj = new TData<int>();
            obj.Result = await dataDictDetailService.GetMaxSort();
            obj.Status = true;
            return obj;
        }
        #endregion

        #region 提交数据
        public async Task<TData<string>> SaveForm(DataDictDetailEntity entity)
        {

            TData<string> obj = new TData<string>();
          
            await dataDictDetailService.SaveForm(entity);
            dataDictDetailCache.Remove();
            obj.Result = entity.Id.ParseToString();
            obj.Status = true;
            return obj;
        }

        public async Task<TData> DeleteForm(string ids)
        {
            TData<long> obj = new TData<long>();
            await dataDictDetailService.DeleteForm(ids);
            dataDictDetailCache.Remove();
            obj.Status = true;
            return obj;
        }
        #endregion
    }
}
