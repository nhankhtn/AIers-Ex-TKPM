"use client";

import { Country, Province, District, Ward } from "@/types/address";
import { COUNTRY_DEFAULT } from "@/types/student";
import {
  TextField,
  Select,
  MenuItem,
  FormControl,
  InputLabel,
  Grid2,
  Autocomplete,
} from "@mui/material";
import { useTranslations } from "next-intl";

interface PermanentAddressProps {
  formik: any;
  countries: Country[];
  provinces: Province[];
  districtsPA: District[];
  wardsPA: Ward[];
}
function PermanentAddress({
  formik,
  countries,
  provinces,
  districtsPA,
  wardsPA,
}: PermanentAddressProps) {
  const t = useTranslations("dashboard.list");
  return (
    <>
      <Grid2 container spacing={2}>
        <Grid2
          size={{
            xs: 12,
            md: 6,
          }}
        >
          <FormControl fullWidth>
            <InputLabel>{t("country")}</InputLabel>
            <Select
              id="permanentCountry"
              label={t("country")}
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
            freeSolo={formik.values.permanentCountry !== COUNTRY_DEFAULT}
            id="permanentProvince"
            options={provinces}
            getOptionLabel={(option) =>
              typeof option === "string" ? option : option.name
            }
            value={
              provinces.find(
                (p) => p.name === formik.values.permanentProvince
              ) || null
            }
            onChange={(_, newValue) => {
              formik.setFieldValue(
                "permanentProvince",
                newValue
                  ? typeof newValue === "string"
                    ? newValue
                    : newValue.name
                  : ""
              );
              formik.setFieldValue("permanentDistrict", "");
              formik.setFieldValue("permanentWard", "");
            }}
            renderInput={(params) => (
              <TextField
                {...params}
                label={t("province")}
                error={
                  formik.touched.permanentProvince &&
                  Boolean(formik.errors.permanentProvince)
                }
                helperText={
                  formik.touched.permanentProvince &&
                  String(formik.errors.permanentProvince || "")
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
            freeSolo={formik.values.permanentCountry !== COUNTRY_DEFAULT}
            id="permanentDistrict"
            options={districtsPA}
            getOptionLabel={(option) =>
              typeof option === "string" ? option : option.name
            }
            value={
              districtsPA.find(
                (d) => d.name === formik.values.permanentDistrict
              ) || null
            }
            onChange={(_, newValue) => {
              formik.setFieldValue(
                "permanentDistrict",
                newValue
                  ? typeof newValue === "string"
                    ? newValue
                    : newValue.name
                  : ""
              );
              formik.setFieldValue("permanentWard", "");
            }}
            disabled={
              !formik.values.permanentProvince &&
              formik.values.permanentCountry === COUNTRY_DEFAULT
            }
            renderInput={(params) => (
              <TextField
                {...params}
                label={t("district")}
                error={
                  formik.touched.permanentDistrict &&
                  Boolean(formik.errors.permanentDistrict)
                }
                helperText={
                  formik.touched.permanentDistrict &&
                  String(formik.errors.permanentDistrict || "")
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
            freeSolo={formik.values.permanentCountry !== COUNTRY_DEFAULT}
            id="permanentWard"
            options={wardsPA}
            getOptionLabel={(option) =>
              typeof option === "string" ? option : option.name
            }
            value={
              wardsPA.find((w) => w.name === formik.values.permanentWard) ||
              null
            }
            onChange={(_, newValue) => {
              formik.setFieldValue(
                "permanentWard",
                newValue
                  ? typeof newValue === "string"
                    ? newValue
                    : newValue.name
                  : ""
              );
            }}
            disabled={
              !formik.values.permanentDistrict &&
              formik.values.permanentCountry === COUNTRY_DEFAULT
            }
            renderInput={(params) => (
              <TextField
                {...params}
                label={t("ward")}
                error={
                  formik.touched.permanentWard &&
                  Boolean(formik.errors.permanentWard)
                }
                helperText={
                  formik.touched.permanentWard &&
                  String(formik.errors.permanentWard || "")
                }
              />
            )}
          />
        </Grid2>
        <Grid2 size={12}>
          <TextField
            id="permanentDetail"
            label={t("addressDetail")}
            fullWidth
            variant="outlined"
            placeholder={t("addressDetailPlaceholder")}
            value={formik.values.permanentDetail}
            onChange={formik.handleChange}
            error={
              formik.touched.permanentDetail &&
              Boolean(formik.errors.permanentDetail)
            }
            helperText={
              formik.touched.permanentDetail &&
              String(formik.errors.permanentDetail || "")
            }
          />
        </Grid2>
      </Grid2>
    </>
  );
}

export default PermanentAddress;
