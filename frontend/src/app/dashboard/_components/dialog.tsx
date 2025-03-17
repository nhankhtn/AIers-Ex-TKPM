"use client";
import React, { useCallback, useEffect } from "react";
import {
  Button,
  TextField,
  DialogActions,
  Dialog as MuiDialog,
  DialogContent,
  DialogTitle,
  Select,
  MenuItem,
  FormControl,
  InputLabel,
} from "@mui/material";
import { Faculty, Gender, Status, Student } from "../../../types/student";
import { useFormik } from "formik";
import * as Yup from "yup";

interface DialogProps {
  student: Student | null;
  isOpen: boolean;
  onClose: () => void;
  addStudent: (student: Student) => void;
  updateStudent: (student: Student) => void;
}

function Dialog({
  student,
  isOpen,
  onClose,
  addStudent,
  updateStudent,
}: DialogProps) {
  const formik = useFormik({
    initialValues: {
      id: student?.id || "",
      name: student?.name || "",
      dateOfBirth: student?.dateOfBirth.split("T")[0] || "",
      gender: student?.gender || Gender.Male,
      email: student?.email || "",
      address: student?.address || "",
      faculty: student?.faculty || Faculty.Law,
      course: student?.course || 0,
      program: student?.program || "",
      phone: student?.phone || "",
      status: student?.status || Status.Studying,
    },
    enableReinitialize: true,
    validationSchema: Yup.object().shape({
      name: Yup.string().required("Vui lòng nhập họ và tên"),
      dateOfBirth: Yup.string().required("Vui lòng nhập ngày tháng năm sinh"),
      email: Yup.string()
        .email("Email không hợp lệ")
        .required("Vui lòng nhập email"),
      address: Yup.string().required("Vui lòng nhập địa chỉ"),
      phone: Yup.string().required("Vui lòng nhập số điện thoại"),
      program: Yup.string().required("Vui lòng nhập chương trình"),
      course: Yup.number().required("Vui lòng nhập khóa học"),
    }),
    onSubmit: async (values) => {
      try {
        if (student) {
          updateStudent(values);
        } else {
          addStudent(values);
        }
      } catch (e) {
        console.error(e);
      }
    },
  });

  useEffect(() => {
    if (!isOpen) {
      formik.resetForm();
    }
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [isOpen]);

  return (
    <MuiDialog open={isOpen} onClose={onClose}>
      <DialogTitle>
        {student ? "Cập nhật sinh viên" : "Thêm sinh viên"}
      </DialogTitle>
      <DialogContent>
        <TextField
          autoFocus
          margin='dense'
          id='name'
          label='Họ và tên'
          type='text'
          fullWidth
          variant='outlined'
          {...formik.getFieldProps("name")}
          error={formik.touched.name && Boolean(formik.errors.name)}
          helperText={formik.touched.name && formik.errors.name}
        />
        <TextField
          margin='dense'
          id='dateOfBirth'
          label='Ngày tháng năm sinh'
          type='date'
          fullWidth
          variant='outlined'
          {...formik.getFieldProps("dateOfBirth")}
          error={
            formik.touched.dateOfBirth && Boolean(formik.errors.dateOfBirth)
          }
          helperText={formik.touched.dateOfBirth && formik.errors.dateOfBirth}
        />
        <FormControl fullWidth>
          <InputLabel>Giới tính</InputLabel>
          <Select
            margin='dense'
            id='gender'
            label='Giới tính'
            fullWidth
            variant='outlined'
            {...formik.getFieldProps("gender")}
          >
            <MenuItem value={Gender.Male}>Nam</MenuItem>
            <MenuItem value={Gender.Female}>Nữ</MenuItem>
            <MenuItem value={Gender.Other}>Khác</MenuItem>
          </Select>
        </FormControl>

        <TextField
          margin='dense'
          id='email'
          label='Email'
          type='email'
          fullWidth
          variant='outlined'
          {...formik.getFieldProps("email")}
          error={formik.touched.email && Boolean(formik.errors.email)}
          helperText={formik.touched.email && formik.errors.email}
        />
        <TextField
          margin='dense'
          id='address'
          label='Địa chỉ'
          type='text'
          fullWidth
          variant='outlined'
          {...formik.getFieldProps("address")}
          error={formik.touched.address && Boolean(formik.errors.address)}
          helperText={formik.touched.address && formik.errors.address}
        />
        <FormControl fullWidth>
          <InputLabel>Khoa</InputLabel>
          <Select
            margin='dense'
            id='faculty'
            label='Khoa'
            fullWidth
            variant='outlined'
            {...formik.getFieldProps("faculty")}
          >
            <MenuItem value={Faculty.Law}>Khoa Luật</MenuItem>
            <MenuItem value={Faculty.BusinessEnglish}>
              Khoa Tiếng Anh thương mại
            </MenuItem>
            <MenuItem value={Faculty.Japanese}>Khoa Tiếng Nhật</MenuItem>
            <MenuItem value={Faculty.French}>Khoa Tiếng Pháp</MenuItem>
          </Select>
        </FormControl>

        <TextField
          margin='dense'
          id='course'
          label='Khóa'
          type='number'
          fullWidth
          variant='outlined'
          {...formik.getFieldProps("course")}
          error={formik.touched.course && Boolean(formik.errors.course)}
          helperText={formik.touched.course && formik.errors.course}
        />
        <TextField
          margin='dense'
          id='program'
          label='Chương trình'
          type='text'
          fullWidth
          variant='outlined'
          {...formik.getFieldProps("program")}
          error={formik.touched.program && Boolean(formik.errors.program)}
          helperText={formik.touched.program && formik.errors.program}
        />
        <TextField
          margin='dense'
          id='phone'
          label='Số điện thoại liên hệ'
          type='text'
          fullWidth
          variant='outlined'
          {...formik.getFieldProps("phone")}
          error={formik.touched.phone && Boolean(formik.errors.phone)}
          helperText={formik.touched.phone && formik.errors.phone}
        />

        <FormControl fullWidth>
          <InputLabel>Tình trạng sinh viên</InputLabel>
          <Select
            margin='dense'
            id='status'
            label='Tình trạng sinh viên'
            fullWidth
            variant='outlined'
            {...formik.getFieldProps("status")}
          >
            <MenuItem value={Status.Studying}>Đang học</MenuItem>
            <MenuItem value={Status.Graduated}>Đã tốt nghiệp</MenuItem>
            <MenuItem value={Status.Droppedout}>Đã thôi học</MenuItem>
            <MenuItem value={Status.Paused}>Tạm dừng học</MenuItem>
          </Select>
        </FormControl>
      </DialogContent>
      <DialogActions>
        <Button onClick={onClose} color='secondary' variant='contained'>
          Hủy
        </Button>
        <Button
          onClick={() => formik.handleSubmit()}
          color='primary'
          variant='contained'
        >
          {student ? "Cập nhật" : "Thêm"}
        </Button>
      </DialogActions>
    </MuiDialog>
  );
}

export default Dialog;
