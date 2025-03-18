using AutoMapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using StudentManagement.Application.DTOs;
using StudentManagement.BLL.DTOs;
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
            CreateMap<Student, StudentDTO>();

            CreateMap<Faculty, FacultyDTO>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id.ToString()));

            CreateMap<StudentStatus, StudentStatusDTO>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id.ToString()));

            CreateMap<Program, ProgramDTO>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id.ToString()));

            CreateMap<FacultyDTO, Faculty>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => ParseGuidSafely(src.Id)));

            CreateMap<StudentStatusDTO, StudentStatus>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => ParseGuidSafely(src.Id)));


            CreateMap<ProgramDTO, Program>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => ParseGuidSafely(src.Id)));



            CreateMap<AddStudentDTO, StudentDTO>();
            CreateMap<StudentDTO, AddStudentDTO>();

            CreateMap<UpdateStudentDTO, StudentDTO>();
            CreateMap<StudentDTO, UpdateStudentDTO>();

            // 
            CreateMap<Address, AddressDTO>();
            //CreateMap<StudentNationalities, StudentNationalitesDTO>();
            CreateMap<Identity, IdentityDTO>();
            CreateMap<AddressDTO, Address>();
            CreateMap<IdentityDTO, Identity>();
            //CreateMap<StudentNationalitesDTO, StudentNationalities>();



            // 
            CreateMap<AddStudentDTO, Student>();
            CreateMap<UpdateStudentDTO, Student>();
        }

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
