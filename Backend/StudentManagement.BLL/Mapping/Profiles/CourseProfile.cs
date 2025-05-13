using AutoMapper;
using StudentManagement.BLL.DTOs.Course;
using StudentManagement.BLL.DTOs.Localize;
using StudentManagement.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentManagement.BLL.Mapping.Profiles
{
    public class CourseProfile : Profile
    {
        public CourseProfile()
        {
            CreateMap<AddCourseDTO, Course>()
                .ForMember(dest => dest.CourseName, opt => opt.MapFrom(src => src.CourseName.Vi))
                .ForMember(dest => dest.CourseNameEng, opt => opt.MapFrom(src => src.CourseName.En))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description.Vi))
                .ForMember(dest => dest.DescriptionEng, opt => opt.MapFrom(src => src.Description.En))
                .ForAllMembers(opt => opt.Condition((src, dest, srcMember) =>
                    srcMember != null &&
                    (!(srcMember is DateTime) || !((DateTime)srcMember).Equals(default)) &&
                    (!(srcMember is int) || !((int)srcMember).Equals(default)) &&
                    (!(srcMember is bool) || !((bool)srcMember).Equals(default)) &&
                    (!(srcMember is Guid) || !((Guid)srcMember).Equals(default)) &&
                    (!(srcMember is string) || !string.IsNullOrEmpty((string)srcMember)) &&
                    (!(srcMember is decimal) || !((decimal)srcMember).Equals(default))
                ));

            CreateMap<Course, AddCourseDTO>()
                .ForMember(dest => dest.CourseName, opt => opt.MapFrom(src => new LocalizedName
                {
                    En = src.CourseNameEng,
                    Vi = src.CourseName
                }))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => new LocalizedName
                {
                    Vi = src.Description,
                    En = src.DescriptionEng
                }));

            CreateMap<UpdateCourseDTO, Course>()
                .ForMember(dest => dest.CourseName, opt => opt.MapFrom(src => src.CourseName.Vi))
                .ForMember(dest => dest.CourseNameEng, opt => opt.MapFrom(src => src.CourseName.En))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description.Vi))
                .ForMember(dest => dest.DescriptionEng, opt => opt.MapFrom(src => src.Description.En))
                .ForAllMembers(opt => opt.Condition((src, dest, srcMember) =>
                    srcMember != null &&
                    (!(srcMember is DateTime) || !((DateTime)srcMember).Equals(default)) &&
                    (!(srcMember is int) || !((int)srcMember).Equals(default)) &&
                    (!(srcMember is bool) || !((bool)srcMember).Equals(default)) &&
                    (!(srcMember is Guid) || !((Guid)srcMember).Equals(default)) &&
                    (!(srcMember is string) || !string.IsNullOrEmpty((string)srcMember)) &&
                    (!(srcMember is decimal) || !((decimal)srcMember).Equals(default))
                ));

            CreateMap<Course, UpdateCourseDTO>()
                .ForMember(dest => dest.CourseName, opt => opt.MapFrom(src => new LocalizedName
                {
                    En = src.CourseNameEng,
                    Vi = src.CourseName
                }))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => new LocalizedName
                {
                    Vi = src.Description,
                    En = src.DescriptionEng
                }));

            CreateMap<Course, GetCourseDTO>()
                .ForMember(dest => dest.FacultyName, act => act.MapFrom(src => src.Faculty.Name))
                .ForMember(dest => dest.RequiredCourseName, act => act.MapFrom(src => src.RequiredCourse!.CourseName))
                .ForMember(dest => dest.CourseName, opt => opt.MapFrom(src => new LocalizedName
                {
                    En = src.CourseNameEng,
                    Vi = src.CourseName
                }))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => new LocalizedName
                {
                    En = src.DescriptionEng,
                    Vi = src.Description
                }));
        }
    }
}
