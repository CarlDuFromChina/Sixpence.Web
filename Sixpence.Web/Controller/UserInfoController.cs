using Sixpence.Web.WebApi;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sixpence.Web.Service;
using Sixpence.Web.Entity;

namespace Sixpence.Web.Controller
{
    public class UserInfoController : EntityBaseController<UserInfo, UserInfoService>
    {
        [HttpGet("{id}"), AllowAnonymous]
        public override UserInfo GetData(string id)
        {
            return base.GetData(id);
        }

        /// <summary>
        /// 是否需要填充信息
        /// </summary>
        /// <returns></returns>
        [HttpGet("is_incomplete")]
        public bool InfoFilled()
        {
            return new UserInfoService().InfoFilled();
        }
    }
}
