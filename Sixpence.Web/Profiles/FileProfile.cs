using AutoMapper;
using Sixpence.Web.Module.DataService;
using Sixpence.Web.Pixabay;
using Sixpence.Web.Store;
using Sixpence.Web.Store.SysFile;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sixpence.Web.Profiles
{
    public class FileProfile : Profile, IProfile
    {
        public FileProfile()
        {
            CreateMap<sys_file, FileInfoModel>()
                .ForMember(dest => dest.id, opt => opt.MapFrom(e => e.id))
                .ForMember(dest => dest.name, opt => opt.MapFrom(e => e.name))
                .ForMember(dest => dest.downloadUrl, opt => opt.MapFrom(e => e.DownloadUrl));
        }
    }
}
