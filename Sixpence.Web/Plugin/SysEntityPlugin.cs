using Sixpence.ORM.Entity;
using Sixpence.ORM.EntityManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sixpence.Web.Cache;
using Sixpence.Web.Service;
using Sixpence.Web.Entity;

namespace Sixpence.Web.Plugin
{
    public class SysEntityPlugin : IEntityManagerPlugin
    {
        public void Execute(EntityManagerPluginContext context)
        {
            if (!EntityCommon.CompareEntityName(nameof(SysEntity), context.Entity.GetEntityName()))
                return;

            var manager = context.EntityManager;
            switch (context.Action)
            {
                case EntityAction.PostCreate:
                    // 创建权限
                    new SysRolePrivilegeService(manager).CreateRoleMissingPrivilege();
                    // 重新注册权限并清除缓存
                    UserPrivilegesCache.Clear(manager);
                    break;
                case EntityAction.PostDelete:
                    var privileges = new SysRolePrivilegeService(manager).GetPrivileges(context.Entity.GetPrimaryColumn().Value).ToArray();
                    manager.Delete(privileges);
                    UserPrivilegesCache.Clear(manager);
                    break;
                default:
                    break;
            }
        }
    }
}
