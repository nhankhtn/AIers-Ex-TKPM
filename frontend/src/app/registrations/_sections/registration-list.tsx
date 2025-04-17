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
import { getRegisteredClassesTableConfig } from "./table-config";
import RowStack from "@/components/row-stack";
import useFunction from "@/hooks/use-function";
import { StudentApi } from "@/api/students";
import ClassFilter from "@/app/_components/class-filter";

export default function RegistrationList() {
  const { faculties } = useMainContext();
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
    successMessage: "Huỷ đăng ký thành công",
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
            variant='outlined'
            color='error'
            onClick={() =>
              deleteRegisterClassApi.call({
                studentId: data.studentId,
                classId: data.classId,
              })
            }
          >
            Huỷ đăng kí
          </Button>
        </RowStack>
      );
    },
    [deleteRegisterClassApi]
  );

  return (
    <Card>
      <CardContent>
        <Box sx={{ mb: 2 }}>
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
        </Box>
        <Stack gap={2}>
          {selectedStudent && (
            <Paper
              variant='outlined'
              sx={{ p: 2, bgcolor: "background.default" }}
            >
              <Typography variant='subtitle1' fontWeight='medium' gutterBottom>
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
                Các lớp học đã đăng ký
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
              configs={getRegisteredClassesTableConfig()}
              loading={getStudentClassApi.loading}
              rows={classes}
              renderRowActions={renderRowActions}
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
  );
}
