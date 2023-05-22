using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using YiSha.Util;
using YiSha.Util.Extension;
using YiSha.Util.Model;
using YiSha.Entity.SystemManage;
using YiSha.Model.Param.SystemManage;
using YiSha.Service.SystemManage;
using YiSha.Model.Result.SystemManage;
using YiSha.Service.Cache;

namespace YiSha.Business.SystemManage
{
    public class DataDictBLL
    {
        private DataDictService dataDictService = new DataDictService();
        private DataDictDetailService dataDictDetailService = new DataDictDetailService();

        private DataDictCache dataDictCache = new DataDictCache();
        private DataDictDetailCache dataDictDetailCache = new DataDictDetailCache();

        #region 获取数据
        public async Task<TData<List<DataDictEntity>>> GetList(DataDictListParam param)
        {
            TData<List<DataDictEntity>> obj = new TData<List<DataDictEntity>>();
            obj.Result = await dataDictService.GetList(param);
            obj.Total = obj.Result.Count;
            obj.Status = true;
            return obj;
        }

        public async Task<TData<List<DataDictEntity>>> GetPageList(DataDictListParam param, Pagination pagination)
        {
            TData<List<DataDictEntity>> obj = new TData<List<DataDictEntity>>();
            obj.Result = await dataDictService.GetPageList(param, pagination);
            obj.Total = pagination.TotalCount;
            obj.Status = true;
            return obj;
        }

        public async Task<TData<DataDictEntity>> GetEntity(long id)
        {
            TData<DataDictEntity> obj = new TData<DataDictEntity>();
            obj.Result = await dataDictService.GetEntity(id);
            obj.Status = true;
            return obj;
        }

        public async Task<TData<int>> GetMaxSort()
        {
            TData<int> obj = new TData<int>();
            obj.Result = await dataDictService.GetMaxSort();
            obj.Status = true;
            return obj;
        }

        /// <summary>
        /// 获取所有的数据字典
        /// </summary>
        /// <returns></returns>
        public async Task<TData<List<DataDictInfo>>> GetDataDictList()
        {
            TData<List<DataDictInfo>> obj = new TData<List<DataDictInfo>>();
            List<DataDictEntity> dataDictList = await dataDictCache.GetList();
            List<DataDictDetailEntity> dataDictDetailList = await dataDictDetailCache.GetList();
            List<DataDictInfo> dataDictInfoList = new List<DataDictInfo>();
            foreach (DataDictEntity dataDict in dataDictList)
            {
                List<DataDictDetailInfo> detailList = dataDictDetailList.Where(p => p.DictType == dataDict.DictType).OrderBy(p => p.DictSort).Select(p => new DataDictDetailInfo
                {
                    DictKey = p.DictKey,
                    DictValue = p.DictValue,
                    ListClass = p.ListClass,
                    IsDefault = p.IsDefault,
                    DictStatus = p.DictStatus,
                    IsSystem = p.IsSystem == 1,
                    Remark = p.Remark
                }).ToList();
                dataDictInfoList.Add(new DataDictInfo
                {
                    DictType = dataDict.DictType,
                    IsSystem = dataDict.IsSystem==1,
                    CanAddItem = dataDict.CanAddItem == 1,
                    Detail = detailList
                });
            }
            obj.Result = dataDictInfoList;
            obj.Status = true;
            return obj;
        }
        #endregion

        #region 提交数据 
        public async Task<TData<string>> SaveForm(DataDictEntity entity)
        {
            TData<string> obj = new TData<string>();
            
            await dataDictService.SaveForm(entity);
            dataDictCache.Remove();
            obj.Result = entity.Id.ParseToString();
            obj.Status = true;
            return obj;
        }

        public async Task<TData> DeleteForm(string ids)
        {
            TData obj = new TData();
          
            await dataDictService.DeleteForm(ids);
            dataDictCache.Remove();
            obj.Status = true;
            return obj;
        }
        #endregion
    }
}
