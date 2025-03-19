"use client";
import React, { useCallback, useEffect, useMemo } from "react";
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
} from "@mui/material";
import { Faculty, Gender, Status, Student } from "../../../types/student";
import { useFormik } from "formik";
import * as Yup from "yup";
import { Address } from "@/types/address";
import useFunction from "@/hooks/use-function";
import { AddressApi } from "@/api/address";

interface DialogProps {
  student: Student | null;
  open: boolean;
  onClose: () => void;
  addStudent: (student: Student) => void;
  updateStudent: (student: Student) => void;
}

function Dialog({
  student,
  open,
  onClose,
  addStudent,
  updateStudent,
}: DialogProps) {
  const getCountriesApi = useFunction(AddressApi.getCountries);
  const getProvincesApi = useFunction(AddressApi.getProvinces);
  const getDistrictOfProvincesApi = useFunction(
    AddressApi.getDistrictOfProvinces
  );
  const getWardOfDistrictApi = useFunction(AddressApi.getWardOfDistrict);

  useEffect(() => {
    if (open) {
      getCountriesApi.call({});
      getProvincesApi.call({});
    }
    //  eslint-disable-next-line react-hooks/exhaustive-deps
  }, [open]);

  const provinces = useMemo(
    () => getProvincesApi.data || [],
    [getProvincesApi.data]
  );
  const countries = useMemo(
    () => getCountriesApi.data || [],
    [getCountriesApi.data]
  );

  const districts = useMemo(
    () => getDistrictOfProvincesApi.data?.districts || [],
    [getDistrictOfProvincesApi.data]
  );

  const wards = useMemo(
    () => getWardOfDistrictApi.data?.wards || [],
    [getWardOfDistrictApi.data]
  );

  const parseAddress = useCallback((addressString: string): Address => {
    try {
      if (addressString) {
        const addressObj = JSON.parse(addressString);
        return {
          province: addressObj.province || "",
          district: addressObj.district || "",
          ward: addressObj.ward || "",
          street: addressObj.detail || "",
          country: "Việt Nam",
        };
      }
    } catch (e) {}
    return {
      province: "",
      district: "",
      ward: "",
      street: "",
      country: "",
    };
  }, []);

  const initialAddress = useMemo(
    () =>
      student?.address
        ? parseAddress(student.address)
        : { province: "", district: "", ward: "", street: "", country: "" },
    [student, parseAddress]
  );

  const formik = useFormik({
    initialValues: {
      id: student?.id || "",
      name: student?.name || "",
      dateOfBirth: student?.dateOfBirth.split("T")[0] || "",
      gender: student?.gender || Gender.Male,
      email: student?.email || "",
      province: initialAddress.province,
      district: initialAddress.district,
      ward: initialAddress.ward,
      street: initialAddress.street,
      country: initialAddress.country,
      faculty: student?.faculty.name || "",
      course: student?.course || 0,
      program: student?.program.name || "",
      phone: student?.phone || "",
      status: student?.status.name || "",
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
      const addressObject = {
        province: values.province,
        district: values.district,
        ward: values.ward,
        street: values.street,
        country: values.country,
      };
      try {
        if (student) {
          updateStudent({
            ...student,
            ...values,
            address: JSON.stringify(addressObject),
          });
        } else {
          addStudent({
            ...values,
            address: JSON.stringify(addressObject),
          });
        }
      } catch (e) {
        console.error(e);
      }
    },
  });

  useEffect(() => {
    if (!open) {
      formik.resetForm();
    }
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [open]);

  useEffect(() => {
    if (formik.values.province) {
      const province = provinces.find(
        (province) => province.name === formik.values.province
      );
      if (!province) return;

      getDistrictOfProvincesApi.call({ province_code: province.code });
    }
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [formik.values.province]);

  useEffect(() => {
    if (formik.values.district) {
      const district = districts.find(
        (district) => district.name === formik.values.district
      );
      if (!district) return;

      getWardOfDistrictApi.call({ district_code: district.code });
    }
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [formik.values.district]);

  return (
    <MuiDialog open={open} onClose={onClose}>
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
        <Grid2 container spacing={2} sx={{ mt: 1 }}>
          <Grid2
            size={{
              xs: 12,
              md: 6,
            }}
          >
            <TextField
              id='dateOfBirth'
              label='Ngày tháng năm sinh'
              type='date'
              fullWidth
              variant='outlined'
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
              md: 6,
            }}
          >
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
          </Grid2>
        </Grid2>
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
        <Grid2 container spacing={2} sx={{ mt: 1 }}>
          <Grid2 size={12}>
            <Typography variant='subtitle1' sx={{ mb: 1 }}>
              Địa chỉ thường trú
            </Typography>
          </Grid2>

          <Grid2
            size={{
              xs: 12,
              md: 6,
            }}
          >
            <FormControl
              fullWidth
              error={formik.touched.province && Boolean(formik.errors.province)}
            >
              <InputLabel>Quốc gia</InputLabel>
              <Select
                id='country'
                label='Quốc gia'
                {...formik.getFieldProps("country")}
              >
                {countries.map((country, index) => (
                  <MenuItem key={index} value={country.name}>
                    {country.name}
                  </MenuItem>
                ))}
              </Select>
              {formik.touched.province && formik.errors.province && (
                <FormHelperText>{formik.errors.province}</FormHelperText>
              )}
            </FormControl>
          </Grid2>

          <Grid2
            size={{
              xs: 12,
              md: 6,
            }}
          >
            <FormControl
              fullWidth
              error={formik.touched.province && Boolean(formik.errors.province)}
            >
              <Autocomplete
                id='province'
                options={provinces}
                getOptionLabel={(option) => option.name}
                value={
                  provinces.find((p) => p.name === formik.values.province) ||
                  null
                }
                onChange={(event, newValue) => {
                  formik.setFieldValue(
                    "province",
                    newValue ? newValue.name : ""
                  );
                }}
                renderInput={(params) => (
                  <TextField
                    {...params}
                    label='Tỉnh/Thành phố'
                    variant='outlined'
                    error={
                      formik.touched.province && Boolean(formik.errors.province)
                    }
                    helperText={
                      formik.touched.province && formik.errors.province
                    }
                  />
                )}
              />
            </FormControl>
          </Grid2>

          <Grid2
            size={{
              xs: 12,
              md: 6,
            }}
          >
            <FormControl
              fullWidth
              error={formik.touched.district && Boolean(formik.errors.district)}
              disabled={!formik.values.province}
            >
              <Autocomplete
                id='district'
                options={districts}
                getOptionLabel={(option) => option.name}
                value={
                  districts.find((d) => d.name === formik.values.district) ||
                  null
                }
                onChange={(event, newValue) => {
                  formik.setFieldValue(
                    "district",
                    newValue ? newValue.name : ""
                  );
                }}
                renderInput={(params) => (
                  <TextField
                    {...params}
                    label='Huyện/Quận'
                    variant='outlined'
                    error={
                      formik.touched.district && Boolean(formik.errors.district)
                    }
                    helperText={
                      formik.touched.district && formik.errors.district
                    }
                  />
                )}
              />
            </FormControl>
          </Grid2>

          <Grid2
            size={{
              xs: 12,
              md: 6,
            }}
          >
            <FormControl
              fullWidth
              error={formik.touched.ward && Boolean(formik.errors.ward)}
              disabled={!formik.values.district}
            >
              <Autocomplete
                id='ward'
                options={wards}
                getOptionLabel={(option) => option.name}
                value={wards.find((w) => w.name === formik.values.ward) || null}
                onChange={(event, newValue) => {
                  formik.setFieldValue("ward", newValue ? newValue.name : "");
                }}
                renderInput={(params) => (
                  <TextField
                    {...params}
                    label='Phường/Xã'
                    variant='outlined'
                    error={formik.touched.ward && Boolean(formik.errors.ward)}
                    helperText={formik.touched.ward && formik.errors.ward}
                  />
                )}
              />
            </FormControl>
          </Grid2>

          <Grid2 size={12}>
            <TextField
              id='addressDetail'
              label='Địa chỉ chi tiết (số nhà, đường, thôn, xóm...)'
              fullWidth
              variant='outlined'
              placeholder='Ví dụ: Tổ 2, thôn Vĩnh Xuân'
              {...formik.getFieldProps("addressDetail")}
              error={formik.touched.street && Boolean(formik.errors.street)}
              helperText={formik.touched.street && formik.errors.street}
            />
          </Grid2>
        </Grid2>
        <Grid2 container spacing={2} sx={{ mt: 2 }}>
          <Grid2
            size={{
              xs: 12,
              md: 6,
            }}
          >
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
          </Grid2>
          <Grid2
            size={{
              xs: 12,
              md: 6,
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
        </Grid2>
        <Grid2 container spacing={2} sx={{ mt: 2 }}>
          <Grid2
            size={{
              xs: 12,
              md: 4,
            }}
          >
            <TextField
              id='phone'
              label='Số điện thoại liên hệ'
              type='text'
              fullWidth
              variant='outlined'
              {...formik.getFieldProps("phone")}
              error={formik.touched.phone && Boolean(formik.errors.phone)}
              helperText={formik.touched.phone && formik.errors.phone}
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
          </Grid2>
          <Grid2
            size={{
              xs: 12,
              md: 4,
            }}
          >
            <TextField
              id='program'
              label='Chương trình'
              type='text'
              fullWidth
              variant='outlined'
              {...formik.getFieldProps("program")}
              error={formik.touched.program && Boolean(formik.errors.program)}
              helperText={formik.touched.program && formik.errors.program}
            />
          </Grid2>
        </Grid2>
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
