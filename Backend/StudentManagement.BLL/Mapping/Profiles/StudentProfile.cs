using AutoMapper;
using StudentManagement.BLL.DTOs.Students;
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
    public class StudentProfile : Profile
    {
        public StudentProfile()
        {
            CreateMap<Student, StudentDTO>()
                .ForMember(dest => dest.Faculty, opt => opt.MapFrom(src => src.FacultyId))
                .ForMember(dest => dest.Program, opt => opt.MapFrom(src => src.ProgramId))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.StatusId))
                .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => src.Gender.ToString()))
                .ForMember(dest => dest.Identity, opt => opt.MapFrom(src => src.Identity));


            CreateMap<StudentDTO, Student>()
                .ForMember(dest => dest.Faculty, opt => opt.Ignore())
                .ForMember(dest => dest.Program, opt => opt.Ignore())
                .ForMember(dest => dest.Status, opt => opt.Ignore())
                .ForMember(dest => dest.FacultyId, opt =>
                {
                    opt.PreCondition(src => src.Faculty != null);
                    opt.MapFrom(src => src.Faculty.ToGuid());
                })
                .ForMember(dest => dest.ProgramId, opt =>
                {
                    opt.PreCondition(src => src.Program != null);
                    opt.MapFrom(src => src.Program.ToGuid());
                })
                .ForMember(dest => dest.StatusId, opt =>
                {
                    opt.PreCondition(src => src.Status != null);
                    opt.MapFrom(src => src.Status.ToGuid());
                })
                .ForMember(dest => dest.Gender, opt =>
                {
                    opt.PreCondition(src => src.Gender != null);
                    opt.MapFrom(src => src.Gender.ToEnum<Gender>());
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
