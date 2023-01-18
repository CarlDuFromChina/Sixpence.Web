using Sixpence.Web.Config;
using Sixpence.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sixpence.Common.IoC;
using Sixpence.ORM.EntityManager;
using Sixpence.Web.Store;
using Sixpence.Web.Entity;
using Sixpence.ORM.Entity;

namespace Sixpence.Web.Plugin
{
    public class SysFilePlugin : IEntityManagerPlugin
    {
        public void Execute(EntityManagerPluginContext context)
        {
            if (!EntityCommon.CompareEntityName(nameof(SysFile), context.Entity.GetEntityName()))
                return;

            var entity = context.Entity;
            switch (context.Action)
            {
                case EntityAction.PreCreate:
                    break;
                case EntityAction.PostCreate:
                    break;
                case EntityAction.PreUpdate:
                    break;
                case EntityAction.PostUpdate:
                    break;
                case EntityAction.PreDelete:
                    break;
                case EntityAction.PostDelete:
                    {
                        #region 如果文件没有实体关联就删除
                        var sql = @"
SELECT
	* 
FROM
	sys_file 
WHERE
	hash_code = @hash_code 
	AND id <> @Id";
                        var paramList = new Dictionary<string, object>()
                        {
                            { "@hash_code", entity.GetAttributeValue<string>("hash_code") },
                            { "@id", entity.GetAttributeValue<string>("id") }
                        };
                        var dataList = context.EntityManager.Query<SysFile>(sql, paramList);
                        if (dataList.IsEmpty())
                        {
                            ServiceContainer.Resolve<IStoreStrategy>(StoreConfig.Config.Type).Delete(new List<string>() { entity.GetAttributeValue<string>("real_name") });
                        }
                        break;
                        #endregion
                    }
                default:
                    break;
            }
        }
    }
}
