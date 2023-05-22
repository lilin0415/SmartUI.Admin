using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using YiSha.Util;
using YiSha.Util.Extension;
using YiSha.Util.Model;
using YiSha.Entity.TestCaseManager;
using YiSha.Model.Param.TestCaseManager;
using YiSha.Service.TestCaseManager;
using YiSha.Service.ProjectManager;
using YiSha.Model.Publishes;
using YiSha.Entity.ProjectManager;
using YiSha.Model.Result;
using YiSha.Service.ProductCategoryManager;
using Koo.Utilities.Helpers;
using Pipelines.Sockets.Unofficial.Arenas;

namespace YiSha.Service.TestCaseManager
{
    /// <summary>
    /// 创 建：admin
    /// 日 期：2022-10-01 20:14
    /// 描 述：业务类
    /// </summary>
    public class VarJsonService
    {

        #region 用例模板的变量

        /// <summary>
        /// 获取变量的文件树
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<List<ZtreeInfo>> GetPublishedDocumentTree(long projectId)
        {
            List<ZtreeInfo> list = new List<ZtreeInfo>();

            var data = await GetProjectVarJson(projectId);

            foreach (var item in data.Result.Documents)
            {
                var firstProject = item;
                list.Add(new ZtreeInfo
                {
                    id = firstProject.Id,
                    pId = firstProject.ParentId,
                    name = firstProject.Name,
                    nodeType = ZtreeInfoNodeType.publishedDocument.ToString(),
                    tag = firstProject,
                    Obj = item,
                });

            }
           
            return list;

        }
        /// <summary>
        /// 获取发用例模板（发布项目）的默认全局变量配置
        /// 对于加密的变量，
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>
        public async Task<TData<PublishedInfo>> GetProjectVarJson(long projectId)
        {
            var service = new PublishedProjectService();
            var entity = await service.GetEntity(projectId);

            var varJson = entity.VarJson;

            PublishedInfoHelper.ParseJson(varJson, out PublishedInfoHeader header, out PublishedInfo publishedInfo);

            publishedInfo.SetParentId();

            return TData.CreateSuccessdValue(publishedInfo);

        }

        /// <summary>
        /// 保存用例模板（发布项目）的默认全局变量
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="items"></param>
        /// <returns></returns>
        public async Task<TData<string>> SaveProjectVarJson(long projectId, List<SavingVarItem> items)
        {
            TData<string> obj = new TData<string>();

            var service = new PublishedProjectService();
            var entityInDb = await service.GetEntity(projectId);

            PublishedInfoHelper.ParseJson(entityInDb.VarJson, out PublishedInfoHeader header, out PublishedInfo infoInDb);

            //根据文档id来分类，不同的文档，里面的变量名称可能相同
            foreach (var doc in items.GroupBy(x => x.DocId))
            {
                var docId = doc.Key;
                var docObjInDb = infoInDb.Documents.FirstOrDefault(x => x.Id == docId);
                if (docObjInDb != null)
                {
                    foreach (var varItem in doc)
                    {
                        var existedVarItemInDb = docObjInDb.Vars.GetByVarName(varItem.VarName);
                        if (existedVarItemInDb != null)
                        {
                            existedVarItemInDb.ValueSource = varItem.ValueSource;

                            //前台不能显示明文密码
                            //所以对于密码变量前台显示的Value为加密之后的值
                            //在保存的时候，判断和数据库中值是否一样，如果一样，则说明没有修改，
                            //如果不一样则说明有修改，需要对传递的Value进行重新加密
                            if (existedVarItemInDb.DataType == VariableDataType.Password)
                            {
                                if (existedVarItemInDb.Value == varItem.Value)
                                {
                                }
                                else
                                {
                                    existedVarItemInDb.Value = await EncryptionService.Instance.EncryptVarPassword(varItem.Value);
                                }
                                
                            }
                            else
                            {
                                existedVarItemInDb.Value = varItem.Value;
                            }
                        }
                    }
                }
            }

            entityInDb.VarJson = PublishedInfoHelper.ToJson(infoInDb);

            await service.SaveForm(entityInDb);

            obj.Result = entityInDb.Id.ParseToString();
            obj.Status = true;
            return obj;
        }

        #endregion


        #region 测试用例变量
        private static void SyncDocumentsFromDefault(ReleasedDocumentCollection defaultDocuments, ReleasedDocumentCollection newDocuments)
        {
            var backup = new ReleasedDocumentCollection(newDocuments.ToList());

            newDocuments.Clear();

            foreach (var defaultDoc in defaultDocuments)
            {
                var existedItem = backup.GetById(defaultDoc.Id);
                //检查当前用例中的文档是否有效
                //在升级之后，新版本的模板（defaultPublishedInfo）里面可能已经没有这个document了
                if (existedItem == null)
                {
                    newDocuments.Add(defaultDoc);
                }
                else
                {
                    newDocuments.Add(existedItem);
                }
            }
        }
        private static void SyncVarsFromDefault(ReleasedVarCollection defaultDocuments, ReleasedVarCollection newDocuments)
        {
            var backup = new ReleasedVarCollection(newDocuments.ToList());

            newDocuments.Clear();

            foreach (var caseVarItem in defaultDocuments)
            {
                var existedItem = backup.GetByVarName(caseVarItem.VarName);
                //检查当前用例中的文档是否有效
                //在升级之后，新版本的模板（defaultPublishedInfo）里面可能已经没有这个document了
                if (existedItem == null)
                {
                    newDocuments.Add(caseVarItem);
                }
                else
                {
                    newDocuments.Add(existedItem);
                }
            }
        }

