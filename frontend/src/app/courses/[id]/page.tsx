import { Box, Typography } from "@mui/material";
import { CourseForm } from "../_components/courses-form";
import { useCourseSearch } from "../_sections/use-course-search";
import { CourseApi } from "@/api/course";
import { notFound } from "next/navigation";
export default async function EditCoursePage({
  params,
}: {
  params: { id: string };
}) {
  const { id } = await params;
  const course = await CourseApi.getCourse(id);
  if (!course || Object.keys(course).length === 0) {
    notFound();
  }
  return (
    <Box sx={{ display: "flex", flexDirection: "column", gap: 3 }}>
      <Typography variant="h4" component="h1" fontWeight="bold">
        Sửa khóa học
      </Typography>
      <CourseForm course={course?.data} />
    </Box>
  );
}
