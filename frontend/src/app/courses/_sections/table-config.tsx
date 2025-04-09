import { Box, Typography } from "@mui/material";
import type { Course } from "@/types/course";
import type { CustomTableConfig } from "@/components/custom-table/custom-table.types";
import { Faculty } from "@/types/student";

export const getTableConfig = (
  faculties: Faculty[]
): CustomTableConfig<string, Course>[] => [
  {
    key: "id",
    headerLabel: "Mã khóa học",
    sortable: true,
    textAlign: "left" as const,
    renderCell: (course) => course.id,
  },
  {
    key: "name",
    headerLabel: "Tên khóa học",
    sortable: true,
    textAlign: "left" as const,
    renderCell: (course) => course.name,
  },
  {
    key: "faculty",
    headerLabel: "Khoa",
    sortable: true,
    textAlign: "left" as const,
    renderCell: (course) => (
      <Typography variant="body2">
        {faculties.find((f) => f.id === course.faculty)?.name}
      </Typography>
    ),
  },
  {
    key: "credits",
    headerLabel: "Tín chỉ",
    sortable: true,
    textAlign: "center" as const,
    renderCell: (course) => course.credits,
  },
  {
    key: "requiredCourseId",
    headerLabel: "Môn tiên quyết",
    sortable: false,
    textAlign: "left" as const,
    renderCell: (course) => (
      <Box>
        {course.requiredCourseId ? (
          <>
            <Typography component="span" sx={{ fontWeight: "medium" }}>
              {course.requiredCourseId}
            </Typography>
            <Typography
              component="span"
              sx={{ color: "text.secondary", ml: 1 }}
            >
              {course.requiredCourseName}
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
    sortable: true,
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
