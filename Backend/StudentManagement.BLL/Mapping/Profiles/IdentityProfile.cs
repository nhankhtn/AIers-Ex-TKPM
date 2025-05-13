using AutoMapper;
using StudentManagement.BLL.DTOs.Identity;
using StudentManagement.Domain.Enums;
using StudentManagement.Domain.Models;
using StudentManagement.Domain.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentManagement.BLL.Mapping.Profiles
{
    public class IdentityProfile : Profile
    {
        public IdentityProfile()
        {
            CreateMap<Identity, IdentityDTO>()
                .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.Type.ToString()))
                .ForMember(dest => dest.CountryIssue, opt => opt.MapFrom(src => src.Country));

            CreateMap<IdentityDTO, Identity>()
                .ForMember(dest => dest.Type, opt =>
                {
                    opt.PreCondition(src => src.Type != null);
                    opt.MapFrom(src => src.Type.ToEnum<IdentityType>());
                })
                .ForMember(dest => dest.Country, opt =>
                {
                    opt.PreCondition(src => src.CountryIssue != null);
                    opt.MapFrom(src => src.CountryIssue);
                })
                .ForAllMembers(opt => opt.Condition((src, dest, srcMember) =>
                    srcMember != null &&
                    (!(srcMember is DateTime) || !((DateTime)srcMember).Equals(default)) &&
                    (!(srcMember is int) || !((int)srcMember).Equals(default)) &&
                    (!(srcMember is bool) || !((bool)srcMember).Equals(default))
                ));


        }
    }
}
