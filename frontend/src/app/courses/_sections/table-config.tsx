import { Box, Typography } from "@mui/material";
import type { Course } from "@/types/course";
import type { CustomTableConfig } from "@/components/custom-table/custom-table.types";

export const getTableConfig = (): CustomTableConfig<Course["id"], Course>[] => [
  {
    key: "courseId",
    headerLabel: "Mã khóa học",
    type: "string",
    renderCell: (data) => (
      <Typography variant='body2'>{data.courseId}</Typography>
    ),
  },
  {
    key: "courseName",
    headerLabel: "Tên khóa học",
    type: "string",
    renderCell: (data) => (
      <Typography variant='body2'>
        {data.courseName.vi} ({data.courseName.en})
      </Typography>
    ),
  },
  {
    key: "credits",
    headerLabel: "Số tín chỉ",
    type: "number",
    renderCell: (data) => (
      <Typography variant='body2'>{data.credits}</Typography>
    ),
  },
  {
    key: "faculty",
    headerLabel: "Khoa phụ trách",
    type: "string",
    renderCell: (data) => (
      <Typography variant='body2'>
        {data.facultyName
          ? `${data.facultyName.vi} (${data.facultyName.en})`
          : ""}
      </Typography>
    ),
  },
  {
    key: "description",
    headerLabel: "Mô tả",
    type: "string",
    renderCell: (data) => (
      <Typography variant='body2'>
        {data.description.vi} ({data.description.en})
      </Typography>
    ),
  },
  {
    key: "requiredCourse",
    headerLabel: "Môn tiên quyết",
    type: "string",
    renderCell: (data) => (
      <Typography variant='body2'>
        {data.requiredCourseName
          ? `${data.requiredCourseName.vi} (${data.requiredCourseName.en})`
          : ""}
      </Typography>
    ),
  },
  {
    key: "isDeleted",
    headerLabel: "Trạng thái",
    textAlign: "center" as const,
    renderCell: (course) =>
      !course.deletedAt ? (
        <Typography color='success.main' fontWeight='medium'>
          Đang hoạt động
        </Typography>
      ) : (
        <Typography color='error.main' fontWeight='medium'>
          Không hoạt động
        </Typography>
      ),
  },
];
