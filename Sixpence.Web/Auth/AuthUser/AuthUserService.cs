using Sixpence.Web.Config;
using Sixpence.ORM.Entity;
using Sixpence.Common;
using Sixpence.Common.Utils;
using System;
using System.Collections.Generic;
using System.Web;
using Sixpence.ORM.EntityManager;
using Sixpence.Common.IoC;
using System.Linq;

namespace Sixpence.Web.Auth
{
    public class AuthUserService : EntityService<auth_user>
    {
        #region 构造函数
        public AuthUserService() : base() { }

        public AuthUserService(IEntityManager manger) : base(manger) { }
        #endregion

        /// <summary>
        /// 获取用户登录信息
        /// </summary>
        /// <param name="code"></param>
        /// <param name="pwd">MD5密码</param>
        /// <returns></returns>
        public auth_user GetData(string code, string pwd)
        {
            var sql = @"
SELECT * FROM auth_user WHERE code = @code AND password = @password;
";
            var paramList = new Dictionary<string, object>() { { "@code", code }, { "@password", pwd } };
            var authUser = Manager.QueryFirst<auth_user>(sql, paramList);
            return authUser;
        }
        public auth_user GetDataByCode(string code)
        {
            var data = Manager.QueryFirst<auth_user>("select * from auth_user where code = @code", new Dictionary<string, object>() { { "@code", code } });
            return data;
        }

        /// <summary>
        /// 锁定用户
        /// </summary>
        /// <param name="id"></param>
        public void LockUser(string id)
        {
            Manager.ExecuteTransaction(() =>
            {
                var userId = UserIdentityUtil.GetCurrentUserId();
                AssertUtil.IsTrue(userId == id, "请勿锁定自己");
                var data = Manager.QueryFirst<auth_user>("select * from auth_user where user_infoid = @id", new Dictionary<string, object>() { { "@id", id } });
                data.is_lock = true;
                UpdateData(data);
            });
        }

        /// <summary>
        /// 解锁用户
        /// </summary>
        /// <param name="id"></param>
        public void UnlockUser(string id)
        {
            Manager.ExecuteTransaction(() =>
            {
                var data = Manager.QueryFirst<auth_user>("select * from auth_user where user_infoid = @id", new Dictionary<string, object>() { { "@id", id } });
                data.is_lock = false;
                UpdateData(data);
            });
        }

        /// <summary>
        /// 绑定用户
        /// </summary>
        /// <param name="type"></param>
        /// <param name="id"></param>
        /// <param name="code"></param>
        public void BindThirdPartyAccount(string type, string id, string code)
        {
            AssertUtil.IsNullOrEmpty(id, "用户id不能为空");
            AssertUtil.IsNullOrEmpty(code, "编码不能为空");
            AssertUtil.IsNull(type, "绑定类型不能为空");
            ServiceContainer.ResolveAll<IThirdPartyBindStrategy>().First(item => item.GetName().Equals(type, StringComparison.OrdinalIgnoreCase))?.Bind(code, id);
        }
    }
}