"use client";

import { useRouter } from "next/navigation";
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
  Autocomplete,
  CircularProgress,
} from "@mui/material";
import { useFormik } from "formik";
import * as Yup from "yup";
import type { Course } from "@/types/course";
import { Faculty } from "@/types/student";
import { useFaculty } from "@/app/dashboard/_sections/use-faculty";
import { useCourseSearch } from "../_sections/use-course-search";
import { useEffect, useState } from "react";

interface CourseFormProps {
  course?: Course | null;
}

const validationSchema = Yup.object().shape({
  courseId: Yup.string()
    .required("Vui lòng nhập mã khóa học")
    .matches(/^[a-zA-Z0-9]+$/, "Mã khóa học chỉ được chứa chữ và số"),
  courseName: Yup.string()
    .required("Vui lòng nhập tên khóa học")
    .min(3, "Tên khóa học phải có ít nhất 3 ký tự"),
  credits: Yup.number()
    .required("Vui lòng nhập số tín chỉ")
    .min(2, "Số tín chỉ phải lớn hơn hoặc bằng 2")
    .integer("Số tín chỉ phải là số nguyên"),
  facultyId: Yup.string().required("Vui lòng chọn khoa phụ trách"),
  description: Yup.string().required("Vui lòng nhập mô tả khóa học"),
  requiredCourseId: Yup.string()
    .nullable()
    .test(
      "not-self",
      "Không thể chọn chính khóa học này làm môn tiên quyết",
      function (value) {
        if (!value || !this.parent.courseId) return true;
        return value !== this.parent.courseId;
      }
    ),
});

