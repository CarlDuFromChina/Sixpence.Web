using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace Sixpence.Web.Module.SysAttrs
{
    public partial class sys_attrs
    {
        [DataMember]
        public string entityCode { get; set; }
    }
}