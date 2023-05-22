using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System.Web;
using Microsoft.AspNetCore.Mvc;
using YiSha.Util.Model;
using YiSha.Entity;
using YiSha.Model;
using YiSha.Entity.ProductCategoryManager;
using YiSha.Business.ProductCategoryManager;
using YiSha.Model.Param.ProductCategoryManager;
using YiSha.Business.OrganizationManage;
using YiSha.Model.Param.OrganizationManage;
using YiSha.Model.Result;
using YiSha.Admin.Web.WebApi.Controllers;
using YiSha.Service.TestTaskManager;
using YiSha.Entity.TestTaskManager;
using YiSha.Model.TestTaskManager;
using YiSha.Service.TestCaseManager;
using YiSha.Model.Publishes;
using YiSha.Model.WebApis;
using Koo.Utilities.Helpers;
using Microsoft.AspNetCore.Http;
using System.IO;
using YiSha.Business.ProjectManager;
using YiSha.Entity.ProjectManager;
using YiSha.Enum;
using YiSha.Service.ProjectManager;
using YiSha.Util;
using FileHelper = Koo.Utilities.Helpers.FileHelper;
using YiSha.Util.Extension;
using Koo.Utilities.Exceptions;

namespace YiSha.Admin.WebApi.Controllers
{
    /// <summary>
    /// 创 建：admin
    /// 日 期：2022-09-24 14:57
    /// 描 述：控制器类
    /// </summary>
 
    public class CaseExecController : AuthorizedBaseApiController
    {
        private ModuleCategoryBLL moduleCategoryBLL = new ModuleCategoryBLL();



        /// <summary>
        /// 获取测试用例配置的变量
        /// </summary>
        /// <param name="testCaseId"></param>
        /// <returns></returns>
        [HttpGet]
  
        public async Task<TData<PublishedInfo>> GetPublishedVarByTestCaseId([FromQuery] long testCaseId)
        {
            var varJsonService = new VarJsonService();
            var obj = await varJsonService.GetTestCaseVarJsonByTestCaseId(testCaseId, false);

            return obj;

        }

        #region 拉取待执行的用例
        /// <summary>
        /// 拉取待消费的作业
        /// </summary>

        /// <param name="size"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<TData<List<CaseExecRecordModel>>> PullCaseList(int size)
        {
            var service = new CaseExecRecordService();
            try
            {
                var ret = await service.PullCaseList(this.CurrentUser, this.AppToken, this.AppVersion, size);

                return TData.CreateSuccessdValue(ret);
            }
            catch (Exception ex)
            {
                return TData.CreateFailedValue<List<CaseExecRecordModel>>(null, ex.Message);
            }
        }

        /// <summary>
        /// 仅拉取当前客户端自己的
        /// </summary>
        /// <param name="size"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<TData<List<CaseExecRecordModel>>> PullMyCaseList(int size)
        {
            var service = new CaseExecRecordService();
            try
            {
                var ret = await service.PullCaseList(this.CurrentUser, this.AppToken, this.AppVersion, size);

                return TData.CreateSuccessdValue(ret);
            }
            catch (Exception ex)
            {
                return TData.CreateFailedValue<List<CaseExecRecordModel>>(null, ex.Message);
            }
        }
        #endregion

        #region 更改用例 执行状态，结束状态

        /// <summary>
        /// 更改用例状态
        /// </summary>
        /// <param name="caseStatus"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<TData<bool>> UpdateCaseStatus(UpdateCaseExecStatusBody caseStatus)
        {
            var service = new CaseExecRecordService();
            var ret = await service.UpdateCaseExecStatus(caseStatus);

            return TData.CreateSuccessdValue(ret);
        }
        #endregion

        #region 更改用例 执行状态，结束状态

        /// <summary>
        /// 更改用例状态
        /// </summary>
        /// <param name="caseStatus"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<TData<bool>> UploadResult(UpdateCaseExecStatusBody caseStatus)
        {
            var service = new CaseExecRecordService();
            var ret = await service.UpdateCaseExecStatus(caseStatus);

            return TData.CreateSuccessdValue(ret);
        }
        #endregion

        #region 上传日志



