using Sixpence.Web.Auth.Privilege;
using Sixpence.ORM.Entity;
using Sixpence.Web.Module.SysEntity;
using Sixpence.Web.Module.SysMenu;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sixpence.ORM.EntityManager;

namespace Sixpence.Web.Auth.Role.BasicRole
{
    /// <summary>
    /// 访客
    /// </summary>
    public class Guest : BasicRole
    {
        public override Role Role => Role.Guest;

        public override IDictionary<string, IEnumerable<sys_role_privilege>> GetMissingPrivilege(IEntityManager manager)
        {
            var dic = new Dictionary<string, IEnumerable<sys_role_privilege>>();

            dic.Add(RoleType.Entity.ToString(), GetMissingEntityPrivileges(manager).Select(item => GenerateRolePrivilege(item, this.GetSysRole(), (int)OperationType.Read)));
            dic.Add(RoleType.Menu.ToString(), GetMissingMenuPrivileges(manager).Select(item => GenerateRolePrivilege(item, this.GetSysRole(), 0)));

            return dic;
        }
    }
}
