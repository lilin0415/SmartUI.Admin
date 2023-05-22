using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System;
using System.Threading.Tasks;
using YiSha.Business.ProjectManager;
using YiSha.Entity.ProjectManager;
using YiSha.Enum;
using YiSha.Util;
using YiSha.Util.Model;
using RouteAttribute = Microsoft.AspNetCore.Mvc.RouteAttribute;
using YiSha.Util.Extension;
using Koo.Utilities.Helpers;
using FileHelper = Koo.Utilities.Helpers.FileHelper;
using NPOI.POIFS.FileSystem;
using YiSha.Service.ProjectManager;

namespace YiSha.Admin.Web.WebApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class PublishController:BaseApiController
    {
        private PublishedProjectBLL publishedProjectBLL = new PublishedProjectBLL();

        [HttpPost]
        public async Task<TData<string>> GetDownloadUrl(string guid,string version)
        {
            var service = new PublishedProjectService();
            var entity = await service.GetEntityByGuidAndVersion(guid, version);
            return TData.CreateSuccessdValue(entity.FilePath);
        }

        /// <summary>
        /// 上传项目
        /// </summary>
        /// <param name="fileModule"></param>
        /// <param name="fileCollection"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<TData> UploadProject(IFormCollection formData)
        {
            var model = new PublishedProjectEntity();
            model.ProductId = DataConverter.ToLong(formData["ProductId"].ToString(), 0);
            model.CateId = DataConverter.ToLong(formData["CateId"].ToString(), 0);
            model.ProjectType = 0;
            model.ProjectGuid = formData["ProjectGuid"].ToString();
            model.Name = formData["Name"].ToString();
            model.Remark = formData["Remark"].ToString();
            model.Version = formData["Version"].ToString();
            model.ReleaseNote = formData["ReleaseNote"].ToString();
            model.ReleaseDate = DataConverter.ToDateTime(formData["ReleaseDate"].ToString());
            model.OriginalVarJson = formData["OriginalVarJson"].ToString();
            model.VarJson = formData["VarJson"].ToString();

            model.AlignedVersion = formData["AlignedVersion"].ToString();
            model.FilePath = formData["FilePath"].ToString();
            model.MD5 = formData["MD5"].ToString();
            if (formData.ContainsKey("AssertionCount"))
            {
                model.AssertionCount = DataConverter.ToInt(formData["AssertionCount"]);
            }
            if (formData.ContainsKey("SupportParallel"))
            {
                model.SupportParallel = DataConverter.ToBool(formData["SupportParallel"]) ? (byte)1 : (byte)0;
            }

            var service = new PublishedProjectService();
            var verifyResult = await service.Verify(model);
            if (!string.IsNullOrEmpty(verifyResult))
            {
                return TData.CreateFailedMsg($"项目[{model.Name}]上传失败:{verifyResult}");
            }

            var files = formData.Files;
            if (files == null || files.Count != 1)
            {
                return TData.CreateFailedMsg("请先选择文件并且只能选择一个文件");
            }
         
            IFormFile file = files[0];
            
            if (Path.GetExtension(file.FileName)!=".zip")
            {
                return TData.CreateFailedMsg("文件格式错误");

            }

            string dir = Path.Combine("Resource", UploadFileType.Project.ToString(), DateTime.Now.ToString("yyyyMM"), model.ProjectGuid);
            string absoluteDir = Path.Combine(GlobalContext.HostingEnvironment.ContentRootPath, dir);
            FileHelper.EnsureDirectory(absoluteDir);

            string newFileName = Path.GetFileName(file.FileName);

            var absoluteFileName = Path.Combine(absoluteDir, newFileName);
            FileHelper.SafeDeleteFile(absoluteFileName);

            try
            {
                using (FileStream fs = System.IO.File.Create(absoluteFileName))
                {
                    await file.CopyToAsync(fs);
                    fs.Flush();
                }

                var md5 =MD5Helper.ComputeFile(absoluteFileName);
                if (model.MD5 != md5)
                {
                    FileHelper.SafeDeleteFile(absoluteFileName);
                    return TData.CreateFailedMsg($"上传文件(版本:{model.Version})校验失败，请重新上传");
                }

                var uploadedPath = Path.AltDirectorySeparatorChar + ConvertDirectoryToHttp(dir)
                    + Path.AltDirectorySeparatorChar + newFileName;

                model.FilePath = uploadedPath;

                TData<string> obj = await publishedProjectBLL.SaveForm(model);
                return obj;

            }
            catch (Exception ex)
            {
                FileHelper.SafeDeleteFile(absoluteFileName);
                return TData.CreateFailedMsg(ex);
            }
        }

        public static string ConvertDirectoryToHttp(string directory)
        {
            directory = directory.ParseToString();
            directory = directory.Replace(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
            return directory;
        }

     /// <summary>
     /// 下载项目
     /// </summary>
     /// <param name="guid"></param>
     /// <param name="version"></param>
     /// <returns></returns>
     /// <exception cref="Exception"></exception>
        [HttpGet]
        public FileContentResult DownloadProject(string guid, string version)
        {
            var service = new PublishedProjectService();
            var entityTask = service.GetEntityByGuidAndVersion(guid, version);
            entityTask.Wait();
            var entity = entityTask.Result;
            var filePath = entity.FilePath;

            filePath = FilterFilePath(filePath);
            if (!filePath.StartsWith("wwwroot") && !filePath.StartsWith("Resource"))
            {
                throw new Exception("非法访问");
            }

            TData<FileContentResult> obj = new TData<FileContentResult>();
            string absoluteFilePath = GlobalContext.HostingEnvironment.ContentRootPath + Path.DirectorySeparatorChar + filePath.Replace(Path.AltDirectorySeparatorChar, Path.DirectorySeparatorChar);

            if (!System.IO.File.Exists(absoluteFilePath))
            {
                throw new Exception($"找不到相应的{GlobalContext.SystemConfig.CaseName}模板文件");
            }

            var md5 = MD5Helper.ComputeFile(absoluteFilePath);
            if (md5 != entity.MD5)
            {
                throw new Exception("文件校验失败");
            }

            byte[] fileBytes = System.IO.File.ReadAllBytes(absoluteFilePath);
         
            string fileName = Path.GetFileName(filePath);
         
            var result = new FileContentResult(fileBytes, "application/octet-stream")
            {
                FileDownloadName = fileName
            };

            return result;
        }

        public static string FilterFilePath(string filePath)
        {
            filePath = filePath.Replace("../", string.Empty);
            filePath = filePath.Replace("..", string.Empty);
            filePath = filePath.TrimStart('/');
            return filePath;
        }
    }
}
