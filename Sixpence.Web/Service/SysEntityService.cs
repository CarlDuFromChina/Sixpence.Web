﻿using Sixpence.Web.Module.SysAttrs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Sixpence.Web.Profiles;
using Sixpence.ORM.EntityManager;
using Sixpence.ORM;
using Sixpence.Web.Model;
using Sixpence.Web.Entity;

namespace Sixpence.Web.Service
{
    public class SysEntityService : EntityService<SysEntity>
    {
        #region 构造函数
        public SysEntityService() : base() { }

        public SysEntityService(IEntityManager manager) : base(manager) { }
        #endregion

        public override IList<EntityView> GetViewList()
        {
            var sql = @"
SELECT
	*
FROM
	sys_entity
";
            var customFilter = new List<string>() { "name" };
            return new List<EntityView>()
            {
                new EntityView()
                {
                    Sql = sql,
                    CustomFilter = customFilter,
                    OrderBy = "name, created_at",
                    ViewId = "FBEC5163-587B-437E-995F-1DC97229C906",
                    Name = "所有的实体"
                }
            };
        }

        /// <summary>
        /// 根据实体 id 查询字段
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IList<SysAttrs> GetEntityAttrs(string id)
        {
            var sql = @"
SELECT
	*
FROM 
	sys_attrs
WHERE
	entityid = @id
";
            return Manager.Query<SysAttrs>(sql, new Dictionary<string, object>() { { "@id", id } }).ToList();
        }

        /// <summary>
        /// 创建实体
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public override string CreateData(SysEntity t)
        {
            var id = "";
            Manager.ExecuteTransaction(() =>
            {
                id = base.CreateData(t);
                var sql = $"CREATE TABLE {t.code} (id VARCHAR(100) PRIMARY KEY)";
                Manager.Execute(sql);
                new SysAttrsService(Manager).AddSystemAttrs(id);
            });
            return id;
        }

        /// <summary>
        /// 删除实体
        /// </summary>
        /// <param name="ids"></param>
        public override void DeleteData(List<string> ids)
        {
            Manager.ExecuteTransaction(() =>
            {
                var dataList = Manager.Query<SysEntity>(ids).ToList();
                base.DeleteData(ids); // 删除实体
                var sql = @"
DELETE FROM sys_attrs WHERE entityid IN (in@ids);
";
                Manager.Execute(sql, new Dictionary<string, object>() { { "in@ids", string.Join(",", ids) } }); // 删除级联字段
                dataList.ForEach(data =>
                {
                    Manager.Execute($"DROP TABLE {data.code}");
                });
            });
        }

        /// <summary>
        /// 导出实体类
        /// </summary>
        /// <param name="entityId"></param>
        /// <returns></returns>
        public (string fileName, string ContentType, byte[] bytes) Export(string entityId)
        {
            var attrs = GetEntityAttrs(entityId);
            var entity = GetData(entityId);

            var attributes = "";
            foreach (var item in attrs)
            {
                var column = MapperHelper.Map<ColumnOptions>(item);

                // 实体id和实体name不需要产生
                if (item.code != entity.code + "id")
                {
                    var attribute = $@"
        /// <summary>
        /// {column.Remark}
        /// </summary>
        [DataMember, Column, Description(""{column.Remark}"")]
        public {Manager.Driver.Convert2CSharpType(column.Type)} {column.Name} {{ get; set; }}
";
                    attributes += attribute;
                }
            }

            var content = $@"
using Sixpence.ORM.Entity;
using System;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace SixpenceStudio.Core
{{
    [Entity(""{entity.code}"",""{entity.GetRemark()}"")]
    public partial class {entity.code} : BaseEntity
    {{
        /// <summary>
        /// 实体id
        /// </summary>
        [DataMember]
        [PrimaryColumn]
        public string {entity.GetPrimaryColumn().Name} {{ get; set; }}

        {attributes}
    }}
}}
";

            using (MemoryStream ms = new MemoryStream())
            {
                using (StreamWriter writer = new StreamWriter(ms))
                {
                    writer.WriteLine(content);
                    writer.Close();
                }
                return ($"{entity.code}.sql", "application/octet-stream", ms.ToArray());
            }
        }
    }
}