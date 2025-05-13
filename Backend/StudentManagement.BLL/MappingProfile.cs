using AutoMapper;
using AutoMapper.Configuration.Conventions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.IdentityModel.Tokens;
using StudentManagement.BLL.DTOs.Class;
using StudentManagement.BLL.DTOs.ClassStudent;
using StudentManagement.BLL.DTOs.Course;
using StudentManagement.BLL.DTOs.Faculty;
using StudentManagement.BLL.DTOs.Identity;
using StudentManagement.BLL.DTOs.Localize;
using StudentManagement.BLL.DTOs.Program;
using StudentManagement.BLL.DTOs.Score;
using StudentManagement.BLL.DTOs.Students;
using StudentManagement.BLL.DTOs.StudentStatus;
using StudentManagement.Domain.Enums;
using StudentManagement.Domain.Models;
using StudentManagement.Domain.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
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
                .ForMember(dest => dest.FacultyId, opt => {
                    opt.PreCondition(src => src.Faculty != null);
                    opt.MapFrom(src => src.Faculty.ToGuid());
                })
                .ForMember(dest => dest.ProgramId, opt => {
                    opt.PreCondition(src => src.Program != null);
                    opt.MapFrom(src => src.Program.ToGuid());
                })
                .ForMember(dest => dest.StatusId, opt => {
                    opt.PreCondition(src => src.Status != null);
                    opt.MapFrom(src => src.Status.ToGuid());
                })
                .ForMember(dest => dest.Gender, opt => {
                    opt.PreCondition(src => src.Gender != null);
                    opt.MapFrom(src => src.Gender.ToEnum<Gender>());
                })
                .ForAllMembers(opt => opt.Condition((src, dest, srcMember) =>
                    srcMember != null &&
                    (!(srcMember is DateTime) || !((DateTime)srcMember).Equals(default(DateTime))) &&
                    (!(srcMember is int) || !((int)srcMember).Equals(default(int))) &&
                    (!(srcMember is bool) || !((bool)srcMember).Equals(default(bool)))
                ));



            // Faculty
            CreateMap<Faculty, FacultyDTO>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id.ToString()));
            CreateMap<FacultyDTO, Faculty>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id.ToGuid()));


            // StudentStatus
            CreateMap<StudentStatus, StudentStatusDTO>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id.ToString()))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => new LocalizedName
                {
                    Vi = src.Name,
                    En = src.NameEng
                }));
            CreateMap<StudentStatusDTO, StudentStatus>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name.Vi))
                .ForMember(dest => dest.NameEng, opt => opt.MapFrom(src => src.Name.En))
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id.ToGuid()));


            // Program
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
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id.ToGuid()));



            // Identity
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
                    (!(srcMember is DateTime) || !((DateTime)srcMember).Equals(default(DateTime))) &&
                    (!(srcMember is int) || !((int)srcMember).Equals(default(int))) &&
                    (!(srcMember is bool) || !((bool)srcMember).Equals(default(bool)))
                ));


            //Course
            CreateMap<AddCourseDTO, Course>();
            CreateMap<Course, AddCourseDTO>();

            CreateMap<UpdateCourseDTO, Course>();
            CreateMap<Course, UpdateCourseDTO>();

            CreateMap<Course, GetCourseDTO>()
                .ForMember(dest => dest.FacultyName, act => act.MapFrom(src => src.Faculty.Name))
                .ForMember(dest => dest.RequiredCourseName, act => act.MapFrom(src => src.RequiredCourse!.CourseName))
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            // Class
            CreateMap<AddClassDTO, Class>();
            CreateMap<Class, AddClassDTO>();

            CreateMap<Class, GetClassDTO>()
                .ForMember(dest => dest.CourseName, act => act.MapFrom(src => src.Course.CourseName));

            CreateMap<UpdateClassDTO, Class>()
                .ForAllMembers(opt => opt.Condition((src, dest, srcMember) =>
                    srcMember != null &&
                    (!(srcMember is DateTime) || !((DateTime)srcMember).Equals(default(DateTime))) &&
                    (!(srcMember is int) || !((int)srcMember).Equals(default(int))) &&
                    (!(srcMember is bool) || !((bool)srcMember).Equals(default(bool))) &&
                    (!(srcMember is Guid) || !((Guid)srcMember).Equals(default(Guid))) &&
                    (!(srcMember is string) || !string.IsNullOrEmpty((string)srcMember)) &&
                    (!(srcMember is decimal) || !((decimal)srcMember).Equals(default(decimal)))
                ));

            // ClassStudent
            CreateMap<AddStudentToClassDTO,  ClassStudent>();

            CreateMap<ClassStudent, GetClassStudentDTO>();

            CreateMap<UpdateScoreDTO, ClassStudent>()
                .ForAllMembers(opt => opt.Condition((src, dest, srcMember) =>
                    srcMember != null &&
                    (!(srcMember is DateTime) || !((DateTime)srcMember).Equals(default(DateTime))) &&
                    (!(srcMember is int) || !((int)srcMember).Equals(default(int))) &&
                    (!(srcMember is bool) || !((bool)srcMember).Equals(default(bool))) &&
                    (!(srcMember is Guid) || !((Guid)srcMember).Equals(default(Guid))) &&
                    (!(srcMember is string) || !string.IsNullOrEmpty((string)srcMember)) &&
                    (!(srcMember is decimal) || !((decimal)srcMember).Equals(default(decimal)))
                ));

            CreateMap<ClassStudent, GetScoreDTO>()
                .ForMember(dest => dest.StudentName, act => act.MapFrom(src => src.Student.Name));

            CreateMap<RegisterCancelationDTO, RegisterCancellationHistory>();
            CreateMap<RegisterCancellationHistory, RegisterCancelationHistoryRow>();

        }
    }
}
