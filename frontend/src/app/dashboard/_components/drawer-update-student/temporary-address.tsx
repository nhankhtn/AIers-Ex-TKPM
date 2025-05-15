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
  Typography,
} from "@mui/material";
import { useTranslations } from "next-intl";

interface TemporaryAddressProps {
  formik: any;
  countries: Country[];
  provinces: Province[];
  districtsTA: District[];
  wardsTA: Ward[];
}
function TemporaryAddress({
  formik,
  countries,
  provinces,
  districtsTA,
  wardsTA,
}: TemporaryAddressProps) {
  const t = useTranslations("dashboard.list");
  return (
    <>
      <Grid2 container spacing={2} sx={{ mt: 1 }}>
        <Grid2 size={12}>
          <Typography variant="subtitle1">{t("temporaryAddress")}</Typography>
        </Grid2>
        <Grid2
          size={{
            xs: 12,
            md: 6,
          }}
        >
          <FormControl fullWidth>
            <InputLabel>{t("country")}</InputLabel>
            <Select
              id="temporaryCountry"
              label={t("country")}
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
            freeSolo={formik.values.temporaryCountry !== COUNTRY_DEFAULT}
            id="temporaryProvince"
            options={provinces}
            getOptionLabel={(option) =>
              typeof option === "string" ? option : option.name
            }
            value={
              provinces.find(
                (p) => p.name === formik.values.temporaryProvince
              ) || null
            }
            onChange={(_, newValue) => {
              formik.setFieldValue(
                "temporaryProvince",
                newValue
                  ? typeof newValue === "string"
                    ? newValue
                    : newValue.name
                  : ""
              );
              formik.setFieldValue("temporaryDistrict", "");
              formik.setFieldValue("temporaryWard", "");
            }}
            renderInput={(params) => (
              <TextField
                {...params}
                label={t("province")}
                error={
                  formik.touched.temporaryProvince &&
                  Boolean(formik.errors.temporaryProvince)
                }
                helperText={
                  formik.touched.temporaryProvince &&
                  String(formik.errors.temporaryProvince || "")
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
            id="temporaryDistrict"
            options={districtsTA}
            freeSolo={formik.values.temporaryCountry !== COUNTRY_DEFAULT}
            getOptionLabel={(option) =>
              typeof option === "string" ? option : option.name
            }
            value={
              districtsTA.find(
                (d) => d.name === formik.values.temporaryDistrict
              ) || null
            }
            onChange={(_, newValue) => {
              formik.setFieldValue(
                "temporaryDistrict",
                newValue
                  ? typeof newValue === "string"
                    ? newValue
                    : newValue.name
                  : ""
              );
              formik.setFieldValue("temporaryWard", "");
            }}
            disabled={
              !formik.values.temporaryProvince &&
              formik.values.temporaryCountry === COUNTRY_DEFAULT
            }
            renderInput={(params) => (
              <TextField
                {...params}
                label={t("district")}
                error={
                  formik.touched.temporaryDistrict &&
                  Boolean(formik.errors.temporaryDistrict)
                }
                helperText={
                  formik.touched.temporaryDistrict &&
                  String(formik.errors.temporaryDistrict || "")
                }
              />
            )}
          />
        </Grid2>
        <Grid2 size={12}>
          <Autocomplete
            id="temporaryWard"
            freeSolo={formik.values.temporaryCountry !== COUNTRY_DEFAULT}
            options={wardsTA}
            getOptionLabel={(option) =>
              typeof option === "string" ? option : option.name
            }
            value={
              wardsTA.find((w) => w.name === formik.values.temporaryWard) ||
              null
            }
            onChange={(_, newValue) => {
              formik.setFieldValue(
                "temporaryWard",
                newValue
                  ? typeof newValue === "string"
                    ? newValue
                    : newValue.name
                  : ""
              );
            }}
            disabled={
              !formik.values.temporaryDistrict &&
              formik.values.temporaryCountry === COUNTRY_DEFAULT
            }
            renderInput={(params) => (
              <TextField
                {...params}
                label={t("ward")}
                error={
                  formik.touched.temporaryWard &&
                  Boolean(formik.errors.temporaryWard)
                }
                helperText={
                  formik.touched.temporaryWard &&
                  String(formik.errors.temporaryWard || "")
                }
              />
            )}
          />
        </Grid2>
        <Grid2 size={12}>
          <TextField
            id="temporaryDetail"
            label={t("addressDetail")}
            fullWidth
            variant="outlined"
            placeholder={t("addressDetailPlaceholder")}
            value={formik.values.temporaryDetail}
            onChange={formik.handleChange}
            error={
              formik.touched.temporaryDetail &&
              Boolean(formik.errors.temporaryDetail)
            }
            helperText={
              formik.touched.temporaryDetail &&
              String(formik.errors.temporaryDetail || "")
            }
          />
        </Grid2>
      </Grid2>
    </>
  );
}

export default TemporaryAddress;
