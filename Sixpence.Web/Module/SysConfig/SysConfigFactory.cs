using Sixpence.ORM.EntityManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sixpence.Web.Module.SysConfig
{
    /// <summary>
    /// 系统参数工厂类
    /// </summary>
    public static class SysConfigFactory
    {
        static IEntityManager manager;
        static SysConfigFactory()
        {
            manager = EntityManagerFactory.GetManager();
        }

        /// <summary>
        /// 获取配置值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static object GetValue<T>() where T : ISysConfig, new()
        {
            var sql = @"
select * from sys_config where code = @code;
";
            var t = new T();
            var data = manager.QueryFirst<sys_config>(sql, new Dictionary<string, object>() { { "@code", t.Code } });
            // 数据库里不存在该配置，则创建一条配置记录
            if (data == null)
            {
                var model = new sys_config()
                {
                    name = t.Name,
                    code = t.Code,
                    value = t.DefaultValue?.ToString()
                };
                manager.Create(model);
                return t.DefaultValue;
            }
            return data.value;
        }
    }
}