export function CourseForm({ course = null }: CourseFormProps) {
  const router = useRouter();
  const { faculties } = useFaculty();
  const { courses, searchCourses, addCourseApi, updateCourseApi } =
    useCourseSearch();
  const [searchTerm, setSearchTerm] = useState("");
  const [selectedCourse, setSelectedCourse] = useState<Course | null>(null);

  useEffect(() => {
    if (searchTerm) {
      searchCourses({ page: 1, limit: 20, courseId: searchTerm });
    }
  }, [searchTerm, searchCourses]);
  const formik = useFormik({
    initialValues: {
      courseId: course?.courseId || "",
      courseName: course?.courseName || "",
      credits: course?.credits || 3,
      facultyId: course?.facultyId || "",
      description: course?.description || "",
      requiredCourseId: course?.requiredCourseId || "",
    },
    enableReinitialize: true,
    validationSchema,
    onSubmit: async (values) => {
      try {
        console.log("Submitting course:", values);
        const { requiredCourseId, ...postCourse } = values;
        let postCourseData: Course = {
          id: course?.courseId || "",
          ...postCourse,
        };
        if (requiredCourseId !== "") {
          postCourseData = {
            ...postCourseData,
            requiredCourseId,
          };
        }
        if (course) {
          await updateCourseApi.call({
            id: course.courseId,
            course: postCourseData,
          });
        } else {
          await addCourseApi.call(postCourseData);
        }
      } catch (error) {
        console.error("Error submitting course:", error);
      }
    },
  });

  const handleCourseSelect = (course: Course | null) => {
    setSelectedCourse(course);
    formik.setFieldValue("requiredCourseId", course?.courseId || "");
  };

  return (
    <Paper sx={{ p: 3 }}>
      <Box component="form" onSubmit={formik.handleSubmit} noValidate>
        <Typography variant="h6" gutterBottom>
          {course ? "Chỉnh sửa khóa học" : "Thêm khóa học mới"}
        </Typography>
        <Typography variant="body2" color="text.secondary" paragraph>
          Nhập thông tin chi tiết về khóa học. Các trường có dấu * là bắt buộc.
        </Typography>

        <Divider sx={{ my: 2 }} />

        <Grid2 container spacing={3}>
          <Grid2 size={{ xs: 12, md: 6 }}>
            <TextField
              required
              id="courseId"
              name="courseId"
              label="Mã khóa học"
              disabled={!!course}
              fullWidth
              variant="outlined"
              value={formik.values.courseId}
              onChange={formik.handleChange}
              error={formik.touched.courseId && Boolean(formik.errors.courseId)}
              helperText={formik.touched.courseId && formik.errors.courseId}
              placeholder="VD: CS101"
            />
          </Grid2>
          <Grid2 size={{ xs: 12, md: 6 }}>
            <TextField
              required
              id="courseName"
              name="courseName"
              label="Tên khóa học"
              fullWidth
              variant="outlined"
              value={formik.values.courseName}
              onChange={formik.handleChange}
              error={
                formik.touched.courseName && Boolean(formik.errors.courseName)
              }
              helperText={formik.touched.courseName && formik.errors.courseName}
              placeholder="VD: Lập trình hướng đối tượng"
            />
          </Grid2>
          <Grid2 size={{ xs: 12, md: 6 }}>
            <FormControl
              fullWidth
              required
              error={
                formik.touched.facultyId && Boolean(formik.errors.facultyId)
              }
            >
              <InputLabel id="faculty-label">Khoa phụ trách</InputLabel>
              <Select
                labelId="faculty-label"
                id="facultyId"
                name="facultyId"
                value={formik.values.facultyId}
                label="Khoa phụ trách"
                onChange={formik.handleChange}
              >
                {faculties.map((faculty) => (
                  <MenuItem key={faculty.id} value={faculty.id}>
                    {faculty.name}
                  </MenuItem>
                ))}
              </Select>
              {formik.touched.facultyId && formik.errors.facultyId && (
                <FormHelperText>{formik.errors.facultyId}</FormHelperText>
              )}
            </FormControl>
          </Grid2>
          <Grid2 size={{ xs: 12, md: 6 }}>
            <TextField
              required
              id="credits"
              name="credits"
              label="Số tín chỉ"
              type="number"
              fullWidth
              variant="outlined"
              value={formik.values.credits}
              onChange={formik.handleChange}
              error={formik.touched.credits && Boolean(formik.errors.credits)}
              helperText={formik.touched.credits && formik.errors.credits}
              InputProps={{ inputProps: { min: 2 } }}
            />
          </Grid2>
          <Grid2 size={{ xs: 12 }}>
            <Autocomplete
              disabled={!!course}
              id="requiredCourseId"
              options={
                course
                  ? courses.filter(
                      (option) => option.courseId !== course.courseId
                    )
                  : courses
              }
              getOptionLabel={(option) =>
                `${option.courseId} - ${option.courseName}`
              }
              value={selectedCourse}
              onChange={(_, newValue) => handleCourseSelect(newValue)}
              onInputChange={(_, newInputValue) => {
                setSearchTerm(newInputValue);
                if (!newInputValue) {
                  console.log("clear");
                  setSelectedCourse(null);
                }
              }}
              renderInput={(params) => (
                <TextField
                  {...params}
                  label="Môn tiên quyết"
                  placeholder="VD: CS101"

                  error={
                    formik.touched.requiredCourseId &&
                    Boolean(formik.errors.requiredCourseId)
                  }
                  helperText={
                    formik.touched.requiredCourseId &&
                    formik.errors.requiredCourseId
                  }
                />
              )}
            />
          </Grid2>
          <Grid2 size={{ xs: 12 }}>
            <TextField
              required
              id="description"
              name="description"
              label="Mô tả khóa học"
              fullWidth
              multiline
              rows={4}
              variant="outlined"
              value={formik.values.description}
              onChange={formik.handleChange}
              error={
                formik.touched.description && Boolean(formik.errors.description)
              }
              helperText={
                formik.touched.description && formik.errors.description
              }
              placeholder="Nhập mô tả chi tiết về khóa học..."
            />
          </Grid2>
        </Grid2>

        <Box
          sx={{ display: "flex", justifyContent: "flex-end", gap: 2, mt: 3 }}
        >
          <Button variant="outlined" onClick={() => router.push("/courses")}>
            Hủy
          </Button>
          <Button
            type="submit"
            variant="contained"
            disabled={formik.isSubmitting}
          >
            {course ? "Cập nhật khóa học" : "Tạo khóa học"}
          </Button>
        </Box>
      </Box>
    </Paper>
  );
}
