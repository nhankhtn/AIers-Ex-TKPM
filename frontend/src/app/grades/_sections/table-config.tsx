import { CustomTableConfig } from "@/components/custom-table";
import { StudentScore } from "@/types/student";
import { TextField, Typography } from "@mui/material";
import { useTranslations } from "next-intl";

export const getGradesTableConfig = ({
  onGradeChange,
}: {
  onGradeChange: (
    studentId: string,
    field: "midTermScore" | "finalScore",
    value: string
  ) => void;
}): CustomTableConfig<string, StudentScore>[] => {
  const t = useTranslations();
  return [
    {
      key: "studentId",
      headerLabel: t("grades.table.studentId"),
      renderCell: (data) => (
        <Typography variant='body2'>{data.studentId}</Typography>
      ),
    },
    {
      key: "studentName",
      headerLabel: t("grades.table.studentName"),
      renderCell: (data) => (
        <Typography variant='body2'>{data.studentName}</Typography>
      ),
    },
    {
      key: "midTermScore",
      headerLabel: t("grades.table.midTermScore"),
      renderCell: (data) => (
        <TextField
          variant='outlined'
          size='small'
          value={data.midTermScore}
          onChange={(e) =>
            onGradeChange(data.studentId, "midTermScore", e.target.value)
          }
          inputProps={{
            style: { textAlign: "center" },
            min: 0,
            max: 10,
            step: 0.1,
          }}
          sx={{ width: 80 }}
        />
      ),
    },
    {
      key: "finalScore",
      headerLabel: t("grades.table.finalScore"),
      renderCell: (data) => (
        <TextField
          variant='outlined'
          size='small'
          value={data.finalScore}
          onChange={(e) =>
            onGradeChange(data.studentId, "finalScore", e.target.value)
          }
          inputProps={{
            style: { textAlign: "center" },
            min: 0,
            max: 10,
            step: 0.1,
          }}
          sx={{ width: 80 }}
        />
      ),
    },
    {
      key: "totalScore",
      headerLabel: t("grades.table.totalScore"),
      renderCell: (data) => (
        <Typography variant='body2'>{data.totalScore || "--"}</Typography>
      ),
    },
    {
      key: "grade",
      headerLabel: t("grades.table.grade"),
      renderCell: (data) => (
        <Typography variant='body2'>{data.grade || "--"}</Typography>
      ),
    },
    {
      key: "isPassed",
      headerLabel: t("grades.table.status"),
      renderCell: (data) => (
        <Typography variant='body2'>
          {data.isPassed ? t("grades.table.passed") : t("grades.table.failed")}
        </Typography>
      ),
    },
  ];
};
