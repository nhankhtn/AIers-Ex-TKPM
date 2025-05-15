"use client";

import { useCallback, useEffect, useMemo, useState } from "react";
import {
  Box,
  Paper,
  TextField,
  FormControl,
  InputLabel,
  Typography,
  Card,
  CardContent,
  Autocomplete,
  Stack,
  Button,
} from "@mui/material";
import { Student, StudentClass } from "@/types/student";
import { useMainContext } from "@/context/main/main-context";
import useRegistrationsSearch from "./use-registrations-search";
import CustomPagination from "@/components/custom-pagination";
import { CustomTable } from "@/components/custom-table";
import { GetRegisteredClassesTableConfig } from "./table-config";
import RowStack from "@/components/row-stack";
import useFunction from "@/hooks/use-function";
import { StudentApi } from "@/api/students";
import ClassFilter from "@/app/_components/class-filter";
import { useLocale, useTranslations } from "next-intl";

export default function RegistrationList() {
  const { faculties } = useMainContext();
  const t = useTranslations("registrations");
  const locale = useLocale() as "en" | "vi";
  const {
    students,
    filter,
    setFilter,
    paginationStudentClass: pagination,
    getStudentClassApi,
  } = useRegistrationsSearch();

  const [selectedStudent, setSelectedStudent] = useState<Student | null>(null);

  const classes = useMemo(
    () => getStudentClassApi.data?.data || [],
    [getStudentClassApi.data]
  );

  useEffect(() => {
    if (selectedStudent?.id)
      getStudentClassApi.call({
        studentId: selectedStudent.id,
        page: pagination.page + 1,
        limit: pagination.rowsPerPage,
      });
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [selectedStudent?.id, pagination.page, pagination.rowsPerPage]);

  const deleteRegisterClassApi = useFunction(StudentApi.deleteRegisterClass, {
    successMessage: t("messages.unregisterSuccess"),
    onSuccess: ({ payload }) => {
      getStudentClassApi.setData({
        data: classes.filter(
          (item: StudentClass) =>
            item.studentId !== payload.studentId ||
            item.classId !== payload.classId
        ),
        total: (getStudentClassApi.data?.total || 1) - 1,
      });
    },
  });
  const renderRowActions = useCallback(
    (data: StudentClass) => {
      return (
        <RowStack>
          <Button
            variant="outlined"
            color="error"
            onClick={() =>
              deleteRegisterClassApi.call({
                studentId: data.studentId,
                classId: data.classId,
              })
            }
          >
            {t("list.unregisterButton")}
          </Button>
        </RowStack>
      );
    },
    [deleteRegisterClassApi,t]
  );

  return (
    <Card>
      <CardContent>
        <Box sx={{ mb: 2 }}>
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
        </Box>
        <Stack gap={2}>
          {selectedStudent && (
            <Paper
              variant="outlined"
              sx={{ p: 2, bgcolor: "background.default" }}
            >
              <Typography variant="subtitle1" fontWeight="medium" gutterBottom>
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
                {t("list.title")}
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
              configs={GetRegisteredClassesTableConfig()}
              loading={getStudentClassApi.loading}
              rows={classes}
              renderRowActions={renderRowActions}
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
  );
}
