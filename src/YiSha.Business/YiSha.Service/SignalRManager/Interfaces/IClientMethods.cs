using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using YiSha.Model.WebApis;

namespace YiSha.Service.SignalRManager.Interfaces
{
    public interface IClientMethods: IBaseMethods
    {
        Task OnNewJobCreated( OnNewJobCreatedBody body);
    }
}
