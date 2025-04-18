import { CustomTableConfig } from "@/components/custom-table";
import { Class } from "@/types/class";
import { UnregisterClass } from "@/types/registration";
import { StudentClass } from "@/types/student";
import { Typography } from "@mui/material";

export const getClassesTableConfig = (): CustomTableConfig<string, Class>[] => [
  {
    key: "code",
    headerLabel: "Mã lớp",
    renderCell: (data) => (
      <Typography variant='body2'>{data.classId}</Typography>
    ),
  },
  {
    key: "name",
    headerLabel: "Khóa học",
    renderCell: (data) => (
      <Typography variant='body2'>{data.courseId}</Typography>
    ),
  },
  {
    key: "teacherName",
    headerLabel: "Giảng viên",
    renderCell: (data) => (
      <Typography variant='body2'>{data.teacherName}</Typography>
    ),
  },
  {
    key: "dayOfWeek",
    headerLabel: "Lịch học",
    renderCell: (data) => (
      <Typography variant='body2'>{data.dayOfWeek}</Typography>
    ),
  },
  {
    key: "time",
    headerLabel: "Sĩ số",
    renderCell: (data) => (
      <Typography variant='body2'>{data.maxStudents}</Typography>
    ),
  },
  {
    key: "room",
    headerLabel: "Phòng học",
    renderCell: (data) => <Typography variant='body2'>{data.room}</Typography>,
  },
];

export const getRegisteredClassesTableConfig = (): CustomTableConfig<
  string,
  StudentClass
>[] => [
  {
    key: "studentId",
    headerLabel: "Khóa học",
    renderCell: (data) => (
      <Typography variant='body2'>{data.studentId}</Typography>
    ),
  },
  {
    key: "studentName",
    headerLabel: "Khóa học",
    renderCell: (data) => (
      <Typography variant='body2'>{data.studentName}</Typography>
    ),
  },
  {
    key: "code",
    headerLabel: "Mã lớp",
    renderCell: (data) => (
      <Typography variant='body2'>{data.classId}</Typography>
    ),
  },
  {
    key: "name",
    headerLabel: "Khóa học",
    renderCell: (data) => (
      <Typography variant='body2'>{data.courseName}</Typography>
    ),
  },
];


export const getUnregisteredClassesTableConfig = (): CustomTableConfig<
  number,
  UnregisterClass
>[] => [
  {
    key: "classId",
    headerLabel: "Mã lớp",
    renderCell: (data) => <Typography variant='body2'>{data.classId}</Typography>,
  },
  {
    key: "courseName",
    headerLabel: "Tên môn học",
    renderCell: (data) => <Typography variant='body2'>{data.courseName}</Typography>,
  },
  {
    key: "semester",
    headerLabel: "Học kỳ",
    renderCell: (data) => <Typography variant='body2'>{data.semester}</Typography>,
  },
  {
    key: "academicYear",
    headerLabel: "Năm học",
    renderCell: (data) => <Typography variant='body2'>{data.academicYear}</Typography>,
  },
  {
    key: "time",
    headerLabel: "Thời gian yêu cầu",
    renderCell: (data) => <Typography variant='body2'>{data.time.split("T")[0]}</Typography>,
  },
];
  
