using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace Sixpence.Web.Module.SysMenu
{
    public partial class SysMenu
    {
        [DataMember]
        public IList<SysMenu> children { get; set; }
    }
}