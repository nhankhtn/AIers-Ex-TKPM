using AutoMapper;
using Azure;
using StudentManagement.BLL.DTOs.ClassStudent;
using StudentManagement.BLL.DTOs.Students;
using StudentManagement.DAL.Data.Repositories.ClassRepo;
using StudentManagement.DAL.Data.Repositories.ClassStudentRepo;
using StudentManagement.DAL.Data.Repositories.RegisterCancellationHistoryRepo;
using StudentManagement.DAL.Data.Repositories.StudentRepo;
using StudentManagement.Domain.Models;
using StudentManagement.Domain.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentManagement.BLL.Services.ClassStudentService
{
    public class ClassStudentService : IClassStudentService
    {
        private readonly IClassStudentRepository _classStudentRepository;
        private readonly IClassRepository _classRepository;
        private readonly IStudentRepository _studentRepository;
        private readonly IRegisterCancellationHistoryRepository _registerCancellationHistoryRepository;
        private readonly IMapper _mapper;

        public ClassStudentService(
            IClassStudentRepository classStudentRepository, 
            IStudentRepository studentRepository, 
            IClassRepository classRepository, 
            IRegisterCancellationHistoryRepository registerCancellationHistoryRepository,
            IMapper mapper)
        {
            _classStudentRepository = classStudentRepository ?? throw new ArgumentNullException(nameof(classStudentRepository));
            _studentRepository = studentRepository ?? throw new ArgumentNullException(nameof(studentRepository));
            _classRepository = classRepository ?? throw new ArgumentNullException(nameof(classRepository));
            _registerCancellationHistoryRepository = registerCancellationHistoryRepository ?? throw new ArgumentNullException(nameof(registerCancellationHistoryRepository));
            _mapper = mapper;
        }

        public async Task<Result<AddStudentToClassDTO>> AddStudentAsync(AddStudentToClassDTO addStudentToClassDTO)
        {
            try
            {
                await _classStudentRepository.AddClassStudentAsync(_mapper.Map<ClassStudent>(addStudentToClassDTO));
                return Result<AddStudentToClassDTO>.Ok(addStudentToClassDTO);
            }
            catch(Exception)
            {
                return Result<AddStudentToClassDTO>.Fail("ADD_STUDENT_TO_CLASS_FAILED");
            }
        }

        public async Task<Result<RegisterCancelationDTO>> RegisterCancelationAsync(RegisterCancelationDTO registerCancelationDTO)
        {
            try
            {
                await _classStudentRepository.DeleteClassStudentAsync(registerCancelationDTO.ClassId, registerCancelationDTO.StudentId);
                var registerCancellationHistory = _mapper.Map<RegisterCancellationHistory>(registerCancelationDTO);
                registerCancellationHistory.Time = DateTime.Now;
                await _registerCancellationHistoryRepository.AddAsync(registerCancellationHistory);
                return Result<RegisterCancelationDTO>.Ok(registerCancelationDTO);
            }
            catch (Exception)
            {
                return Result<RegisterCancelationDTO>.Fail("REGISTER_CANCELATION_FAILED");
            }
        }

        public async Task<Result<GetClassStudentsDTO>> GetClassStudentsAsync(int? classId = null, string? studentId = null, int? page = null, int? limit = null)
        {
            try
            {
                var res = await _classStudentRepository.GetClassStudentsAsync(classId, studentId, page, limit);
                return Result<GetClassStudentsDTO>.Ok(new GetClassStudentsDTO()
                {
                    Data = _mapper.Map<IEnumerable<GetClassStudentDTO>>(res),
                    Total = res.Count()
                });
            }
            catch (Exception)
            {
                return Result<GetClassStudentsDTO>.Fail("GET_ALL_CLASS_STUDENTS_FAILED");
            }
        }

        public async Task<Result<GetClassStudentDTO>> UpdateClassStudentAsync(int classId, string studentId, UpdateClassStudentDTO updateClassStudentDTO)
        {
            try
            {
                var classStudent = await _classStudentRepository.GetClassStudentByIdAsync(classId, studentId);
                if (classStudent is null)
                    return Result<GetClassStudentDTO>.Fail("CLASS_STUDENT_NOT_FOUND");
                _mapper.Map(updateClassStudentDTO, classStudent);
                await _classStudentRepository.UpdateClassStudentAsync(classStudent);
                return Result<GetClassStudentDTO>.Ok(_mapper.Map<GetClassStudentDTO>(classStudent));
            }
            catch (Exception)
            {
                return Result<GetClassStudentDTO>.Fail("UPDATE_CLASS_STUDENTS_FAILED");
            }
        }
    }
}
