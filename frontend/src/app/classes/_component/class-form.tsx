"use client";

import { useRouter, useSearchParams } from "next/navigation";
import {
  Box,
  Paper,
  Typography,
  TextField,
  Button,
  Grid2,
  FormControl,
  InputLabel,
  Select,
  MenuItem,
  FormHelperText,
  Divider,
} from "@mui/material";
import { useFormik } from "formik";
import * as Yup from "yup";
import { mockCourses } from "@/lib/mock-data";
import type { Class, ClassFormData } from "@/types/class";

interface ClassFormProps {
  classData?: Class | null;
}

const validationSchema = Yup.object().shape({
  code: Yup.string()
    .required("Mã lớp học không được để trống")
    .matches(
      /^[A-Z0-9-]+$/,
      "Mã lớp học chỉ được chứa chữ hoa, số và dấu gạch ngang"
    ),
  courseId: Yup.string().required("Vui lòng chọn khóa học"),
  year: Yup.string().required("Vui lòng chọn năm học"),
  semester: Yup.string().required("Vui lòng chọn học kỳ"),
  instructor: Yup.string()
    .required("Tên giảng viên không được để trống")
    .min(3, "Tên giảng viên phải có ít nhất 3 ký tự"),
  maxStudents: Yup.number()
    .required("Vui lòng nhập số lượng sinh viên tối đa")
    .min(1, "Số lượng sinh viên tối đa phải lớn hơn 0")
    .integer("Số lượng sinh viên phải là số nguyên"),
  schedule: Yup.string().required("Lịch học không được để trống"),
  room: Yup.string().required("Phòng học không được để trống"),
});

