using System.Collections.Generic;
using Sixpence.Web.Auth.Role.BasicRole;
using Sixpence.Web.WebApi;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Sixpence.Web.Auth.Privilege
{
    public class SysRolePrivilegeController : EntityBaseController<sys_role_privilege, SysRolePrivilegeService>
    {
        [HttpGet("{roleid}/{roleType}")]
        public IEnumerable<sys_role_privilege> GetUserPrivileges(string roleid, RoleType roleType)
        {
            return new SysRolePrivilegeService().GetUserPrivileges(roleid, roleType);
        }

        [HttpPost("bulk_save")]
        public void BulkSave([FromBody] string dataList)
        {
            var privileges = string.IsNullOrEmpty(dataList) ? null : JsonConvert.DeserializeObject<List<sys_role_privilege>>(dataList);
            new SysRolePrivilegeService().BulkSave(privileges);
        }
    }
}
