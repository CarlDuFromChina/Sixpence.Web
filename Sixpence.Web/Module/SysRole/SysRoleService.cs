using Sixpence.Web.Auth;
using Sixpence.Web.Auth.UserInfo;
using Sixpence.Web.Entity;
using Sixpence.Web.Extensions;
using Sixpence.ORM.Entity;
using Sixpence.ORM.EntityManager;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Sixpence.Web.Module.Role
{
    public class SysRoleService : EntityService<sys_role>
    {
        #region 构造函数
        public SysRoleService() : base() { }

        public SysRoleService(IEntityManager manager) : base(manager) { }
        #endregion

        public IEnumerable<SelectOption> GetBasicRole()
        {
            var roles = Repository.GetAllEntity();
            var currentRoleId = Manager.QueryFirst<user_info>(UserIdentityUtil.GetCurrentUserId())?.roleid;
            var role = roles.FirstOrDefault(item => item.id == currentRoleId);

            return roles.Where(item => UserIdentityUtil.IsOwner(role.is_basic.Value ? role.id : role.parent_roleid, item.is_basic.Value ? item.id : item.parent_roleid))
                .Where(item => item.is_basic.Value)
                .Select(item => new SelectOption(item.name, item.id));
        }


        public IEnumerable<SelectOption> GetRoles()
        {
            var roles = Repository.GetAllEntity();
            var currentRoleId = Manager.QueryFirst<user_info>(UserIdentityUtil.GetCurrentUserId())?.roleid;
            var role = roles.FirstOrDefault(item => item.id == currentRoleId);

            return roles.Where(item => UserIdentityUtil.IsOwner(role.is_basic.Value ? role.id : role.parent_roleid, item.is_basic.Value ? item.id : item.parent_roleid))
                .Select(item => new SelectOption(item.name, item.id));
        }

        public sys_role GetGuest() => Manager.QueryFirst<sys_role>("222222222-22222-2222-2222-222222222222");

        public bool AllowCreateOrUpdateRole(string roleid)
        {
            var curid = UserIdentityUtil.GetCurrentUserId();
            var curRoleId = string.Empty;

            switch (curid)
            {
                case UserIdentityUtil.SYSTEM_ID:
                    curRoleId = curid;
                    break;
                case UserIdentityUtil.ANONYMOUS_ID:
                    curRoleId = curid;
                    break;
                case UserIdentityUtil.ADMIN_ID:
                    curRoleId = curid;
                    break;
                case UserIdentityUtil.USER_ID:
                    curRoleId = curid;
                    break;
                default:
                    curRoleId = Manager.QueryFirst<user_info>(UserIdentityUtil.GetCurrentUserId())?.roleid;
                    break;
            }

            if (string.IsNullOrEmpty(curRoleId))
            {
                return false;
            }
            var toRoleId = roleid;

            return Convert.ToInt32(toRoleId.FirstOrDefault().ToString()) >= Convert.ToInt32(curRoleId.FirstOrDefault().ToString());
        }
    }
}
