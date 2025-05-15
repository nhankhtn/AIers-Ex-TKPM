"use client";
import { ClassForm } from "@/app/classes/_components/class-form";
import { Box, Typography } from "@mui/material";
import { useSearchParams } from "next/navigation";
import { useTranslations } from "next-intl";

export default function NewClassPage() {
  const searchParams = useSearchParams();
  const courseId = searchParams.get("courseId");
  const t = useTranslations();

  return (
    <Box sx={{ display: "flex", flexDirection: "column", gap: 3 }}>
      <Typography variant="h4" fontWeight="bold">
        {t("classes.form.addTitle")}
      </Typography>
      <ClassForm forCourseId={courseId} />
    </Box>
  );
}
