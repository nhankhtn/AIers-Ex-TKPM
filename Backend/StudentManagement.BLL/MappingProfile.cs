using AutoMapper;
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
            CreateMap<Faculty, FacultyDTO>();
            CreateMap<StudentStatus, StudentStatusDTO>();
            CreateMap<Program, ProgramDTO>();
        }
    }
}
