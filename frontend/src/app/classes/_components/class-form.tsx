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
  Divider,
  Autocomplete,
} from "@mui/material";
import { useFormik } from "formik";
import * as Yup from "yup";
import type { Class } from "@/types/class";
import type { Course } from "@/types/course";
import { useCourseSearch } from "@/app/courses/_sections/use-course-search";
import { useCallback, useEffect, useState } from "react";
import { useClassSearch } from "@/app/classes/_sections/use-class-search";
import { CourseApi } from "@/api/course";
interface ClassFormProps {
  classData?: Class | null;
}

const validationSchema = Yup.object().shape({
  classId: Yup.string()
    .required("Mã lớp học không được để trống")
    .matches(
      /^[a-zA-Z0-9-]+$/,
      "Mã lớp học chỉ được chứa chữ và số, dấu gạch ngang"
    ),
  courseId: Yup.string().required("Vui lòng chọn khóa học"),
  teacherName: Yup.string()
    .required("Tên giảng viên không được để trống")
    .min(3, "Tên giảng viên phải có ít nhất 3 ký tự"),
  academicYear: Yup.number()
    .required("Vui lòng nhập năm học")
    .min(2000, "Năm học phải lớn hơn 2000")
    .max(2100, "Năm học phải nhỏ hơn 2100")
    .integer("Năm học phải là số nguyên"),
  semester: Yup.number()
    .required("Vui lòng chọn học kỳ")
    .oneOf([1, 2, 3], "Học kỳ không hợp lệ"),
  room: Yup.string().required("Phòng học không được để trống"),
  startTime: Yup.number()
    .required("Vui lòng nhập thời gian bắt đầu")
    .min(0, "Thời gian phải lớn hơn hoặc bằng 0")
    .max(23, "Thời gian phải nhỏ hơn 24"),
  endTime: Yup.number()
    .required("Vui lòng nhập thời gian kết thúc")
    .min(0, "Thời gian phải lớn hơn hoặc bằng 0")
    .max(23, "Thời gian phải nhỏ hơn 24")
    .test(
      "is-greater",
      "Thời gian kết thúc phải lớn hơn thời gian bắt đầu",
      function (value) {
        return value > this.parent.startTime;
      }
    ),
  maxStudents: Yup.number()
    .required("Vui lòng nhập số lượng sinh viên tối đa")
    .min(1, "Số lượng sinh viên tối đa phải lớn hơn 0")
    .integer("Số lượng sinh viên phải là số nguyên"),
  deadline: Yup.date()
    .required("Vui lòng chọn hạn đăng ký")
    .min(new Date(), "Hạn đăng ký phải lớn hơn ngày hiện tại"),
  dayOfWeek: Yup.number()
    .required("Vui lòng chọn ngày trong tuần")
    .oneOf([2, 3, 4, 5, 6, 7, 8], "Ngày trong tuần không hợp lệ"),
});

