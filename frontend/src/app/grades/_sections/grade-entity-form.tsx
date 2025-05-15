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
import { useLocale, useTranslations } from "next-intl";

export function GradeEntryForm() {
  const locale = useLocale() as "en" | "vi";
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
  const t = useTranslations();

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
        title={t("grades.form.title")}
        subheader={t("grades.form.description")}
        action={
          <Button
            variant='contained'
            startIcon={<SaveIcon />}
            onClick={handleSaveGrades}
            disabled={!selectedClass || classScores.length === 0}
          >
            {t("grades.form.saveGrades")}
          </Button>
        }
      />
      <CardContent sx={{ pt: 0 }}>
        <Stack sx={{ display: "flex", flexDirection: "column", gap: 3 }}>
          <RowStack gap={1}>
            <Box flex={1}>
              <FormControl fullWidth required>
                <InputLabel id='class-select-label'>
                  {t("grades.form.class")}
                </InputLabel>
                <Autocomplete
                  id='student-autocomplete'
                  options={classes}
                  getOptionLabel={(option) =>
                    `${option.classId} - ${option.courseName[locale]}`
                  }
                  value={selectedClass}
                  onChange={(event, newValue) => {
                    setSelectedClass(newValue);
                  }}
                  renderInput={(params) => (
                    <TextField
                      {...params}
                      label={t("grades.form.class")}
                      variant='outlined'
                    />
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
                  {t("grades.form.classInfo")}
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
                      {t("grades.form.classCode")}:
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
                      {t("grades.form.courseName")}:
                    </Typography>{" "}
                    <Typography variant='body2' component='span'>
                      {selectedClass.courseName[locale]}
                    </Typography>
                  </Box>
                  <Box>
                    <Typography
                      variant='body2'
                      color='text.secondary'
                      component='span'
                    >
                      {t("grades.form.teacher")}:
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
                      {t("grades.form.schedule")}:
                    </Typography>{" "}
                    <Typography variant='body2' component='span'>
                      {t("grades.form.dayOfWeek", {
                        day: selectedClass.dayOfWeek,
                      })}{" "}
                      ({selectedClass.startTime}:00 - {selectedClass.endTime}
                      :00)
                    </Typography>
                  </Box>
                </Box>
              </Paper>
              <CustomTable
                configs={getGradesTableConfig({
                  onGradeChange: handleGradeChange,
                })}
                rows={classScores}
                loading={getClassScoresApi.loading}
              />
            </>
          )}
        </Stack>
      </CardContent>
    </Card>
  );
}
