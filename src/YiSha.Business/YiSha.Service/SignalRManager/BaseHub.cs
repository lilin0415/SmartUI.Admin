using Koo.SocketCommands;
using Koo.Utilities.FileFormaKoo.APMLHelper;
using Koo.Utilities.Helpers;
using Koo.Utilities.Messaging;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using YiSha.Service.SignalRManager.ClientCommands;

namespace YiSha.Service.SignalRManager
{

    public abstract class BaseHub:Hub// where T : class
    {
        private OnlineUserCollection onlineUsers = new OnlineUserCollection();

        #region connection/disconnection
        public override Task OnConnectedAsync()
        {
            var id = this.Context.ConnectionId;//客户标识
            
            var userToken = this.Context.GetHttpContext().Request.Query["UserToken"].ToString();
            var appToken = this.Context.GetHttpContext().Request.Query["AppToken"].ToString();
            //var tenantId = this.Context.GetHttpContext().Request.Query["TenantId"].ToString();


            this.onlineUsers.Remove(id);

            var newUser = OnlineUser.Create(id, userToken, appToken);

            this.onlineUsers.Add(newUser);
            Debug.WriteLine("新用户已上线:" + newUser.ToString());
            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            var id = this.Context.ConnectionId;//客户标识

            var onlineUser = this.onlineUsers.Get(id);
            if (onlineUser!=null)
            {
                Debug.WriteLine("用户已下线:" + onlineUser.ToString());
            }

            this.onlineUsers.Remove(id);

            return base.OnDisconnectedAsync(exception);
        }
        #endregion

        #region get clients
      
        public List<IClientProxy> GetClients(Func<OnlineUser, bool> predicate)
        {
            var ret = new List<IClientProxy>();

            var users = this.onlineUsers.Where(predicate);
            foreach (var u in users)
            {
                ret.Add(this.Clients.Client(u.ConnectionId));
            }
            return ret;
        }
        #endregion

        #region send message
        public async Task SendMessage(IEnumerable<OnlineUser>users, object data, string action)
        {
            var msg = Message.Create(action, data);

            var json = msg.ToJson();

            var tos = string.Join("\r\n", users.Select(x => x.ToString()));
            Debug.WriteLine($"{DateTimeHelper.NowAsLongStringWithFFF} send message to:" + tos + ", data:" + json);

            foreach (var u in users)
            {
                await this.Clients.Client(u.ConnectionId).SendAsync("OnReceivedMessage", json);
            }
            
        }
        public async Task SendMessageToTenant(object data, [CallerMemberName]string action=null)
        {
            IEnumerable<OnlineUser> users = this.onlineUsers.GetAll();

            await SendMessage(users, data, action);
        }
        #endregion


        protected SocketCommandDictionary commands
        {
            get; private set;
        } = new SocketCommandDictionary();


      

        public async Task OnReceivedMessage(string json)
        {
            Debug.WriteLine($"{DateTimeHelper.NowAsLongStringWithFFF} ReceivedMessage , data:" + json);
            ProcessReceivedMessage(json);
        }

        private void ProcessReceivedMessage(string json)
        {
            var obj = Message.FromJson(json);

            if (commands.ExecuteMessage(obj, out string error))
            {
            }
            else
            {

            }
        }
    }
}
