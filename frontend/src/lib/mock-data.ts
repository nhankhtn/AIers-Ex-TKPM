import { Course } from "@/types/course";

// Mock data for courses
export const mockCourses: Course[] = [
  {
    id: "1",
    courseId: "CS101",
    courseName: "Nhập môn lập trình",
    facultyId: "CNTT",
    facultyName: "Khoa Công nghệ thông tin",
    credits: 3,
    requiredCourseId: undefined,
    requiredCourseName: undefined,
    description: "Nhập môn lập trình",
  },
  {
    id: "2",
    courseId: "CS202",
    courseName: "Cấu trúc dữ liệu và giải thuật",
    facultyId: "CNTT",
    facultyName: "Khoa Công nghệ thông tin",
    credits: 4,
    requiredCourseId: "1",
    requiredCourseName: "Nhập môn lập trình",
    description: "Cấu trúc dữ liệu và giải thuật",
  },
  {
    id: "3",
    courseId: "CS203",
    courseName: "Cơ sở dữ liệu",
    facultyId: "CNTT",
    facultyName: "Khoa Công nghệ thông tin",
    credits: 3,
    requiredCourseId: "2",
    requiredCourseName: "Cấu trúc dữ liệu và giải thuật",
    description: "Cơ sở dữ liệu",
  },
  {
    id: "4",
    courseId: "CS301",
    courseName: "Trí tuệ nhân tạo",
    facultyId: "CNTT",
    facultyName: "Khoa Công nghệ thông tin",
    credits: 3,
    requiredCourseId: "2",
    requiredCourseName: "Cấu trúc dữ liệu và giải thuật",
    description: "Trí tuệ nhân tạo",
  },
  {
    id: "5",
    courseId: "MATH101",
    courseName: "Giải tích 1",
    facultyId: "MATH",
    facultyName: "Khoa Toán học",
    credits: 3,
    requiredCourseId: undefined,
    requiredCourseName: undefined,
    description: "Giải tích 1",
  },
  {
    id: "6",
    courseId: "MATH102",
    courseName: "Đại số tuyến tính",
    facultyId: "MATH",
    facultyName: "Khoa Toán học",
    credits: 3,
    requiredCourseId: "5",
    requiredCourseName: "Giải tích 1",
    description: "Đại số tuyến tính",
  },
  {
    id: "7",
    courseId: "PHYS101",
    courseName: {
      vi: "Vật lý đại cương",
      en: "General Physics",
    },
    facultyId: "PHYS",
    facultyName: {
      vi: "Khoa Vật lý",
      en: "Faculty of Physics",
    },
    credits: 4,
    description: {
      vi: "Vật lý đại cương",
      en: "General Physics",
    },
    requiredCourseId: undefined,
    requiredCourseName: undefined,
    deletedAt: undefined,
    createdAt: undefined,
  },
  {
    id: "8",
    courseId: "ECON101",
    courseName: {
      vi: "Kinh tế học đại cương",
      en: "Introduction to Economics",
    },
    facultyId: "ECON",
    facultyName: {
      vi: "Khoa Kinh tế",
      en: "Faculty of Economics",
    },
    credits: 3,
    description: {
      vi: "Kinh tế học đại cương",
      en: "Introduction to Economics",
    },
    requiredCourseId: undefined,
    requiredCourseName: undefined,
    deletedAt: undefined,
    createdAt: undefined,
  },
];

// Mock data for classes
export const mockClasses = [
  {
    id: "1",
    code: "CS101-01",
    courseId: 1,
    courseCode: "CS101",
    courseName: "Nhập môn lập trình",
    instructor: "TS. Nguyễn Văn A",
    year: "2023-2024",
    semester: 1,
    schedule: "Thứ 2, 4 (7:30-9:30)",
    room: "A1.203",
    maxStudents: 40,
    enrolledStudents: 38,
  },
  {
    id: "2",
    code: "CS101-02",
    courseId: 1,
    courseCode: "CS101",
    courseName: "Nhập môn lập trình",
    instructor: "TS. Trần Thị B",
    year: "2023-2024",
    semester: 1,
    schedule: "Thứ 3, 5 (13:30-15:30)",
    room: "A1.204",
    maxStudents: 40,
    enrolledStudents: 40,
  },
  {
    id: "3",
    code: "CS202-01",
    courseId: 2,
    courseCode: "CS202",
    courseName: "Cấu trúc dữ liệu và giải thuật",
    instructor: "TS. Lê Văn C",
    year: "2023-2024",
    semester: 1,
    schedule: "Thứ 2, 4 (9:30-11:30)",
    room: "A2.301",
    maxStudents: 35,
    enrolledStudents: 30,
  },
  {
    id: "4",
    code: "MATH101-01",
    courseId: 5,
    courseCode: "MATH101",
    courseName: "Giải tích 1",
    instructor: "TS. Phạm Thị D",
    year: "2023-2024",
    semester: 1,
    schedule: "Thứ 3, 5 (7:30-9:30)",
    room: "B1.101",
    maxStudents: 50,
    enrolledStudents: 45,
  },
  {
    id: "5",
    code: "MATH201-01",
    courseId: 6,
    courseCode: "MATH201",
    courseName: "Đại số tuyến tính",
    instructor: "TS. Hoàng Văn E",
    year: "2023-2024",
    semester: 1,
    schedule: "Thứ 6 (13:30-17:30)",
    room: "B1.102",
    maxStudents: 45,
    enrolledStudents: 40,
  },
  {
    id: "6",
    code: "CS303-01",
    courseId: 3,
    courseCode: "CS303",
    courseName: "Cơ sở dữ liệu",
    instructor: "TS. Nguyễn Văn A",
    year: "2023-2024",
    semester: 2,
    schedule: "Thứ 2, 4 (13:30-15:30)",
    room: "A2.302",
    maxStudents: 35,
    enrolledStudents: 0,
  },
];