        /// <summary>
        /// 上传日志
        /// </summary>
        /// <param name="fileModule"></param>
        /// <param name="fileCollection"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<TData> UploadLogs(IFormCollection formData)
        {
            //var caseExecId = DataConverter.ToLong(formData["CaseExecId"].ToString(), 0);
            var caseExecGuid = formData["CaseExecGuid"].ToString();
            var md5 = formData["MD5"].ToString();

            var files = formData.Files;
            if (files == null || files.Count != 1)
            {
                return TData.CreateFailedMsg("请先选择文件并且只能选择一个文件");
            }

            IFormFile file = files[0];

            if (Path.GetExtension(file.FileName) != ".zip")
            {
                return TData.CreateFailedMsg("文件格式错误");

            }

            //结果按照输出到日期目录
            //Resource\CaseExecJob\202303\20230325212559
            string dir = Path.Combine("Resource", UploadFileType.CaseExecJob.ToString(), DateTime.Now.ToString("yyyyMMdd"), caseExecGuid);
            string absoluteDir = Path.Combine(GlobalContext.HostingEnvironment.ContentRootPath, dir);

            //删除作业id目录
            //Resource\CaseExecJob\202303\20230325212559
            //FileHelper.SafeDeleteDirectory(absoluteDir);
            Koo.Utilities.Helpers.FileHelper.EnsureDirectory(absoluteDir);

            string newFileName = Path.GetFileName(file.FileName);

            //日志会有多个文件，分批上传
            //Resource\CaseExecJob\202303\20230325212559\xxxx1.zip
            //Resource\CaseExecJob\202303\20230325212559\xxxx2.zip
            var absoluteFileName = Path.Combine(absoluteDir, newFileName);
         
            try
            {
                FileHelper.SafeDeleteFile(absoluteFileName);

                using (FileStream fs = System.IO.File.Create(absoluteFileName))
                {
                    await file.CopyToAsync(fs);
                    fs.Flush();
                }

                if (!string.IsNullOrWhiteSpace(md5))
                {
                    var localmd5 = MD5Helper.ComputeFile(absoluteFileName);
                    if (localmd5 != md5)
                    {
                        FileHelper.SafeDeleteFile(absoluteFileName);
                        return TData.CreateFailedMsg($"作业({caseExecGuid})日志md5校验失败，请重新上传");
                    }
                }

                //解压到临时目录中
                //压缩文件 Resource\CaseExecJob\202303\20230325212559\xxxx2.zip
                //解压缩目录Resource\CaseExecJob\202303\20230325212559\xxxx2\内容

                //absoluteDir:  Resource\\CaseExecJob\\202304\\391a2ad2f4d440dbb3111eea501e56c0
                //unzipDir:     Resource\\CaseExecJob\\202304\\391a2ad2f4d440dbb3111eea501e56c0\\tmp
                var unzipDir = FileHelper.Combine(absoluteDir, "tmp");
                ZipHelper.UnZip(absoluteFileName, unzipDir);

                //outputDir:     Resource\\CaseExecJob\\202304\\391a2ad2f4d440dbb3111eea501e56c0\\output
                var outputDir = Path.Combine(absoluteDir, "output");
                FileHelper.EnsureDirectory(outputDir);

                //移动Screenshots到output目录中
                var sourceScreenshotsDir = FileHelper.Combine(unzipDir, "Screenshots");
                if (FileHelper.ExistsDirectory(sourceScreenshotsDir))
                {
                    var outputScreenshotsDir = FileHelper.Combine(outputDir, "Screenshots");
                    Directory.Move(sourceScreenshotsDir, outputScreenshotsDir);
                }
                

                //移动debug.log
                var sourceDebugFile = FileHelper.Combine(unzipDir, "debug.log");
                if(FileHelper.Exists(sourceDebugFile))
                {
                    System.IO.File.Move(sourceDebugFile, FileHelper.Combine(outputDir, "debug.log"));
                }

                //移动result.log
                var sourceResultDebugFile = FileHelper.Combine(unzipDir, "result.log");
                if (FileHelper.Exists(sourceResultDebugFile))
                {
                    System.IO.File.Move(sourceResultDebugFile, FileHelper.Combine(outputDir, "result.log"));
                }


                //读取日志
                var execlogFile = FileHelper.Combine(unzipDir, "exec.log");
                if (FileHelper.Exists(execlogFile))
                {
                    System.IO.File.Move(execlogFile, FileHelper.Combine(outputDir, "exec.log"));
                    execlogFile = FileHelper.Combine(outputDir, "exec.log");
                }
                else
                {
                    throw new BizException("未找到日志文件:exec.log");
                }
              
                var logItems = FileHelper.ReadFromJson<List<CaseExecLogEntity>>(execlogFile);

                var caseExecId = await new CaseExecRecordService().GetIdByGuid(caseExecGuid);
                if (caseExecId == null)
                {
                    throw new BizException("未找到作业信息文件,"+ caseExecGuid);
                }

                //由于是分批上传日志，所以需要有结束的日志更新到开始
                var service = new CaseExecLogService();
                foreach (var item in logItems)
                {
                    item.CaseExecId = caseExecId;
                    if (!string.IsNullOrEmpty(item.BeforeScreenshot))
                    {
                        var path = Path.Combine(dir, "output", item.BeforeScreenshot);
                        item.BeforeScreenshot = Path.AltDirectorySeparatorChar + ConvertDirectoryToHttp(path);
                    }

                    if (!string.IsNullOrEmpty(item.AfterScreenshot))
                    {
                        var path = Path.Combine(dir, "output", item.AfterScreenshot);
                        item.AfterScreenshot = Path.AltDirectorySeparatorChar + ConvertDirectoryToHttp(path);
                    }

                    if (item.TransStep == (byte)TransStep.End)
                    {
                        var entityInDb = await service.GetStartEntity(caseExecId.Value, item.ExecutionPathMd5);
                        entityInDb.EndTime = item.EndTime;
                        entityInDb.Status = item.Status;
                        entityInDb.Reason = item.Reason;
                        entityInDb.OutputParameters = item.OutputParameters;
                        entityInDb.AfterScreenshot = item.AfterScreenshot;

                        await service.SaveForm(entityInDb);
                    }
                    else
                    {
                        await service.SaveForm(item);
                    }
                }

                TData<string> obj = new TData<string>();
                obj.Status = true;
                return obj;

            }
            catch (Exception ex)
            {
                FileHelper.SafeDeleteDirectory(absoluteDir);

                return TData.CreateFailedMsg(ex);
            }
        }

        public static string ConvertDirectoryToHttp(string directory)
        {
            directory = directory.ParseToString();
            directory = directory.Replace(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
            return directory;
        }
        #endregion
    }
}
