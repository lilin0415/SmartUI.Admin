using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YiSha.Entity;
using YiSha.Service;
using YiSha.Service.SystemManage;
using YiSha.Entity.SystemManage;
using YiSha.Util;
using YiSha.Util.Extension;
using YiSha.Model;
using YiSha.Util.Model;
using YiSha.Model.Param.SystemManage;
using YiSha.Enum.SystemManage;
using YiSha.Service.Cache;
using YiSha.Web.Code;

namespace YiSha.Business.SystemManage
{
    public class RoleBLL
    {
        private RoleService roleService = new RoleService();
        private MenuAuthorizeService menuAuthorizeService = new MenuAuthorizeService();

        

        #region 获取数据
        public async Task<TData<List<RoleModel>>> GetList(RoleListParam param,bool systemRole)
        {
            TData<List<RoleModel>> obj = new TData<List<RoleModel>>();
            obj.Result = await roleService.GetList(param);
            obj.Total = obj.Result.Count;
            obj.Status = true;
            return obj;
        }

        public async Task<TData<List<RoleModel>>> GetPageList(RoleListParam param, Pagination pagination,bool systemRole)
        {
            TData<List<RoleModel>> obj = new TData<List<RoleModel>>();
            obj.Result = await roleService.GetPageList(param, pagination);
            obj.Total = pagination.TotalCount;
            obj.Status = true;
            return obj;
        }

        public async Task<TData<RoleEntity>> GetEntity(long id)
        {
            TData<RoleEntity> obj = new TData<RoleEntity>();
            RoleEntity roleEntity = await roleService.GetEntity(id);
            List<MenuAuthorizeEntity> menuAuthorizeList = await menuAuthorizeService.GetList(new MenuAuthorizeEntity
            {
                AuthorizeId = id,
                AuthorizeType = AuthorizeTypeEnum.Role.ParseToInt()
            });
            // 获取角色对应的权限
            roleEntity.MenuIds = string.Join(",", menuAuthorizeList.Select(p => p.MenuId));

            obj.Result = roleEntity;
            obj.Status = true;
            return obj;
        }

        public async Task<TData<int>> GetMaxSort()
        {
            TData<int> obj = new TData<int>();
            obj.Result = await roleService.GetMaxSort();
            obj.Status = true;
            return obj;
        }
        #endregion

        #region 提交数据
        public async Task<TData<string>> SaveForm(RoleEntity entity,bool systemRole)
        {
            TData<string> obj = new TData<string>();

            await roleService.SaveForm(entity);

            MenuAuthorizeCache menuAuthorizeCache = new MenuAuthorizeCache();
            // 清除缓存里面的权限数据
            menuAuthorizeCache.Remove();

            obj.Result = entity.Id.ParseToString();
            obj.Status = true;

            return obj;
        }

        public async Task<TData> DeleteForm(string ids)
        {
            TData obj = new TData();

            await roleService.DeleteForm(ids);

         
            MenuAuthorizeCache menuAuthorizeCache = new MenuAuthorizeCache();
            // 清除缓存里面的权限数据
            menuAuthorizeCache.Remove();

            obj.Status = true;
            return obj;
        }
        #endregion

    }
}
