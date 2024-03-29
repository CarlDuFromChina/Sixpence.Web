﻿using Sixpence.Web;
using Sixpence.Web.Auth;
using Sixpence.Web.Extensions;
using Sixpence.Web.Utils;
using Sixpence.ORM.EntityManager;
using Sixpence.ORM.Models;
using Sixpence.ORM.Repository;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Sixpence.ORM.Entity;
using Sixpence.Web.Model;
using Sixpence.Web.Cache;
using Sixpence.Web.Entity;

namespace Sixpence.Web.Service
{
    /// <summary>
    /// 实体服务类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class EntityService<T>
        where T : BaseEntity, new()
    {
        public EntityService()
        {
            Repository = new Repository<T>();
        }

        public EntityService(IEntityManager manager)
        {
            Repository = new Repository<T>(manager);
        }

        /// <summary>
        /// 实体操作
        /// </summary>
        protected IRepository<T> Repository;

        /// <summary>
        /// 数据库持久化
        /// </summary>
        protected IEntityManager Manager => Repository.Manager;

        #region 实体表单

        /// <summary>
        /// 获取视图
        /// </summary>
        /// <returns></returns>
        public virtual IList<EntityView> GetViewList()
        {
            var sql = $"SELECT * FROM {new T().GetEntityName()} WHERE 1=1";
            return new List<EntityView>()
            {
                new EntityView()
                {
                    Sql = sql,
                    CustomFilter = new List<string>() { "name" }, // name 是每个实体必须要添加字段
                    OrderBy = "created_at DESC",
                    ViewId = ""
                }
            };
        }

        /// <summary>
        /// 获取所有记录
        /// </summary>
        /// <returns></returns>
        public IEnumerable<T> GetAllData()
        {
            return Repository.GetAllEntity();
        }

        /// <summary>
        /// 获取所有实体记录
        /// </summary>
        /// <returns></returns>
        public virtual IEnumerable<T> GetDataList(IList<SearchCondition> searchList, string orderBy, string viewId = "", string searchValue = "")
        {
            var view = string.IsNullOrEmpty(viewId) ? GetViewList().ToList().FirstOrDefault() : GetViewList().ToList().Find(item => item.ViewId == viewId);
            return Repository.GetDataList(view, searchList, orderBy);
        }

        /// <summary>
        /// 获取所有实体记录
        /// </summary>
        /// <returns></returns>
        public virtual DataModel<T> GetDataList(IList<SearchCondition> searchList, string orderBy, int pageSize, int pageIndex, string viewId = "", string searchValue = "")
        {
            var view = string.IsNullOrEmpty(viewId) ? GetViewList().ToList().FirstOrDefault() : GetViewList().ToList().Find(item => item.ViewId == viewId);
            var data = Repository.GetDataList(view, searchList, orderBy, pageSize, pageIndex, out var recordCount, searchValue);
            return new DataModel<T>()
            {
                DataList = data.ToList(),
                RecordCount = recordCount
            };
        }

        /// <summary>
        /// 获取实体记录
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual T GetData(string id)
        {
            var obj = Repository.FilteredQueryFirst(id);
            return obj;
        }

        /// <summary>
        /// 创建数据
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public virtual string CreateData(T t)
        {
            return Repository.FilteredCreate(t);
        }

        /// <summary>
        /// 更新记录
        /// </summary>
        /// <param name="t"></param>
        public virtual void UpdateData(T t)
        {
            Repository.FilteredUpdate(t);
        }

        /// <summary>
        /// 创建或更新记录
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public virtual string CreateOrUpdateData(T t)
        {
            return Repository.FilteredCreateOrUpdateData(t);
        }

        /// <summary>
        /// 删除记录
        /// </summary>
        /// <param name="ids"></param>
        public virtual void DeleteData(List<string> ids)
        {
            Repository.FilteredDelete(ids);
        }

        /// <summary>
        /// 导出CSV文件
        /// </summary>
        /// <returns></returns>
        public virtual string Export()
        {
            var fileName = $"{new T().GetEntityName()}.csv";
            var fullFilePath = Path.Combine(FolderType.Temp.GetPath(), fileName);
            var dataList = GetAllData();
            CsvUtil.Write(dataList, fullFilePath);
            return fullFilePath;
        }
        #endregion

        /// <summary>
        /// 获取用户对实体的权限
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public EntityPrivilegeResponse GetPrivilege()
        {
            var sql = @"
SELECT * FROM sys_role_privilege
WHERE sys_roleid = @id and object_type = 'sys_entity'
and objectid = @entityid";
            var user = Manager.QueryFirst<UserInfo>(UserIdentityUtil.GetCurrentUserId());
            var paramList = new Dictionary<string, object>() { { "@id", user.roleid }, { "@entityid", EntityCache.GetEntity(new T().GetEntityName())?.GetPrimaryColumn().Value } };
            var data = Manager.QueryFirst<SysRolePrivilege>(sql, paramList);

            return new EntityPrivilegeResponse()
            {
                read = data.privilege >= 1,
                create = data.privilege >= 3,
                delete = data.privilege >= 7
            };
        }
    }
}
