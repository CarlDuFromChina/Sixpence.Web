using Sixpence.Web.Auth.Privilege;
using Sixpence.Web.Auth.Role.BasicRole;
using Microsoft.AspNetCore.Builder;
using Sixpence.Common;
using Sixpence.Common.IoC;
using Sixpence.Common.Utils;
using Sixpence.ORM.EntityManager;

namespace Sixpence.Web.Module.SysRole
{
    public static class SysRoleExtension
    {
        public static IApplicationBuilder UseSysRole(this IApplicationBuilder app)
        {
            var roles = ServiceContainer.ResolveAll<IRole>();
            var manager = EntityManagerFactory.GetManager();
            new SysRolePrivilegeService(manager).CreateRoleMissingPrivilege();

            // 权限读取到缓存
            roles.Each(item => MemoryCacheUtil.Set(item.GetRoleKey, new RolePrivilegeModel() { Role = item.GetSysRole(), Privileges = item.GetRolePrivilege() }, 3600 * 12));

            return app;
        }
    }
}
