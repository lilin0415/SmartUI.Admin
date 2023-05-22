using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using YiSha.Model.WebApis;
using YiSha.Service.SignalRManager.ClientCommands;
using YiSha.Service.SignalRManager.Interfaces;

namespace YiSha.Service.SignalRManager
{
    public class ClientHub : BaseHub, IClientMethods
    {
        public ClientHub()
        {
            this.InitOverride();
        }
        protected void InitOverride()
        {
            var asm = Assembly.GetAssembly(this.GetType());
            commands.ResolveCommands<IClientCommand>(asm);
        }

        public async Task OnNewJobCreated( OnNewJobCreatedBody body)
        {
            await this.SendMessageToTenant( body);
        }
    }
}
