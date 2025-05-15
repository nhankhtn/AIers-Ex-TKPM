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
import { useTranslations } from "next-intl";

interface ClassFormProps {
  classData?: Class | null;
  forCourseId?: string | null;
}

const validationSchema = (t: any) =>
  Yup.object().shape({
    classId: Yup.string()
      .required(t("classes.validation.classIdRequired"))
      .matches(/^[a-zA-Z0-9-]+$/, t("classes.validation.classIdFormat")),
    courseId: Yup.string().required(t("classes.validation.courseRequired")),
    teacherName: Yup.string()
      .required(t("classes.validation.teacherRequired"))
      .min(3, t("classes.validation.teacherMinLength")),
    academicYear: Yup.number()
      .required(t("classes.validation.academicYearRequired"))
      .min(2000, t("classes.validation.academicYearMin"))
      .max(2100, t("classes.validation.academicYearMax"))
      .integer(t("classes.validation.academicYearInteger")),
    semester: Yup.number()
      .required(t("classes.validation.semesterRequired"))
      .oneOf([1, 2, 3], t("classes.validation.semesterInvalid")),
    room: Yup.string().required(t("classes.validation.roomRequired")),
    startTime: Yup.number()
      .required(t("classes.validation.startTimeRequired"))
      .min(0, t("classes.validation.startTimeMin"))
      .max(23, t("classes.validation.startTimeMax")),
    endTime: Yup.number()
      .required(t("classes.validation.endTimeRequired"))
      .min(0, t("classes.validation.endTimeMin"))
      .max(23, t("classes.validation.endTimeMax"))
      .test(
        "is-greater",
        t("classes.validation.endTimeGreater"),
        function (value) {
          return value > this.parent.startTime;
        }
      ),
    maxStudents: Yup.number()
      .required(t("classes.validation.maxStudentsRequired"))
      .min(1, t("classes.validation.maxStudentsMin"))
      .integer(t("classes.validation.maxStudentsInteger")),
    deadline: Yup.date()
      .required(t("classes.validation.deadlineRequired"))
      .min(new Date(), t("classes.validation.deadlineFuture")),
    dayOfWeek: Yup.number()
      .required(t("classes.validation.dayOfWeekRequired"))
      .oneOf([2, 3, 4, 5, 6, 7, 8], t("classes.validation.dayOfWeekInvalid")),
  });

