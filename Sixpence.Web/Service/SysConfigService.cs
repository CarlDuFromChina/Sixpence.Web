using Sixpence.ORM.EntityManager;
using Sixpence.Web.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sixpence.Web.Service
{
    public class SysConfigService : EntityService<Entity.SysConfig>
    {
        #region 构造函数
        public SysConfigService() : base() { }

        public SysConfigService(IEntityManager manager) : base(manager) { }
        #endregion

        public object GetValue(string code)
        {
            if (!string.IsNullOrEmpty(code))
            {
                var sql = @"
select * from sys_config where code = @code;
";
                var data = Manager.QueryFirst<Entity.SysConfig>(sql, new Dictionary<string, object>() { { "@code", code } });
                return data?.value;
            }
            return "";
        }
    }
}