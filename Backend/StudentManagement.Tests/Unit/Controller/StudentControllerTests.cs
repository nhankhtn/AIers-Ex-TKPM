using Microsoft.AspNetCore.Mvc;
using Moq;
using StudentManagement.API.Controllers;
using StudentManagement.BLL.DTOs.Students;
using StudentManagement.Domain.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentManagement.Tests.Unit.Controller
{
    public class StudentControllerTests
    {
        //private readonly Mock<IStudentService> _studentServiceMock;
        //private readonly StudentsController _studentsController;

        //public StudentControllerTests()
        //{
        //    _studentServiceMock = new Mock<IStudentService>();
        //    _studentsController = new StudentsController(_studentServiceMock.Object);
        //}

        //[Fact]
        //public async Task GetAllStudents_StudentsExist_ReturnOkResult()
        //{
        //    // Arrange
        //    int page = 1;
        //    int limit = 10;
        //    var students = new List<StudentDTO>
        //    {
        //        new StudentDTO { Id = "1", Name = "Student 1", DateOfBirth = new DateTime(2024, 1, 1),
        //                Email="student1@gmail.com", Faculty=2, Gender=1, Phone="0389277431", Program="chuong trinh chuan", Status=0}
        //    };
        //    var getStudentsDto = new GetStudentsDto
        //    {
        //        Students = students,
        //        PageIndex = 1,
        //        PageSize = 10,
        //        Total = 1
        //    };
        //    _studentServiceMock.Setup(x => x.GetAllStudentsAsync(page, limit, null)).ReturnsAsync(Result<GetStudentsDto>.Ok(getStudentsDto));
        //    // Act
        //    var result = await _studentsController.GetAllStudents(page, limit, null);

        //    // Assert
        //    Assert.IsType<OkObjectResult>(result.Result);
        //}
    }
}
