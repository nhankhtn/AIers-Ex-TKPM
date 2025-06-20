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

interface MailingAddressProps {
  formik: any;
  countries: any[];
  provinces: any[];
  districtsMA: any[];
  wardsMA: any[];
}

function MailingAddress({
  formik,
  countries,
  provinces,
  districtsMA,
  wardsMA,
}: MailingAddressProps) {
  const t = useTranslations("dashboard.list");
  return (
    <>
      <Grid2 container spacing={2} sx={{ mt: 1 }}>
        <Grid2 size={12}>
          <Typography variant="subtitle1">{t("mailingAddress")}</Typography>
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
              id="mailingCountry"
              label={t("country")}
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
            id="mailingProvince"
            freeSolo={formik.values.mailingCountry !== COUNTRY_DEFAULT}
            options={
              formik.values.mailingCountry !== COUNTRY_DEFAULT ? [] : provinces
            }
            getOptionLabel={(option) =>
              typeof option === "string" ? option : option.name
            }
            value={
              provinces.find((p) => p.name === formik.values.mailingProvince) ||
              null
            }
            onChange={(_, newValue) => {
              formik.setFieldValue(
                "mailingProvince",
                newValue
                  ? typeof newValue === "string"
                    ? newValue
                    : newValue.name
                  : ""
              );
              formik.setFieldValue("mailingDistrict", "");
              formik.setFieldValue("mailingWard", "");
            }}
            onInputChange={(_, newInputValue) => {
              if (formik.values.mailingCountry !== COUNTRY_DEFAULT) {
                formik.setFieldValue("mailingProvince", newInputValue);
              }
            }}
            renderInput={(params) => (
              <TextField
                {...params}
                label={t("province")}
                error={
                  formik.touched.mailingProvince &&
                  Boolean(formik.errors.mailingProvince)
                }
                helperText={
                  formik.touched.mailingProvince &&
                  String(formik.errors.mailingProvince || "")
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
            id="mailingDistrict"
            options={
              formik.values.mailingCountry !== COUNTRY_DEFAULT
                ? []
                : districtsMA
            }
            freeSolo={formik.values.mailingCountry !== COUNTRY_DEFAULT}
            getOptionLabel={(option) =>
              typeof option === "string" ? option : option.name
            }
            value={
              districtsMA.find(
                (d) => d.name === formik.values.mailingDistrict
              ) || null
            }
            onChange={(_, newValue) => {
              formik.setFieldValue(
                "mailingDistrict",
                newValue
                  ? typeof newValue === "string"
                    ? newValue
                    : newValue.name
                  : ""
              );
              formik.setFieldValue("mailingWard", "");
            }}
            onInputChange={(_, newInputValue) => {
              if (formik.values.mailingCountry !== COUNTRY_DEFAULT) {
                formik.setFieldValue("mailingDistrict", newInputValue);
              }
            }}
            disabled={
              !formik.values.mailingProvince &&
              formik.values.mailingCountry === COUNTRY_DEFAULT
            }
            renderInput={(params) => (
              <TextField
                {...params}
                label={t("district")}
                error={
                  formik.touched.mailingDistrict &&
                  Boolean(formik.errors.mailingDistrict)
                }
                helperText={
                  formik.touched.mailingDistrict &&
                  String(formik.errors.mailingDistrict || "")
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
            id="mailingWard"
            options={
              formik.values.mailingCountry !== COUNTRY_DEFAULT ? [] : wardsMA
            }
            freeSolo={formik.values.mailingCountry !== COUNTRY_DEFAULT}
            getOptionLabel={(option) =>
              typeof option === "string" ? option : option.name
            }
            value={
              wardsMA.find((w) => w.name === formik.values.mailingWard) || null
            }
            onChange={(_, newValue) => {
              formik.setFieldValue(
                "mailingWard",
                newValue
                  ? typeof newValue === "string"
                    ? newValue
                    : newValue.name
                  : ""
              );
            }}
            onInputChange={(_, newInputValue) => {
              if (formik.values.mailingCountry !== COUNTRY_DEFAULT) {
                formik.setFieldValue("mailingWard", newInputValue);
              }
            }}
            disabled={
              !formik.values.mailingDistrict &&
              formik.values.mailingCountry === COUNTRY_DEFAULT
            }
            renderInput={(params) => (
              <TextField
                {...params}
                label={t("ward")}
                error={
                  formik.touched.mailingWard &&
                  Boolean(formik.errors.mailingWard)
                }
                helperText={
                  formik.touched.mailingWard &&
                  String(formik.errors.mailingWard || "")
                }
              />
            )}
          />
        </Grid2>
        <Grid2 size={12}>
          <TextField
            id="mailingDetail"
            label={t("addressDetail")}
            fullWidth
            variant="outlined"
            placeholder={t("addressDetailPlaceholder")}
            value={formik.values.mailingDetail}
            onChange={formik.handleChange}
            error={
              formik.touched.mailingDetail &&
              Boolean(formik.errors.mailingDetail)
            }
            helperText={
              formik.touched.mailingDetail &&
              String(formik.errors.mailingDetail || "")
            }
          />
        </Grid2>
      </Grid2>
    </>
  );
}

export default MailingAddress;
