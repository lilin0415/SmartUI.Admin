using Koo.Utilities.Helpers;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YiSha.Web.Code;

namespace YiSha.Service.SignalRManager
{
    public class OnlineUser
    {
        public string ConnectionId
        {
            get;set;
        }
        public string UserToken
        {
            get;set;
        }
        public string AppToken
        {
            get;set;
        }

        public long UserId
        {
            get;set;
        }
        public int AppType
        {
            get;set;
        }
    

        public DateTime CreateTime
        {
            get;set;
        }
        public DateTime LastActiveTime
        {
            get;set;
        }

        public static OnlineUser Create(string connectionId, string userToken, string appToken)
        {
            var appTokenInfo = AppTokenInfo.FromToken(appToken);
            var newUser = new OnlineUser();
            newUser.ConnectionId = connectionId;
            newUser.UserToken = userToken;
            newUser.AppToken = appToken;

            newUser.UserId = appTokenInfo.UserId.GetValueOrDefault();
            newUser.AppType = appTokenInfo.AppType;

            newUser.CreateTime = DateTimeHelper.Now;
            newUser.LastActiveTime = DateTimeHelper.Now;
            return newUser;
        }

        public override string ToString()
        {
            return $"ConnectionId:{this.ConnectionId}, UserId:{this.UserId}, AppType:{this.AppType}, UserToken:{this.UserToken},AppToken:{this.AppToken} ";
        }
    }

    public class OnlineUserCollection
    {
        private ConcurrentDictionary<string, OnlineUser> list = new ConcurrentDictionary<string, OnlineUser>();
        public void Add(OnlineUser user)
        {
            this.list.TryAdd(user.ConnectionId,user);
        }
        public OnlineUser Get(string connectionId)
        {
            this.list.TryGetValue(connectionId, out OnlineUser user);
            return user;
        }

        public void Remove(OnlineUser user)
        {
            this.list.TryRemove(user.ConnectionId, out _);
        }
        public void Remove(string connectionId)
        {
            this.list.TryRemove(connectionId, out _);
        }

        public List<OnlineUser> Where(Func<OnlineUser, bool> predicate)
        {
            return this.list.Where(x => predicate(x.Value)).Select(x => x.Value).ToList();
        }
        public List<OnlineUser> GetListByUserId(long? userId)
        {
            return this.list.Where(x => x.Value.UserId == userId.GetValueOrDefault()).Select(x=>x.Value).ToList();
        }
        public List<OnlineUser> GetAll()
        {
            return this.list.Values.ToList();
        }
    }
}
