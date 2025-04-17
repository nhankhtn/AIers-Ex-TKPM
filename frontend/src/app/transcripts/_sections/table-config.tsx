import { CustomTableConfig } from "@/components/custom-table";
import { StudentTranscript } from "@/types/student";
import { Typography } from "@mui/material";

export const getTranscriptTableConfig = (): CustomTableConfig<
  string,
  StudentTranscript
>[] => [
  {
    key: "courseId",
    headerLabel: "Mã học phần",
    renderCell: (data) => (
      <Typography variant='body2'>{data.classId}</Typography>
    ),
  },
  {
    key: "courseName",
    headerLabel: "Tên học phần",
    renderCell: (data) => (
      <Typography variant='body2'>{data.courseName}</Typography>
    ),
  },
  {
    key: "credits",
    headerLabel: "Số tín chỉ",
    renderCell: (data) => (
      <Typography variant='body2'>{data.credit}</Typography>
    ),
  },
  {
    key: "grade",
    headerLabel: "Điểm",
    renderCell: (data) => (
      <Typography variant='body2'>{data.totalScore}</Typography>
    ),
  },
  {
    key: "gradeLetter",
    headerLabel: "Điểm chữ",
    renderCell: (data) => <Typography variant='body2'>{data.grade}</Typography>,
  },
];
