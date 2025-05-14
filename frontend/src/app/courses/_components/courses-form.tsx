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
} from "@mui/material";
import { useFormik } from "formik";
import * as Yup from "yup";
import type { Course } from "@/types/course";
import { useCourseSearch } from "../_sections/use-course-search";
import { useCallback, useEffect, useState } from "react";
import { useMainContext } from "@/context/main/main-context";

interface CourseFormProps {
  course?: Course | null;
}

const validationSchema = Yup.object().shape({
  courseId: Yup.string()
    .required("Vui lòng nhập mã khóa học")
    .matches(/^[a-zA-Z0-9]+$/, "Mã khóa học chỉ được chứa chữ và số"),
  courseNameVi: Yup.string()
    .required("Vui lòng nhập tên khóa học tiếng Việt")
    .min(3, "Tên khóa học phải có ít nhất 3 ký tự"),
  courseNameEn: Yup.string()
    .required("Vui lòng nhập tên khóa học tiếng Anh")
    .min(3, "Tên khóa học phải có ít nhất 3 ký tự"),
  credits: Yup.number()
    .required("Vui lòng nhập số tín chỉ")
    .min(2, "Số tín chỉ phải lớn hơn hoặc bằng 2")
    .integer("Số tín chỉ phải là số nguyên"),
  facultyId: Yup.string().required("Vui lòng chọn khoa phụ trách"),
  descriptionVi: Yup.string().required(
    "Vui lòng nhập mô tả khóa học tiếng Việt"
  ),
  descriptionEn: Yup.string().required(
    "Vui lòng nhập mô tả khóa học tiếng Anh"
  ),
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
  const { faculties } = useMainContext();
  const {
    courses,
    searchCourses,
    addCourseApi,
    updateCourseApi,
    getCourseApi,
  } = useCourseSearch();
  const [searchTerm, setSearchTerm] = useState("");
  const [selectedCourse, setSelectedCourse] = useState<Course | null>(null);

  // Initial search when component mounts
  useEffect(() => {
    searchCourses({ page: 1, limit: 20 });
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, []);

  const handleSearch = useCallback(
    (inputValue: string) => {
      searchCourses({ page: 1, limit: 20, courseId: inputValue });
    },
    [searchCourses]
  );

  useEffect(() => {
    const fetchCourse = async () => {
      if (course) {
        const response = await getCourseApi.call(course.courseId);
        setSelectedCourse(response.data || null);
      }
    };
    fetchCourse();
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [course]);
  const formik = useFormik({
    initialValues: {
      courseId: course?.courseId || "",
      courseNameVi: course?.courseName.vi || "",
      courseNameEn: course?.courseName.en || "",
      credits: course?.credits || 3,
      facultyId: course?.facultyId || "",
      descriptionVi: course?.description.vi || "",
      descriptionEn: course?.description.en || "",
      requiredCourseId: course?.requiredCourseId || "",
    },
    enableReinitialize: true,
    validateOnChange: false,
    validationSchema,
    onSubmit: async (values) => {
      try {
        const {
          requiredCourseId,
          courseNameVi,
          courseNameEn,
          descriptionVi,
          descriptionEn,
          ...rest
        } = values;
        let postCourseData: Course = {
          id: course?.courseId || "",
          ...rest,
          courseName: {
            vi: courseNameVi,
            en: courseNameEn,
          },
          description: {
            vi: descriptionVi,
            en: descriptionEn,
          },
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
      <Box component='form' onSubmit={formik.handleSubmit} noValidate>
        <Typography variant='h6' gutterBottom>
          {course ? "Chỉnh sửa khóa học" : "Thêm khóa học mới"}
        </Typography>
        <Typography variant='body2' color='text.secondary' paragraph>
          Nhập thông tin chi tiết về khóa học. Các trường có dấu * là bắt buộc.
        </Typography>

        <Divider sx={{ my: 2 }} />

        <Grid2 container spacing={3}>
          <Grid2 size={{ xs: 12, md: 6 }}>
            <TextField
              required
              id='courseId'
              name='courseId'
              label='Mã khóa học'
              disabled={!!course}
              fullWidth
              variant='outlined'
              value={formik.values.courseId}
              onChange={formik.handleChange}
              error={formik.touched.courseId && Boolean(formik.errors.courseId)}
              helperText={formik.touched.courseId && formik.errors.courseId}
              placeholder='VD: CS101'
            />
          </Grid2>
          <Grid2 size={{ xs: 12, md: 6 }}>
            <TextField
              required
              id='courseNameVi'
              name='courseNameVi'
              label='Tên khóa học (Tiếng Việt)'
              fullWidth
              variant='outlined'
              value={formik.values.courseNameVi}
              onChange={formik.handleChange}
              error={
                formik.touched.courseNameVi &&
                Boolean(formik.errors.courseNameVi)
              }
              helperText={
                formik.touched.courseNameVi && formik.errors.courseNameVi
              }
              placeholder='VD: Lập trình hướng đối tượng'
            />
          </Grid2>
          <Grid2 size={{ xs: 12, md: 6 }}>
            <TextField
              required
              id='courseNameEn'
              name='courseNameEn'
              label='Tên khóa học (Tiếng Anh)'
              fullWidth
              variant='outlined'
              value={formik.values.courseNameEn}
              onChange={formik.handleChange}
              error={
                formik.touched.courseNameEn &&
                Boolean(formik.errors.courseNameEn)
              }
              helperText={
                formik.touched.courseNameEn && formik.errors.courseNameEn
              }
              placeholder='Ex: Object-Oriented Programming'
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
              <InputLabel id='faculty-label'>Khoa phụ trách</InputLabel>
              <Select
                labelId='faculty-label'
                id='facultyId'
                name='facultyId'
                value={formik.values.facultyId}
                label='Khoa phụ trách'
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
              id='credits'
              name='credits'
              label='Số tín chỉ'
              type='number'
              fullWidth
              variant='outlined'
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
              id='requiredCourseId'
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
                handleSearch(newInputValue);
                if (!newInputValue) {
                  setSelectedCourse(null);
                }
              }}
              renderInput={(params) => (
                <TextField
                  {...params}
                  label='Môn tiên quyết'
                  placeholder='VD: CS101'
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
          <Grid2 size={{ xs: 12, md: 6 }}>
            <TextField
              required
              id='descriptionVi'
              name='descriptionVi'
              label='Mô tả khóa học (Tiếng Việt)'
              fullWidth
              multiline
              rows={4}
              variant='outlined'
              value={formik.values.descriptionVi}
              onChange={formik.handleChange}
              error={
                formik.touched.descriptionVi &&
                Boolean(formik.errors.descriptionVi)
              }
              helperText={
                formik.touched.descriptionVi && formik.errors.descriptionVi
              }
              placeholder='Nhập mô tả chi tiết về khóa học bằng tiếng Việt...'
            />
          </Grid2>
          <Grid2 size={{ xs: 12, md: 6 }}>
            <TextField
              required
              id='descriptionEn'
              name='descriptionEn'
              label='Mô tả khóa học (Tiếng Anh)'
              fullWidth
              multiline
              rows={4}
              variant='outlined'
              value={formik.values.descriptionEn}
              onChange={formik.handleChange}
              error={
                formik.touched.descriptionEn &&
                Boolean(formik.errors.descriptionEn)
              }
              helperText={
                formik.touched.descriptionEn && formik.errors.descriptionEn
              }
              placeholder='Enter detailed course description in English...'
            />
          </Grid2>
        </Grid2>

        <Box
          sx={{ display: "flex", justifyContent: "flex-end", gap: 2, mt: 3 }}
        >
          <Button variant='outlined' onClick={() => router.push("/courses")}>
            Hủy
          </Button>
          <Button
            type='submit'
            variant='contained'
            disabled={formik.isSubmitting}
          >
            {course ? "Cập nhật khóa học" : "Tạo khóa học"}
          </Button>
        </Box>
      </Box>
    </Paper>
  );
}
