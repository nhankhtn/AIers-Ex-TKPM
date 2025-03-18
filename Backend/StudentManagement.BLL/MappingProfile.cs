using AutoMapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using StudentManagement.BLL.DTOs.Faculty;
using StudentManagement.BLL.DTOs.Identity;
using StudentManagement.BLL.DTOs.Program;
using StudentManagement.BLL.DTOs.Students;
using StudentManagement.BLL.DTOs.StudentStatus;
using StudentManagement.Domain.Enums;
using StudentManagement.Domain.Models;
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
                .ForMember(dest => dest.Faculty, opt => opt.MapFrom(src => src.Faculty.Name))
                .ForMember(dest => dest.Program, opt => opt.MapFrom(src => src.Program.Name))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.Name));

            CreateMap<StudentDTO, Student>()
                .ForMember(dest => dest.Faculty, opt => opt.Ignore())
                .ForMember(dest => dest.Program, opt => opt.Ignore())
                .ForMember(dest => dest.Status, opt => opt.Ignore()); ;


            // Faculty
            CreateMap<Faculty, FacultyDTO>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id.ToString()));
            CreateMap<FacultyDTO, Faculty>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => ParseGuidSafely(src.Id)));


            // StudentStatus
            CreateMap<StudentStatus, StudentStatusDTO>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id.ToString()));
            CreateMap<StudentStatusDTO, StudentStatus>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => ParseGuidSafely(src.Id)));


            // Program
            CreateMap<Program, ProgramDTO>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id.ToString()));
            CreateMap<ProgramDTO, Program>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => ParseGuidSafely(src.Id)));


            // Identity
            CreateMap<Identity, IdentityDTO>();
            CreateMap<IdentityDTO, Identity>();
        }

        /// <summary>
        /// Parse string to Guid safely
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        private Guid ParseGuidSafely(string? input)
        {
            if (string.IsNullOrEmpty(input))
                return Guid.Empty; // or Guid.NewGuid()

            if (Guid.TryParse(input, out Guid result))
                return result;

            return Guid.Empty; // or Guid.NewGuid()
        }
    }
}
