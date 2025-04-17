import { Box, Typography, Grid2, Stack } from "@mui/material";
import type { Student } from "@/types/student";
import useFunction from "@/hooks/use-function";
import { StudentApi } from "@/api/students";
import { useEffect, useMemo } from "react";
import { useMainContext } from "@/context/main/main-context";
import { CustomTable } from "@/components/custom-table";
import { getTranscriptTableConfig } from "./table-config";

interface TranscriptPreviewProps {
  student: Student;
}

export function TranscriptPreview({ student }: TranscriptPreviewProps) {
  const { faculties } = useMainContext();
  const getTranscriptApi = useFunction(StudentApi.getStudentTranscript);

  useEffect(() => {
    getTranscriptApi.call(student.id);
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [student.id]);

  const transcriptData = useMemo(
    () => getTranscriptApi.data,
    [getTranscriptApi.data]
  );
  return (
    <Stack className='print:p-10' sx={{ gap: 3 }} px={3} pt={3}>
      <Box
        sx={{
          textAlign: "center",
          pb: 2,
          borderBottom: 1,
          borderColor: "divider",
        }}
      >
        <Typography variant='h4' fontWeight='bold' gutterBottom>
          BẢNG ĐIỂM CHÍNH THỨC
        </Typography>
        <Typography variant='subtitle1' color='text.secondary'>
          Trường Đại học ABC
        </Typography>
      </Box>

      <Grid2 container spacing={2}>
        <Grid2 size={{ xs: 12, md: 6 }}>
          <Typography variant='body1'>
            <Typography component='span' fontWeight='medium'>
              Họ và tên:
            </Typography>{" "}
            {student.name}
          </Typography>
          <Typography variant='body1'>
            <Typography component='span' fontWeight='medium'>
              Mã sinh viên:
            </Typography>{" "}
            {student.id}
          </Typography>
        </Grid2>
        <Grid2 size={{ xs: 12, md: 6 }}>
          <Typography variant='body1'>
            <Typography component='span' fontWeight='medium'>
              Khoa:
            </Typography>{" "}
            {faculties.find((f) => student.faculty === f.id)?.name}
          </Typography>
          <Typography variant='body1'>
            <Typography component='span' fontWeight='medium'>
              Khóa:
            </Typography>{" "}
            {student.course}
          </Typography>
        </Grid2>
      </Grid2>

      <CustomTable
        configs={getTranscriptTableConfig()}
        rows={transcriptData?.transcript || []}
        loading={getTranscriptApi.loading}
      />

      <Stack gap={1}>
        <Typography variant='body1'>
          <Typography component='span' fontWeight='medium'>
            Tổng số tín chỉ:
          </Typography>{" "}
          {transcriptData?.totalCredit}
        </Typography>
        <Typography variant='body1'>
          <Typography component='span' fontWeight='medium'>
            Số tín chỉ đã hoàn thành:
          </Typography>{" "}
          {transcriptData?.passedCredit}
        </Typography>
        <Typography variant='body1'>
          <Typography component='span' fontWeight='medium'>
            Điểm trung bình tích lũy:
          </Typography>{" "}
          {transcriptData?.gpa.toFixed(2)}
        </Typography>
      </Stack>

      <Grid2 container spacing={4} sx={{ mt: 4, print: { mt: 8 } }}>
        <Grid2 size={{ xs: 6 }} sx={{ textAlign: "center" }}>
          <Typography variant='body1' fontWeight='medium'>
            Sinh viên
          </Typography>
          <Typography variant='body2' color='text.secondary'>
            (Ký và ghi rõ họ tên)
          </Typography>
          <Box sx={{ height: 80 }}></Box>
          <Typography variant='body1'>{student.name}</Typography>
        </Grid2>
        <Grid2 size={{ xs: 6 }} sx={{ textAlign: "center" }}>
          <Typography variant='body1' fontWeight='medium'>
            Trưởng phòng đào tạo
          </Typography>
          <Typography variant='body2' color='text.secondary'>
            (Ký và ghi rõ họ tên)
          </Typography>
          <Box sx={{ height: 80 }}></Box>
          <Typography variant='body1'>TS. Nguyễn Văn X</Typography>
        </Grid2>
      </Grid2>
    </Stack>
  );
}
