using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace YiSha.Util
{
    public class AsyncTaskHelper
    {
        /// <summary>
        /// 开始异步任务
        /// </summary>
        /// <param name="action"></param>
        public static void StartTask<T>(T obj, Action<T> action)
        {
            try
            {
                Task task = new Task((x) => {
                    try
                    {
                        action((T)x);
                    }
                    catch (Exception ex)
                    {
                        LogHelper.Error(ex);
                        throw;
                    }
                },obj);
                task.Start();
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
        }
    }
}
