﻿using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Org.BouncyCastle.Crypto.Agreement.Srp;
using YiSha.Util;

namespace YiSha.Model.Param.TestTaskManager
{
    /// <summary>
    /// 创 建：admin
    /// 日 期：2022-10-02 16:20
    /// 描 述：实体查询类
    /// </summary>
    public class TestTaskListParam
    {
        public long? Id
        {
            get;set;
        }
        public string Name
        {
            get;set;
        }
        public string CaseCode
        {
            get;set;
        }
        public string CaseName
        {
            get;set;
        }
        public long? EnvId
        {
            get;set;
        }
    }
}