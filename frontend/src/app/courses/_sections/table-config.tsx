import { Box, Typography } from "@mui/material";
import type { Course } from "@/types/course";
import type { CustomTableConfig } from "@/components/custom-table/custom-table.types";

export const getTableConfig = (): CustomTableConfig<string, Course>[] => [
  {
    key: "courseId",
    headerLabel: "Mã khóa học",
    textAlign: "left" as const,
    renderCell: (course) => course.courseId,
  },
  {
    key: "courseName",
    headerLabel: "Tên khóa học",
    textAlign: "left" as const,
    renderCell: (course) => course.courseName,
  },
  {
    key: "faculty",
    headerLabel: "Khoa",
    textAlign: "left" as const,
    renderCell: (course) => course.facultyName,
  },
  {
    key: "credits",
    headerLabel: "Tín chỉ",
    textAlign: "center" as const,
    renderCell: (course) => course.credits,
  },
  {
    key: "requiredCourseId",
    headerLabel: "Môn tiên quyết",
    textAlign: "left" as const,
    renderCell: (course) => (
      <Box>
        {course.requiredCourseId ? (
          <>
            <Typography component="span">
              {course.requiredCourseId}
            </Typography>
            <Typography
              component="span"
              sx={{ color: "text.secondary", ml: 1, fontSize: "14px" }}
            >
              - {course.requiredCourseName}
            </Typography>
          </>
        ) : (
          <Typography color="text.secondary">Không có</Typography>
        )}
      </Box>
    ),
  },
  {
    key: "isDeleted",
    headerLabel: "Trạng thái",
    textAlign: "center" as const,
    renderCell: (course) =>
      !course.isDeleted ? (
        <Typography color="success.main" fontWeight="medium">
          Đang hoạt động
        </Typography>
      ) : (
        <Typography color="error.main" fontWeight="medium">
          Không hoạt động
        </Typography>
      ),
  },
];
