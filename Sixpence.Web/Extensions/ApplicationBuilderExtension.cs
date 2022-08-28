using System;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Quartz;
using Sixpence.Common;
using Sixpence.Common.IoC;
using Sixpence.Common.Utils;
using Sixpence.ORM.EntityManager;
using Sixpence.Web.Auth.Privilege;
using Sixpence.Web.Auth.Role.BasicRole;
using Sixpence.Web.Job;
using Sixpence.Web.RobotMessageTask;

namespace Sixpence.Web.Extensions
{
    public static class ApplicationBuilderExtension
    {
        public static IApplicationBuilder UseStaticHttpContext(this IApplicationBuilder app)
        {
            var httpContextAccessor = app.ApplicationServices.GetRequiredService<IHttpContextAccessor>();
            HttpCurrentContext.Configure(httpContextAccessor);
            return app;
        }

        public static IApplicationBuilder UseJob(this IApplicationBuilder app)
        {
            AssemblyUtil
                .GetAssemblies("Sixpence.*.dll")
                .GetTypes()
                .Where(type => !type.IsAbstract && !type.IsInterface && type.GetInterfaces().Contains(typeof(IJob)) && !type.IsDefined(typeof(DynamicJobAttribute), true))
                .Each(type => ServiceContainer.Register(typeof(IJob), type));
            JobHelpers.Start();
            new RobotMessageTaskService().GetAllData().Each(item =>
            {
                JobHelpers.RegisterJob(new RobotMessageTaskJob(item.name, item.robotid_name, item.runtime), item, item.job_state.ToTriggerState());
            });
            return app;
        }

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

