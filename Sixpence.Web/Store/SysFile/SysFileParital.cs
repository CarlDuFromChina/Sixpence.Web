using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sixpence.Web.Store.SysFile
{
    public partial class sys_file
    {
        [DataMember]
        public string DownloadUrl { get; set; }

        public string GetFilePath() => Path.Combine(FolderType.Storage.GetPath(), this.real_name);

        public static string GetFilePath(string fileName) => Path.Combine(FolderType.Storage.GetPath(), fileName);
    }
}
