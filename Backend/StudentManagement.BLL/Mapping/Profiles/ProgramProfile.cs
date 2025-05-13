using AutoMapper;
using StudentManagement.BLL.DTOs.Localize;
using StudentManagement.BLL.DTOs.Program;
using StudentManagement.Domain.Models;
using StudentManagement.Domain.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentManagement.BLL.Mapping.Profiles
{
    public class ProgramProfile : Profile
    {
        public ProgramProfile()
        {
            CreateMap<Program, ProgramDTO>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id.ToString()))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => new LocalizedName
                {
                    Vi = src.Name,
                    En = src.NameEng
                }));

            CreateMap<ProgramDTO, Program>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name.Vi))
                .ForMember(dest => dest.NameEng, opt => opt.MapFrom(src => src.Name.En))
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id.ToGuid()))
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }
}
