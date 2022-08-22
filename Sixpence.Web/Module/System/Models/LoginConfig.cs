using Sixpence.Web.Auth.Gitee.Model;
using Sixpence.Web.Auth.Github.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sixpence.Web.Module.System.Models
{
    public class LoginConfig
    {
        public GithubConfig github { get; set; }
        public GiteeConfig gitee { get; set; }
    }
}
