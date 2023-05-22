using Koo.Utilities.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YiSha.Data.Repository;
using YiSha.Enum;
using YiSha.Enum.OrganizationManage;
using YiSha.Util;
using YiSha.Util.Extension;

namespace YiSha.Web.Code
{
    public class DataRepository : RepositoryFactory
    {
        public async Task<OperatorInfo> GetUserByToken(string token)
        {
            if (!SecurityHelper.IsSafeSqlParam(token))
            {
                return null;
            }
            token = token.ParseToString().Trim();

            var strSql = new StringBuilder();
            strSql.Append(@"SELECT  a.Id as UserId,
                                    a.UserStatus,
                                    a.IsOnline,
                                    a.UserName,
                                    a.RealName,
                                    a.Portrait,
                                    
                                    a.IsSystem,
                                    b.AppType, b.Token  as UserToken
                                    , c.DepartmentName 
                                    , c.Id as DepartmentId
                            FROM    sysuser a
                            inner join sysusertoken b on a.Id = b.UserId 
                            left join sysdepartment c  on  a.DepartmentId = c.Id 
                            WHERE   b.Token = '" + token + "'");
            var operatorInfo = await BaseRepository().FindObject<OperatorInfo>(strSql.ToString());
            await RefreshNewTenant(operatorInfo);

            return operatorInfo;
        }

        public async Task RefreshNewTenant(OperatorInfo operatorInfo)
        {
            if (operatorInfo == null)
            {
                return;
            }

            #region 查询当前用户 在当前租户和系统租户中的所有角色
         
            var strSql = new StringBuilder();

            strSql.Append(
$@"
SELECT  a.*
       
    FROM    sysrole a 
        inner join sysuserbelong b 
            on a.Id = b.BelongId and b.BelongType={(int)UserBelongTypeEnum.Role} 
    WHERE   
         b.UserId = {operatorInfo.UserId} 
        
");
         

            var belongDataList = (await BaseRepository().FindList<MyRoleInfo>(strSql.ToString())).ToList();

            //belongDataList.Add(RoleHelper.DefaultRegisterUser);

            operatorInfo.SetRoles(belongDataList);
           

            #endregion

            #region 查询用户的岗位
            strSql.Clear();
            strSql.Append(@"SELECT  
                                    a.BelongId
                                    ,a.BelongType
                                FROM    sysuserbelong a
                                WHERE   1=1 ");

         
            strSql.Append($"       and  a.UserId = {operatorInfo.UserId}");
            strSql.Append($"       and  a.BelongType = {(int)UserBelongTypeEnum.Position}");

            var positionDataList = await BaseRepository().FindList<UserBelongData>(strSql.ToString());
            var positionList = positionDataList.Where(x => x.BelongType == (int)UserBelongTypeEnum.Position );
            operatorInfo.PositionIds = string.Join(",", positionList.Select(x => x.BelongId));
            #endregion
        }


    }
}
