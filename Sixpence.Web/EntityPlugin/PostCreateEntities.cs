﻿using AutoMapper;
using Sixpence.Web;
using Sixpence.Web.Auth;
using Sixpence.Web.Module.SysAttrs;
using Sixpence.Web.Utils;
using Sixpence.Common;
using Sixpence.Common.Logging;
using Sixpence.Common.Utils;
using Sixpence.ORM.EntityManager;
using Sixpence.ORM.Extensions;
using Sixpence.ORM.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using Sixpence.Web.Entity;
using Sixpence.Web.Cache;
using Sixpence.Web.Service;
using Sixpence.ORM;
using Sixpence.ORM.Entity;

namespace Sixpence.Web.EntityPlugin
{
    /// <summary>
    /// 自动创建实体插件，记录实体
    /// </summary>
    public class PostCreateEntities : IPostCreateEntities
    {
        public void Execute(IEntityManager manager, IEnumerable<IEntity> entities)
        {
            var logger = LoggerFactory.GetLogger("entity");
            UserIdentityUtil.SetCurrentUser(UserIdentityUtil.GetSystem());
            entities.Each(item =>
                {
                    #region 实体添加自动写入记录
                    var entityName = item.GetEntityName();
                    var entity = manager.QueryFirst<SysEntity>("select * from sys_entity where code = @code", new Dictionary<string, object>() { { "@code", entityName } });
                    if (entity == null)
                    {
                        entity = new SysEntity()
                        {
                            id = Guid.NewGuid().ToString(),
                            name = item.GetRemark(),
                            code = item.GetEntityName(),
                        };
                        manager.Create(entity, false);
                    }
                    #endregion

                    var attrs = EntityCommon.GetColumns(item, manager.Driver);
                    var attrsList = new SysEntityService(manager).GetEntityAttrs(entity.id).Select(e => e.code);

                    #region 实体字段变更（删除字段）
                    attrsList.Each(attr =>
                    {
                        if (!attrs.Any(item => item.Name.ToLower() == attr.ToLower()))
                        {
                            var sql = @"DELETE FROM sys_attrs WHERE lower(code) = @code AND entityid = @entityid";
                            manager.Execute(sql, new Dictionary<string, object>() { { "@code", attr.ToLower() }, { "@entityid", entity.id } });
                            sql = manager.Driver.GetDropColumnSql(item.GetEntityName(), new List<ColumnOptions>() { new ColumnOptions() { Name = attr } });
                            manager.Execute(sql);
                            logger.Debug($"实体{item.GetRemark()} （{item.GetEntityName()}）删除字段：{attr}");
                        }
                    });
                    #endregion

                    #region 实体字段变更（新增字段）
                    attrs.Each(attr =>
                    {
                        if (!attrsList.Contains(attr.Name))
                        {
                            var _attr = new SysAttrs()
                            {
                                id = Guid.NewGuid().ToString(),
                                name = attr.Remark,
                                code = attr.Name,
                                entityid = entity.id,
                                entityid_name = entity.name,
                                entityCode = entity.code,
                                attr_type = attr.Type.ToString().ToLower(),
                                attr_length = attr.Length,
                                isrequire = attr.IsRequire.HasValue && attr.IsRequire.Value,
                                default_value = ConvertUtil.ConToString(attr.DefaultValue)
                            };
                            manager.Create(_attr);
                            logger.Debug($"实体{item.GetRemark()}（{item.GetEntityName()}）创建字段：{attr.Remark}（{attr.Name}）成功");
                        }
                    });
                    #endregion
                });

            #region 执行版本更新脚本
            {
                var vLogger = LoggerFactory.GetLogger("version");
                FileHelper.GetFileList("*.*", FolderType.Version)
                        .OrderBy(item => Path.GetFileName(item))
                        .ToList()
                        .Each(filePath =>
                        {
                            try
                            {
                                if (filePath.EndsWith(".sql") || filePath.EndsWith(".csv"))
                                {
                                    var count = new VersionScriptExecutionLogService(manager).ExecuteScript(filePath);
                                    if (count == 1)
                                    {
                                        vLogger.Info($"脚本：{Path.GetFileName(filePath)}执行成功");
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                vLogger.Error($"脚本：{Path.GetFileName(filePath)}执行失败", ex);
                            }
                        });
            }
            #endregion
        }
    }
}
