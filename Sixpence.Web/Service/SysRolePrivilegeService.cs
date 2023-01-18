using Sixpence.Web.Auth.Role.BasicRole;
using Sixpence.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sixpence.Web.Module.SysMenu;
using Sixpence.Common.IoC;
using Sixpence.ORM.Repository;
using Sixpence.ORM.EntityManager;
using Sixpence.Web.Entity;
using Sixpence.ORM.Entity;

namespace Sixpence.Web.Service
{
    public class SysRolePrivilegeService : EntityService<SysRolePrivilege>
    {
        #region 构造函数
        public SysRolePrivilegeService()
        {
            Repository = new Repository<SysRolePrivilege>();
        }

        public SysRolePrivilegeService(IEntityManager manger)
        {
            Repository = new Repository<SysRolePrivilege>(manger);
        }
        #endregion

        /// <summary>
        /// 获取角色权限
        /// </summary>
        /// <param name="roleid"></param>
        /// <returns></returns>
        public IEnumerable<SysRolePrivilege> GetUserPrivileges(string roleid, RoleType roleType)
        {
            var role = Manager.QueryFirst<SysRole>(roleid);
            var privileges = new List<SysRolePrivilege>();

            if (role.is_basic.Value)
            {
                privileges = ServiceContainer.ResolveAll<IRole>().FirstOrDefault(item => item.Role.GetDescription() == role.name).GetRolePrivilege().ToList();
            }
            else
            {
                var sql = @"
SELECT * FROM sys_role_privilege
WHERE sys_roleid = @id
";
                privileges = Manager.Query<SysRolePrivilege>(sql, new Dictionary<string, object>() { { "@id", roleid } }).ToList();
            }

            switch (roleType)
            {
                case RoleType.All:
                    return privileges;
                case RoleType.Entity:
                    return privileges.Where(item => EntityCommon.CompareEntityName(nameof(SysEntity), item.object_type));
                case RoleType.Menu:
                    return privileges.Where(item => EntityCommon.CompareEntityName(nameof(SysMenu), item.object_type));
                default:
                    return new List<SysRolePrivilege>();
            }
        }

        /// <summary>
        /// 获取角色权限
        /// </summary>
        /// <param name="entityid"></param>
        /// <returns></returns>
        public IEnumerable<SysRolePrivilege> GetPrivileges(string entityid)
        {
            var sql = @"
SELECT * FROM sys_role_privilege
WHERE objectid = @id";
            return Manager.Query<SysRolePrivilege>(sql, new Dictionary<string, object>() { { "@id", entityid } });
        }

        /// <summary>
        /// 批量更新或创建
        /// </summary>
        /// <param name="dataList"></param>
        public void BulkSave(List<SysRolePrivilege> dataList)
        {
            Manager.BulkCreateOrUpdate(dataList);
        }

        /// <summary>
        /// 自动生成权限
        /// </summary>
        public void CreateRoleMissingPrivilege()
        {
            var roles = ServiceContainer.ResolveAll<IRole>();
            var privileges = new List<SysRolePrivilege>();

            roles.Each(item =>
            {
                item.GetMissingPrivilege(Manager)
                    .Each(item =>
                    {
                        if (!item.Value.IsEmpty())
                        {
                            privileges.AddRange(item.Value);
                        }
                    });
            });

            Manager.ExecuteTransaction(() => Manager.BulkCreate(privileges));
        }
    }
}
