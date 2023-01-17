using Sixpence.ORM.EntityManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sixpence.Web.Module.SysMenu;
using Sixpence.Web.Cache;
using Sixpence.Web.Service;

namespace Sixpence.Web.Plugin
{
    public class SysMenuPlugin : IEntityManagerPlugin
    {
        public void Execute(EntityManagerPluginContext context)
        {
            var manager = context.EntityManager;
            var entity = context.Entity as SysMenu;

            switch (context.Action)
            {
                case EntityAction.PreCreate:
                case EntityAction.PreUpdate:
                    WriteStateCodeName(entity);
                    break;
                case EntityAction.PostCreate:
                    // 创建权限
                    new SysRolePrivilegeService(manager).CreateRoleMissingPrivilege();
                    // 重新注册权限并清除缓存
                    UserPrivilegesCache.Clear(manager);
                    break;
                case EntityAction.PostDelete:
                    var privileges = new SysRolePrivilegeService(manager).GetPrivileges(context.Entity.GetPrimaryColumn().Value)?.ToArray();
                    manager.Delete(privileges);
                    UserPrivilegesCache.Clear(manager);
                    break;
                default:
                    break;
            }
        }

        private void WriteStateCodeName(SysMenu menu)
        {
            if (menu.statecode == true)
                menu.statecode_name = "启用";
            else
                menu.statecode_name = "停用";
        }
    }
}
