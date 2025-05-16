import { CustomTableConfig } from "@/components/custom-table";
import { StudentScore } from "@/types/student";
import { TextField, Typography } from "@mui/material";

export const getGradesTableConfig = ({
  onGradeChange,
  t,
}: {
  onGradeChange: (
    studentId: string,
    field: "midTermScore" | "finalScore",
    value: string
  ) => void;
  t: (key: string) => string;
}): CustomTableConfig<string, StudentScore>[] => {
  return [
    {
      key: "studentId",
      headerLabel: t("table.studentId"),
      renderCell: (data) => (
        <Typography variant='body2'>{data.studentId}</Typography>
      ),
    },
    {
      key: "studentName",
      headerLabel: t("table.studentName"),
      renderCell: (data) => (
        <Typography variant='body2'>{data.studentName}</Typography>
      ),
    },
    {
      key: "midTermScore",
      headerLabel: t("table.midTermScore"),
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
      headerLabel: t("table.finalScore"),
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
      headerLabel: t("table.totalScore"),
      renderCell: (data) => (
        <Typography variant='body2'>{data.totalScore || "--"}</Typography>
      ),
    },
    {
      key: "grade",
      headerLabel: t("table.grade"),
      renderCell: (data) => (
        <Typography variant='body2'>{data.grade || "--"}</Typography>
      ),
    },
    {
      key: "isPassed",
      headerLabel: t("table.status"),
      renderCell: (data) => (
        <Typography variant='body2'>
          {data.isPassed ? t("table.passed") : t("table.failed")}
        </Typography>
      ),
    },
  ];
};
