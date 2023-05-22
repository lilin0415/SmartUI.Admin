using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Koo.Utilities.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using YiSha.Enum;
using YiSha.Service.ProjectManager;
using YiSha.Util;
using YiSha.Util.Extension;
using YiSha.Util.Model;

namespace YiSha.Admin.WebApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class FileController : ControllerBase
    {
        #region 上传单个文件
        [HttpPost]
        public async Task<TData<string>> UploadFile(int fileModule, IFormCollection fileList)
        {
            TData<string> obj = await Util.FileHelper.UploadFile((UploadFileType)fileModule, fileList.Files);
            return obj;
        }
        #endregion

        #region 删除单个文件
        [HttpPost]
        public TData<string> DeleteFile(int fileModule, string filePath)
        {
            TData<string> obj = Util.FileHelper.DeleteFile(fileModule, filePath);
            return obj;
        }
        #endregion

        #region 下载文件
        [HttpGet]
        public FileContentResult DownloadFile(string filePath, int delete = 1)
        {
            TData<FileContentResult> obj = Util.FileHelper.DownloadFile(filePath, delete);
            if (obj.Status)
            {
                return obj.Result;
            }
            else
            {
                throw new Exception("下载失败：" + obj.Message);
            }
        }
        #endregion

        #region 下载项目
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
                throw new Exception("找不到相应的用例模板文件");
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
        #endregion
    }
}