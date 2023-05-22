using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using YiSha.Enum;
using YiSha.Util;
using YiSha.Util.Extension;
using YiSha.Util.Model;

namespace YiSha.Admin.Web.Controllers
{
    public class FileController : BaseController
    {
        #region 上传单个文件
        [HttpPost]
        [AuthorizeFilter]
        public async Task<TData<string>> UploadFile(int fileModule, IFormCollection fileList)
        {
            TData<string> obj = await BizFileHelper.UploadFile((UploadFileType)fileModule, fileList.Files);
            return obj;
        }
        #endregion

        #region 删除单个文件
        [HttpPost]
        [AuthorizeFilter]
        public TData<string> DeleteFile(int fileModule, string filePath)
        {
            TData<string> obj = BizFileHelper.DeleteFile(fileModule, filePath);
            return obj;
        }
        #endregion

        #region 下载文件
        [HttpGet]
        public FileContentResult DownloadFile(string filePath, int delete = 1)
        {
            TData<FileContentResult> obj = BizFileHelper.DownloadFile(filePath, delete);
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
    }
}