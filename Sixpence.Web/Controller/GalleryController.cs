﻿using Sixpence.Web.WebApi;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Sixpence.Web.Model.Pixabay;
using Sixpence.Web.Entity;
using Sixpence.Web.Service;
using Microsoft.AspNetCore.Authorization;

namespace Sixpence.Web.Controller
{
    public class GalleryController : EntityBaseController<Gallery, GalleryService>
    {
        /// <summary>
        /// 搜索云图库
        /// </summary>
        /// <param name="searchValue"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [HttpGet("cloud/search")]
        public ImagesModel GetImages(string searchValue, int pageIndex, int pageSize)
        {
            return new PixabayService().GetImages(searchValue, pageIndex, pageSize);
        }

        /// <summary>
        /// 上传图片
        /// </summary>
        /// <param name="image"></param>
        /// <returns></returns>
        [HttpPost("upload")]
        public List<string> UploadImage(ImageModel image)
        {
            return new GalleryService().UploadImage(image);
        }

        /// <summary>
        /// 获取随机图片
        /// </summary>
        /// <returns></returns>
        [HttpGet("random_image"), AllowAnonymous]
        public Gallery RandomImage()
        {
            return new GalleryService().GetRandomImage();
        }
    }
}
