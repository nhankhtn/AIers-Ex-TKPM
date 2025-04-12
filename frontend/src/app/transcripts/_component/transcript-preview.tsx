import {
  Box,
  Paper,
  Typography,
  Table,
  TableBody,
  TableCell,
  TableHead,
  TableRow,
  TableContainer,
  Grid2,
} from "@mui/material";
import { mockTranscriptData } from "@/lib/mock-data";
import type { Student } from "@/types/student";
import type { CourseGrade } from "@/types/course";


interface TranscriptData {
  courses: CourseGrade[];
  gpa: number;
  totalCredits: number;
  completedCredits: number;
}

interface TranscriptPreviewProps {
  student: Student;
}

export function TranscriptPreview({ student }: TranscriptPreviewProps) {
  // Get transcript data for the student
  const mockData = (
    mockTranscriptData as unknown as Record<string, TranscriptData>
  )[student.id];
  const transcriptData: TranscriptData = mockData || {
    courses: [],
    gpa: 0,
    totalCredits: 0,
    completedCredits: 0,
  };

  return (
    <Box
      className="print:p-10"
      sx={{ display: "flex", flexDirection: "column", gap: 3 }}
    >
      <Box
        sx={{
          textAlign: "center",
          pb: 2,
          borderBottom: 1,
          borderColor: "divider",
        }}
      >
        <Typography variant="h4" fontWeight="bold" gutterBottom>
          BẢNG ĐIỂM CHÍNH THỨC
        </Typography>
        <Typography variant="subtitle1" color="text.secondary">
          Trường Đại học ABC
        </Typography>
      </Box>

      <Grid2 container spacing={2}>
        <Grid2 size={{ xs: 12, md: 6 }}>
          <Typography variant="body1">
            <Typography component="span" fontWeight="medium">
              Họ và tên:
            </Typography>{" "}
            {student.name}
          </Typography>
          <Typography variant="body1">
            <Typography component="span" fontWeight="medium">
              Mã sinh viên:
            </Typography>{" "}
            {student.id}
          </Typography>
        </Grid2>
        <Grid2 size={{ xs: 12, md: 6 }}>
          <Typography variant="body1">
            <Typography component="span" fontWeight="medium">
              Khoa:
            </Typography>{" "}
            {student.faculty}
          </Typography>
          <Typography variant="body1">
            <Typography component="span" fontWeight="medium">
              Khóa:
            </Typography>{" "}
            {student.course}
          </Typography>
        </Grid2>
      </Grid2>

      <TableContainer component={Paper}>
        <Table>
          <TableHead>
            <TableRow>
              <TableCell width={80}>STT</TableCell>
              <TableCell width={100}>Mã môn</TableCell>
              <TableCell>Tên môn học</TableCell>
              <TableCell align="center">Tín chỉ</TableCell>
              <TableCell align="center">Điểm</TableCell>
              <TableCell align="center">Điểm chữ</TableCell>
            </TableRow>
          </TableHead>
          <TableBody>
            {transcriptData.courses.length === 0 ? (
              <TableRow>
                <TableCell colSpan={6} align="center" sx={{ py: 3 }}>
                  Sinh viên chưa hoàn thành môn học nào.
                </TableCell>
              </TableRow>
            ) : (
              transcriptData.courses.map(
                (course: CourseGrade, index: number) => (
                  <TableRow key={index}>
                    <TableCell>{index + 1}</TableCell>
                    <TableCell>{course.code}</TableCell>
                    <TableCell>{course.name}</TableCell>
                    <TableCell align="center">{course.credits}</TableCell>
                    <TableCell align="center">
                      {course.score.toFixed(1)}
                    </TableCell>
                    <TableCell align="center">{course.grade}</TableCell>
                  </TableRow>
                )
              )
            )}
          </TableBody>
        </Table>
      </TableContainer>

      <Grid2 container spacing={2}>
        <Grid2 size={{ xs: 12, md: 6 }}>
          <Typography variant="body1">
            <Typography component="span" fontWeight="medium">
              Tổng số tín chỉ:
            </Typography>{" "}
            {transcriptData.totalCredits}
          </Typography>
          <Typography variant="body1">
            <Typography component="span" fontWeight="medium">
              Số tín chỉ đã hoàn thành:
            </Typography>{" "}
            {transcriptData.completedCredits}
          </Typography>
        </Grid2>
        <Grid2 size={{ xs: 12, md: 6 }}>
          <Typography variant="body1">
            <Typography component="span" fontWeight="medium">
              Điểm trung bình tích lũy:
            </Typography>{" "}
            {transcriptData.gpa.toFixed(2)}
          </Typography>
        </Grid2>
      </Grid2>

      <Grid2 container spacing={4} sx={{ mt: 4, print: { mt: 8 } }}>
        <Grid2 size={{ xs: 6 }} sx={{ textAlign: "center" }}>
          <Typography variant="body1" fontWeight="medium">
            Sinh viên
          </Typography>
          <Typography variant="body2" color="text.secondary">
            (Ký và ghi rõ họ tên)
          </Typography>
          <Box sx={{ height: 80 }}></Box>
          <Typography variant="body1">{student.name}</Typography>
        </Grid2>
        <Grid2 size={{ xs: 6 }} sx={{ textAlign: "center" }}>
          <Typography variant="body1" fontWeight="medium">
            Trưởng phòng đào tạo
          </Typography>
          <Typography variant="body2" color="text.secondary">
            (Ký và ghi rõ họ tên)
          </Typography>
          <Box sx={{ height: 80 }}></Box>
          <Typography variant="body1">TS. Nguyễn Văn X</Typography>
        </Grid2>
      </Grid2>
    </Box>
  );
}