export function ClassForm({ classData = null }: ClassFormProps) {
  const router = useRouter();
  const { courses, searchCourses } = useCourseSearch();
  const [selectedCourse, setSelectedCourse] = useState<Course | null>(null);
  const { addClassApi, updateClassApi } = useClassSearch();
  const handleSearch = useCallback(
    (inputValue: string) => {
      if (inputValue) {
        searchCourses({ page: 1, limit: 20, courseId: inputValue });
      }
    },
    [searchCourses]
  );

  useEffect(() => {
    if (classData) {
      CourseApi.getCourse(classData.courseId).then((response) => {
        if (response?.data) {
          setSelectedCourse(response.data);
        }
      });
    }
  }, [classData]);

  const formik = useFormik({
    initialValues: {
      classId: classData?.classId || "",
      courseId: classData?.courseId || "",
      teacherName: classData?.teacherName || "",
      academicYear: classData?.academicYear || 2025,
      semester: classData?.semester || 1,
      room: classData?.room || "",
      startTime: classData?.startTime || 7,
      endTime: classData?.endTime || 9,
      maxStudents: classData?.maxStudents || 40,
      dayOfWeek: classData?.dayOfWeek || 2,
      deadline:
        classData?.deadline.toString().split("T")[0] ||
        new Date(Date.now() + 7 * 24 * 60 * 60 * 1000)
          .toISOString()
          .split("T")[0],
    },
    validationSchema,
    onSubmit: async (values) => {
      try {
        console.log("Submitting class:", values);
        if (classData) {
          await updateClassApi.call({
            classId: classData.classId.toString(),
            class: values,
          });
        } else {
          await addClassApi.call({
            ...values,
            id: values.classId,
          });
        }
      } catch (error) {
        console.error("Error submitting class:", error);
      }
    },
  });

  const handleCourseSelect = (course: Course | null) => {
    setSelectedCourse(course);
    formik.setFieldValue("courseId", course?.courseId || "");
  };

  return (
    <Paper sx={{ p: 3 }}>
      <Box component="form" onSubmit={formik.handleSubmit} noValidate>
        <Typography variant="h6" gutterBottom>
          {classData ? "Chỉnh sửa lớp học" : "Mở lớp học mới"}
        </Typography>
        <Typography variant="body2" color="text.secondary">
          Nhập thông tin chi tiết về lớp học. Các trường có dấu * là bắt buộc.
        </Typography>

        <Divider sx={{ my: 2 }} />

        <Grid2 container spacing={3}>
          <Grid2 size={{ xs: 12, md: 6 }}>
            <TextField
              required
              id="id"
              name="classId"
              label="Mã lớp học"
              fullWidth
              variant="outlined"
              value={formik.values.classId}
              onChange={formik.handleChange}
              error={formik.touched.classId && Boolean(formik.errors.classId)}
              helperText={formik.touched.classId && formik.errors.classId}
              placeholder="VD: CS101-01"
              disabled={!!classData}
            />
          </Grid2>

          <Grid2 size={{ xs: 12, md: 6 }}>
            <Autocomplete
              id="courseId"
              options={courses}
              getOptionLabel={(option) =>
                `${option.courseId} - ${option.courseName}`
              }
              value={selectedCourse}
              onChange={(_, newValue) => handleCourseSelect(newValue)}
              onInputChange={(_, newInputValue) => {
                handleSearch(newInputValue);
                if (!newInputValue) {
                  setSelectedCourse(null);
                }
              }}
              renderInput={(params) => (
                <TextField
                  {...params}
                  required
                  label="Khóa học"
                  placeholder="Tìm kiếm khóa học..."
                  error={
                    formik.touched.courseId && Boolean(formik.errors.courseId)
                  }
                  helperText={formik.touched.courseId && formik.errors.courseId}
                />
              )}
            />
          </Grid2>

          <Grid2 size={{ xs: 12, md: 6 }}>
            <TextField
              required
              id="teacherName"
              name="teacherName"
              label="Giảng viên"
              fullWidth
              variant="outlined"
              value={formik.values.teacherName}
              onChange={formik.handleChange}
              error={
                formik.touched.teacherName && Boolean(formik.errors.teacherName)
              }
              helperText={
                formik.touched.teacherName && formik.errors.teacherName
              }
              placeholder="TS. Nguyễn Văn A"
            />
          </Grid2>

          <Grid2 size={{ xs: 12, md: 6 }}>
            <TextField
              required
              id="academicYear"
              name="academicYear"
              label="Năm học"
              type="number"
              fullWidth
              variant="outlined"
              value={formik.values.academicYear}
              onChange={formik.handleChange}
              error={
                formik.touched.academicYear &&
                Boolean(formik.errors.academicYear)
              }
              helperText={
                formik.touched.academicYear && formik.errors.academicYear
              }
              InputProps={{ inputProps: { min: 2000, max: 2100 } }}
            />
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
                <MenuItem value={1}>Học kỳ 1</MenuItem>
                <MenuItem value={2}>Học kỳ 2</MenuItem>
                <MenuItem value={3}>Học kỳ hè</MenuItem>
              </Select>
            </FormControl>
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

          <Grid2 size={{ xs: 12, md: 6 }}>
            <TextField
              required
              id="startTime"
              name="startTime"
              label="Thời gian bắt đầu (giờ)"
              type="number"
              fullWidth
              variant="outlined"
              value={formik.values.startTime}
              onChange={formik.handleChange}
              error={
                formik.touched.startTime && Boolean(formik.errors.startTime)
              }
              helperText={formik.touched.startTime && formik.errors.startTime}
              InputProps={{ inputProps: { min: 0, max: 23 } }}
            />
          </Grid2>

          <Grid2 size={{ xs: 12, md: 6 }}>
            <TextField
              required
              id="endTime"
              name="endTime"
              label="Thời gian kết thúc (giờ)"
              type="number"
              fullWidth
              variant="outlined"
              value={formik.values.endTime}
              onChange={formik.handleChange}
              error={formik.touched.endTime && Boolean(formik.errors.endTime)}
              helperText={formik.touched.endTime && formik.errors.endTime}
              InputProps={{ inputProps: { min: 0, max: 23 } }}
            />
          </Grid2>

          <Grid2 size={{ xs: 12, md: 6 }}>
            <FormControl fullWidth required>
              <InputLabel id="dayOfWeek-label">Ngày học</InputLabel>
              <Select
                labelId="dayOfWeek-label"
                id="dayOfWeek"
                name="dayOfWeek"
                value={formik.values.dayOfWeek}
                onChange={formik.handleChange}
              >
                <MenuItem value={2}>Thứ 2</MenuItem>
                <MenuItem value={3}>Thứ 3</MenuItem>
                <MenuItem value={4}>Thứ 4</MenuItem>
                <MenuItem value={5}>Thứ 5</MenuItem>
                <MenuItem value={6}>Thứ 6</MenuItem>
                <MenuItem value={7}>Thứ 7</MenuItem>
                <MenuItem value={8}>Chủ nhật</MenuItem>
              </Select>
            </FormControl>
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
              id="deadline"
              name="deadline"
              label="Hạn đăng ký"
              type="date"
              fullWidth
              variant="outlined"
              value={formik.values.deadline}
              onChange={formik.handleChange}
              error={formik.touched.deadline && Boolean(formik.errors.deadline)}
              helperText={formik.touched.deadline && formik.errors.deadline}
              InputLabelProps={{ shrink: true }}
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
