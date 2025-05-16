"use client";

import { useRef, useState } from "react";
import {
  Box,
  Button,
  Card,
  CardContent,
  CardHeader,
  CardActions,
  Typography,
  FormControl,
  InputLabel,
  Paper,
  Autocomplete,
  TextField,
  Stack,
} from "@mui/material";
import PrintIcon from "@mui/icons-material/Print";
import { Student } from "@/types/student";
import { useMainContext } from "@/context/main/main-context";
import RowStack from "@/components/row-stack";
import { useReactToPrint } from "react-to-print";
import useTranscriptsSearch from "./use-transcripts-search";
import { TranscriptPreview } from "./transcript-preview";
import { useLocale, useTranslations } from "next-intl";

export function TranscriptForm() {
  const { faculties } = useMainContext();
  const { students } = useTranscriptsSearch();
  const [selectedStudent, setSelectedStudent] = useState<Student | null>(null);
  const [showPreview, setShowPreview] = useState<boolean>(false);
  const componentRef = useRef<HTMLDivElement>(null);
  const locale = useLocale();
  const t = useTranslations("transcripts");
  const componentT = useTranslations("components");

  const handlePrint = useReactToPrint({
    contentRef: componentRef,
    documentTitle: `Bang_diem_${selectedStudent?.id}`,
  });

  return (
    <Stack sx={{ gap: 3 }}>
      <Card>
        <CardHeader
          title={t("printTranscript")}
          subheader={t("selectStudent")}
        />
        <CardContent sx={{ pt: 0 }}>
          <Stack sx={{ gap: 2 }}>
            <FormControl fullWidth required>
              <InputLabel id='student-select-label'>
                {t("form.studentLabel")}
              </InputLabel>
              <Autocomplete
                id='student-autocomplete'
                options={students}
                getOptionLabel={(option) => option.name}
                value={selectedStudent}
                onChange={(event, newValue) => {
                  setSelectedStudent(newValue);
                }}
                renderInput={(params) => (
                  <TextField
                    {...params}
                    label={t("form.studentLabel")}
                    variant='outlined'
                  />
                )}
                noOptionsText={componentT("select.noOptions")}
                isOptionEqualToValue={(option, value) => option.id === value.id}
              />
            </FormControl>

            {selectedStudent && (
              <Paper
                variant='outlined'
                sx={{ p: 2, bgcolor: "background.default" }}
              >
                <Typography
                  variant='subtitle1'
                  fontWeight='medium'
                  gutterBottom
                >
                  {t("studentInfo.title")}
                </Typography>
                <Box
                  sx={{
                    display: "grid",
                    gridTemplateColumns: "1fr 1fr",
                    gap: 1,
                  }}
                >
                  <Box>
                    <Typography
                      variant='body2'
                      color='text.secondary'
                      component='span'
                    >
                      {t("studentInfo.studentId")}:
                    </Typography>{" "}
                    <Typography variant='body2' component='span'>
                      {selectedStudent.id}
                    </Typography>
                  </Box>
                  <Box>
                    <Typography
                      variant='body2'
                      color='text.secondary'
                      component='span'
                    >
                      {t("studentInfo.fullName")}:
                    </Typography>{" "}
                    <Typography variant='body2' component='span'>
                      {selectedStudent.name}
                    </Typography>
                  </Box>
                  <Box>
                    <Typography
                      variant='body2'
                      color='text.secondary'
                      component='span'
                    >
                      {t("studentInfo.faculty")}:
                    </Typography>{" "}
                    <Typography variant='body2' component='span'>
                      {
                        faculties.find((f) => selectedStudent.faculty === f.id)
                          ?.name[locale as "vi" | "en"]
                      }
                    </Typography>
                  </Box>
                  <Box>
                    <Typography
                      variant='body2'
                      color='text.secondary'
                      component='span'
                    >
                      {t("studentInfo.course")}:
                    </Typography>{" "}
                    <Typography variant='body2' component='span'>
                      {selectedStudent.course}
                    </Typography>
                  </Box>
                </Box>
              </Paper>
            )}
          </Stack>
        </CardContent>
        <CardActions sx={{ px: 3, pb: 3 }}>
          <RowStack justifyContent={"flex-end"} width={"100%"}>
            <Button
              variant='contained'
              onClick={() => setShowPreview(true)}
              disabled={!selectedStudent}
            >
              {t("actions.generateTranscript")}
            </Button>
          </RowStack>
        </CardActions>
      </Card>

      {showPreview && selectedStudent && (
        <Stack sx={{ gap: 2 }}>
          <RowStack
            sx={{
              justifyContent: "flex-end",
            }}
          >
            <Button
              variant='contained'
              startIcon={<PrintIcon />}
              onClick={() => handlePrint()}
              className='print:hidden'
            >
              {t("actions.printTranscript")}
            </Button>
          </RowStack>
          <Stack ref={componentRef}>
            <TranscriptPreview student={selectedStudent} />
          </Stack>
        </Stack>
      )}
    </Stack>
  );
}
