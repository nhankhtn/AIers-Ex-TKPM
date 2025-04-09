using AutoMapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using StudentManagement.BLL.DTOs.Course;
using StudentManagement.BLL.DTOs.Faculty;
using StudentManagement.BLL.DTOs.Identity;
using StudentManagement.BLL.DTOs.Program;
using StudentManagement.BLL.DTOs.Students;
using StudentManagement.BLL.DTOs.StudentStatus;
using StudentManagement.Domain.Enums;
using StudentManagement.Domain.Models;
using StudentManagement.Domain.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentManagement.BLL
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Student
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
                .ForMember(dest => dest.FacultyId, opt => opt.MapFrom(src => src.Faculty.ToGuid()))
                .ForMember(dest => dest.ProgramId, opt => opt.MapFrom(src => src.Program.ToGuid()))
                .ForMember(dest => dest.StatusId, opt => opt.MapFrom(src => src.Status.ToGuid()))
                .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => src.Gender.ToEnum<Gender>()))
                .ForMember(dest => dest.Identity, opt => opt.MapFrom(src => src.Identity));


            // Faculty
            CreateMap<Faculty, FacultyDTO>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id.ToString()));
            CreateMap<FacultyDTO, Faculty>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id.ToGuid()));


            // StudentStatus
            CreateMap<StudentStatus, StudentStatusDTO>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id.ToString()));
            CreateMap<StudentStatusDTO, StudentStatus>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id.ToGuid()));


            // Program
            CreateMap<Program, ProgramDTO>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id.ToString()));
            CreateMap<ProgramDTO, Program>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id.ToGuid()));


            // Identity
            CreateMap<Identity, IdentityDTO>()
                .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.Type.ToString()))
                .ForMember(dest => dest.CountryIssue, opt => opt.MapFrom(src => src.Country));
            CreateMap<IdentityDTO, Identity>()
                .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.Type.ToEnum<IdentityType>()))
                .ForMember(dest => dest.Country, opt => opt.MapFrom(src => src.CountryIssue))
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            //Course
            CreateMap<AddCourseDTO, Course>();
            CreateMap<Course, AddCourseDTO>();

            CreateMap<UpdateCourseDTO, Course>();
            CreateMap<Course, UpdateCourseDTO>();

            CreateMap<Course, GetCourseDTO>()
                .ForMember(dest => dest.FacultyName, act => act.MapFrom(src => src.Faculty.Name))
                .ForMember(dest => dest.RequiredCourseName, act => act.MapFrom(src => src.RequiredCourse!.CourseName))
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }
}
