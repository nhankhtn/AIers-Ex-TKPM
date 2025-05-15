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
import { useLocale, useTranslations } from "next-intl";

interface CourseFormProps {
  course?: Course | null;
}

const validationSchema = (t: any) =>
  Yup.object().shape({
    courseId: Yup.string()
      .required(t("courses.form.validation.courseIdRequired"))
      .matches(/^[a-zA-Z0-9]+$/, t("courses.form.validation.courseIdFormat")),
    courseNameVi: Yup.string()
      .required(t("courses.form.validation.courseNameViRequired"))
      .min(3, t("courses.form.validation.courseNameMinLength")),
    courseNameEn: Yup.string()
      .required(t("courses.form.validation.courseNameEnRequired"))
      .min(3, t("courses.form.validation.courseNameMinLength")),
    credits: Yup.number()
      .required(t("courses.form.validation.creditsRequired"))
      .min(2, t("courses.form.validation.creditsMin"))
      .integer(t("courses.form.validation.creditsInteger")),
    facultyId: Yup.string().required(
      t("courses.form.validation.facultyRequired")
    ),
    descriptionVi: Yup.string().required(
      t("courses.form.validation.descriptionViRequired")
    ),
    descriptionEn: Yup.string().required(
      t("courses.form.validation.descriptionEnRequired")
    ),
    requiredCourseId: Yup.string()
      .nullable()
      .test(
        "not-self",
        t("courses.form.validation.requiredCourseNotSelf"),
        function (value) {
          if (!value || !this.parent.courseId) return true;
          return value !== this.parent.courseId;
        }
      ),
  });

export function CourseForm({ course = null }: CourseFormProps) {
  const router = useRouter();
  const { faculties } = useMainContext();
  const t = useTranslations();
  const locale = useLocale() as "en" | "vi";  
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
    validationSchema: validationSchema(t),
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
      <Box component="form" onSubmit={formik.handleSubmit} noValidate>
        <Typography variant="h6" gutterBottom>
          {course ? t("courses.form.editTitle") : t("courses.form.addTitle")}
        </Typography>
        <Typography variant="body2" color="text.secondary" paragraph>
          {t("courses.form.description")}
        </Typography>

        <Divider sx={{ my: 2 }} />

        <Grid2 container spacing={3}>
          <Grid2 size={{ xs: 12, md: 6 }}>
            <TextField
              required
              id="courseId"
              name="courseId"
              label={t("courses.form.courseId")}
              disabled={!!course}
              fullWidth
              variant="outlined"
              value={formik.values.courseId}
              onChange={formik.handleChange}
              error={formik.touched.courseId && Boolean(formik.errors.courseId)}
              helperText={formik.touched.courseId && formik.errors.courseId}
              placeholder={t("courses.form.placeholders.courseId")}
            />
          </Grid2>
          <Grid2 size={{ xs: 12, md: 6 }}>
            <TextField
              required
              id="courseNameVi"
              name="courseNameVi"
              label={t("courses.form.courseNameVi")}
              fullWidth
              variant="outlined"
              value={formik.values.courseNameVi}
              onChange={formik.handleChange}
              error={
                formik.touched.courseNameVi &&
                Boolean(formik.errors.courseNameVi)
              }
              helperText={
                formik.touched.courseNameVi && formik.errors.courseNameVi
              }
              placeholder={t("courses.form.placeholders.courseNameVi")}
            />
          </Grid2>
          <Grid2 size={{ xs: 12, md: 6 }}>
            <TextField
              required
              id="courseNameEn"
              name="courseNameEn"
              label={t("courses.form.courseNameEn")}
              fullWidth
              variant="outlined"
              value={formik.values.courseNameEn}
              onChange={formik.handleChange}
              error={
                formik.touched.courseNameEn &&
                Boolean(formik.errors.courseNameEn)
              }
              helperText={
                formik.touched.courseNameEn && formik.errors.courseNameEn
              }
              placeholder={t("courses.form.placeholders.courseNameEn")}
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
              <InputLabel id="faculty-label">
                {t("courses.form.faculty")}
              </InputLabel>
              <Select
                labelId="faculty-label"
                id="facultyId"
                name="facultyId"
                value={formik.values.facultyId}
                label={t("courses.form.faculty")}
                onChange={formik.handleChange}
              >
                {faculties.map((faculty) => (
                  <MenuItem key={faculty.id} value={faculty.id}>
                    {faculty.name[locale]}
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
              label={t("courses.form.credits")}
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
                  label={t("courses.form.requiredCourse")}
                  placeholder={t("courses.form.placeholders.requiredCourse")}
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
              id="descriptionVi"
              name="descriptionVi"
              label={t("courses.form.descriptionVi")}
              fullWidth
              multiline
              rows={4}
              variant="outlined"
              value={formik.values.descriptionVi}
              onChange={formik.handleChange}
              error={
                formik.touched.descriptionVi &&
                Boolean(formik.errors.descriptionVi)
              }
              helperText={
                formik.touched.descriptionVi && formik.errors.descriptionVi
              }
              placeholder={t("courses.form.placeholders.descriptionVi")}
            />
          </Grid2>
          <Grid2 size={{ xs: 12, md: 6 }}>
            <TextField
              required
              id="descriptionEn"
              name="descriptionEn"
              label={t("courses.form.descriptionEn")}
              fullWidth
              multiline
              rows={4}
              variant="outlined"
              value={formik.values.descriptionEn}
              onChange={formik.handleChange}
              error={
                formik.touched.descriptionEn &&
                Boolean(formik.errors.descriptionEn)
              }
              helperText={
                formik.touched.descriptionEn && formik.errors.descriptionEn
              }
              placeholder={t("courses.form.placeholders.descriptionEn")}
            />
          </Grid2>
        </Grid2>

        <Box
          sx={{ display: "flex", justifyContent: "flex-end", gap: 2, mt: 3 }}
        >
          <Button variant="outlined" onClick={() => router.push("/courses")}>
            {t("common.actions.cancel")}
          </Button>
          <Button
            type="submit"
            variant="contained"
            disabled={formik.isSubmitting}
          >
            {course ? t("common.actions.save") : t("common.actions.add")}
          </Button>
        </Box>
      </Box>
    </Paper>
  );
}
