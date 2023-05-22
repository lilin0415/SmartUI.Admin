using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YiSha.Data.Repository;
using YiSha.Entity.OrganizationManage;
using YiSha.Entity.SystemManage;
using YiSha.Enum.OrganizationManage;
using YiSha.Util.Extension;
using YiSha.Util.Model;
using YiSha.Web.Code;

namespace YiSha.Service.OrganizationManage
{
    public class UserBelongService : BaseRepositoryService
    {
        #region 获取数据
        public async Task<List<UserBelongEntity>> GetList(UserBelongEntity entity)
        {
            var expression = CreateFilter<UserBelongEntity>();
            if (entity != null)
            {
                if (entity.BelongType != null)
                {
                    expression = expression.And(t => t.BelongType == entity.BelongType);
                }
                if (entity.UserId != null)
                {
                    expression = expression.And(t => t.UserId == entity.UserId);
                }
            }
            var list = await this.BaseRepository().FindList(expression);
            return list.ToList();
        }

        public async Task<UserBelongEntity> GetEntity(long id)
        {
            return await this.BaseRepository().FindEntity<UserBelongEntity>(id);
        }
        #endregion

        #region 保存
        public async Task<long> SaveForm(UserBelongEntity entity)
        {
            if (entity.Id.IsNullOrZero())
            {
                entity.Create();

                return await VerifyIsMyDataAndInsert(entity);
            }
            else
            {
                entity.Modify();

                return await VerifyIsMyDataAndUpdate(entity);
            }
        }

        #endregion

        #region 删除
        public async Task DeleteForm(long id)
        {
            this.VerifyIsMyDataOnDelete<UserBelongEntity>(id.ToString());
            await this.BaseRepository().Delete<UserBelongEntity>(id);
        }
        #endregion

        public async Task FillPositionAndRoleData(UserEntity userEntity)
        {
            #region 查询当前用户 在当前租户和系统租户中的所有角色

            var strSql = new StringBuilder();

            strSql.Append(
$@"
SELECT  a.*
       
    FROM    sysrole a 
        inner join sysuserbelong b 
            on a.Id = b.BelongId and b.BelongType={(int)UserBelongTypeEnum.Role} 
    WHERE   
         b.UserId = {userEntity.Id.Value} 
        
");


            var belongDataList = (await BaseRepository().FindList<RoleEntity>(strSql.ToString())).ToList();

            userEntity.RoleIds = String.Join(",", belongDataList.Select(x => x.Id));
            userEntity.RoleNames = String.Join(",", belongDataList.Select(x => x.RoleName));
            


            #endregion

            #region 查询用户的岗位
            strSql.Clear();
            strSql.Append($@"SELECT  
                                    a.*
                                    
                                FROM    sysposition a 
                                    inner join sysuserbelong b on a.Id = b.BelongId and b.BelongType = {(int)UserBelongTypeEnum.Position}
                                WHERE   1=1 ");


            strSql.Append($"       and  b.UserId = {userEntity.Id.Value}");
            

            var positionDataList = await BaseRepository().FindList<PositionEntity>(strSql.ToString());

            userEntity.PositionIds = string.Join(",", positionDataList.Select(x => x.Id));
            userEntity.PositionNames = String.Join(",", positionDataList.Select(x => x.PositionName));
            #endregion
        }
    }
}
