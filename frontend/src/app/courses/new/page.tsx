import { Box, Typography } from "@mui/material"
import { CourseForm } from "../_components/courses-form"

export default function NewCoursePage() {
  return (
    <Box sx={{ display: "flex", flexDirection: "column", gap: 3 }}>
      <Typography variant="h4" component="h1" fontWeight="bold">
        Thêm khóa học mới
      </Typography>
      <CourseForm />
    </Box>
  )
}