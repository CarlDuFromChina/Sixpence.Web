using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Sixpence.Web.Auth.Github.Model
{
    public class GithubConfig
    {
        public string client_id { get;set; }

        [JsonIgnore]
        public string client_secret { get;set; }
    }
}
