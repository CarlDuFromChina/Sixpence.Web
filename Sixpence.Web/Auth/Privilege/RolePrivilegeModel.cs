using Sixpence.Web.Module.Role;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sixpence.Web.Auth.Privilege
{
    public class RolePrivilegeModel
    {
        public sys_role Role { get; set; }
        public IEnumerable<sys_role_privilege> Privileges { get; set; }
    }
}
