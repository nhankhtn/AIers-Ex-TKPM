import { CustomTableConfig } from "@/components/custom-table";
import { StudentScore } from "@/types/student";
import { TextField, Typography } from "@mui/material";

export const getGradesTableConfig = ({
  onGradeChange,
}: {
  onGradeChange: (
    studentId: string,
    field: "midTermScore" | "finalScore",
    value: string
  ) => void;
}): CustomTableConfig<string, StudentScore>[] => [
  {
    key: "studentId",
    headerLabel: "MSSV",
    renderCell: (data) => (
      <Typography variant='body2'>{data.studentId}</Typography>
    ),
  },
  {
    key: "studentName",
    headerLabel: "Họ và tên",
    renderCell: (data) => (
      <Typography variant='body2'>{data.studentName}</Typography>
    ),
  },
  {
    key: "midTermScore",
    headerLabel: "Điểm giữa kỳ",
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
    headerLabel: "Điểm cuối kỳ",
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
    headerLabel: "Tổng điểm",
    renderCell: (data) => (
      <Typography variant='body2'>{data.totalScore || "--"}</Typography>
    ),
  },
  {
    key: "grade",
    headerLabel: "Xếp loại",
    renderCell: (data) => (
      <Typography variant='body2'>{data.grade || "--"}</Typography>
    ),
  },
  {
    key: "isPassed",
    headerLabel: "Trạng thái",
    renderCell: (data) => (
      <Typography variant='body2'>
        {data.isPassed ? "Đã qua" : "Chưa qua"}
      </Typography>
    ),
  },
];
