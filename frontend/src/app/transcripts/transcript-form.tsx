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
import { TranscriptPreview } from "@/app/transcripts/transcript-preview";
import { Student } from "@/types/student";
import useTranscriptsSearch from "./use-transcripts-search";
import { useMainContext } from "@/context/main/main-context";
import RowStack from "@/components/row-stack";
import { useReactToPrint } from "react-to-print";

export function TranscriptForm() {
  const { faculties } = useMainContext();
  const { students } = useTranscriptsSearch();
  const [selectedStudent, setSelectedStudent] = useState<Student | null>(null);
  const [showPreview, setShowPreview] = useState<boolean>(false);
  const componentRef = useRef<HTMLDivElement>(null);

  const handlePrint = useReactToPrint({
    contentRef: componentRef,
    documentTitle: `Bang_diem_${selectedStudent?.id}`,
  });

  return (
    <Stack sx={{ gap: 3 }}>
      <Card>
        <CardHeader
          title='In bảng điểm chính thức'
          subheader='Chọn sinh viên để in bảng điểm chính thức.'
        />
        <CardContent sx={{ pt: 0 }}>
          <Stack sx={{ gap: 2 }}>
            <FormControl fullWidth required>
              <InputLabel id='student-select-label'>Sinh viên</InputLabel>
              <Autocomplete
                id='student-autocomplete'
                options={students}
                getOptionLabel={(option) => option.name}
                value={selectedStudent}
                onChange={(event, newValue) => {
                  setSelectedStudent(newValue);
                }}
                renderInput={(params) => (
                  <TextField {...params} label='Sinh viên' variant='outlined' />
                )}
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
                  Thông tin sinh viên
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
                      Mã sinh viên:
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
                      Họ tên:
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
                      Khoa:
                    </Typography>{" "}
                    <Typography variant='body2' component='span'>
                      {
                        faculties.find((f) => selectedStudent.faculty === f.id)
                          ?.name
                      }
                    </Typography>
                  </Box>
                  <Box>
                    <Typography
                      variant='body2'
                      color='text.secondary'
                      component='span'
                    >
                      Khóa:
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
              Tạo bảng điểm
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
              In bảng điểm
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
