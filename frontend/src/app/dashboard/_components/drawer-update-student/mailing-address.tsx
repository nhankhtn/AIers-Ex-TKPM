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
interface MailingAddressProps {
  formik: any;
  countries: any[];
  provinces: any[];
  districtsMA: any[];
  wardsMA: any[];
}
function MailingAddress({ formik, countries, provinces, districtsMA, wardsMA }: MailingAddressProps) {
  return (
    <>
      <Grid2 container spacing={2} sx={{ mt: 1 }}>
        <Grid2 size={12}>
          <Typography variant="subtitle1">Địa chỉ nhận thư</Typography>
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
              id="mailingCountry"
              label="Quốc gia"
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
            options={provinces}
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
            renderInput={(params) => (
              <TextField
                {...params}
                label="Tỉnh/Thành phố"
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
            options={districtsMA}
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
            disabled={
              !formik.values.mailingProvince &&
              formik.values.mailingCountry === COUNTRY_DEFAULT
            }
            renderInput={(params) => (
              <TextField
                {...params}
                label="Quận/Huyện"
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
            options={wardsMA}
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
            disabled={
              !formik.values.mailingDistrict &&
              formik.values.mailingCountry === COUNTRY_DEFAULT
            }
            renderInput={(params) => (
              <TextField
                {...params}
                label="Phường/Xã"
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
            label="Địa chỉ chi tiết (số nhà, đường, thôn, xóm...)"
            fullWidth
            variant="outlined"
            placeholder="Ví dụ: Tổ 2, thôn Vĩnh Xuân"
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
