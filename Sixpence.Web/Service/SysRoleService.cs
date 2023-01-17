using Sixpence.Web.Auth;
using Sixpence.Web.Extensions;
using Sixpence.ORM.EntityManager;
using System;
using System.Collections.Generic;
using System.Linq;
using Sixpence.Web.Model;
using Sixpence.Web.Entity;

namespace Sixpence.Web.Service
{
    public class SysRoleService : EntityService<SysRole>
    {
        #region 构造函数
        public SysRoleService() : base() { }

        public SysRoleService(IEntityManager manager) : base(manager) { }
        #endregion

        public IEnumerable<SelectOption> GetBasicRole()
        {
            var roles = Repository.GetAllEntity();
            var currentRoleId = Manager.QueryFirst<UserInfo>(UserIdentityUtil.GetCurrentUserId())?.roleid;
            var role = roles.FirstOrDefault(item => item.id == currentRoleId);

            return roles.Where(item => UserIdentityUtil.IsOwner(role.is_basic.Value ? role.id : role.parent_roleid, item.is_basic.Value ? item.id : item.parent_roleid))
                .Where(item => item.is_basic.Value)
                .Select(item => new SelectOption(item.name, item.id));
        }


        public IEnumerable<SelectOption> GetRoles()
        {
            var roles = Repository.GetAllEntity();
            var currentRoleId = Manager.QueryFirst<UserInfo>(UserIdentityUtil.GetCurrentUserId())?.roleid;
            var role = roles.FirstOrDefault(item => item.id == currentRoleId);

            return roles.Where(item => UserIdentityUtil.IsOwner(role.is_basic.Value ? role.id : role.parent_roleid, item.is_basic.Value ? item.id : item.parent_roleid))
                .Select(item => new SelectOption(item.name, item.id));
        }

        public SysRole GetGuest() => Manager.QueryFirst<SysRole>("222222222-22222-2222-2222-222222222222");

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
                    curRoleId = Manager.QueryFirst<UserInfo>(UserIdentityUtil.GetCurrentUserId())?.roleid;
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
