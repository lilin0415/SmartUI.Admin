using Koo.Utilities.Exceptions;
using Koo.Utilities.FileFormaKoo.APMLHelper;
using Koo.Utilities.Helpers;
using NPOI.POIFS.FileSystem;
using NPOI.SS.Formula.Functions;
using Org.BouncyCastle.Crypto;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YiSha.Data.Repository;
using YiSha.Util;

namespace YiSha.Web.Code
{
    public class TenantHelper
    {
      

        #region 验证id所对应的数据是否为我自己的数据

        /// <summary>
        /// 新增数据的时候
        /// 验证数据的租户和用户
        /// </summary>
        /// <param name="data"></param>
        /// <exception cref="BizException"></exception>
        public static void VerifyIsMyDataOnCreate(IBaseEntity entity)
        {
            if (entity == null)
            {
                throw new BizException("数据为空");
            }

            var operatorInfo = Operator.Instance.CurrentInfo();
            if (operatorInfo == null)
            {
                //未登录的时候，记录的日志可能为null
                return;
            }

            var type = typeof(T);
            //新增数据
            var ids = "0";

            if (entity.BaseCreatorId.GetValueOrDefault() == 0)
            {
                entity.BaseCreatorId = operatorInfo.UserId;
            }

            //新建数据的时候不需要验证是否是管理园
            //都需要验证id
            if (entity.BaseCreatorId != operatorInfo.UserId)
            {
                throw new BizException("数据验证失败，只能操作自己的数据", new Exception($"数据:{ids}，类型{type.Name}的所属用户信息验证失败，当前用户:{operatorInfo.UserId}"));
            }


         
        }
        /// <summary>
        /// 验证修改的数据是否为我的数据
        /// 用户只能修改自己的数据
        /// </summary>
        /// <param name="data"></param>
        /// <param name="verifyTenant"></param>
        public static void VerifyIsMyData(IBaseEntity data)
        {
            var id = data.Id;

            var operatorInfo = Operator.Instance.CurrentInfo();
            ////系统管理员可以修改其它租户的数据，所以不需要验证
            if (!operatorInfo.HasSystemRole)
            {
              

            }

            VerifyIsMyData(data.GetType(), id.ToString());
        }
       
        /// <summary>
        /// 验证修改的数据是否为我的数据
        /// 用户只能修改自己的数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ids"></param>
        /// <param name="verifyTenant"></param>
        /// <exception cref="BizException"></exception>
        public static void VerifyIsMyData<T>(string ids)
        {
            VerifyIsMyData(typeof(T), ids);
        }

        /// <summary>
        /// 验证修改的数据是否为我的数据
        /// 用户只能修改自己的数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ids"></param>
        /// <param name="verifyTenant"></param>
        /// <exception cref="BizException"></exception>
        public static void VerifyIsMyData(Type type,string ids)
        {
            long[] idArr = TextHelper.SplitToArray<long>(ids, ',');
            if (!idArr.Any())
            {
                return;
            }

            var operatorInfo = Operator.Instance.CurrentInfo();

            //系统管理员可以修改任意用户、任意租户的数据
            if (operatorInfo.HasSystemRole)
            {
                return;
            }

            var tblAtt = TypeHelper.GetAttribute<TableAttribute>(type);
            var tblName = tblAtt.Name;
         

            List<VerifyMyDataModel> list = new List<VerifyMyDataModel>();

            Task.Run(() =>
            {
                var fields =  "BaseCreatorId";
                var sql = $"select {fields} from {tblName} where id in ({string.Join(",", idArr)})";
                Repository repo = new RepositoryFactory().BaseRepository();
                var tenantIdTable = repo.FindTable(sql);
                tenantIdTable.Wait();
                list = DataTableHelper.ToList<VerifyMyDataModel>(tenantIdTable.Result);
            }).Wait();

            if (!operatorInfo.HasManagerPower)
            {
                if (list.Any(x => x.BaseCreatorId != operatorInfo.UserId))
                {
                    throw new BizException("数据验证失败，只能操作自己的数据", new Exception($"数据:{ids}，类型{type.Name}的所属用户信息验证失败，当前用户:{operatorInfo.UserId}"));
                }
            }


        }

        public class VerifyMyDataModel
        {
            public long? BaseCreatorId
            {
                get; set;
            }
         
        }
        #endregion
    }
}
