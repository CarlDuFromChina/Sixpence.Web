using AutoMapper;
using Sixpence.Web.Entity;
using Sixpence.Web.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sixpence.Web.Profiles
{
    public class FileProfile : Profile, IProfile
    {
        public FileProfile()
        {
            CreateMap<SysFile, FileInfoModel>()
                .ForMember(dest => dest.id, opt => opt.MapFrom(e => e.id))
                .ForMember(dest => dest.name, opt => opt.MapFrom(e => e.name))
                .ForMember(dest => dest.downloadUrl, opt => opt.MapFrom(e => e.DownloadUrl));
        }
    }
}
