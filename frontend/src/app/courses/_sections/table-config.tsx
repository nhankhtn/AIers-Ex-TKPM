import { Box, Typography } from "@mui/material";
import type { Course } from "@/types/course";
import type { CustomTableConfig } from "@/components/custom-table/custom-table.types";
import { useLocale, useTranslations } from "next-intl";
import { textAlign } from "@mui/system";

export const GetTableConfig = (): CustomTableConfig<Course["id"], Course>[] => {
  const t = useTranslations();
  const locale = useLocale() as "en" | "vi";
  return [
    {
      key: "courseId",
      headerLabel: t("courses.list.code"),
      type: "string",
      renderCell: (data) => (
        <Typography variant="body2">{data.courseId}</Typography>
      ),
    },
    {
      key: "courseName",
      headerLabel: t("courses.list.name"),
      type: "string",
      renderCell: (data) => (
        <Typography variant="body2">{data.courseName[locale]}</Typography>
      ),
    },
    {
      key: "credits",
      headerLabel: t("courses.list.credits"),
      type: "number",
      renderCell: (data) => (
        <Typography variant="body2" sx={{ textAlign: 'left' }}>{data.credits}</Typography>
      ),
    },
    {
      key: "faculty",
      headerLabel: t("courses.list.faculty"),
      type: "string",
      renderCell: (data) => (
        <Typography variant="body2">
          {data.facultyName?.[locale]}
        </Typography>
      ),
    },
    {
      key: "description",
      headerLabel: t("courses.list.description"),
      type: "string",
      renderCell: (data) => (
        <Typography variant="body2">{data.description[locale]}</Typography>
      ),
    },
    {
      key: "requiredCourse",
      headerLabel: t("courses.list.prerequisites"),
      type: "string",
      renderCell: (data) => (
        <Typography variant="body2">
          {data.requiredCourseName ? `${data.requiredCourseName[locale]}` : ""}
        </Typography>
      ),
    },
    {
      key: "isDeleted",
      headerLabel: t("courses.list.status"),
      textAlign: "center" as const,
      renderCell: (course) =>
        !course.deletedAt ? (
          <Typography color="success.main" fontWeight="medium">
            {t("courses.list.active")}
          </Typography>
        ) : (
          <Typography color="error.main" fontWeight="medium">
            {t("courses.list.inactive")}
          </Typography>
        ),
    },
  ];
};
