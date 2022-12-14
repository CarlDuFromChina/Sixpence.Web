using Sixpence.ORM.Entity;
using Sixpence.ORM.EntityManager;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sixpence.Web.Auth.Privilege
{
    public class SysRolePrivilegePlugin : IEntityManagerPlugin
    {
        public void Execute(EntityManagerPluginContext context)
        {
            if (context.Entity.GetEntityName() != nameof(sys_role_privilege)) return;

            switch (context.Action)
            {
                case EntityAction.PostCreate:
                case EntityAction.PostDelete:
                case EntityAction.PostUpdate:
                    UserPrivilegesCache.Clear(context.EntityManager);
                    break;
                default:
                    break;
            }
        }
    }
}