export function ClassForm({
  classData = null,
  forCourseId = null,
}: ClassFormProps) {
  const router = useRouter();
  const { courses, searchCourses, getCourseApi } = useCourseSearch();
  const t = useTranslations();

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
    searchCourses({ page: 1, limit: 20 });
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, []);
  useEffect(() => {
    const fetchCourse = async () => {
      if (classData) {
        const response = await getCourseApi.call(classData.courseId);
        setSelectedCourse(response.data || null);
      }
    };
    fetchCourse();
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [classData]);

  useEffect(() => {
    const fetchCourse = async () => {
      if (forCourseId) {
        const response = await getCourseApi.call(forCourseId);
        setSelectedCourse(response.data || null);
        formik.setFieldValue("courseId", response.data?.courseId || "");
      }
    };
    fetchCourse();
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [forCourseId]);
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
    validationSchema: validationSchema(t),
    onSubmit: async (values) => {
      try {
        if (classData) {
          await updateClassApi.call({
            classId: classData.classId.toString(),
            classData: values,
          });
        } else {
          await addClassApi.call({
            ...values,
            id: values.classId,
            courseName: selectedCourse?.courseName || { vi: "", en: "" },
          });
        }
      } catch (error) {
        console.error("Error submitting class:", error);
      }
    },
  });

  const handleCourseSelect = useCallback(
    (course: Course | null) => {
      setSelectedCourse(course);
      formik.setFieldValue("courseId", course?.courseId || "");
    },
    [formik]
  );

  return (
    <Paper sx={{ p: 3 }}>
      <Box component="form" onSubmit={formik.handleSubmit} noValidate>
        <Typography variant="h6" gutterBottom>
          {classData ? t("classes.form.editTitle") : t("classes.form.addTitle")}
        </Typography>
        <Typography variant="body2" color="text.secondary">
          {t("classes.form.description")}
        </Typography>

        <Divider sx={{ my: 2 }} />

        <Grid2 container spacing={3}>
          <Grid2 size={{ xs: 12, md: 6 }}>
            <TextField
              required
              id="id"
              name="classId"
              label={t("classes.form.classId")}
              fullWidth
              variant="outlined"
              value={formik.values.classId}
              onChange={formik.handleChange}
              error={formik.touched.classId && Boolean(formik.errors.classId)}
              helperText={formik.touched.classId && formik.errors.classId}
              placeholder={t("classes.form.placeholders.classId")}
              disabled={!!classData}
            />
          </Grid2>

          <Grid2 size={{ xs: 12, md: 6 }}>
            <Autocomplete
              id="courseId"
              options={courses}
              disabled={getCourseApi.loading}
              getOptionLabel={(option) =>
                `${option.courseId} - ${option.courseName.vi} (${option.courseName.en})`
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
                  label={t("classes.form.course")}
                  placeholder={t("classes.form.searchCourse")}
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
              label={t("classes.form.teacher")}
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
              placeholder={t("classes.form.placeholders.teacher")}
            />
          </Grid2>

          <Grid2 size={{ xs: 12, md: 6 }}>
            <TextField
              required
              id="academicYear"
              name="academicYear"
              label={t("classes.form.academicYear")}
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
              <InputLabel id="semester-label">
                {t("classes.form.semester")}
              </InputLabel>
              <Select
                labelId="semester-label"
                id="semester"
                name="semester"
                value={formik.values.semester}
                label={t("classes.form.semester")}
                onChange={formik.handleChange}
              >
                <MenuItem value={1}>{t("classes.form.semester1")}</MenuItem>
                <MenuItem value={2}>{t("classes.form.semester2")}</MenuItem>
                <MenuItem value={3}>{t("classes.form.semester3")}</MenuItem>
              </Select>
            </FormControl>
          </Grid2>

          <Grid2 size={{ xs: 12, md: 6 }}>
            <TextField
              required
              id="room"
              name="room"
              label={t("classes.form.room")}
              fullWidth
              variant="outlined"
              value={formik.values.room}
              onChange={formik.handleChange}
              error={formik.touched.room && Boolean(formik.errors.room)}
              helperText={formik.touched.room && formik.errors.room}
              placeholder={t("classes.form.placeholders.room")}
            />
          </Grid2>

          <Grid2 size={{ xs: 12, md: 6 }}>
            <TextField
              required
              id="startTime"
              name="startTime"
              label={t("classes.form.startTime")}
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
              label={t("classes.form.endTime")}
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
              <InputLabel id="dayOfWeek-label">
                {t("classes.form.dayOfWeek")}
              </InputLabel>
              <Select
                labelId="dayOfWeek-label"
                id="dayOfWeek"
                name="dayOfWeek"
                value={formik.values.dayOfWeek}
                onChange={formik.handleChange}
              >
                <MenuItem value={2}>{t("classes.form.monday")}</MenuItem>
                <MenuItem value={3}>{t("classes.form.tuesday")}</MenuItem>
                <MenuItem value={4}>{t("classes.form.wednesday")}</MenuItem>
                <MenuItem value={5}>{t("classes.form.thursday")}</MenuItem>
                <MenuItem value={6}>{t("classes.form.friday")}</MenuItem>
                <MenuItem value={7}>{t("classes.form.saturday")}</MenuItem>
                <MenuItem value={8}>{t("classes.form.sunday")}</MenuItem>
              </Select>
            </FormControl>
          </Grid2>
          <Grid2 size={{ xs: 12, md: 6 }}>
            <TextField
              required
              id="maxStudents"
              name="maxStudents"
              label={t("classes.form.maxStudents")}
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
              label={t("classes.form.deadline")}
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
            {t("classes.form.cancel")}
          </Button>
          <Button
            type="submit"
            variant="contained"
            disabled={formik.isSubmitting}
          >
            {classData ? t("classes.form.update") : t("classes.form.create")}
          </Button>
        </Box>
      </Box>
    </Paper>
  );
}
