using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace Sixpence.Web.Module.SysMenu
{
    public partial class sys_menu
    {
        [DataMember]
        public IList<sys_menu> children { get; set; }
    }
}