using Sixpence.Common.Logging;
using Sixpence.Web.Store.SysFile;
using Sixpence.Common.Utils;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

using System.Web;
using Sixpence.ORM.EntityManager;

namespace Sixpence.Web.Store
{
    public class SystemStore : IStoreStrategy
    {
        /// <summary>
        /// 批量删除文件
        /// </summary>
        /// <param name="fileName"></param>
        public void Delete(IList<string> fileName)
        {
            fileName.ToList().ForEach(item =>
            {
                var filePath = sys_file.GetFilePath(item);
                FileUtil.DeleteFile(filePath);
            });
        }

        /// <summary>
        /// 下载文件
        /// </summary>
        /// <param name="filePath"></param>
        public async Task<IActionResult> DownLoad(string objectId)
        {
            var manager = EntityManagerFactory.GetManager();
            var data = manager.QueryFirst<sys_file>(objectId) ?? manager.QueryFirst<sys_file>("select * from sys_file where hash_code = @id", new Dictionary<string, object>() { { "@id", objectId } });
            var fileInfo = new FileInfo(data.GetFilePath());
            if (fileInfo.Exists)
            {
                var stream = await FileUtil.GetFileStreamAsync(fileInfo.FullName);
                HttpCurrentContext.Response.Headers.Add("Content-Disposition", "attachment; filename=" + HttpUtility.UrlEncode(fileInfo.Name, System.Text.Encoding.UTF8));
                return new FileStreamResult(stream, "application/octet-stream");
            }
            LogUtil.Error($"文件{fileInfo.Name}未找到，文件路径：{fileInfo.FullName}");
            throw new FileNotFoundException();
        }

        /// <summary>
        /// 获取文件流
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Stream GetStream(string id)
        {
            var manager = EntityManagerFactory.GetManager();
            var data = manager.QueryFirst<sys_file>(id);
            return FileUtil.GetFileStream(data.GetFilePath());
        }

        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="fileName"></param>
        public void Upload(Stream stream, string fileName, out string filePath)
        {
            filePath = $"{Path.AltDirectorySeparatorChar}storage{Path.AltDirectorySeparatorChar}{fileName}"; // 相对路径
            FileUtil.SaveFile(stream, sys_file.GetFilePath(fileName));
        }
    }
}
