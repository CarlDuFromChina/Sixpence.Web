using Sixpence.ORM.Entity;
using Sixpence.Web.Auth;
using Sixpence.Web.Cache;
using Sixpence.Web.Module.SysMenu;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sixpence.Web.Extensions
{
    public static class SysMenuExtension
    {
        public static IEnumerable<SysMenu> Filter(this IEnumerable<SysMenu> sysMenus)
        {
            var privileges = UserPrivilegesCache.GetUserPrivileges(UserIdentityUtil.GetCurrentUserId()).Where(item => EntityCommon.CompareEntityName(nameof(SysMenu), item.object_type));
            return sysMenus.Where(item =>
            {
                var data = privileges.FirstOrDefault(e => e.objectid == item.id);
                return data != null && data.privilege > 0;
            });
        }
    }
}