        class GetCasePublishedInfoAndSyncFromDefaultResult
        {
            public TestCaseEntity testCaseEntity = null;
            public  PublishedInfo defaultPublishedInfo = null;
            public PublishedInfo publishedInfo = null;
            public bool isUsingDefaultVars = false;
        }

        private async Task<GetCasePublishedInfoAndSyncFromDefaultResult> GetCasePublishedInfoAndSyncFromDefault(long testCaseId)
        {
            GetCasePublishedInfoAndSyncFromDefaultResult result = new GetCasePublishedInfoAndSyncFromDefaultResult();

            TestCaseEntity testCaseEntity = null;

            PublishedInfo defaultPublishedInfo = null;
            PublishedInfo publishedInfo = null;
            bool isUsingDefaultVars = false;

            //1、获取测试用例中的变量配置
            testCaseEntity = await (new TestCaseService()).GetEntity(testCaseId);

            //2、获取所使用的项目模板的默认变量值
            var service = new PublishedProjectService();
            var defaultEntity = await service.GetEntityByGuidAndVersion(testCaseEntity.ProjectGuid, testCaseEntity.SpecialVersion);

            PublishedInfoHelper.ParseJson(defaultEntity.VarJson, out PublishedInfoHeader _, out defaultPublishedInfo);

            //第一次的时候为空，使用项目模板的数据
            if (string.IsNullOrWhiteSpace(testCaseEntity.VarJson))
            {
                //如果当前测试用例中配置的变量为空，使用项目模板中配置的变量
                isUsingDefaultVars = true;
                publishedInfo = defaultPublishedInfo;
            }
            else
            {
                PublishedInfoHelper.ParseJson(testCaseEntity.VarJson, out PublishedInfoHeader header, out publishedInfo);
            }

            if (publishedInfo == null)
            {
                return null;
            }

            if (!isUsingDefaultVars)
            {
                //同步文件，
                //检查当前用例中的文档是否有效
                //在升级之后，新版本的模板（defaultPublishedInfo）里面可能已经没有这个document了
                SyncDocumentsFromDefault(defaultPublishedInfo.Documents, publishedInfo.Documents);

                for (var i = 0; i < publishedInfo.Documents.Count; i++)
                {
                    var caseDoc = publishedInfo.Documents[i];

                    var defaultDoc = defaultPublishedInfo.Documents.GetById(caseDoc.Id);

                    //从用例模块中同步变量到用例中
                    //检查当前变量是否有效
                    //在升级之后，新版本的Var中可能已经没有这个变量了
                    SyncVarsFromDefault(defaultDoc.Vars, caseDoc.Vars);
                }
            }

            result.testCaseEntity = testCaseEntity;
            result.defaultPublishedInfo = defaultPublishedInfo;
            result.publishedInfo = publishedInfo;
            result.isUsingDefaultVars = isUsingDefaultVars;

            return result;
    }

