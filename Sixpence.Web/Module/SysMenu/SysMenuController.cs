using Sixpence.Web.WebApi;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sixpence.Web.Module.SysMenu
{
    public class SysMenuController : EntityBaseController<sys_menu, SysMenuService>
    {
        [HttpGet]
        [Route("first_menu")]
        public IList<sys_menu> GetFirstMenu()
        {
            return new SysMenuService().GetFirstMenu();
        }
    }
}