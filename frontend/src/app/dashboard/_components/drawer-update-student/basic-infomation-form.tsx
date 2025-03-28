"use client";
import {
  Button,
  TextField,
  Select,
  MenuItem,
  FormControl,
  InputLabel,
  Grid2,
  Typography,
  IconButton,
} from "@mui/material";
import { Gender } from "../../../../types/student";
import RowStack from "@/components/row-stack";
import { countriesPhoneFormat } from "@/utils/phone-helper";
import { useMainContext } from "@/context";
import { useDialog } from "@/hooks/use-dialog";
import DialogConfigEmail from "../dialog-config-email";
import { Edit } from "@mui/icons-material";
interface BasicInfomationFormProps {
  formik: any;
  countries: {
    name: string;
  }[];
}
function BasicInfomationForm({ formik, countries }: BasicInfomationFormProps) {
  const { settings } = useMainContext();
  const dialogConfigEmail = useDialog();

  return (
    <>
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
            value={formik.values.name}
            onChange={formik.handleChange}
            error={formik.touched.name && Boolean(formik.errors.name)}
            helperText={formik.touched.name && String(formik.errors.name || "")}
          />
        </Grid2>
        <Grid2
          size={{
            xs: 12,
            md: 6,
          }}
        >
          <RowStack gap={1}>
            <TextField
              id='email'
              label='Email'
              type='email'
              fullWidth
              variant='outlined'
              placeholder={settings.allowedEmailDomains.join(", ")}
              value={formik.values.email}
              onChange={formik.handleChange}
              error={formik.touched.email && Boolean(formik.errors.email)}
              helperText={
                formik.touched.email && String(formik.errors.email || "")
              }
            />
            <IconButton onClick={dialogConfigEmail.handleOpen}>
              <Edit />
            </IconButton>
          </RowStack>
        </Grid2>
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
            InputLabelProps={{ shrink: true }}
            value={formik.values.dateOfBirth}
            onChange={formik.handleChange}
            error={
              formik.touched.dateOfBirth && Boolean(formik.errors.dateOfBirth)
            }
            helperText={
              formik.touched.dateOfBirth &&
              String(formik.errors.dateOfBirth || "")
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
              id='gender'
              label='Giới tính'
              value={formik.values.gender}
              onChange={(event) =>
                formik.setFieldValue("gender", event.target.value)
              }
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
            md: 6,
          }}
        >
          <RowStack gap={1}>
            <FormControl sx={{ minWidth: "65px" }}>
              <InputLabel>Quốc gia</InputLabel>
              <Select
                id='phoneCode'
                label='Quốc tịch'
                value={formik.values.phoneCode}
                onChange={(event) =>
                  formik.setFieldValue("phoneCode", event.target.value)
                }
              >
                {countriesPhoneFormat.map((country) => (
                  <MenuItem key={country.name} value={country.name}>
                    {country.name}({country.format})
                  </MenuItem>
                ))}
              </Select>
            </FormControl>
            <TextField
              id='phone'
              label='Số điện thoại'
              sx={{ flex: "1" }}
              variant='outlined'
              value={formik.values.phone}
              onChange={formik.handleChange}
              error={formik.touched.phone && Boolean(formik.errors.phone)}
              helperText={
                formik.touched.phone && String(formik.errors.phone || "")
              }
            />
          </RowStack>
        </Grid2>
        <Grid2
          size={{
            xs: 12,
            md: 6,
          }}
        >
          <FormControl fullWidth>
            <InputLabel>Quốc tịch</InputLabel>
            <Select
              id='nationality'
              label='Quốc tịch'
              value={formik.values.nationality}
              onChange={(event) =>
                formik.setFieldValue("nationality", event.target.value)
              }
            >
              {countries.map((country) => (
                <MenuItem key={country.name} value={country.name}>
                  {country.name}
                </MenuItem>
              ))}
            </Select>
          </FormControl>
        </Grid2>
      </Grid2>
      <DialogConfigEmail
        open={dialogConfigEmail.open}
        onClose={dialogConfigEmail.handleClose}
        allowedEmail={settings.allowedEmailDomains}
      />
    </>
  );
}

export default BasicInfomationForm;
