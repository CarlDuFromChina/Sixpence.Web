using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Sixpence.Web.Auth.Gitee.Model
{
    public class GiteeConfig
    {
        public string client_id { get; set; }

        [JsonIgnore]
        public string client_secret { get; set; }
    }
}
