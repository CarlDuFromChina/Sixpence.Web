using Sixpence.Web.Auth.Role.BasicRole;
using Sixpence.ORM.Entity;
using Sixpence.Common;
using Sixpence.Common.Utils;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sixpence.Common.IoC;
using Sixpence.ORM.EntityManager;
using Sixpence.Web.Entity;
using Sixpence.Web.Model;

namespace Sixpence.Web.Cache
{
    public static class UserPrivilegesCache
    {
        private const string UserPrivilegesPrefix = "UserPrivileges";

        private static readonly ConcurrentDictionary<string, IEnumerable<SysRolePrivilege>> UserPrivliege = new ConcurrentDictionary<string, IEnumerable<SysRolePrivilege>>();

        /// <summary>
        /// 获取用户权限信息
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public static IEnumerable<SysRolePrivilege> GetUserPrivileges(string userId)
        {
            return UserPrivliege.GetOrAdd(UserPrivilegesPrefix + userId, (key) =>
            {
                var manager = EntityManagerFactory.GetManager();
                var user = manager.QueryFirst<AuthUser>(userId);
                return manager.Query<SysRolePrivilege>("select * from sys_role_privilege where sys_roleid = @id", new Dictionary<string, object>() { { "@id", user.roleid } }).ToList();
            });
        }

        /// <summary>
        /// 清除用户权限信息缓存
        /// </summary>
        /// <param name="id"></param>
        public static void Clear(string id)
        {
            UserPrivliege.TryRemove(UserPrivilegesPrefix + id, out var privileges);
        }

        /// <summary>
        /// 清除用户权限信息缓存
        /// </summary>
        public static void Clear(IEntityManager manager)
        {
            UserPrivliege.Clear();
            ServiceContainer.ResolveAll<IRole>().Each(item =>
            {
                (item as BasicRole).Manager = manager;
                item.ClearCache();
                MemoryCacheUtil.RemoveCacheItem(item.GetRoleKey);
                MemoryCacheUtil.Set(item.GetRoleKey, new RolePrivilegeModel() { Role = item.GetSysRole(), Privileges = item.GetRolePrivilege() }, 3600 * 12);
            });
        }
    }
}
