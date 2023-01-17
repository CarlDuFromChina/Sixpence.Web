using Sixpence.Common.Utils;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using Sixpence.Web.Config;
using Sixpence.Common.Logging;
using Sixpence.ORM.EntityManager;
using Sixpence.Web.Entity;
using Sixpence.Web.Model;

namespace Sixpence.Web.Service
{
    public class MailVertificationService : EntityService<MailVertification>
    {
        #region 构造函数
        public MailVertificationService() : base() { }

        public MailVertificationService(IEntityManager manager) : base(manager) { }
        #endregion

        /// <summary>
        /// 查询未激活且未过期的邮件
        /// </summary>
        /// <param name="mail"></param>
        /// <returns></returns>
        public MailVertification GetDataByMailAdress(string mail, MailType mailType = MailType.Activation)
        {
            var sql = @"SELECT * FROM mail_vertification
WHERE mail_address = @address
AND is_active is false
AND expire_time > CURRENT_TIMESTAMP
AND mail_type = @type";
            return Manager.QueryFirst<MailVertification>(sql, new Dictionary<string, object>() { { "@address", mail }, { "@type", mailType.ToString() } });
        }

        /// <summary>
        /// 邮箱激活用户
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public string ActivateUser(string id)
        {
            return Manager.ExecuteTransaction(() =>
            {
                var data = GetData(id);
                if (data == null)
                    return "激活失败";

                if (data.expire_time < DateTime.Now)
                    return "激活失败，激活链接已过期";

                #region 创建用户
                var model = JsonConvert.DeserializeObject<LoginRequest>(data.login_request.ToString());
                var role = new SysRoleService(Manager).GetGuest();
                var user = new UserInfo()
                {
                    id = Guid.NewGuid().ToString(),
                    code = model.code,
                    password = model.password,
                    name = model.code.Split("@")[0],
                    mailbox = model.code,
                    roleid = role.id,
                    roleid_name = role.name,
                    statecode = true,
                    statecode_name = "启用"
                };
                Manager.Create(user, false);
                var _authUser = new AuthUser()
                {
                    id = user.id,
                    name = user.name,
                    code = user.code,
                    roleid = user.roleid,
                    roleid_name = user.roleid_name,
                    user_infoid = user.id,
                    is_lock = false,
                    last_login_time = DateTime.Now,
                    password = model.password
                };
                Manager.Create(_authUser);
                #endregion

                data.is_active = true;
                Manager.Update(data);

                return "激活成功";
            });
        }

        /// <summary>
        /// 重置密码
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public string ResetPassword(string id)
        {
            try
            {
                return Manager.ExecuteTransaction(() =>
                {
                    var data = GetData(id);
                    if (data == null)
                        return "重置失败";

                    if (data.expire_time < DateTime.Now)
                        return "重置失败，重置链接已过期";

                    new SystemService(Manager).ResetPassword(data.created_by);
                    return $"重置成功，初始密码为：{SystemConfig.Config.DefaultPassword}";
                });
            }
            catch (Exception ex)
            {
                LogUtil.Error("重置密码失败", ex);
                return "服务器内部错误，请联系管理员";
            }
        }
    }
}
