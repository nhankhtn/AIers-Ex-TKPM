"use client";

import { useCallback, useEffect, useState } from "react";
import {
  Box,
  Button,
  Card,
  CardContent,
  CardHeader,
  Typography,
  FormControl,
  InputLabel,
  Paper,
  Stack,
  Autocomplete,
  TextField,
} from "@mui/material";
import type { Student } from "@/types/student";
import { CustomTable } from "@/components/custom-table";
import CustomPagination from "@/components/custom-pagination";
import RowStack from "@/components/row-stack";
import { useSelection } from "@/hooks/use-selection";
import { useMainContext } from "@/context/main/main-context";
import useFunction from "@/hooks/use-function";
import { StudentApi } from "@/api/students";
import { Class } from "@/types/class";
import useRegistrationsSearch from "./use-registrations-search";
import ClassFilter from "@/app/_components/class-filter";
import { GetClassesTableConfig } from "./table-config";
import { useLocale, useTranslations } from "next-intl";

export function RegistrationForm() {
  const { faculties } = useMainContext();
  const t = useTranslations("registrations");
  const locale = useLocale() as "en" | "vi";
  const {
    students,
    getStudentsApi,
    paginationRegisterClass: pagination,
    getRegisterableClassApi,
    classes,
    filter,
    setFilter,
    setClasses,
  } = useRegistrationsSearch();

  useEffect(() => {
    setClasses(
      (getRegisterableClassApi.data?.data || [])
        .slice(
          pagination.page * pagination.rowsPerPage,
          (pagination.page + 1) * pagination.rowsPerPage
        )
        .filter(
          (item: Class) =>
            (item.semester === Number(filter.semester) ||
              filter.semester === "") &&
            (item.academicYear === Number(filter.academicYear) ||
              filter.academicYear === "")
        )
    );
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [
    getRegisterableClassApi.data,
    pagination.page,
    pagination.rowsPerPage,
    filter.semester,
    filter.academicYear,
  ]);

  const [selectedStudent, setSelectedStudent] = useState<Student | null>(null);

  const selection = useSelection(classes);

  const registerClassApi = useFunction(StudentApi.registerClass, {
    successMessage: t("messages.registerSuccess"),
    onSuccess: ({ payload }) => {
      setClasses((prev) =>
        prev.filter((item) => !payload.classIds.includes(item.classId))
      );
      setSelectedStudent(null);
      selection.handleDeselectAll();
    },
  });
  useEffect(() => {
    if (selectedStudent?.id) getRegisterableClassApi.call(selectedStudent.id);
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [selectedStudent?.id]);

  const handleRegister = useCallback(() => {
    registerClassApi.call({
      studentId: selectedStudent?.id || "",
      classIds: selection.selected.map((s) => s.classId),
    });
  }, [registerClassApi, selectedStudent, selection.selected]);

  return (
    <Stack sx={{ gap: 3 }}>
      <Card>
        <CardHeader
          title={t("form.title")}
          subheader={t("form.subtitle")}
          action={
            <Button
              variant="contained"
              onClick={handleRegister}
              disabled={!selectedStudent || selection.selected.length === 0}
            >
              {t("form.registerButton")}
            </Button>
          }
        />
        <CardContent sx={{ pt: 0 }}>
          <Stack sx={{ gap: 3 }}>
            <FormControl fullWidth required>
              <InputLabel id="student-select-label">
                {t("form.student")}
              </InputLabel>
              <Autocomplete
                id="student-autocomplete"
                options={students}
                getOptionLabel={(option) => option.name}
                value={selectedStudent}
                onChange={(event, newValue) => {
                  setSelectedStudent(newValue);
                }}
                renderInput={(params) => (
                  <TextField
                    {...params}
                    label={t("form.student")}
                    variant="outlined"
                  />
                )}
                isOptionEqualToValue={(option, value) => option.id === value.id}
              />
            </FormControl>

            {selectedStudent && (
              <Paper
                variant="outlined"
                sx={{ p: 2, bgcolor: "background.default" }}
              >
                <Typography
                  variant="subtitle1"
                  fontWeight="medium"
                  gutterBottom
                >
                  {t("form.studentInfo")}
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
                      variant="body2"
                      color="text.secondary"
                      component="span"
                    >
                      {t("form.studentId")}:
                    </Typography>{" "}
                    <Typography variant="body2" component="span">
                      {selectedStudent.id}
                    </Typography>
                  </Box>
                  <Box>
                    <Typography
                      variant="body2"
                      color="text.secondary"
                      component="span"
                    >
                      {t("form.name")}:
                    </Typography>{" "}
                    <Typography variant="body2" component="span">
                      {selectedStudent.name}
                    </Typography>
                  </Box>
                  <Box>
                    <Typography
                      variant="body2"
                      color="text.secondary"
                      component="span"
                    >
                      {t("form.faculty")}:
                    </Typography>{" "}
                    <Typography variant="body2" component="span">
                      {
                        faculties.find((f) => selectedStudent.faculty === f.id)
                          ?.name[locale]
                      }
                    </Typography>
                  </Box>
                  <Box>
                    <Typography
                      variant="body2"
                      color="text.secondary"
                      component="span"
                    >
                      {t("form.course")}:
                    </Typography>{" "}
                    <Typography variant="body2" component="span">
                      {selectedStudent.course}
                    </Typography>
                  </Box>
                </Box>
              </Paper>
            )}

            <Box>
              <RowStack pb={3}>
                <Typography
                  variant="subtitle1"
                  fontWeight="medium"
                  gutterBottom
                  flex={1}
                >
                  {t("form.selectClasses")}
                </Typography>
                <Box width={444}>
                  <ClassFilter
                    filter={filter}
                    onChange={(key: string, value: string) => {
                      setFilter((prev) => ({
                        ...prev,
                        [key]: value,
                      }));
                    }}
                  />
                </Box>
              </RowStack>
              <CustomTable
                select={selection}
                configs={GetClassesTableConfig()}
                loading={getRegisterableClassApi.loading}
                rows={classes}
              />
              {classes.length > 0 && (
                <CustomPagination
                  pagination={pagination}
                  justifyContent="end"
                  p={2}
                  borderTop={1}
                  borderColor={"divider"}
                  rowsPerPageOptions={[10, 15, 20]}
                />
              )}
            </Box>
          </Stack>
        </CardContent>
      </Card>
    </Stack>
  );
}
