using Sixpence.ORM.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sixpence.ORM.EntityManager;
using Sixpence.Web.Entity;

namespace Sixpence.Web.Auth.Role.BasicRole
{
    /// <summary>
    /// 普通用户
    /// </summary>
    public class User : BasicRole
    {
        public override Role Role => Role.User;

        public override IDictionary<string, IEnumerable<SysRolePrivilege>> GetMissingPrivilege(IEntityManager manager)
        {
            var dic = new Dictionary<string, IEnumerable<SysRolePrivilege>>();

            dic.Add(RoleType.Entity.ToString(), GetMissingEntityPrivileges(manager).Select(item => GenerateRolePrivilege(item, this.GetSysRole(), (int)OperationType.Read + (int)OperationType.Write + (int)OperationType.Delete)));
            dic.Add(RoleType.Menu.ToString(), GetMissingMenuPrivileges(manager).Select(item => GenerateRolePrivilege(item, this.GetSysRole(), 0)));

            return dic;
        }
    }
}
