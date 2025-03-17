﻿using AutoMapper;
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
            CreateMap<Faculty, FacultyDTO>();
            CreateMap<StudentStatus, StudentStatusDTO>();
            CreateMap<Program, ProgramDTO>();

            CreateMap<FacultyDTO, Faculty>();
            CreateMap<StudentStatusDTO, StudentStatus>();
            CreateMap<ProgramDTO, Program>();

            CreateMap<AddStudentDTO, StudentDTO>();
            CreateMap<StudentDTO, AddStudentDTO>();
            CreateMap<UpdateStudentDTO, StudentDTO>();
            CreateMap<StudentDTO, UpdateStudentDTO>();

            CreateMap<Address, AddressDTO>();
            CreateMap<Identity, IdentityDTO>();

            CreateMap<AddStudentDTO, Student>();
            CreateMap<UpdateStudentDTO, Student>();


            CreateMap<AddressDTO, Address>();
            CreateMap<IdentityDTO, Identity>(); 
        }
    }
}
