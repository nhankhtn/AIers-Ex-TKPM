import { CustomTableConfig } from "@/components/custom-table";
import { Class } from "@/types/class";
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
