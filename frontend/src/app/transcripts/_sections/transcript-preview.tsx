import { Box, Typography, Grid2, Stack } from "@mui/material";
import type { Student } from "@/types/student";
import useFunction from "@/hooks/use-function";
import { StudentApi } from "@/api/students";
import { useEffect, useMemo } from "react";
import { useMainContext } from "@/context/main/main-context";
import { CustomTable } from "@/components/custom-table";
import { getTranscriptTableConfig } from "./table-config";
import { useTranslations, useLocale } from "next-intl";

interface TranscriptPreviewProps {
  student: Student;
}

export function TranscriptPreview({ student }: TranscriptPreviewProps) {
  const { faculties } = useMainContext();
  const getTranscriptApi = useFunction(StudentApi.getStudentTranscript);
  const t = useTranslations("transcripts");
  const locale = useLocale() as "en" | "vi";

  useEffect(() => {
    getTranscriptApi.call(student.id);
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [student.id]);

  const transcriptData = useMemo(
    () => getTranscriptApi.data,
    [getTranscriptApi.data]
  );

  const faculty = faculties.find((f) => student.faculty === f.id);

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
          {t("preview.title")}
        </Typography>
        <Typography variant='subtitle1' color='text.secondary'>
          {t("preview.universityName")}
        </Typography>
      </Box>

      <Grid2 container spacing={2}>
        <Grid2 size={{ xs: 12, md: 6 }}>
          <Typography variant='body1'>
            <Typography component='span' fontWeight='medium'>
              {t("preview.studentName")}:
            </Typography>{" "}
            {student.name}
          </Typography>
          <Typography variant='body1'>
            <Typography component='span' fontWeight='medium'>
              {t("preview.studentId")}:
            </Typography>{" "}
            {student.id}
          </Typography>
        </Grid2>
        <Grid2 size={{ xs: 12, md: 6 }}>
          <Typography variant='body1'>
            <Typography component='span' fontWeight='medium'>
              {t("preview.faculty")}:
            </Typography>{" "}
            {faculty?.name[locale]}
          </Typography>
          <Typography variant='body1'>
            <Typography component='span' fontWeight='medium'>
              {t("preview.course")}:
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
            {t("preview.totalCredits")}:
          </Typography>{" "}
          {transcriptData?.totalCredit}
        </Typography>
        <Typography variant='body1'>
          <Typography component='span' fontWeight='medium'>
            {t("preview.completedCredits")}:
          </Typography>{" "}
          {transcriptData?.passedCredit}
        </Typography>
        <Typography variant='body1'>
          <Typography component='span' fontWeight='medium'>
            {t("preview.gpa")}:
          </Typography>{" "}
          {transcriptData?.gpa.toFixed(2)}
        </Typography>
      </Stack>

      <Grid2 container spacing={4} sx={{ mt: 4, print: { mt: 8 } }}>
        <Grid2 size={{ xs: 6 }} sx={{ textAlign: "center" }}>
          <Typography variant='body1' fontWeight='medium'>
            {t("preview.student")}
          </Typography>
          <Typography variant='body2' color='text.secondary'>
            {t("preview.signature")}
          </Typography>
          <Box sx={{ height: 80 }}></Box>
          <Typography variant='body1'>{student.name}</Typography>
        </Grid2>
        <Grid2 size={{ xs: 6 }} sx={{ textAlign: "center" }}>
          <Typography variant='body1' fontWeight='medium'>
            {t("preview.registrar")}
          </Typography>
          <Typography variant='body2' color='text.secondary'>
            {t("preview.signature")}
          </Typography>
          <Box sx={{ height: 80 }}></Box>
          <Typography variant='body1'>{t("preview.registrarName")}</Typography>
        </Grid2>
      </Grid2>
    </Stack>
  );
}
