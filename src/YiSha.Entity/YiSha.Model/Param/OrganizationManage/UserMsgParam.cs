using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using YiSha.Util;

namespace YiSha.Model.Param.OrganizationManage
{
    /// <summary>
    /// 创 建：admin
    /// 日 期：2022-11-24 10:41
    /// 描 述：实体查询类
    /// </summary>
    public class UserMsgListParam
    {
        /// <summary>
        /// 0：全部
        /// 1：接收
        /// 2：发出
        /// </summary>
        public int MsgDirection
        {
            get;set;
        }
    }

    public enum UserMsgDirectionEnumType
    {
        /// <summary>
        /// 我接收到的消息
        /// </summary>
        Received=1,
        /// <summary>
        /// 我发出的消息
        /// </summary>
        Sended = 2,
    }
}
