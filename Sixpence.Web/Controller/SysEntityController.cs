using Sixpence.Web.Module.SysAttrs;
using Sixpence.Web.WebApi;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using Sixpence.Web.Service;
using Sixpence.Web.Entity;

namespace Sixpence.Web.Controller
{
    public class SysEntityController : EntityBaseController<SysEntity, SysEntityService>
    {
        /// <summary>
        /// 根据实体 id 查询字段
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("attrs")]
        public IList<SysAttrs> GetEntityAttrs(string id)
        {
            return new SysEntityService().GetEntityAttrs(id);
        }

        /// <summary>
        /// 导出实体类
        /// </summary>
        /// <param name="entityId"></param>
        /// <returns></returns>
        [HttpGet("export/cs")]
        public IActionResult ExportCs(string entityId)
        {
            HttpContext.Response.Headers.Add("Access-Control-Expose-Headers", "Content-Disposition");
            var result = new SysEntityService().Export(entityId);
            return File(result.bytes, result.ContentType, result.fileName);
        }
    }
}