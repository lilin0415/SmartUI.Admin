using Koo.Utilities.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using YiSha.Data.Repository;
using YiSha.Entity.SystemManage;
using YiSha.Enum.SystemManage;
using YiSha.Model.Param.SystemManage;
using YiSha.Service.Cache;
using YiSha.Util;
using YiSha.Util.Extension;

namespace YiSha.Service.SystemManage
{
    public class MenuService : BaseRepositoryService
    {
        #region 获取数据
        /// <summary>
        /// 返回所有的菜单，用于缓存
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<List<MenuEntity>> GetList(MenuListParam param)
        {
            var sql = ListFilter(param);

            var list = await this.BaseRepository().FindList<MenuEntity>(sql);
            var ret = list.OrderBy(p => p.MenuSort).ToList();

            return ret;
        }

        public async Task<MenuEntity> GetEntity(long id)
        {
            return await this.BaseRepository().FindEntity<MenuEntity>(id);
        }

        public async Task<int> GetMaxSort(long parentId)
        {
            string where = string.Empty;
            if (parentId > 0)
            {
                where += " AND ParentId = " + parentId;
            }
            object result = await this.BaseRepository().FindObject("SELECT MAX(MenuSort) FROM sysmenu where BaseIsDelete = 0 " + where);
            int sort = result.ParseToInt();
            sort++;
            return sort;
        }

        public bool ExistMenuName(MenuEntity entity)
        {
            var expression = LinqExtensions.True<MenuEntity>();
            expression = expression.And(t => t.BaseIsDelete == 0);
            if (entity.Id.IsNullOrZero())
            {
                expression = expression.And(t => t.MenuName == entity.MenuName && t.MenuType == entity.MenuType);
            }
            else
            {
                expression = expression.And(t => t.MenuName == entity.MenuName && t.MenuType == entity.MenuType && t.Id != entity.Id);
            }
            return this.BaseRepository().IQueryable(expression).Count() > 0 ? true : false;
        }
        #endregion

        #region 提交数据
        #region 获取所有子节点数据
        private async Task<bool> HasChild(long[] ids)
        {
            if (ids.Length == 0)
            {
                return false;
            }
            var expression = ListFilter(new MenuListParam());
            var list = await this.BaseRepository().FindList<MenuEntity>(expression);
            var items = list.ToList();

            return items.Any(x => ids.Contains(x.ParentId.Value));
        }

        public async Task<List<MenuEntity>> GetAllChildren(long? id)
        {
            var expression = ListFilter(new MenuListParam());
            var list = await this.BaseRepository().FindList<MenuEntity>(expression);
            var items = list.ToList();

            var ret = new List<MenuEntity>();

            GetAllChildrenRecursive(items, id, ret);

            return ret;
        }
        private void GetAllChildrenRecursive(List<MenuEntity> items, long? id, List<MenuEntity> ret)
        {
            var subItems = items.Where(x => x.ParentId == id);
            if (subItems.Any())
            {
                ret.AddRange(subItems);

                foreach (var item in subItems)
                {
                    GetAllChildrenRecursive(items, item.Id, ret);
                }
            }
        }
        #endregion

