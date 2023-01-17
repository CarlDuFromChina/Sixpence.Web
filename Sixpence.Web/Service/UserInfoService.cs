using Sixpence.Common;
using Sixpence.Common.Utils;
using System.Collections.Generic;
using Sixpence.ORM.Repository;
using Sixpence.ORM.EntityManager;
using System.Linq;
using Sixpence.Web.Model;
using Sixpence.Web.Auth;
using Sixpence.Web.Entity;

namespace Sixpence.Web.Service
{
    public class UserInfoService : EntityService<UserInfo>
    {
        #region 构造函数
        public UserInfoService()
        {
            Repository = new Repository<UserInfo>();
        }

        public UserInfoService(IEntityManager manager)
        {
            Repository = new Repository<UserInfo>(manager);
        }
        #endregion

        public override IList<EntityView> GetViewList()
        {
            var sql = @"
SELECT
    is_lock_name,
	user_info.*
FROM
	user_info
LEFT JOIN (
    SELECT
        user_infoid,
        is_lock_name
    FROM auth_user
) au ON user_info.id = au.user_infoid
";
            var customFilter = new List<string>() { "name" };
            return new List<EntityView>()
            {
                new EntityView()
                {
                    Sql = sql,
                    CustomFilter = customFilter,
                    OrderBy = "name, created_at",
                    ViewId = "59F908EB-A353-4205-ABE4-FA9DB27DD434",
                    Name = "所有的用户信息"
                }
            };
        }

        public UserInfo GetData()
        {
            return Repository.FindOne(UserIdentityUtil.GetCurrentUserId());
        }

        /// <summary>
        /// 通过Code查询用户信息
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public UserInfo GetDataByCode(string code)
        {
            return Repository.FindOne(new Dictionary<string, object>() { { "code", code } });
        }

        /// <summary>
        /// 是否需要填充信息
        /// </summary>
        /// <returns></returns>
        public bool InfoFilled()
        {
            var user = Repository.FindOne(UserIdentityUtil.GetCurrentUserId());
            AssertUtil.IsNull(user, "未查询到用户");
            if (user.id == UserIdentityUtil.ADMIN_ID)
            {
                return false;
            }
            return !user.gender.HasValue || CheckEmpty(user.mailbox, user.cellphone, user.realname);
        }

        private bool CheckEmpty(params string[] args)
        {
            return args.Any(item => string.IsNullOrEmpty(item));
        }
    }
}
