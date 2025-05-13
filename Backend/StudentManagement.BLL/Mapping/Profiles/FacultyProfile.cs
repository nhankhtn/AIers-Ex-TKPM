using AutoMapper;
using StudentManagement.BLL.DTOs.Faculty;
using StudentManagement.BLL.DTOs.Localize;
using StudentManagement.Domain.Models;
using StudentManagement.Domain.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentManagement.BLL.Mapping.Profiles
{
    public class FacultyProfile : Profile
    {
        public FacultyProfile()
        {
            CreateMap<Faculty, FacultyDTO>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id.ToString()))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => new LocalizedName
                {
                    En = src.NameEng,
                    Vi = src.Name
                }));

            CreateMap<FacultyDTO, Faculty>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id.ToGuid()))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name.Vi))
                .ForMember(dest => dest.NameEng, opt => opt.MapFrom(src => src.Name.En))
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }

    }
}
