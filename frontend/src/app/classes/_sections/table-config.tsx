import { Box, Typography } from "@mui/material";
import type { Class } from "@/types/class";
import type { CustomTableConfig } from "@/components/custom-table/custom-table.types";
import { format } from "date-fns";

export const getTableConfig = (): CustomTableConfig<number, Class>[] => [
  {
    key: "id",
    headerLabel: "Mã lớp",
    textAlign: "left" as const,
    renderCell: (classData) => classData.id,
  },
  {
    key: "course",
    headerLabel: "Khóa học",
    textAlign: "left" as const,
    renderCell: (classData) => (
      <Box>
        <Typography variant="body2" fontWeight="medium">
          {classData.courseId}
        </Typography>
      </Box>
    ),
  },
  {
    key: "teacher",
    headerLabel: "Giảng viên",
    textAlign: "left" as const,
    renderCell: (classData) => classData.teacherName,
  },
  {
    key: "year",
    headerLabel: "Năm học",
    textAlign: "left" as const,
    renderCell: (classData) => classData.academicYear,
  },
  {
    key: "semester",
    headerLabel: "Học kỳ",
    textAlign: "left" as const,
    renderCell: (classData) =>
      classData.semester === 1
        ? "Học kỳ 1"
        : classData.semester === 2
        ? "Học kỳ 2"
        : "Học kỳ hè",
  },
  {
    key: "room",
    headerLabel: "Phòng học",
    textAlign: "left" as const,
    renderCell: (classData) => classData.room,
  },
  {
    key: "time",
    headerLabel: "Thời gian",
    textAlign: "left" as const,
    renderCell: (classData) => (
      <Typography>
        {classData.startTime}:00 - {classData.endTime}:00
      </Typography>
    ),
  },
  {
    key: "maxStudents",
    headerLabel: "Sĩ số tối đa",
    textAlign: "center" as const,
    renderCell: (classData) => <Typography>{classData.maxStudents}</Typography>,
  },
  {
    key: "deadline",
    headerLabel: "Hạn đăng ký",
    textAlign: "left" as const,
    renderCell: (classData) => {
      const deadline = new Date(classData.deadline);
      const now = new Date();
      const isExpired = deadline < now;

      return (
        <Typography
          color={isExpired ? "error.main" : "success.main"}
          fontWeight={500}
        >
          {format(deadline, "dd/MM/yyyy")}
        </Typography>
      );
    },
  },
];
