using Koo.Utilities.Exceptions;
using Koo.Utilities.Helpers;
using NPOI.POIFS.FileSystem;
using NPOI.SS.Formula.Functions;
using Org.BouncyCastle.Crypto;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using YiSha.Data.Repository;
using YiSha.Entity;
using YiSha.Entity.OrganizationManage;
using YiSha.Entity.SystemManage;
using YiSha.Model.Param.OrganizationManage;
using YiSha.Util;
using YiSha.Util.Extension;
using YiSha.Web.Code;

namespace YiSha.Service
{
    public class BaseRepositoryService : RepositoryFactory
    {
        public OperatorInfo GetCurrentUser()
        {
            return Operator.Instance.CurrentInfo();
        }
        public long? GetCurrentUserId()
        {
            var operatorInfo = GetCurrentUser();
            return operatorInfo?.UserId;
        }

        public Expression<Func<T, bool>> CreateFilter<T>() where T:BaseEntity
        {
            var expression = LinqExtensions.True<T>();
          
            return expression;
        }

        #region 新增、修改、删除数据的时候，验证所操作的数据是否为我的数据
        public void VerifyIsMyDataOnCreate<T>(T entity) where T : BaseEntity
        {
            TenantHelper.VerifyIsMyDataOnCreate(entity);
        }

        public void VerifyIsMyDataOnModify<T>(T entity) where T : BaseEntity
        {
          TenantHelper.VerifyIsMyData<T>(entity.Id.ToString());
        }
     
        public void VerifyIsMyDataOnDelete<T>(string ids)
        {
            TenantHelper.VerifyIsMyData<T>(ids);
        }
        #endregion

        public void VerifyHasSystemRole()
        {
            if (this.GetCurrentUser().HasSystemRole)
            {

            }
            else
            {
                throw new ForbidOperationExection("没有此权限");
            }
        }
        public void VerifyHasManagerPower()
        {
            if (this.GetCurrentUser().HasManagerPower)
            {

            }
            else
            {
                throw new ForbidOperationExection("没有此权限");
            }
        }

        public async Task<int> VerifyIsMyDataAndInsert<T>(T entity) where T : BaseEntity
        {
            this.VerifyIsMyDataOnCreate(entity);
            return await this.BaseRepository().Insert(entity);
        }

        public async Task<int> VerifyIsMyDataAndUpdate<T>(T entity) where T : BaseEntity
        {
            this.VerifyIsMyDataOnModify(entity);
            return await this.BaseRepository().Update(entity);
        }
    }
}