        /// <summary>
        /// 获取变量的文件树
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<List<ZtreeInfo>> GetPublishedDcoumentTreeByTestCaseId(long testCaseId)
        {
            List<ZtreeInfo> list = new List<ZtreeInfo>();

            var result = await GetCasePublishedInfoAndSyncFromDefault(testCaseId);

            PublishedInfo publishedInfo = result.publishedInfo;

            publishedInfo.SetParentId();

            foreach (var item in publishedInfo.Documents)
            {
                var firstProject = item;
                list.Add(new ZtreeInfo
                {
                    id = firstProject.Id,
                    pId = firstProject.ParentId,
                    name = firstProject.Name,
                    nodeType = ZtreeInfoNodeType.publishedDocument.ToString(),
                    tag = firstProject,
                    Obj = item,
                });

            }

            return list;

        }

    
        /// <summary>
        /// 获取测试用例的变量配置
        /// </summary>
        /// <param name="testCaseId"></param>
        /// <param name="isFinalResult">用于在界面上面编辑，还是生成最终的用例执行记录</param>
        /// <returns></returns>
        public async Task<TData<PublishedInfo>> GetTestCaseVarJsonByTestCaseId(long testCaseId,bool isFinalResult)
        {
            var result = await GetCasePublishedInfoAndSyncFromDefault(testCaseId);

            TestCaseEntity testCaseEntity=result.testCaseEntity;
            PublishedInfo defaultPublishedInfo = result.defaultPublishedInfo;
            PublishedInfo publishedInfo =result.publishedInfo;
            bool isUsingDefaultVars = result.isUsingDefaultVars;

            publishedInfo.SetParentId();

            for (var i = 0; i < publishedInfo.Documents.Count; i++)
            {
                var caseDoc= publishedInfo.Documents[i];

                var defaultDoc = defaultPublishedInfo.Documents.GetById(caseDoc.Id);

                for (var varIndex = caseDoc.Vars.Count - 1; varIndex >= 0; varIndex--)
                {
                    var caseVarItem = caseDoc.Vars[varIndex];

                    //设置当前用例变量中的默认值（项目模板中的值）
                    var defaultVarItem = defaultDoc.Vars.GetByVarName(caseVarItem.VarName);

                    //设置默认值
                    caseVarItem.DefaultValue = defaultVarItem.Value;

                    //如果当前用例没有设置变量，全部使用的是模板中变量
                    //所以全部为继承值类型
                    if (isUsingDefaultVars)
                    {
                        //当前值为空，值来源为继承自模板
                        caseVarItem.Value = String.Empty;
                        //如果为第一次，变量默认都使用 默认值
                        caseVarItem.ValueSource = VarValueSource.Inherit;
                    }

                    //如果是生成执行用例记录，得给Value赋值，最终使用的时候都会的这个Value值
                    if (isFinalResult)
                    {
                        //如果是最终运行时的变量配置

                        //如果为继承，则使用继承的value
                        if (caseVarItem.ValueSource == VarValueSource.Inherit)
                        {
                            caseVarItem.Value = caseVarItem.DefaultValue;
                        }
                        else
                        {
                            //caseVarItem.Value = caseVarItem.Value;
                        }
                        //清空默认值，给客户端取值时减少json大小
                        caseVarItem.DefaultValue = String.Empty;
                    }
                    else
                    {  

                        //用于前端页面显示
                        //如果当前变量为密码变量，则在前端页面password控件显示的值为加密之后的字符串
                        //在保存的时候会根据前端传递的密码字符串和后端存储的是否一样，
                        //一样则说明密码没有更改，不一样说明密码更改了，需要重新对传入的字符串加密
                        if (caseVarItem.DataType == VariableDataType.Password)
                        {
                            //如果有默认值，则应该显示为*
                            if (!string.IsNullOrEmpty(caseVarItem.DefaultValue))
                            {
                                caseVarItem.DefaultValue = "******";
                            }

                            if (caseVarItem.ValueSource == VarValueSource.Inherit)
                            {  
                                //如果使用的本地值，则在前台应该绑定
                                //caseVarItem.DecryptedValue =string.Empty;
                            }
                            else
                            {
                                //如果使用的本地值，则在前台应该绑定
                                //caseVarItem.DecryptedValue = EncryptionService.Instance.DecryptVarPassword(caseVarItem.Value);
                            }
                        }
                    }
                }
            }
            return TData.CreateSuccessdValue(publishedInfo);

        }
        public async Task<TData<string>> SaveTestCaseVarJsonByTestCaseId(long testCaseId, List<SavingVarItem> items)
        {
            TData<string> obj = new TData<string>();

            var result = await GetCasePublishedInfoAndSyncFromDefault(testCaseId);

            TestCaseEntity testCaseEntity = result.testCaseEntity;
            PublishedInfo defaultPublishedInfo = result.defaultPublishedInfo;
            PublishedInfo infoToSave = result.publishedInfo;
            bool isUsingDefaultVars = result.isUsingDefaultVars;

            infoToSave.SetParentId();

            foreach (var doc in items.GroupBy(x => x.DocId))
            {
                var docId = doc.Key;
                var docObjToSave = infoToSave.Documents.FirstOrDefault(x => x.Id == docId);
                if (docObjToSave == null)
                {
                    continue;
                }

                foreach (var varItem in doc)
                {
                    var existedVarItemInDb = docObjToSave.Vars.GetByVarName(varItem.VarName);
                    if (existedVarItemInDb != null)
                    {
                        existedVarItemInDb.ValueSource = varItem.ValueSource;
                        //不管是使用的默认值还是本地值，都保存用户录入的Value

                        //如果是密码变量，对从前台传递地数据进行加密
                        //前台不能显示明文密码
                        //所以对于密码变量前台显示的Value为加密之后的值
                        //在保存的时候，判断和数据库中值是否一样，如果一样，则说明没有修改，
                        //如果不一样则说明有修改，需要对传递的Value进行重新加密
                        if (existedVarItemInDb.DataType == VariableDataType.Password)
                        {
                            if (existedVarItemInDb.Value == varItem.Value)
                            {
                            }
                            else
                            {
                                existedVarItemInDb.Value = await EncryptionService.Instance.EncryptVarPassword(varItem.Value);
                            }
                        }

                        else
                        {
                            existedVarItemInDb.Value = varItem.Value;
                        }

                    }
                }

            }

            testCaseEntity.VarJson = PublishedInfoHelper.ToJson(infoToSave);

            var service = new TestCaseService();
            await service.SaveForm(testCaseEntity);

            obj.Result = testCaseEntity.Id.ParseToString();
            obj.Status = true;
            return obj;
        }
        #endregion

    }
}
