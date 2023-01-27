using Sixpence.Common;
using Sixpence.ORM.Entity;
using Sixpence.ORM.EntityManager;
using Sixpence.Web.Cache;
using Sixpence.Web.Entity;
using Sixpence.Web.SysConfig;
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
            return SysConfigCache.GetValue(code);
        }

        public void CreateMissingConfig(IEnumerable<ISysConfig> settings)
        {
            settings.Each(item =>
            {
                var data = Manager.QueryFirst<Entity.SysConfig>("select * from sys_config where code = @code", new { code = item.Code });
                if (data == null)
                {
                    data = new Entity.SysConfig()
                    {
                        id = EntityCommon.GenerateGuid(),
                        name = item.Name,
                        code = item.Code,
                        value = item.DefaultValue.ToString(),
                        description = item.Description
                    };
                    Manager.Create(data);
                }
            });
        }
    }
}