using Sixpence.Web.Pixabay;
using Sixpence.Web.WebApi;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Sixpence.Web.Module.Gallery
{
    public class GalleryController : EntityBaseController<gallery, GalleryService>
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
        [HttpGet("random_image")]
        public gallery RandomImage()
        {
            return new GalleryService().GetRandomImage();
        }
    }
}