// Mock data for students
export const mockStudents = [
  {
    id: "1",
    code: "SV001",
    name: "Nguyễn Văn A",
    department: "Công nghệ thông tin",
    batch: "K62",
  },
  {
    id: "2",
    code: "SV002",
    name: "Trần Thị B",
    department: "Công nghệ thông tin",
    batch: "K62",
  },
  {
    id: "3",
    code: "SV003",
    name: "Lê Văn C",
    department: "Công nghệ thông tin",
    batch: "K62",
  },
  {
    id: "4",
    code: "SV004",
    name: "Phạm Thị D",
    department: "Toán học",
    batch: "K62",
  },
  {
    id: "5",
    code: "SV005",
    name: "Hoàng Văn E",
    department: "Vật lý",
    batch: "K63",
  },
];

// Mock data for registrations
export const mockRegistrations = [
  {
    id: "1",
    studentId: "1",
    classId: "1",
    registrationDate: "2023-08-15T10:30:00",
    status: "active",
  },
  {
    id: "2",
    studentId: "1",
    classId: "4",
    registrationDate: "2023-08-15T10:35:00",
    status: "active",
  },
  {
    id: "3",
    studentId: "2",
    classId: "1",
    registrationDate: "2023-08-16T09:20:00",
    status: "active",
  },
  {
    id: "4",
    studentId: "2",
    classId: "3",
    registrationDate: "2023-08-16T09:25:00",
    status: "active",
  },
  {
    id: "5",
    studentId: "3",
    classId: "2",
    registrationDate: "2023-08-17T14:10:00",
    status: "cancelled",
  },
  {
    id: "6",
    studentId: "3",
    classId: "5",
    registrationDate: "2023-08-17T14:15:00",
    status: "active",
  },
  {
    id: "7",
    studentId: "4",
    classId: "4",
    registrationDate: "2023-08-18T11:05:00",
    status: "active",
  },
  {
    id: "8",
    studentId: "5",
    classId: "4",
    registrationDate: "2023-08-19T10:30:00",
    status: "active",
  },
];

// Mock data for transcripts
export const mockTranscriptData = {
  1: {
    courses: [
      {
        code: "CS101",
        name: "Nhập môn lập trình",
        credits: 3,
        score: 8.5,
        grade: "A",
      },
      {
        code: "MATH101",
        name: "Giải tích 1",
        credits: 3,
        score: 7.0,
        grade: "B",
      },
      {
        code: "PHYS101",
        name: "Vật lý đại cương",
        credits: 4,
        score: 6.5,
        grade: "C+",
      },
    ],
    gpa: 7.35,
    totalCredits: 10,
    completedCredits: 10,
  },
  2: {
    courses: [
      {
        code: "CS101",
        name: "Nhập môn lập trình",
        credits: 3,
        score: 9.0,
        grade: "A+",
      },
      {
        code: "CS202",
        name: "Cấu trúc dữ liệu và giải thuật",
        credits: 4,
        score: 8.0,
        grade: "A",
      },
      {
        code: "MATH101",
        name: "Giải tích 1",
        credits: 3,
        score: 7.5,
        grade: "B+",
      },
    ],
    gpa: 8.2,
    totalCredits: 10,
    completedCredits: 10,
  },
  3: {
    courses: [
      {
        code: "CS101",
        name: "Nhập môn lập trình",
        credits: 3,
        score: 7.0,
        grade: "B",
      },
      {
        code: "MATH201",
        name: "Đại số tuyến tính",
        credits: 3,
        score: 6.0,
        grade: "C",
      },
    ],
    gpa: 6.5,
    totalCredits: 6,
    completedCredits: 6,
  },
  4: {
    courses: [
      {
        code: "MATH101",
        name: "Giải tích 1",
        credits: 3,
        score: 9.5,
        grade: "A+",
      },
      {
        code: "MATH201",
        name: "Đại số tuyến tính",
        credits: 3,
        score: 9.0,
        grade: "A+",
      },
    ],
    gpa: 9.25,
    totalCredits: 6,
    completedCredits: 6,
  },
  5: {
    courses: [
      {
        code: "PHYS101",
        name: "Vật lý đại cương",
        credits: 4,
        score: 8.5,
        grade: "A",
      },
      {
        code: "MATH101",
        name: "Giải tích 1",
        credits: 3,
        score: 7.0,
        grade: "B",
      },
    ],
    gpa: 7.86,
    totalCredits: 7,
    completedCredits: 7,
  },
};
