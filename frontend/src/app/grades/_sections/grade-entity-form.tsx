"use client";

import { useState, useEffect, useCallback } from "react";
import {
  Box,
  Card,
  CardHeader,
  CardContent,
  Typography,
  FormControl,
  InputLabel,
  TextField,
  Button,
  Paper,
  Autocomplete,
  Stack,
} from "@mui/material";
import SaveIcon from "@mui/icons-material/Save";
import useGradesSearch from "./use-grades-search";
import { Class } from "@/types/class";
import RowStack from "@/components/row-stack";
import { CustomTable } from "@/components/custom-table";
import { getGradesTableConfig } from "./table-config";
import CustomPagination from "@/components/custom-pagination";
import { StudentScore } from "@/types/student";
import useFunction from "@/hooks/use-function";
import { ClassApi } from "@/api/class";
import useAppSnackbar from "@/hooks/use-app-snackbar";
import ClassFilter from "@/app/_components/class-filter";

export function GradeEntryForm() {
  const { showSnackbarError } = useAppSnackbar();
  const {
    getClassesApi,
    filter,
    setFilter,
    classes,
    getClassScoresApi,
    pagination,
  } = useGradesSearch();
  const [selectedClass, setSelectedClass] = useState<Class | null>(null);
  const [classScores, setClassScores] = useState<StudentScore[]>([]);
  const updateClassScoresApi = useFunction(ClassApi.createScoresStudent, {
    successMessage: "Cập nhật điểm thành công",
    onSuccess: ({ result }) => {
      if (result) {
        setClassScores(result);
      }
    },
  });

  useEffect(() => {
    const fetchData = async () => {
      if (selectedClass) {
        const { data } = await getClassScoresApi.call(selectedClass.classId);
        if (data) {
          setClassScores(data);
        }
      }
    };
    fetchData();
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [selectedClass]);

  const handleGradeChange = useCallback(
    (
      studentId: string,
      field: "midTermScore" | "finalScore",
      value: string
    ) => {
      setClassScores((prevScores) =>
        prevScores.map((student) =>
          student.studentId === studentId
            ? {
                ...student,
                [field]: Number(value) || 0,
              }
            : student
        )
      );
    },
    []
  );

  const handleSaveGrades = useCallback(() => {
    const scoresToUpdate = classScores.map((student) => ({
      studentId: student.studentId,
      midTermScore: student.midTermScore,
      finalScore: student.finalScore,
    }));
    if (
      scoresToUpdate.some(
        (score) =>
          score.midTermScore < 0 ||
          score.finalScore < 0 ||
          score.midTermScore > 10 ||
          score.finalScore > 10
      )
    ) {
      showSnackbarError("Điểm không hợp lệ. Vui lòng kiểm tra lại.");
      return;
    }

    updateClassScoresApi.call({
      classId: selectedClass?.classId || "",
      scores: scoresToUpdate,
    });
  }, [classScores, selectedClass, updateClassScoresApi, showSnackbarError]);

  return (
    <Card>
      <CardHeader
        title='Nhập điểm cho sinh viên'
        subheader='Chọn lớp học và nhập điểm giữa kỳ, cuối kỳ cho sinh viên.'
        action={
          <Button
            variant='contained'
            startIcon={<SaveIcon />}
            onClick={handleSaveGrades}
            disabled={!selectedClass || classScores.length === 0}
          >
            Lưu điểm
          </Button>
        }
      />
      <CardContent sx={{ pt: 0 }}>
        <Stack sx={{ display: "flex", flexDirection: "column", gap: 3 }}>
          <RowStack gap={1}>
            <Box flex={1}>
              <FormControl fullWidth required>
                <InputLabel id='class-select-label'>Lớp học</InputLabel>
                <Autocomplete
                  id='student-autocomplete'
                  options={classes}
                  getOptionLabel={(option) =>
                    `${option.classId} - ${option.courseName}`
                  }
                  value={selectedClass}
                  onChange={(event, newValue) => {
                    setSelectedClass(newValue);
                  }}
                  renderInput={(params) => (
                    <TextField {...params} label='Lớp học' variant='outlined' />
                  )}
                  isOptionEqualToValue={(option, value) =>
                    option.id === value.id
                  }
                />
              </FormControl>
            </Box>

            <Box width={444}>
              <ClassFilter
                filter={filter}
                onChange={(key, value) => {
                  setFilter((prev) => ({
                    ...prev,
                    [key]: value,
                  }));
                }}
              />
            </Box>
          </RowStack>

          {selectedClass && (
            <>
              <Paper
                variant='outlined'
                sx={{ p: 2, bgcolor: "background.default" }}
              >
                <Typography
                  variant='subtitle1'
                  fontWeight='medium'
                  gutterBottom
                >
                  Thông tin lớp học
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
                      Mã lớp:
                    </Typography>{" "}
                    <Typography variant='body2' component='span'>
                      {selectedClass.classId}
                    </Typography>
                  </Box>
                  <Box>
                    <Typography
                      variant='body2'
                      color='text.secondary'
                      component='span'
                    >
                      Khóa học:
                    </Typography>{" "}
                    <Typography variant='body2' component='span'>
                      {selectedClass.courseId}: {selectedClass.courseName}
                    </Typography>
                  </Box>
                  <Box>
                    <Typography
                      variant='body2'
                      color='text.secondary'
                      component='span'
                    >
                      Giảng viên:
                    </Typography>{" "}
                    <Typography variant='body2' component='span'>
                      {selectedClass.teacherName}
                    </Typography>
                  </Box>
                  <Box>
                    <Typography
                      variant='body2'
                      color='text.secondary'
                      component='span'
                    >
                      Học kỳ:
                    </Typography>{" "}
                    <Typography variant='body2' component='span'>
                      {selectedClass.semester === 1
                        ? "Học kỳ 1"
                        : selectedClass.semester === 2
                        ? "Học kỳ 2"
                        : "Học kỳ hè"}
                      , {selectedClass.academicYear}
                    </Typography>
                  </Box>
                </Box>
              </Paper>

              <Box>
                <RowStack
                  sx={{
                    justifyContent: "space-between",
                    mb: 1,
                  }}
                >
                  <Typography variant='subtitle1' fontWeight='medium'>
                    Danh sách sinh viên và điểm số
                  </Typography>
                </RowStack>
                <CustomTable
                  configs={getGradesTableConfig({
                    onGradeChange: handleGradeChange,
                  })}
                  rows={classScores}
                  loading={getClassScoresApi.loading}
                />
                {classScores.length > 0 && (
                  <CustomPagination
                    pagination={pagination}
                    justifyContent='end'
                    p={2}
                    borderTop={1}
                    borderColor={"divider"}
                    rowsPerPageOptions={[10, 15, 20]}
                  />
                )}
              </Box>
            </>
          )}
        </Stack>
      </CardContent>
    </Card>
  );
}
