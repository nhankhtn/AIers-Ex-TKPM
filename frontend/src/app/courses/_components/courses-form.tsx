"use client";

import { useState, type ChangeEvent, type FormEvent } from "react";
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
  type SelectChangeEvent,
} from "@mui/material";
import type { Course } from "@/types/course";

interface CourseFormProps {
  course?: Course | null;
}

interface CourseFormData {
  id?: string;
  name: string;
  credits: number;
  faculty: string;
  description: string;
  requiredCourseId?: string;
  requiredCourseName?: string;
  isDeleted?: string;
}

interface FormError {
  [key: string]: string | null;
}

export function CourseForm({ course = null }: CourseFormProps) {
  const router = useRouter();
  const [formData, setFormData] = useState<CourseFormData>({
    id: course?.id,
    name: course?.name || "",
    credits: course?.credits || 3,
    faculty: course?.faculty || "",
    description: course?.description || "",
    requiredCourseId: course?.requiredCourseId,
    requiredCourseName: course?.requiredCourseName,
    isDeleted: course?.isDeleted,
  });
  const [errors, setErrors] = useState<FormError>({});

  const handleChange = (
    e: ChangeEvent<HTMLInputElement | HTMLTextAreaElement>
  ): void => {
    const { name, value } = e.target;
    setFormData({
      ...formData,
      [name]: value,
    });

    // Clear error when field is edited
    if (errors[name]) {
      setErrors({
        ...errors,
        [name]: null,
      });
    }
  };

  const handleSelectChange = (e: SelectChangeEvent<string>): void => {
    const { name, value } = e.target;
    setFormData({
      ...formData,
      [name]: value,
    });

    // Clear error when field is edited
    if (errors[name]) {
      setErrors({
        ...errors,
        [name]: null,
      });
    }
  };

  const validateForm = (): boolean => {
    const newErrors: FormError = {};

    if (!formData.name.trim()) {
      newErrors.name = "Tên khóa học không được để trống";
    }

    if (!formData.faculty) {
      newErrors.faculty = "Vui lòng chọn khoa phụ trách";
    }

    const credits = Number.parseInt(formData.credits.toString());
    if (isNaN(credits) || credits < 2) {
      newErrors.credits = "Số tín chỉ phải lớn hơn hoặc bằng 2";
    }

    setErrors(newErrors);
    return Object.keys(newErrors).length === 0;
  };

  const handleSubmit = (e: FormEvent<HTMLFormElement>): void => {
    e.preventDefault();

    if (!validateForm()) {
      return;
    }

    // In a real app, this would call an API to save the course
    console.log("Submitting course:", formData);

    // Redirect back to courses list
    router.push("/courses");
  };

  return (
    <Paper sx={{ p: 3 }}>
      <Box component="form" onSubmit={handleSubmit} noValidate>
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
              id="name"
              name="name"
              label="Tên khóa học"
              fullWidth
              variant="outlined"
              value={formData.name}
              onChange={handleChange}
              error={Boolean(errors.name)}
              helperText={errors.name}
              placeholder="VD: Nhập môn lập trình"
            />
          </Grid2>
          <Grid2 size={{ xs: 12, md: 6 }}>
            <FormControl fullWidth required error={Boolean(errors.faculty)}>
              <InputLabel id="faculty-label">Khoa phụ trách</InputLabel>
              <Select
                labelId="faculty-label"
                id="faculty"
                name="faculty"
                value={formData.faculty}
                label="Khoa phụ trách"
                onChange={handleSelectChange}
              >
                <MenuItem value="CNTT">Công nghệ thông tin</MenuItem>
                <MenuItem value="MATH">Toán học</MenuItem>
                <MenuItem value="PHYS">Vật lý</MenuItem>
                <MenuItem value="CHEM">Hóa học</MenuItem>
                <MenuItem value="ECON">Kinh tế</MenuItem>
              </Select>
              {errors.faculty && (
                <FormHelperText>{errors.faculty}</FormHelperText>
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
              value={formData.credits}
              onChange={handleChange}
              error={Boolean(errors.credits)}
              helperText={errors.credits}
              InputProps={{ inputProps: { min: 2 } }}
            />
          </Grid2>
          <Grid2 size={{ xs: 12 }}>
            <TextField
              id="requiredCourseId"
              name="requiredCourseId"
              label="Môn tiên quyết"
              fullWidth
              variant="outlined"
              value={formData.requiredCourseId || ""}
              onChange={handleChange}
              placeholder="VD: CS101"
              helperText="Nhập mã khóa học tiên quyết"
            />
          </Grid2>
          <Grid2 size={{ xs: 12 }}>
            <TextField
              id="description"
              name="description"
              label="Mô tả khóa học"
              fullWidth
              multiline
              rows={4}
              variant="outlined"
              value={formData.description}
              onChange={handleChange}
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
          <Button type="submit" variant="contained">
            {course ? "Cập nhật khóa học" : "Tạo khóa học"}
          </Button>
        </Box>
      </Box>
    </Paper>
  );
}
