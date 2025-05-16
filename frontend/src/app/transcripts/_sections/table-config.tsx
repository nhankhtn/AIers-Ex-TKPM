import { CustomTableConfig } from "@/components/custom-table";
import { StudentTranscript } from "@/types/student";
import { Typography } from "@mui/material";
import { useTranslations, useLocale } from "next-intl";

export const getTranscriptTableConfig = (): CustomTableConfig<
  string,
  StudentTranscript
>[] => {
  const t = useTranslations("transcripts");
  const locale = useLocale() as "en" | "vi";

  return [
    {
      key: "courseId",
      headerLabel: t("preview.table.courseId"),
      renderCell: (data) => (
        <Typography variant='body2'>{data.classId}</Typography>
      ),
    },
    {
      key: "courseName",
      headerLabel: t("preview.table.courseName"),
      renderCell: (data) => (
        <Typography variant='body2'>{data.courseName[locale]}</Typography>
      ),
    },
    {
      key: "credits",
      headerLabel: t("preview.table.credits"),
      renderCell: (data) => (
        <Typography variant='body2'>{data.credit}</Typography>
      ),
    },
    {
      key: "grade",
      headerLabel: t("preview.table.grade"),
      renderCell: (data) => (
        <Typography variant='body2'>{data.totalScore}</Typography>
      ),
    },
    {
      key: "gradeLetter",
      headerLabel: t("preview.table.gradeLetter"),
      renderCell: (data) => (
        <Typography variant='body2'>{data.grade}</Typography>
      ),
    },
  ];
};
