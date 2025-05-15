"use client";
import { Box, Typography } from "@mui/material";
import { CourseForm } from "../_components/courses-form";
import { useCourseSearch } from "../_sections/use-course-search";
import { useParams, useRouter } from "next/navigation";
import { useEffect } from "react";
import { useTranslations } from "next-intl";

export default function EditCoursePage() {
  const { id } = useParams();
  const router = useRouter();
  const { getCourseApi } = useCourseSearch();
  const t = useTranslations();

  useEffect(() => {
    const fetchCourse = async () => {
      if (!id) return;
      const response = await getCourseApi.call(id as string);
      if (!response.data || Object.keys(response.data).length === 0) {
        router.replace("/noCourseFound");
      }
    };
    fetchCourse();

  // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [id, router]);

  return (
    <Box sx={{ display: "flex", flexDirection: "column", gap: 3 }}>
      <Typography variant="h4" component="h1" fontWeight="bold">
        {t("courses.form.editTitle")}
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
          <Typography variant="h6">{t("common.loading")}</Typography>
        </Box>
      )}
      {getCourseApi.data && <CourseForm course={getCourseApi.data} />}
    </Box>
  );
}
