"use client";

import { useState } from "react";
import { useFormik } from "formik";
import * as Yup from "yup";
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
  Select,
  MenuItem,
  Paper,
  Table,
  TableBody,
  TableCell,
  TableContainer,
  TableHead,
  TableRow,
  Checkbox,
  Alert,
  AlertTitle,
  type SelectChangeEvent,
} from "@mui/material";
import CheckCircleIcon from "@mui/icons-material/CheckCircle";
import WarningIcon from "@mui/icons-material/Warning";
import { mockStudents, mockClasses, mockCourses } from "@/lib/mock-data";
import type { Student } from "@/types/student";
import type { Class } from "@/types/class";


interface RegistrationStatus {
  success: boolean;
  message: string;
}

const validationSchema = Yup.object().shape({
  studentId: Yup.string().required("Vui lòng chọn sinh viên"),
  selectedClasses: Yup.array().min(1, "Vui lòng chọn ít nhất một lớp học"),
});

export function RegistrationForm() {
  const formik = useFormik({
    initialValues: {
      studentId: "",
      selectedClasses: [] as number[],
    },
    validationSchema,
    onSubmit: async (values) => {
      try {
        // In a real app, this would call an API to register the student
        console.log(
          "Registering student:",
          values.studentId,
          "for classes:",
          values.selectedClasses
        );

        // Simulate successful registration
        setRegistrationStatus({
          success: true,
          message: "Đăng ký khóa học thành công!",
        });

        // Reset selection after successful registration
        formik.setFieldValue("selectedClasses", []);
      } catch (error) {
        console.error("Error submitting registration:", error);
        setRegistrationStatus({
          success: false,
          message: "Đăng ký khóa học thất bại. Vui lòng thử lại.",
        });
      }
    },
  });

  const [registrationStatus, setRegistrationStatus] =
    useState<RegistrationStatus | null>(null);

  const handleStudentChange = (event: SelectChangeEvent<string>): void => {
    formik.setFieldValue("studentId", event.target.value);
    formik.setFieldValue("selectedClasses", []);
    setRegistrationStatus(null);
  };

  const handleClassSelect = (classId: number): void => {
    const currentSelected = formik.values.selectedClasses;
    const newSelected = currentSelected.includes(classId)
      ? currentSelected.filter((id) => id !== classId)
      : [...currentSelected, classId];
    formik.setFieldValue("selectedClasses", newSelected);
  };

  const selectedStudent = formik.values.studentId
    ? mockStudents.find(
        (student) => student.id.toString() === formik.values.studentId
      )
    : undefined;

  // Get available classes for the selected student
  const availableClasses: Class[] = mockClasses.filter((classItem) => {
    // Check if class is not full
    if (classItem.enrolledStudents >= classItem.maxStudents) {
      return false;
    }

    // Get the course for this class
    const course = mockCourses.find(
      (c) => c.id.toString() === classItem.courseId.toString()
    );

    // Check prerequisites if the student is selected
    if (selectedStudent && course) {
      // In a real app, we would check if the student has completed the prerequisites
      // For this demo, we'll assume they have
      return true;
    }

    return true;
  });

  return (
    <Box sx={{ display: "flex", flexDirection: "column", gap: 3 }}>
      <Card>
        <CardHeader
          title="Đăng ký khóa học cho sinh viên"
          subheader="Chọn sinh viên và các lớp học cần đăng ký."
        />
        <CardContent sx={{ pt: 0 }}>
          <Box sx={{ display: "flex", flexDirection: "column", gap: 3 }}>
            <FormControl
              fullWidth
              required
              error={
                formik.touched.studentId && Boolean(formik.errors.studentId)
              }
            >
              <InputLabel id="student-select-label">Sinh viên</InputLabel>
              <Select
                labelId="student-select-label"
                id="student-select"
                value={formik.values.studentId}
                label="Sinh viên"
                onChange={handleStudentChange}
              >
                {mockStudents.map((student) => (
                  <MenuItem key={student.id} value={student.id.toString()}>
                    {student.id}: {student.name}
                  </MenuItem>
                ))}
              </Select>
              {formik.touched.studentId && formik.errors.studentId && (
                <Typography variant="caption" color="error">
                  {formik.errors.studentId}
                </Typography>
              )}
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
                      variant="body2"
                      color="text.secondary"
                      component="span"
                    >
                      Mã sinh viên:
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
                      Họ tên:
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
                      Khoa:
                    </Typography>{" "}
                    <Typography variant="body2" component="span">
                      {selectedStudent.department}
                    </Typography>
                  </Box>
                  <Box>
                    <Typography
                      variant="body2"
                      color="text.secondary"
                      component="span"
                    >
                      Khóa:
                    </Typography>{" "}
                    <Typography variant="body2" component="span">
                      {selectedStudent.batch}
                    </Typography>
                  </Box>
                </Box>
              </Paper>
            )}

            {selectedStudent && (
              <Box>
                <Typography
                  variant="subtitle1"
                  fontWeight="medium"
                  gutterBottom
                >
                  Chọn lớp học cần đăng ký
                </Typography>
                <TableContainer component={Paper} variant="outlined">
                  <Table>
                    <TableHead>
                      <TableRow>
                        <TableCell padding="checkbox"></TableCell>
                        <TableCell>Mã lớp</TableCell>
                        <TableCell>Khóa học</TableCell>
                        <TableCell
                          sx={{ display: { xs: "none", md: "table-cell" } }}
                        >
                          Giảng viên
                        </TableCell>
                        <TableCell
                          sx={{ display: { xs: "none", md: "table-cell" } }}
                        >
                          Lịch học
                        </TableCell>
                        <TableCell
                          sx={{ display: { xs: "none", md: "table-cell" } }}
                        >
                          Sĩ số
                        </TableCell>
                      </TableRow>
                    </TableHead>
                    <TableBody>
                      {availableClasses.length === 0 ? (
                        <TableRow>
                          <TableCell colSpan={6} align="center" sx={{ py: 3 }}>
                            Không có lớp học nào khả dụng.
                          </TableCell>
                        </TableRow>
                      ) : (
                        availableClasses.map((classItem) => {
                          const course = mockCourses.find(
                            (c) =>
                              c.id.toString() === classItem.courseId.toString()
                          );
                          return (
                            <TableRow
                              key={classItem.id}
                              hover
                              selected={formik.values.selectedClasses.includes(
                                classItem.id
                              )}
                              onClick={() => handleClassSelect(classItem.id)}
                              sx={{ cursor: "pointer" }}
                            >
                              <TableCell padding="checkbox">
                                <Checkbox
                                  checked={formik.values.selectedClasses.includes(
                                    classItem.id
                                  )}
                                  onChange={() =>
                                    handleClassSelect(classItem.id)
                                  }
                                  color="primary"
                                />
                              </TableCell>
                              <TableCell>{classItem.code}</TableCell>
                              <TableCell>
                                <Typography variant="body2" fontWeight="medium">
                                  {course?.courseId}
                                </Typography>
                                <Typography
                                  variant="body2"
                                  color="text.secondary"
                                >
                                  {course?.courseName}
                                </Typography>
                              </TableCell>
                              <TableCell
                                sx={{
                                  display: { xs: "none", md: "table-cell" },
                                }}
                              >
                                {classItem.instructor}
                              </TableCell>
                              <TableCell
                                sx={{
                                  display: { xs: "none", md: "table-cell" },
                                }}
                              >
                                {classItem.schedule}
                              </TableCell>
                              <TableCell
                                sx={{
                                  display: { xs: "none", md: "table-cell" },
                                }}
                              >
                                {classItem.enrolledStudents}/
                                {classItem.maxStudents}
                              </TableCell>
                            </TableRow>
                          );
                        })
                      )}
                    </TableBody>
                  </Table>
                </TableContainer>
                {formik.touched.selectedClasses &&
                  formik.errors.selectedClasses && (
                    <Typography
                      variant="caption"
                      color="error"
                      sx={{ mt: 1, display: "block" }}
                    >
                      {formik.errors.selectedClasses}
                    </Typography>
                  )}
              </Box>
            )}

            {registrationStatus && (
              <Alert
                severity={registrationStatus.success ? "success" : "error"}
                icon={
                  registrationStatus.success ? (
                    <CheckCircleIcon />
                  ) : (
                    <WarningIcon />
                  )
                }
              >
                <AlertTitle>
                  {registrationStatus.success ? "Thành công" : "Lỗi"}
                </AlertTitle>
                {registrationStatus.message}
              </Alert>
            )}
          </Box>
        </CardContent>
        <CardActions sx={{ px: 3, pb: 3 }}>
          <Button
            variant="contained"
            onClick={() => formik.handleSubmit()}
            disabled={
              !formik.values.studentId ||
              formik.values.selectedClasses.length === 0 ||
              formik.isSubmitting
            }
          >
            Đăng ký
          </Button>
        </CardActions>
      </Card>
    </Box>
  );
}