export function ClassForm({ classData = null }: ClassFormProps) {
  const router = useRouter();
  const searchParams = useSearchParams();
  const courseId = searchParams.get("courseId");

  const selectedCourse = courseId
    ? mockCourses.find((course) => course.id.toString() === courseId)
    : null;

  const formik = useFormik({
    initialValues: {
      code: classData?.code || "",
      courseId:
        classData?.courseId ||
        (selectedCourse?.id ? selectedCourse.id.toString() : ""),
      year: classData?.year || "2023-2024",
      semester: classData?.semester ? classData.semester.toString() : "1",
      instructor: classData?.instructor || "",
      maxStudents: classData?.maxStudents || 40,
      schedule: classData?.schedule || "",
      room: classData?.room || "",
    },
    validationSchema,
    onSubmit: async (values) => {
      try {
        // In a real app, this would call an API to save the class
        console.log("Submitting class:", values);
        router.push("/classes");
      } catch (error) {
        console.error("Error submitting class:", error);
      }
    },
  });

  return (
    <Paper sx={{ p: 3 }}>
      <Box component="form" onSubmit={formik.handleSubmit} noValidate>
        <Typography variant="h6" gutterBottom>
          {classData ? "Chỉnh sửa lớp học" : "Mở lớp học mới"}
        </Typography>
        <Typography variant="body2" color="text.secondary" paragraph>
          Nhập thông tin chi tiết về lớp học. Các trường có dấu * là bắt buộc.
        </Typography>

        <Divider sx={{ my: 2 }} />

        <Grid2 container spacing={3}>
          <Grid2 size={{ xs: 12, md: 6 }}>
            <TextField
              required
              id="code"
              name="code"
              label="Mã lớp học"
              fullWidth
              variant="outlined"
              value={formik.values.code}
              onChange={formik.handleChange}
              error={formik.touched.code && Boolean(formik.errors.code)}
              helperText={formik.touched.code && formik.errors.code}
              placeholder="CS101-01"
            />
          </Grid2>
          <Grid2 size={{ xs: 12, md: 6 }}>
            <FormControl
              fullWidth
              required
              error={formik.touched.courseId && Boolean(formik.errors.courseId)}
            >
              <InputLabel id="courseId-label">Khóa học</InputLabel>
              <Select
                labelId="courseId-label"
                id="courseId"
                name="courseId"
                value={formik.values.courseId}
                label="Khóa học"
                onChange={formik.handleChange}
                disabled={!!selectedCourse}
              >
                {mockCourses
                  .filter((course) => course.isDeleted !== null)
                  .map((course) => (
                    <MenuItem key={course.id} value={course.id.toString()}>
                      {course.courseId}: {course.courseName}
                    </MenuItem>
                  ))}
              </Select>
              {formik.touched.courseId && formik.errors.courseId && (
                <FormHelperText>{formik.errors.courseId}</FormHelperText>
              )}
            </FormControl>
          </Grid2>

          <Grid2 size={{ xs: 12, md: 6 }}>
            <FormControl fullWidth required>
              <InputLabel id="year-label">Năm học</InputLabel>
              <Select
                labelId="year-label"
                id="year"
                name="year"
                value={formik.values.year}
                label="Năm học"
                onChange={formik.handleChange}
              >
                <MenuItem value="2023-2024">2023-2024</MenuItem>
                <MenuItem value="2022-2023">2022-2023</MenuItem>
                <MenuItem value="2021-2022">2021-2022</MenuItem>
              </Select>
            </FormControl>
          </Grid2>
          <Grid2 size={{ xs: 12, md: 6 }}>
            <FormControl fullWidth required>
              <InputLabel id="semester-label">Học kỳ</InputLabel>
              <Select
                labelId="semester-label"
                id="semester"
                name="semester"
                value={formik.values.semester}
                label="Học kỳ"
                onChange={formik.handleChange}
              >
                <MenuItem value="1">Học kỳ 1</MenuItem>
                <MenuItem value="2">Học kỳ 2</MenuItem>
                <MenuItem value="3">Học kỳ hè</MenuItem>
              </Select>
            </FormControl>
          </Grid2>

          <Grid2 size={{ xs: 12, md: 6 }}>
            <TextField
              required
              id="instructor"
              name="instructor"
              label="Giảng viên"
              fullWidth
              variant="outlined"
              value={formik.values.instructor}
              onChange={formik.handleChange}
              error={
                formik.touched.instructor && Boolean(formik.errors.instructor)
              }
              helperText={formik.touched.instructor && formik.errors.instructor}
              placeholder="TS. Nguyễn Văn A"
            />
          </Grid2>
          <Grid2 size={{ xs: 12, md: 6 }}>
            <TextField
              required
              id="maxStudents"
              name="maxStudents"
              label="Số lượng sinh viên tối đa"
              type="number"
              fullWidth
              variant="outlined"
              value={formik.values.maxStudents}
              onChange={formik.handleChange}
              error={
                formik.touched.maxStudents && Boolean(formik.errors.maxStudents)
              }
              helperText={
                formik.touched.maxStudents && formik.errors.maxStudents
              }
              InputProps={{ inputProps: { min: 1 } }}
            />
          </Grid2>

          <Grid2 size={{ xs: 12, md: 6 }}>
            <TextField
              required
              id="schedule"
              name="schedule"
              label="Lịch học"
              fullWidth
              variant="outlined"
              value={formik.values.schedule}
              onChange={formik.handleChange}
              error={formik.touched.schedule && Boolean(formik.errors.schedule)}
              helperText={formik.touched.schedule && formik.errors.schedule}
              placeholder="Thứ 2, 4 (7:30-9:30)"
            />
          </Grid2>
          <Grid2 size={{ xs: 12, md: 6 }}>
            <TextField
              required
              id="room"
              name="room"
              label="Phòng học"
              fullWidth
              variant="outlined"
              value={formik.values.room}
              onChange={formik.handleChange}
              error={formik.touched.room && Boolean(formik.errors.room)}
              helperText={formik.touched.room && formik.errors.room}
              placeholder="A1.203"
            />
          </Grid2>
        </Grid2>

        <Box
          sx={{ display: "flex", justifyContent: "flex-end", gap: 2, mt: 3 }}
        >
          <Button variant="outlined" onClick={() => router.push("/classes")}>
            Hủy
          </Button>
          <Button
            type="submit"
            variant="contained"
            disabled={formik.isSubmitting}
          >
            {classData ? "Cập nhật lớp học" : "Tạo lớp học"}
          </Button>
        </Box>
      </Box>
    </Paper>
  );
}
