"use client";
import React, { useCallback, useEffect, useMemo, useState } from "react";
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
  Grid2,
  Typography,
  FormHelperText,
  Autocomplete,
  Drawer,
  Stack,
  FormControlLabel,
  Checkbox,
  Divider,
  Box,
} from "@mui/material";
import {
  Faculty,
  Gender,
  Program,
  Status,
  Student,
  validationStudent,
} from "../../../../types/student";
import { useFormik } from "formik";
import * as Yup from "yup";
import { Address } from "@/types/address";
import useFunction from "@/hooks/use-function";
import { AddressApi } from "@/api/address";
import RowStack from "@/components/row-stack";
import AddressStudentForm from "./address-student-form";

interface DrawerUpdateStudentProps {
  student: Student | null;
  open: boolean;
  onClose: () => void;
  addStudent: (student: Student) => void;
  updateStudent: (student: Student) => void;
  faculties: Faculty[];
  statuses: Status[];
  programs: Program[];
}

const IDENTITY_TYPES = [
  {
    key: "cccd",
    name: "Căn cước nhân dân",
  },
  {
    key: "cmmd",
    name: "Chứng minh nhân dân",
  },
  {
    key: "passport",
    name: "Hộ chiếu",
  },
];

function DrawerUpdateStudent({
  student,
  open,
  onClose,
  addStudent,
  updateStudent,
  faculties,
  statuses,
  programs,
}: DrawerUpdateStudentProps) {
  const getCountriesApi = useFunction(AddressApi.getCountries);

  const countries = useMemo(
    () => getCountriesApi.data || [],
    [getCountriesApi.data]
  );

  useEffect(() => {
    if (open) {
      getCountriesApi.call({});
    } else {
      formik.resetForm();
    }
    //  eslint-disable-next-line react-hooks/exhaustive-deps
  }, [open]);

  const parseAddress = (addressString?: string) => {
    if (!addressString)
      return {
        country: "Việt Nam",
        province: "",
        district: "",
        ward: "",
        detail: "",
      };
    try {
      return JSON.parse(addressString);
    } catch (e) {
      return {
        country: "Việt Nam",
        province: "",
        district: "",
        ward: "",
        detail: addressString,
      };
    }
  };

  const permanentAddress = parseAddress(student?.permanentAddress);
  const temporaryAddress = parseAddress(student?.temporaryAddress);
  const mailingAddress = parseAddress(student?.mailingAddress);

  const formik = useFormik({
    initialValues: {
      id: student?.id || "",
      name: student?.name || "",
      dateOfBirth: student?.dateOfBirth.split("T")[0] || "",
      gender: student?.gender || Gender.Male,
      email: student?.email || "",

      // Permanent address
      permanentCountry: permanentAddress.country || "Việt Nam",
      permanentProvince: permanentAddress.province || "",
      permanentDistrict: permanentAddress.district || "",
      permanentWard: permanentAddress.ward || "",
      permanentDetail: permanentAddress.detail || "",

      // Temporary address
      useTemporaryAddress: !!student?.temporaryAddress,
      temporaryCountry: temporaryAddress.country || "Việt Nam",
      temporaryProvince: temporaryAddress.province || "",
      temporaryDistrict: temporaryAddress.district || "",
      temporaryWard: temporaryAddress.ward || "",
      temporaryDetail: temporaryAddress.detail || "",

      // Mailing address
      useMailingAddress: !!student?.mailingAddress,
      mailingCountry: mailingAddress.country || "Việt Nam",
      mailingProvince: mailingAddress.province || "",
      mailingDistrict: mailingAddress.district || "",
      mailingWard: mailingAddress.ward || "",
      mailingDetail: mailingAddress.detail || "",

      // Academic info
      faculty: faculties.find((f) => f.id === student?.faculty)?.name || "",
      course: student?.course || 0,
      program: programs.find((p) => p.id === student?.program)?.name || "",
      phone: student?.phone || "",
      status: statuses.find((s) => s.id === student?.status)?.name || "",

      // Identity info
      identityType: student?.identity.type || 0,
      identityDocumentNumber: student?.identity.documentNumber || "",
      identityIssueDate: student?.identity.issueDate || new Date(),
      identityIssuePlace: student?.identity.issuePlace || "",
      identityExpiryDate: student?.identity.expiryDate || new Date(),
      identityCountry: student?.identity.country || "",
      identityIsChip: !!student?.identity.isChip,
      identityNotes: student?.identity.notes || "",
    },
    // enableReinitialize: true,
    validationSchema: validationStudent,
    onSubmit: async (values) => {
      const permanentAddress = {
        country: values.permanentCountry,
        province: values.permanentProvince,
        district: values.permanentDistrict,
        ward: values.permanentWard,
        detail: values.permanentDetail,
      };

      // Optional addresses
      let temporaryAddress;
      if (values.useTemporaryAddress) {
        temporaryAddress = {
          country: values.temporaryCountry,
          province: values.temporaryProvince,
          district: values.temporaryDistrict,
          ward: values.temporaryWard,
          detail: values.temporaryDetail,
        };
      }

      let mailingAddress;
      if (values.useMailingAddress) {
        mailingAddress = {
          country: values.mailingCountry,
          province: values.mailingProvince,
          district: values.mailingDistrict,
          ward: values.mailingWard,
          detail: values.mailingDetail,
        };
      }

      // Construct identity object
      const identity = {
        type: values.identityType,
        documentNumber: values.identityDocumentNumber,
        issueDate: new Date(values.identityIssueDate),
        issuePlace: values.identityIssuePlace,
        expiryDate: new Date(values.identityExpiryDate),
        country: values.identityCountry,
        isChip: values.identityIsChip,
        notes: values.identityNotes,
      };

      // Construct student object
      const studentData: Student = {
        id: values.id,
        name: values.name,
        dateOfBirth: values.dateOfBirth,
        gender: values.gender,
        email: values.email,
        permanentAddress: JSON.stringify(permanentAddress),
        temporaryAddress: values.useTemporaryAddress
          ? JSON.stringify(temporaryAddress)
          : undefined,
        mailingAddress: values.useMailingAddress
          ? JSON.stringify(mailingAddress)
          : undefined,
        faculty: values.faculty,
        course: values.course,
        program: values.program,
        phone: values.phone,
        status: values.status,
        identity: identity,
      };

      try {
        if (student) {
          updateStudent(studentData);
        } else {
          addStudent(studentData);
        }
      } catch (e) {
        console.error(e);
      }
    },
  });

  return (
    <Drawer
      open={open}
      onClose={onClose}
      anchor='right'
      PaperProps={{ sx: { width: { xs: "100%", md: "50%" } } }}
    >
      <Stack px={3} py={2} spacing={2} sx={{ overflowY: "auto" }}>
        <RowStack justifyContent='space-between' alignItems='center'>
          <Typography variant='h5'>
            {student ? "Cập nhật sinh viên" : "Thêm sinh viên"}
          </Typography>
          <RowStack gap={1.5}>
            <Button onClick={onClose} color='inherit' variant='outlined'>
              Hủy
            </Button>
            <Button
              onClick={() => formik.handleSubmit()}
              color='primary'
              variant='contained'
            >
              {student ? "Cập nhật" : "Thêm"}
            </Button>
          </RowStack>
        </RowStack>

        <Divider />

        {/* Basic Information */}
        <Typography variant='h6'>Thông tin cơ bản</Typography>
        <Grid2 container spacing={2}>
          <Grid2
            size={{
              xs: 12,
              md: 6,
            }}
          >
            <TextField
              autoFocus
              id='name'
              label='Họ và tên'
              fullWidth
              variant='outlined'
              {...formik.getFieldProps("name")}
              error={formik.touched.name && Boolean(formik.errors.name)}
              helperText={formik.touched.name && formik.errors.name}
            />
          </Grid2>
          <Grid2
            size={{
              xs: 12,
              md: 6,
            }}
          >
            <TextField
              id='email'
              label='Email'
              type='email'
              fullWidth
              variant='outlined'
              {...formik.getFieldProps("email")}
              error={formik.touched.email && Boolean(formik.errors.email)}
              helperText={formik.touched.email && formik.errors.email}
            />
          </Grid2>
          <Grid2
            size={{
              xs: 12,
              md: 4,
            }}
          >
            <TextField
              id='dateOfBirth'
              label='Ngày tháng năm sinh'
              type='date'
              fullWidth
              variant='outlined'
              InputLabelProps={{ shrink: true }}
              {...formik.getFieldProps("dateOfBirth")}
              error={
                formik.touched.dateOfBirth && Boolean(formik.errors.dateOfBirth)
              }
              helperText={
                formik.touched.dateOfBirth && formik.errors.dateOfBirth
              }
            />
          </Grid2>
          <Grid2
            size={{
              xs: 12,
              md: 4,
            }}
          >
            <FormControl fullWidth>
              <InputLabel>Giới tính</InputLabel>
              <Select
                id='gender'
                label='Giới tính'
                {...formik.getFieldProps("gender")}
              >
                <MenuItem value={Gender.Male}>Nam</MenuItem>
                <MenuItem value={Gender.Female}>Nữ</MenuItem>
                <MenuItem value={Gender.Other}>Khác</MenuItem>
              </Select>
            </FormControl>
          </Grid2>
          <Grid2
            size={{
              xs: 12,
              md: 4,
            }}
          >
            <TextField
              id='phone'
              label='Số điện thoại liên hệ'
              fullWidth
              variant='outlined'
              {...formik.getFieldProps("phone")}
              error={formik.touched.phone && Boolean(formik.errors.phone)}
              helperText={formik.touched.phone && formik.errors.phone}
            />
          </Grid2>
        </Grid2>

        <Divider />

        <AddressStudentForm formik={formik} open={open} countries={countries} />

        <Divider />

        {/* Academic Information */}
        <Typography variant='h6'>Thông tin học tập</Typography>
        <Grid2 container spacing={2}>
          <Grid2
            size={{
              xs: 12,
              md: 4,
            }}
          >
            <FormControl fullWidth>
              <InputLabel>Khoa</InputLabel>
              <Select
                id='faculty'
                label='Khoa'
                {...formik.getFieldProps("faculty")}
                error={formik.touched.faculty && Boolean(formik.errors.faculty)}
              >
                {faculties.map((faculty) => (
                  <MenuItem key={faculty.id} value={faculty.id}>
                    {faculty.name}
                  </MenuItem>
                ))}
              </Select>
              {formik.touched.faculty && formik.errors.faculty && (
                <FormHelperText error>{formik.errors.faculty}</FormHelperText>
              )}
            </FormControl>
          </Grid2>
          <Grid2
            size={{
              xs: 12,
              md: 4,
            }}
          >
            <FormControl fullWidth>
              <InputLabel>Chương trình</InputLabel>
              <Select
                id='program'
                label='Chương trình'
                {...formik.getFieldProps("program")}
                error={formik.touched.program && Boolean(formik.errors.program)}
              >
                {programs.map((program) => (
                  <MenuItem key={program.id} value={program.id}>
                    {program.name}
                  </MenuItem>
                ))}
              </Select>
              {formik.touched.program && formik.errors.program && (
                <FormHelperText error>{formik.errors.program}</FormHelperText>
              )}
            </FormControl>
          </Grid2>
          <Grid2
            size={{
              xs: 12,
              md: 4,
            }}
          >
            <TextField
              id='course'
              label='Khóa'
              type='number'
              fullWidth
              variant='outlined'
              {...formik.getFieldProps("course")}
              error={formik.touched.course && Boolean(formik.errors.course)}
              helperText={formik.touched.course && formik.errors.course}
            />
          </Grid2>
          <Grid2
            size={{
              xs: 12,
              md: 4,
            }}
          >
            <FormControl fullWidth>
              <InputLabel>Tình trạng sinh viên</InputLabel>
              <Select
                id='status'
                label='Tình trạng sinh viên'
                {...formik.getFieldProps("status")}
                error={formik.touched.status && Boolean(formik.errors.status)}
              >
                {statuses.map((status) => (
                  <MenuItem key={status.id} value={status.id}>
                    {status.name}
                  </MenuItem>
                ))}
              </Select>
              {formik.touched.status && formik.errors.status && (
                <FormHelperText error>{formik.errors.status}</FormHelperText>
              )}
            </FormControl>
          </Grid2>
        </Grid2>

        <Divider />

        {/* Identity Information */}
        <Typography variant='h6'>Thông tin giấy tờ tùy thân</Typography>
        <Grid2 container spacing={2}>
          <Grid2
            size={{
              xs: 12,
              md: 4,
            }}
          >
            <FormControl fullWidth>
              <InputLabel>Loại giấy tờ</InputLabel>
              <Select
                id='identityType'
                label='Loại giấy tờ'
                {...formik.getFieldProps("identityType")}
              >
                {IDENTITY_TYPES.map(({ key, name }) => (
                  <MenuItem key={key} value={key}>
                    {name}
                  </MenuItem>
                ))}
              </Select>
            </FormControl>
          </Grid2>
          <Grid2
            size={{
              xs: 12,
              md: 4,
            }}
          >
            <TextField
              id='documentNumber'
              label='Số giấy tờ'
              fullWidth
              variant='outlined'
              {...formik.getFieldProps("documentNumber")}
              error={
                formik.touched.identityDocumentNumber &&
                Boolean(formik.errors.identityDocumentNumber)
              }
              helperText={
                formik.touched.identityDocumentNumber &&
                String(formik.errors.identityDocumentNumber)
              }
            />
          </Grid2>
          <Grid2
            size={{
              xs: 12,
              md: 4,
            }}
          >
            <FormControl fullWidth>
              <InputLabel>Quốc gia cấp</InputLabel>
              <Select
                id='identityCountry'
                label='Quốc gia cấp'
                {...formik.getFieldProps("identityCountry")}
              >
                {countries.map((country) => (
                  <MenuItem key={country.name} value={country.name}>
                    {country.name}
                  </MenuItem>
                ))}
              </Select>
            </FormControl>
          </Grid2>
          <Grid2
            size={{
              xs: 12,
              md: 4,
            }}
          >
            <TextField
              id='issueDate'
              label='Ngày cấp'
              type='date'
              fullWidth
              variant='outlined'
              InputLabelProps={{ shrink: true }}
              {...formik.getFieldProps("issueDate")}
              error={
                formik.touched.identityIssueDate &&
                Boolean(formik.errors.identityIssueDate)
              }
              helperText={
                formik.touched.identityIssueDate &&
                String(formik.errors.identityIssueDate)
              }
            />
          </Grid2>
          <Grid2
            size={{
              xs: 12,
              md: 4,
            }}
          >
            <TextField
              id='issuePlace'
              label='Nơi cấp'
              fullWidth
              variant='outlined'
              {...formik.getFieldProps("issuePlace")}
              error={
                formik.touched.identityIssuePlace &&
                Boolean(formik.errors.identityIssuePlace)
              }
              helperText={
                formik.touched.identityIssuePlace &&
                String(formik.errors.identityIssuePlace)
              }
            />
          </Grid2>
          <Grid2
            size={{
              xs: 12,
              md: 4,
            }}
          >
            <TextField
              id='expiryDate'
              label='Ngày hết hạn'
              type='date'
              fullWidth
              variant='outlined'
              InputLabelProps={{ shrink: true }}
              {...formik.getFieldProps("expiryDate")}
              error={
                formik.touched.identityExpiryDate &&
                Boolean(formik.errors.identityExpiryDate)
              }
              helperText={
                formik.touched.identityExpiryDate &&
                String(formik.errors.identityExpiryDate)
              }
            />
          </Grid2>
          <Grid2
            size={{
              xs: 12,
              md: 6,
            }}
          >
            <FormControlLabel
              control={
                <Checkbox
                  checked={formik.values.identityIsChip}
                  onChange={(e) =>
                    formik.setFieldValue("isChip", e.target.checked)
                  }
                  name='isChip'
                />
              }
              label='Thẻ chip'
            />
          </Grid2>
          <Grid2 size={12}>
            <TextField
              id='identityNotes'
              label='Ghi chú'
              fullWidth
              multiline
              rows={2}
              variant='outlined'
              {...formik.getFieldProps("identityNotes")}
            />
          </Grid2>
        </Grid2>
      </Stack>
    </Drawer>
  );
}

export default DrawerUpdateStudent;
