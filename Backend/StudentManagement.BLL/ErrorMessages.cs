using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentManagement.BLL
{
    public static class ErrorMessages
    {
        public const string ClassNotFound = "Không tìm thấy lớp học.";
        public const string ClassAlreadyExists = "Lớp học đã tồn tại.";
        public const string StudentNotFound = "Không tìm thấy sinh viên.";
        public const string StudentAlreadyExists = "Sinh viên đã tồn tại.";
        public const string DuplicateClassStudent = "Sinh viên đã đăng ký lớp học này.";
        public const string RegisterFailed = "Đăng ký lớp học không thành công.";
        public const string RegisterCancellationFailed = "Hủy đăng ký lớp học không thành công.";
        public const string CourseNotFound = "Khóa học không tồn tại.";
        public const string CourseAlreadyExist = "Khóa học đã tồn tại.";
    }
}
