using AutoMapper;
using Sixpence.Common.Current;
using System;
using System.Collections.Generic;
using System.Text;
using Sixpence.Web.Entity;

namespace Sixpence.Web.Profiles
{
    public class UserInfoProfile : Profile, IProfile
    {
        public UserInfoProfile()
        {
            CreateMap<UserInfo, CurrentUserModel>()
                .ForMember(dest => dest.Code, opt => opt.MapFrom(e => e.code))
                .ForMember(dest => dest.Id, opt => opt.MapFrom(e => e.id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(e => e.name));
        }
    }
}
