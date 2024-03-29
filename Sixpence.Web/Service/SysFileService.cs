﻿using Sixpence.Web.Config;
using Sixpence.Common;
using Sixpence.Common.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.AspNetCore.Http;
using Sixpence.Web.Profiles;
using Sixpence.Common.IoC;
using Sixpence.ORM.EntityManager;
using System.Linq;
using Sixpence.Web.Model;
using Sixpence.Web.Store;
using Sixpence.Web.Entity;
using Sixpence.Common.Crypto;
using Sixpence.Web.Utils;

namespace Sixpence.Web.Service
{
    public class SysFileService : EntityService<SysFile>
    {
        #region 构造函数
        public SysFileService() : base() { }

        public SysFileService(IEntityManager manager) : base(manager) { }
        #endregion

        public override IList<EntityView> GetViewList()
        {
            return new List<EntityView>()
            {
                new EntityView()
                {
                    Name = "所有文件",
                    ViewId = "DD1D72FB-D7DE-49AC-B387-273375E6A7BA",
                    Sql = @"
SELECT
	id,
	NAME,
	file_type,
	content_type,
	created_at,
	created_by_name
FROM
	sys_file
",
                    OrderBy = "created_at desc",
                    CustomFilter = new List<string>(){ "name" }
                },
                new EntityView()
                {
                    Name = "图库",
                    ViewId = "3BCF6C07-2B49-4D69-9EB1-A3D5B721C976",
                    Sql = $@"
SELECT
	id,
	NAME,
	file_type,
	created_at,
	created_by_name,
	concat('{GetDownloadUrl("")}', id) AS downloadUrl
FROM
	sys_file
",
                    OrderBy = "",
                    CustomFilter = new List<string>(){ "name" }
                }
            };
        }

        public IList<SysFile> GetDattaByCode(string code)
        {
            var sql = @"
SELECT * FROM sys_file
WHERE hash_code = @code
";
            return Manager.Query<SysFile>(sql, new Dictionary<string, object>() { { "@code", code } }).ToList();
        }

        public SysFile UploadFile(Stream stream, string fileSuffix, string fileType, string contentType, string objectId, string fileName = "")
        {
            var id = Guid.NewGuid().ToString();
            var hash_code = SHAUtil.GetFileSHA1(stream);
            var newFileName = $"{id}.{fileSuffix}"; // GUID 生成文件名

            // 保存图片到本地
            // TODO：执行失败回滚操作
            var config = StoreConfig.Config;
            ServiceContainer.Resolve<IStoreStrategy>(config.Type).Upload(stream, newFileName, out var filePath);

            var sysFile = new SysFile()
            {
                id = id,
                name = fileName,
                real_name = newFileName,
                hash_code = hash_code,
                file_type = fileType,
                objectId = objectId,
                content_type = contentType,
                DownloadUrl = GetDownloadUrl(id)
            };
            CreateData(sysFile);

            return sysFile;
        }

        /// <summary>
        /// 上传大图并自动生成缩略图
        /// </summary>
        /// <param name="file"></param>
        /// <param name="fileType"></param>
        /// <param name="objectId"></param>
        /// <returns></returns>
        public IEnumerable<FileInfoModel> UploadBigImage(IFormFile file, string fileType, string objectId = "")
        {
            var stream = file.OpenReadStream();
            var contentType = file.ContentType;
            var suffix = file.FileName.GetFileType();

            return Manager.ExecuteTransaction(() =>
            {
                // 上传大图
                var image = UploadFile(stream, suffix, fileType, contentType, objectId, file.FileName);
                var thumbStream = ImageHelper.GetThumbnail(image.GetFilePath());
                var image2 = UploadFile(thumbStream, suffix, fileType, contentType, objectId, GetPreviewImageFileName(file.FileName));
                return new List<FileInfoModel>()
                {
                    MapperHelper.Map<FileInfoModel>(image),
                    MapperHelper.Map<FileInfoModel>(image2)
                };
            });
        }

        /// <summary>
        /// 获取本地下载地址
        /// </summary>
        /// <param name="fileid"></param>
        /// <returns></returns>
        public static string GetDownloadUrl(string fileid, bool isRelative = true)
        {
            if (isRelative)
            {
                return $"/api/sys_file/download?objectId={fileid}";
            }
            var config = SystemConfig.Config;
            return $"{config.Protocol}://{config.Domain}/api/sys_file/download?objectId={fileid}";
        }

        /// <summary>
        /// 生成预览图文件名
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public string GetPreviewImageFileName(string fileName)
        {
            AssertUtil.IsNullOrEmpty(fileName, "上传文件文件名不能为空");
            var fileNameArr = fileName.Split(".");
            AssertUtil.IsTrue(fileNameArr.Length != 2, "上传文件文件名格式错误");
            return $"{fileNameArr[0]}_small.{fileNameArr[1]}";
        }
    }
}