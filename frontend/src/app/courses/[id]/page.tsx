"use client";
import { Box, Typography } from "@mui/material";
import { CourseForm } from "../_components/courses-form";
import { useCourseSearch } from "../_sections/use-course-search";
import { useParams,useRouter } from "next/navigation";
import { useEffect } from "react";
export default function EditCoursePage() {
  const { id } = useParams();
  const router = useRouter();
  const { getCourseApi } = useCourseSearch();
  useEffect(() => {
    const fetchCourse = async () => {
      if (!id) return;
      const response = await getCourseApi.call(id as string);
      if ( !response.data || Object.keys(response.data).length === 0) {
        router.replace("/noCourseFound");
      }
    };
    fetchCourse();
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [id]);
  return (
    <Box sx={{ display: "flex", flexDirection: "column", gap: 3 }}>
      <Typography variant="h4" component="h1" fontWeight="bold">
        Sửa khóa học
      </Typography>
      {getCourseApi.loading && (
        <Box
          sx={{
            display: "flex",
            justifyContent: "center",
            alignItems: "center",
            height: "100%",
          }}
        >
          <Typography variant="h6">Loading...</Typography>
        </Box>
      )}
      {getCourseApi.data && <CourseForm course={getCourseApi.data.data} />}
    </Box>
  );
}
