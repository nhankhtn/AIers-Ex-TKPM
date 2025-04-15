"use client";

import { useCallback, useEffect, useState } from "react";
import { useFormik } from "formik";
import * as Yup from "yup";
import {
  Box,
  Button,
  Card,
  CardContent,
  CardHeader,
  Typography,
  FormControl,
  InputLabel,
  Select,
  MenuItem,
  Paper,
  Alert,
  AlertTitle,
  type SelectChangeEvent,
  Stack,
  Autocomplete,
  TextField,
} from "@mui/material";
import CheckCircleIcon from "@mui/icons-material/CheckCircle";
import WarningIcon from "@mui/icons-material/Warning";
import type { Student } from "@/types/student";
import useRegistrationsSearch from "./use-registrations-search";
import { CustomTable } from "@/components/custom-table";
import { getClassesTableConfig } from "./table-config";
import CustomPagination from "@/components/custom-pagination";
import RowStack from "@/components/row-stack";
import SelectFilter from "../dashboard/_components/select-filter";
import { useSelection } from "@/hooks/use-selection";
import { useMainContext } from "@/context/main/main-context";
import useFunction from "@/hooks/use-function";
import { StudentApi } from "@/api/students";

export function RegistrationForm() {
  const { faculties } = useMainContext();
  const {
    students,
    getStudentsApi,
    pagination,
    getRegisterableClassApi,
    classes,
    classFilterConfig,
    filter,
    setFilter,
    setClasses,
  } = useRegistrationsSearch();

  const [selectedStudent, setSelectedStudent] = useState<Student | null>(null);

  const selection = useSelection(classes);

  const registerClassApi = useFunction(StudentApi.registerClass, {
    successMessage: "Đăng ký thành công",
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
          title='Đăng ký khóa học cho sinh viên'
          subheader='Chọn sinh viên và các lớp học cần đăng ký.'
          action={
            <Button
              variant='contained'
              onClick={handleRegister}
              disabled={!selectedStudent || selection.selected.length === 0}
            >
              Đăng ký
            </Button>
          }
        />
        <CardContent sx={{ pt: 0 }}>
          <Stack sx={{ gap: 3 }}>
            <FormControl fullWidth required>
              <InputLabel id='student-select-label'>Sinh viên</InputLabel>
              {/* <Select
                labelId='student-select-label'
                id='student-select'
                value={selectedStudent?.id || ""}
                label='Sinh viên'
                onChange={(event: SelectChangeEvent<string>) => {
                  setSelectedStudent(
                    students.find((f) => f.id === event.target.value) || null
                  );
                }}
              >
                {students.map((student, index) => (
                  <MenuItem key={index} value={student.id}>
                    {student.name}
                  </MenuItem>
                ))}
              </Select> */}
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

            <Box>
              <RowStack pb={3}>
                <Typography
                  variant='subtitle1'
                  fontWeight='medium'
                  gutterBottom
                  flex={1}
                >
                  Chọn lớp học cần đăng ký
                </Typography>
                <Box width={444}>
                  <SelectFilter
                    configs={classFilterConfig}
                    filter={filter as any}
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
                configs={getClassesTableConfig()}
                loading={getRegisterableClassApi.loading}
                rows={classes}
              />
              {classes.length > 0 && (
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
          </Stack>
        </CardContent>
      </Card>
    </Stack>
  );
}
