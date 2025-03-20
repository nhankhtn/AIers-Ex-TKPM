"use client";

import { AddressApi } from "@/api/address";
import RowStack from "@/components/row-stack";
import useFunction from "@/hooks/use-function";
import { Country } from "@/types/address";
import { InfoOutlined } from "@mui/icons-material";
import {
  TextField,
  Select,
  MenuItem,
  FormControl,
  InputLabel,
  Grid2,
  Typography,
  Autocomplete,
  FormControlLabel,
  Checkbox,
  Box,
  Tooltip,
} from "@mui/material";
import { useEffect, useMemo } from "react";

const AddressStudentForm = ({
  formik,
  open,
  countries,
}: {
  formik: any;
  open: boolean;
  countries: Country[];
}) => {
  const getProvincesApi = useFunction(AddressApi.getProvinces);
  const getDistrictOfProvincesPAApi = useFunction(
    AddressApi.getDistrictOfProvinces
  );
  const getDistrictOfProvincesTAApi = useFunction(
    AddressApi.getDistrictOfProvinces
  );
  const getDistrictOfProvincesMAApi = useFunction(
    AddressApi.getDistrictOfProvinces
  );
  const getWardOfDistrictPAApi = useFunction(AddressApi.getWardOfDistrict);
  const getWardOfDistrictTAApi = useFunction(AddressApi.getWardOfDistrict);
  const getWardOfDistrictMAApi = useFunction(AddressApi.getWardOfDistrict);

  useEffect(() => {
    if (open) {
      getProvincesApi.call({});
    }
    //  eslint-disable-next-line react-hooks/exhaustive-deps
  }, [open]);

  const provinces = useMemo(
    () => getProvincesApi.data || [],
    [getProvincesApi.data]
  );

  const districtsPA = useMemo(
    () => getDistrictOfProvincesPAApi.data?.districts || [],
    [getDistrictOfProvincesPAApi.data]
  );
  const districtsTA = useMemo(
    () => getDistrictOfProvincesPAApi.data?.districts || [],
    [getDistrictOfProvincesPAApi.data]
  );

  const districtsMA = useMemo(
    () => getDistrictOfProvincesPAApi.data?.districts || [],
    [getDistrictOfProvincesPAApi.data]
  );

  const wardsPA = useMemo(
    () => getWardOfDistrictPAApi.data?.wards || [],
    [getWardOfDistrictPAApi.data]
  );
  const wardsTA = useMemo(
    () => getWardOfDistrictPAApi.data?.wards || [],
    [getWardOfDistrictPAApi.data]
  );
  const wardsMA = useMemo(
    () => getWardOfDistrictPAApi.data?.wards || [],
    [getWardOfDistrictPAApi.data]
  );
  useEffect(() => {
    if (formik.values.permanentProvince) {
      const province = provinces.find(
        (province) => province.name === formik.values.permanentProvince
      );
      if (!province) return;

      getDistrictOfProvincesPAApi.call({ province_code: province.code });
    }
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [formik.values.permanentProvince]);
  useEffect(() => {
    if (formik.values.temporaryCountry) {
      const province = provinces.find(
        (province) => province.name === formik.values.temporaryCountry
      );
      if (!province) return;

      getDistrictOfProvincesTAApi.call({ province_code: province.code });
    }
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [formik.values.temporaryCountry]);

  useEffect(() => {
    if (formik.values.mailingProvince) {
      const province = provinces.find(
        (province) => province.name === formik.values.mailingProvince
      );
      if (!province) return;

      getDistrictOfProvincesMAApi.call({ province_code: province.code });
    }
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [formik.values.mailingProvince]);

  useEffect(() => {
    if (formik.values.permanentDistrict) {
      const district = districtsPA.find(
        (district) => district.name === formik.values.permanentDistrict
      );
      if (!district) return;

      getWardOfDistrictPAApi.call({ district_code: district.code });
    }
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [formik.values.permanentDistrict]);
  useEffect(() => {
    if (formik.values.temporaryDistrict) {
      const district = districtsTA.find(
        (district) => district.name === formik.values.temporaryDistrict
      );
      if (!district) return;

      getWardOfDistrictTAApi.call({ district_code: district.code });
    }
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [formik.values.temporaryDistrict]);
  useEffect(() => {
    if (formik.values.mailingDistrict) {
      const district = districtsMA.find(
        (district) => district.name === formik.values.mailingDistrict
      );
      if (!district) return;

      getWardOfDistrictMAApi.call({ district_code: district.code });
    }
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [formik.values.mailingDistrict]);

  return (
    <>
      <Typography variant='h6'>Địa chỉ thường trú</Typography>
      <Grid2 container spacing={2}>
        <Grid2
          size={{
            xs: 12,
            md: 6,
          }}
        >
          <FormControl fullWidth>
            <InputLabel>Quốc gia</InputLabel>
            <Select
              id='permanentCountry'
              label='Quốc gia'
              {...formik.getFieldProps("permanentCountry")}
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
            md: 6,
          }}
        >
          <Autocomplete
            id='permanentProvince'
            options={provinces}
            getOptionLabel={(option) => option.name}
            value={
              provinces.find(
                (p) => p.name === formik.values.permanentProvince
              ) || null
            }
            onChange={(_, newValue) => {
              formik.setFieldValue(
                "permanentProvince",
                newValue ? newValue.name : ""
              );
              formik.setFieldValue("permanentDistrict", "");
              formik.setFieldValue("permanentWard", "");
            }}
            renderInput={(params) => (
              <TextField
                {...params}
                label='Tỉnh/Thành phố'
                error={
                  formik.touched.permanentProvince &&
                  Boolean(formik.errors.permanentProvince)
                }
                helperText={
                  formik.touched.permanentProvince &&
                  String(formik.errors.permanentProvince)
                }
              />
            )}
          />
        </Grid2>
        <Grid2
          size={{
            xs: 12,
            md: 6,
          }}
        >
          <Autocomplete
            id='permanentDistrict'
            options={districtsPA}
            getOptionLabel={(option) => option.name}
            value={
              districtsPA.find(
                (d) => d.name === formik.values.permanentDistrict
              ) || null
            }
            onChange={(_, newValue) => {
              formik.setFieldValue(
                "permanentDistrict",
                newValue ? newValue.name : ""
              );
              formik.setFieldValue("permanentWard", "");
            }}
            disabled={!formik.values.permanentProvince}
            renderInput={(params) => (
              <TextField
                {...params}
                label='Quận/Huyện'
                error={
                  formik.touched.permanentDistrict &&
                  Boolean(formik.errors.permanentDistrict)
                }
                helperText={
                  formik.touched.permanentDistrict &&
                  String(formik.errors.permanentDistrict)
                }
              />
            )}
          />
        </Grid2>
        <Grid2
          size={{
            xs: 12,
            md: 6,
          }}
        >
          <Autocomplete
            id='permanentWard'
            options={wardsPA}
            getOptionLabel={(option) => option.name}
            value={
              wardsPA.find((w) => w.name === formik.values.permanentWard) ||
              null
            }
            onChange={(_, newValue) => {
              formik.setFieldValue(
                "permanentWard",
                newValue ? newValue.name : ""
              );
            }}
            disabled={!formik.values.permanentDistrict}
            renderInput={(params) => (
              <TextField
                {...params}
                label='Phường/Xã'
                error={
                  formik.touched.permanentWard &&
                  Boolean(formik.errors.permanentWard)
                }
                helperText={
                  formik.touched.permanentWard &&
                  String(formik.errors.permanentWard)
                }
              />
            )}
          />
        </Grid2>
        <Grid2 size={12}>
          <TextField
            id='permanentDetail'
            label='Địa chỉ chi tiết (số nhà, đường, thôn, xóm...)'
            fullWidth
            variant='outlined'
            placeholder='Ví dụ: Tổ 2, thôn Vĩnh Xuân'
            {...formik.getFieldProps("permanentDetail")}
            error={
              formik.touched.permanentDetail &&
              Boolean(formik.errors.permanentDetail)
            }
            helperText={
              formik.touched.permanentDetail &&
              String(formik.errors.permanentDetail)
            }
          />
        </Grid2>
      </Grid2>

      {/* Temporary Address */}
      <Box>
        <RowStack>
          <FormControlLabel
            control={
              <Checkbox
                checked={formik.values.useTemporaryAddress}
                onChange={() => {
                  formik.setFieldValue(
                    "useTemporaryAddress",
                    !formik.values.useTemporaryAddress
                  );
                }}
                name='useTemporaryAddress'
              />
            }
            label='Thêm địa chỉ tạm trú'
          />
          <Tooltip
            title='Địa chỉ tạm trú là địa chỉ bạn đang ở hiện tại, nếu khác với địa chỉ thường trú'
            placement='top'
          >
            <InfoOutlined
              sx={{
                color: "action.active",
              }}
            />
          </Tooltip>
        </RowStack>

        {formik.values.useTemporaryAddress && (
          <Grid2 container spacing={2} sx={{ mt: 1 }}>
            <Grid2 size={12}>
              <Typography variant='subtitle1'>Địa chỉ tạm trú</Typography>
            </Grid2>
            <Grid2
              size={{
                xs: 12,
                md: 6,
              }}
            >
              <FormControl fullWidth>
                <InputLabel>Quốc gia</InputLabel>
                <Select
                  id='temporaryCountry'
                  label='Quốc gia'
                  {...formik.getFieldProps("temporaryCountry")}
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
                md: 6,
              }}
            >
              <Autocomplete
                id='temporaryProvince'
                options={provinces}
                getOptionLabel={(option) => option.name}
                value={
                  provinces.find(
                    (p) => p.name === formik.values.temporaryProvince
                  ) || null
                }
                onChange={(_, newValue) => {
                  formik.setFieldValue(
                    "temporaryProvince",
                    newValue ? newValue.name : ""
                  );
                  formik.setFieldValue("temporaryDistrict", "");
                  formik.setFieldValue("temporaryWard", "");
                }}
                renderInput={(params) => (
                  <TextField
                    {...params}
                    label='Tỉnh/Thành phố'
                    error={
                      formik.touched.temporaryProvince &&
                      Boolean(formik.errors.temporaryProvince)
                    }
                    helperText={
                      formik.touched.temporaryProvince &&
                      String(formik.errors.temporaryProvince)
                    }
                  />
                )}
              />
            </Grid2>
            <Grid2
              size={{
                xs: 12,
                md: 6,
              }}
            >
              <Autocomplete
                id='temporaryDistrict'
                options={districtsTA}
                getOptionLabel={(option) => option.name}
                value={
                  districtsTA.find(
                    (d) => d.name === formik.values.temporaryDistrict
                  ) || null
                }
                onChange={(_, newValue) => {
                  formik.setFieldValue(
                    "temporaryDistrict",
                    newValue ? newValue.name : ""
                  );
                  formik.setFieldValue("temporaryWard", "");
                }}
                disabled={!formik.values.temporaryProvince}
                renderInput={(params) => (
                  <TextField
                    {...params}
                    label='Quận/Huyện'
                    error={
                      formik.touched.temporaryDistrict &&
                      Boolean(formik.errors.temporaryDistrict)
                    }
                    helperText={
                      formik.touched.temporaryDistrict &&
                      String(formik.errors.temporaryDistrict)
                    }
                  />
                )}
              />
            </Grid2>
            <Grid2 size={12}>
              <Autocomplete
                id='temporaryWard'
                options={wardsTA}
                getOptionLabel={(option) => option.name}
                value={
                  wardsTA.find((w) => w.name === formik.values.temporaryWard) ||
                  null
                }
                onChange={(_, newValue) => {
                  formik.setFieldValue(
                    "temporaryWard",
                    newValue ? newValue.name : ""
                  );
                }}
                disabled={!formik.values.temporaryDistrict}
                renderInput={(params) => (
                  <TextField
                    {...params}
                    label='Phường/Xã'
                    error={
                      formik.touched.temporaryWard &&
                      Boolean(formik.errors.temporaryWard)
                    }
                    helperText={
                      formik.touched.temporaryWard &&
                      String(formik.errors.temporaryWard)
                    }
                  />
                )}
              />
            </Grid2>
            <Grid2 size={12}>
              <TextField
                id='temporaryDetail'
                label='Địa chỉ chi tiết (số nhà, đường, thôn, xóm...)'
                fullWidth
                variant='outlined'
                placeholder='Ví dụ: Tổ 2, thôn Vĩnh Xuân'
                {...formik.getFieldProps("temporaryDetail")}
                error={
                  formik.touched.temporaryDetail &&
                  Boolean(formik.errors.temporaryDetail)
                }
                helperText={
                  formik.touched.temporaryDetail &&
                  String(formik.errors.temporaryDetail)
                }
              />
            </Grid2>
          </Grid2>
        )}
      </Box>

      {/* Mailing Address */}
      <Box>
        <RowStack>
          <FormControlLabel
            control={
              <Checkbox
                checked={formik.values.useMailingAddress}
                onChange={() => {
                  formik.setFieldValue(
                    "useMailingAddress",
                    !formik.values.useMailingAddress
                  );
                }}
                name='useMailingAddress'
              />
            }
            label='Thêm địa chỉ nhận thư'
          />
          <Tooltip
            title='Địa chỉ nhận thư là địa chỉ bạn nhận thư từ trường, nếu khác với địa chỉ thường trú'
            placement='top'
          >
            <InfoOutlined
              sx={{
                color: "action.active",
              }}
            />
          </Tooltip>
        </RowStack>

        {formik.values.useMailingAddress && (
          <Grid2 container spacing={2} sx={{ mt: 1 }}>
            <Grid2 size={12}>
              <Typography variant='subtitle1'>Địa chỉ nhận thư</Typography>
            </Grid2>
            <Grid2
              size={{
                xs: 12,
                md: 6,
              }}
            >
              <FormControl fullWidth>
                <InputLabel>Quốc gia</InputLabel>
                <Select
                  id='mailingCountry'
                  label='Quốc gia'
                  {...formik.getFieldProps("mailingCountry")}
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
                md: 6,
              }}
            >
              <Autocomplete
                id='mailingProvince'
                options={provinces}
                getOptionLabel={(option) => option.name}
                value={
                  provinces.find(
                    (p) => p.name === formik.values.mailingProvince
                  ) || null
                }
                onChange={(_, newValue) => {
                  formik.setFieldValue(
                    "mailingProvince",
                    newValue ? newValue.name : ""
                  );
                  formik.setFieldValue("mailingDistrict", "");
                  formik.setFieldValue("mailingWard", "");
                }}
                renderInput={(params) => (
                  <TextField
                    {...params}
                    label='Tỉnh/Thành phố'
                    error={
                      formik.touched.mailingProvince &&
                      Boolean(formik.errors.mailingProvince)
                    }
                    helperText={
                      formik.touched.mailingProvince &&
                      String(formik.errors.mailingProvince)
                    }
                  />
                )}
              />
            </Grid2>
            <Grid2
              size={{
                xs: 12,
                md: 6,
              }}
            >
              <Autocomplete
                id='mailingDistrict'
                options={districtsMA}
                getOptionLabel={(option) => option.name}
                value={
                  districtsMA.find(
                    (d) => d.name === formik.values.mailingDistrict
                  ) || null
                }
                onChange={(_, newValue) => {
                  formik.setFieldValue(
                    "mailingDistrict",
                    newValue ? newValue.name : ""
                  );
                  formik.setFieldValue("mailingWard", "");
                }}
                disabled={!formik.values.mailingProvince}
                renderInput={(params) => (
                  <TextField
                    {...params}
                    label='Quận/Huyện'
                    error={
                      formik.touched.mailingDistrict &&
                      Boolean(formik.errors.mailingDistrict)
                    }
                    helperText={
                      formik.touched.mailingDistrict &&
                      String(formik.errors.mailingDistrict)
                    }
                  />
                )}
              />
            </Grid2>
            <Grid2
              size={{
                xs: 12,
                md: 6,
              }}
            >
              <Autocomplete
                id='mailingWard'
                options={wardsMA}
                getOptionLabel={(option) => option.name}
                value={
                  wardsMA.find((w) => w.name === formik.values.mailingWard) ||
                  null
                }
                onChange={(_, newValue) => {
                  formik.setFieldValue(
                    "mailingWard",
                    newValue ? newValue.name : ""
                  );
                }}
                disabled={!formik.values.mailingDistrict}
                renderInput={(params) => (
                  <TextField
                    {...params}
                    label='Phường/Xã'
                    error={
                      formik.touched.mailingWard &&
                      Boolean(formik.errors.mailingWard)
                    }
                    helperText={
                      formik.touched.mailingWard &&
                      String(formik.errors.mailingWard)
                    }
                  />
                )}
              />
            </Grid2>
            <Grid2 size={12}>
              <TextField
                id='mailingDetail'
                label='Địa chỉ chi tiết (số nhà, đường, thôn, xóm...)'
                fullWidth
                variant='outlined'
                placeholder='Ví dụ: Tổ 2, thôn Vĩnh Xuân'
                {...formik.getFieldProps("mailingDetail")}
                error={
                  formik.touched.mailingDetail &&
                  Boolean(formik.errors.mailingDetail)
                }
                helperText={
                  formik.touched.mailingDetail &&
                  String(formik.errors.mailingDetail)
                }
              />
            </Grid2>
          </Grid2>
        )}
      </Box>
    </>
  );
};

export default AddressStudentForm;