        private async Task VerifyParentId(MenuEntity entity)
        {
            if (entity.ParentId.HasValue && entity.ParentId != 0)
            {
                if (entity.ParentId == entity.Id)
                {

                    throw new DataInvalidException("不能选择自己作为上级菜单");
                }

                var parentEntity = await this.GetEntity(entity.ParentId.Value);
                //if (parentEntity.MenuType != (int)MenuTypeEnum.Directory)
                //{
                //    throw new BizException("上级菜单只能选择目录");
                //}
                if (entity.Id > 0)
                {
                    //修改节点的时候，需要查询当前节点下面的子节点
                    var children = await GetAllChildren(entity.Id);
                    if (children.Any(x => x.Id == entity.ParentId))
                    {
                        throw new DataInvalidException("不能选择下级数据作为上级菜单");
                    }
                }
                else
                {
                    //新增的时候不需要
                }
            }
        }
        public async Task SaveForm(MenuEntity entity)
        {
            this.VerifyHasManagerPower();

            if (string.IsNullOrWhiteSpace(entity.MenuName))
            {
                throw new CanNotBeEmptyException("名称不能为空");
            }
            entity.MenuName = entity.MenuName.Trim();

            //throw new ForbidUpdateExection();
            await this.VerifyParentId(entity);

            if (entity.MenuType == (int)MenuTypeEnum.Button)
            {
                //按钮可以重名
            }
            else
            {
                if (this.ExistMenuName(entity))
                {
                    throw new DuplicationDataExection("菜单名称已经存在");
                }
            }
           

            //如果为系统菜单，则禁止修改（非系统菜单伪造成系统菜单）
            if (entity.IsSystem == 1)
            {
                VerifyHasSystemRole();
            }


            if (entity.Id.IsNullOrZero())
            {
                entity.Create();
                this.VerifyIsMyDataOnCreate(entity);

                await this.BaseRepository().Insert(entity);
            }
            else
            {
                var existedEntity = await GetEntity(entity.Id.Value);
                if (existedEntity == null)
                {
                    throw new DataNotExistedException("当前菜单已不存在");
                }

                //把系统菜单伪造成非系统菜单
                if (existedEntity.IsSystem == 1)
                {
                    VerifyHasSystemRole();
                }
                if (entity.ParentId == null)
                {
                    entity.ParentId = 0;
                }
                entity.Modify();
                this.VerifyIsMyDataOnModify(entity);

                await this.BaseRepository().Update(entity);
            }

            this.ClearCache();
        }

        public async Task DeleteForm(string ids)
        {
            this.VerifyHasManagerPower();

            long[] idArr = TextHelper.SplitToArray<long>(ids, ',');

            if (await HasChild(idArr))
            {
                throw new ForbidDeleteExection("请先删除子级数据");
            }

            this.VerifyIsMyDataOnDelete<MenuEntity>(ids);
            var db = await this.BaseRepository().BeginTrans();
            try
            {
                
                await db.Delete<MenuEntity>(p => idArr.Contains(p.Id.Value) || idArr.Contains(p.ParentId.Value));
                await db.Delete<MenuAuthorizeEntity>(p => idArr.Contains(p.MenuId.Value));
                await db.CommitTrans();
            }
            catch
            {
                await db.RollbackTrans();
                throw;
            }
            this.ClearCache();
        }
        #endregion

        private void ClearCache()
        {
            var menuCache = new MenuCache();
            menuCache.Remove();
        }

        #region 私有方法
        private Expression<Func<MenuEntity, bool>> ListFilter(MenuListParam param)
        {
            var expression = LinqExtensions.True<MenuEntity>();
            if (param != null)
            {
                if (!string.IsNullOrEmpty(param.MenuName))
                {
                    expression = expression.And(t => t.MenuName.Contains(param.MenuName));
                }
                if (param.MenuStatus > -1)
                {
                    expression = expression.And(t => t.MenuStatus == param.MenuStatus);
                }
            }
            return expression;
        }

        /// <summary>
        /// 过滤菜单树
        /// </summary>
        /// <param name="list"></param>
        /// <param name="onlyEnabled"></param>
        /// <returns></returns>
        public static List<MenuEntity> FilterTree(IEnumerable<MenuEntity> list, bool onlyEnabled)
        {
            if (onlyEnabled)
            {
                list = list.Where(x => x.MenuStatus == 1);
            }
            
            if (GlobalContext.SystemConfig.IsReleased)
            {
                //如果发布模式，只显示
                list = list.Where(x => x.MenuType == (int)MenuTypeEnum.Button || (x.MenuType == (int)MenuTypeEnum.Directory || x.MenuType == (int)MenuTypeEnum.Menu) && x.IsPublic == 1);
            }

            var retItems = list.Where(x => x.ParentId == 0).ToList();

            foreach (var item in list)
            {
                //如果
                if (retItems.Contains(item))
                {
                    continue;
                }
            }

            var validItems = FindAllChildren(list.ToList(), 0);

            //对于ispublic属性，父级不可见，子级菜单自动不可见
            var ret = validItems.OrderBy(p => p.MenuSort).ToList();

            return ret;
        }
        private static List<MenuEntity> FindAllChildren(List<MenuEntity> allItems, long parentId)
        {
            var ret = new List<MenuEntity>();

            var foundItems = allItems.Where(x=>x.ParentId== parentId).ToList();

            ret.AddRange(foundItems);

            foreach (var subItem in foundItems)
            {
                var subs = FindAllChildren(allItems, subItem.Id.Value);
                ret.AddRange(subs);
            }
            return ret;
        }

        #endregion

    }
}
