using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YiSha.Entity;
using YiSha.Service;
using YiSha.Service.SystemManage;
using YiSha.Entity.SystemManage;
using YiSha.Model.Result;
using YiSha.Model;
using YiSha.Util.Model;
using YiSha.Service.Cache;
using YiSha.Model.Param.SystemManage;
using YiSha.Util.Extension;
using YiSha.Web.Code;
using YiSha.Enum.SystemManage;

namespace YiSha.Business.SystemManage
{
    public class MenuBLL: MenuBLLBase
    {
        private MenuService menuService = new MenuService();


        #region 获取租户、系统的菜单列表


        /// <summary>
        /// 获取中可见的菜单列表
        /// </summary>
        /// <param name="param"></param>
        /// <param name="includeButton"></param>
        /// <returns></returns>
        public async Task<TData<List<MenuEntity>>> GetMenuListForEdit()
        {
            var obj = new TData<List<MenuEntity>>();

            MenuCache menuCache = new MenuCache();

            List<MenuEntity> list = await menuCache.GetList(false);
         
            obj.Result = list;
            obj.Status = true;
            return obj;
        }
        #endregion

        #region 获取数据


        public async Task<TData<MenuEntity>> GetEntity(long id)
        {
            TData<MenuEntity> obj = new TData<MenuEntity>();
            obj.Result = await menuService.GetEntity(id);
            if (obj.Result != null)
            {
                long parentId = obj.Result.ParentId.Value;
                if (parentId > 0)
                {
                    MenuEntity parentMenu = await menuService.GetEntity(parentId);
                    if (parentMenu != null)
                    {
                        obj.Result.ParentName = parentMenu.MenuName;
                    }
                }
                else
                {
                    obj.Result.ParentName = "主目录";
                }
            }
            obj.Status = true;
            return obj;
        }

        public async Task<TData<int>> GetMaxSort(long parentId)
        {
            TData<int> obj = new TData<int>();
            obj.Result = await menuService.GetMaxSort(parentId);
            obj.Status = true;
            return obj;
        }
        #endregion

        #region 提交数据
        public async Task<TData<string>> SaveForm(MenuEntity entity)
        {
            TData<string> obj = new TData<string>();
           
            await menuService.SaveForm(entity);

            obj.Result = entity.Id.ParseToString();
            obj.Status = true;
            return obj;
        }

        public async Task<TData> DeleteForm(string ids)
        {
            TData obj = new TData();
            await menuService.DeleteForm(ids);

            obj.Status = true;
            return obj;
        }
        #endregion

        #region 私有方法
        private List<MenuEntity> ListFilter(MenuListParam param, List<MenuEntity> list)
        {
            if (param != null)
            {
                if (!string.IsNullOrEmpty(param.MenuName))
                {
                    list = list.Where(p => p.MenuName.Contains(param.MenuName)).ToList();
                }
                if (param.MenuStatus > 0)
                {
                    list = list.Where(p => p.MenuStatus == param.MenuStatus).ToList();
                }
            }
            return list;
        }
        #endregion

        /// <summary>
        /// 角色编辑页面显示的菜单树，在设置角色的时候显示，需要显示到按钮级别
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<TData<List<ZtreeInfo>>> GetRoleMenuTreeList(MenuListParam param)
        {
            var obj = new TData<List<ZtreeInfo>>();
            obj.Result = new List<ZtreeInfo>();


            MenuCache menuCache = new MenuCache();

            List<MenuEntity> list = await menuCache.GetList(true);

            foreach (MenuEntity menu in list)
            {
                obj.Result.Add(new ZtreeInfo
                {
                    id = menu.Id.ToString(),
                    pId = menu.ParentId.ToString(),
                    name = menu.MenuName
                });
            }

            obj.Status = true;
            return obj;
        }


        /// <summary>
        /// 菜单编辑页面，选择父级菜单
        /// 
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<TData<List<ZtreeInfo>>> GetMenuChooseTreeListJson(MenuListParam param)
        {
            var obj = new TData<List<ZtreeInfo>>();
            obj.Result = new List<ZtreeInfo>();


            MenuCache menuCache = new MenuCache();

            List<MenuEntity> list = await menuCache.GetList(false);
           
            foreach (MenuEntity menu in list)
            {
                obj.Result.Add(new ZtreeInfo
                {
                    id = menu.Id.ToString(),
                    pId = menu.ParentId.ToString(),
                    name = menu.MenuName
                });
            }

            obj.Status = true;
            return obj;
        }
    }
}
