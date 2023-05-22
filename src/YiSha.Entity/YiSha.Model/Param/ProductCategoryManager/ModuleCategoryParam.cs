using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using YiSha.Util;

namespace YiSha.Model.Param.ProductCategoryManager
{
    /// <summary>
    /// 创 建：admin
    /// 日 期：2022-09-24 14:57
    /// 描 述：实体查询类
    /// </summary>
    public class ModuleCategoryListParam
    {
        public long? Id
        {
            get;set;
        }

        public long? ProductId
        {
            get; set;
        }
    }

    //public class UserListParam : DateTimeParam
    //{
    //    public string UserName
    //    {
    //        get; set;
    //    }

    //    public string Mobile
    //    {
    //        get; set;
    //    }

    //    public int? UserStatus
    //    {
    //        get; set;
    //    }

     

    //    public List<long> ChildrenDepartmentIdList
    //    {
    //        get; set;
    //    }

    //    public string UserIds
    //    {
    //        get; set;
    //    }
    //}
}
