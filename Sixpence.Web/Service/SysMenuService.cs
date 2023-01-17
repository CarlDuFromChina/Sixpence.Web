using Sixpence.Web.Auth;
using Sixpence.ORM.EntityManager;
using Sixpence.ORM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sixpence.Web.Module.SysMenu;
using Sixpence.Web.Extensions;

namespace Sixpence.Web.Service
{
    public class SysMenuService : EntityService<SysMenu>
    {
        #region 构造函数
        public SysMenuService() : base() { }
        public SysMenuService(IEntityManager manager) : base(manager) { }
        #endregion

        public override IEnumerable<SysMenu> GetDataList(IList<SearchCondition> searchList, string orderBy, string viewId = "", string searchValue = "")
        {
            var data = base.GetDataList(searchList, orderBy, viewId).Filter().ToList();
            var firstMenu = data
                .Where(e => string.IsNullOrEmpty(e.parentid))
                .Select(item =>
                {
                    item.children = data
                        .Where(e => e.parentid == item.id)
                        .OrderBy(e => e.menu_index)
                        .ToList();
                    return item;
                })
                .OrderBy(e => e.menu_index)
                .ToList();
            return firstMenu;
        }

        public override DataModel<SysMenu> GetDataList(IList<SearchCondition> searchList, string orderBy, int pageSize, int pageIndex, string viewId = "", string searchValue = "")
        {
            var model = base.GetDataList(searchList, orderBy, pageSize, pageIndex, viewId);
            var data = model.DataList.Filter().ToList();
            var firstMenu = data.Where(e => string.IsNullOrEmpty(e.parentid)).ToList();
            firstMenu.ForEach(item =>
            {
                item.children = new List<SysMenu>();
                data.ForEach(item2 =>
                {
                    if (item2.parentid == item.id)
                    {
                        item.children.Add(item2);
                    }
                });
                item.children = item.children.OrderBy(e => e.menu_index).ToList();
            });
            firstMenu = firstMenu.OrderBy(e => e.menu_index).ToList();
            return new DataModel<SysMenu>()
            {
                DataList = firstMenu,
                RecordCount = data.Count()
            };
        }

        public IList<SysMenu> GetFirstMenu()
        {
            var sql = @"
SELECT * FROM sys_menu
WHERE parentid IS NULL OR parentid = ''
ORDER BY menu_index
";
            var data = Manager.Query<SysMenu>(sql).ToList();
            return data;
        }
    }
}