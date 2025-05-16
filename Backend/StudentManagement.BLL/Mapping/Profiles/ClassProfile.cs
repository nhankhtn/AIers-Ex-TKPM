using AutoMapper;
using StudentManagement.BLL.DTOs.Class;
using StudentManagement.BLL.DTOs.ClassStudent;
using StudentManagement.BLL.DTOs.Localize;
using StudentManagement.BLL.DTOs.Score;
using StudentManagement.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentManagement.BLL.Mapping.Profiles
{
    public class ClassProfile : Profile
    {
        public ClassProfile()
        {
            // Class
            CreateMap<AddClassDTO, Class>();
            CreateMap<Class, AddClassDTO>();

            CreateMap<Class, GetClassDTO>()
                .ForMember(dest => dest.CourseName, act => act.MapFrom(src => new LocalizedName
                {
                    Vi = src.Course.CourseName,
                    En = src.Course.CourseNameEng
                }));

            CreateMap<UpdateClassDTO, Class>()
                .ForAllMembers(opt => opt.Condition((src, dest, srcMember) =>
                    srcMember != null &&
                    (!(srcMember is DateTime) || !((DateTime)srcMember).Equals(default)) &&
                    (!(srcMember is int) || !((int)srcMember).Equals(default)) &&
                    (!(srcMember is bool) || !((bool)srcMember).Equals(default)) &&
                    (!(srcMember is Guid) || !((Guid)srcMember).Equals(default)) &&
                    (!(srcMember is string) || !string.IsNullOrEmpty((string)srcMember)) &&
                    (!(srcMember is decimal) || !((decimal)srcMember).Equals(default))
                ));

            // ClassStudent
            CreateMap<AddStudentToClassDTO, ClassStudent>();

            CreateMap<ClassStudent, GetClassStudentDTO>();

            CreateMap<UpdateScoreDTO, ClassStudent>()
                .ForAllMembers(opt => opt.Condition((src, dest, srcMember) =>
                    srcMember != null &&
                    (!(srcMember is DateTime) || !((DateTime)srcMember).Equals(default)) &&
                    (!(srcMember is int) || !((int)srcMember).Equals(default)) &&
                    (!(srcMember is bool) || !((bool)srcMember).Equals(default)) &&
                    (!(srcMember is Guid) || !((Guid)srcMember).Equals(default)) &&
                    (!(srcMember is string) || !string.IsNullOrEmpty((string)srcMember)) &&
                    (!(srcMember is decimal) || !((decimal)srcMember).Equals(default))
                ));

            CreateMap<ClassStudent, GetScoreDTO>()
                .ForMember(dest => dest.StudentName, act => act.MapFrom(src => src.Student.Name));

            CreateMap<RegisterCancelationDTO, RegisterCancellationHistory>();
            CreateMap<RegisterCancellationHistory, RegisterCancelationHistoryRow>();

        }
    }
}
