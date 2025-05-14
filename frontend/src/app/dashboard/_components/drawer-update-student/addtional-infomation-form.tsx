"use client";
import React from "react";
import {
  TextField,
  Select,
  MenuItem,
  FormControl,
  InputLabel,
  Grid2,
  Typography,
  FormHelperText,
  FormControlLabel,
  Checkbox,
  Divider,
} from "@mui/material";
import { Faculty, Program, Status } from "@/types/student";
import { Country } from "@/types/address";
import { IDENTITY_TYPES } from "./drawer-update-student";

const AdditionalInformationForm = ({
  formik,
  faculties,
  programs,
  statuses,
  countries,
}: {
  formik: any;
  faculties: Faculty[];
  programs: Program[];
  statuses: Status[];
  countries: Country[];
}) => {
  return (
    <>
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
              value={formik.values.faculty}
              onChange={(event) =>
                formik.setFieldValue("faculty", event.target.value)
              }
              error={formik.touched.faculty && Boolean(formik.errors.faculty)}
            >
              {faculties.map((faculty) => (
                <MenuItem key={faculty.id} value={faculty.id}>
                  {faculty.name.vi} ({faculty.name.en})
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
              value={formik.values.program}
              onChange={(event) =>
                formik.setFieldValue("program", event.target.value)
              }
              error={formik.touched.program && Boolean(formik.errors.program)}
            >
              {programs.map((program) => (
                <MenuItem key={program.id} value={program.id}>
                  {program.name.vi} ({program.name.en})
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
            value={formik.values.course}
            onChange={(event) =>
              formik.setFieldValue("course", event.target.value)
            }
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
              value={formik.values.status}
              onChange={(event) =>
                formik.setFieldValue("status", event.target.value)
              }
              error={formik.touched.status && Boolean(formik.errors.status)}
            >
              {statuses.map((status) => (
                <MenuItem key={status.id} value={status.id}>
                  {status.name.vi} ({status.name.en})
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
              value={formik.values.identityType}
              onChange={(event) =>
                formik.setFieldValue("identityType", event.target.value)
              }
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
            value={formik.values.identityDocumentNumber}
            onChange={(event) =>
              formik.setFieldValue("identityDocumentNumber", event.target.value)
            }
            error={
              formik.touched.identityDocumentNumber &&
              Boolean(formik.errors.identityDocumentNumber)
            }
            helperText={
              formik.touched.identityDocumentNumber &&
              String(formik.errors.identityDocumentNumber || "")
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
            value={formik.values.indentityIssuePlace}
            onChange={(event) =>
              formik.setFieldValue("identityIssuePlace", event.target.value)
            }
            error={
              formik.touched.identityIssuePlace &&
              Boolean(formik.errors.identityIssuePlace)
            }
            helperText={
              formik.touched.identityIssuePlace &&
              String(formik.errors.identityIssuePlace || "")
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
            id='issueDate'
            label='Ngày cấp'
            type='date'
            fullWidth
            variant='outlined'
            InputLabelProps={{ shrink: true }}
            value={formik.values.identityIssueDate}
            onChange={(event) =>
              formik.setFieldValue("identityIssueDate", event.target.value)
            }
            error={
              formik.touched.identityIssueDate &&
              Boolean(formik.errors.identityIssueDate)
            }
            helperText={
              formik.touched.identityIssueDate &&
              String(formik.errors.identityIssueDate || "")
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
            value={formik.values.identityExpiryDate}
            onChange={(event) =>
              formik.setFieldValue("identityExpiryDate", event.target.value)
            }
            error={
              formik.touched.identityExpiryDate &&
              Boolean(formik.errors.identityExpiryDate)
            }
            helperText={
              formik.touched.identityExpiryDate &&
              String(formik.errors.identityExpiryDate || "")
            }
          />
        </Grid2>
        <Grid2
          size={{
            xs: 12,
            md: 4,
          }}
        >
          {formik.getFieldProps("identityType").value === "Passport" && (
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
          )}
        </Grid2>
        {formik.getFieldProps("identityType").value === "Passport" && (
          <>
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
                      formik.setFieldValue("identityIsChip", e.target.checked)
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
          </>
        )}
      </Grid2>
    </>
  );
};

export default AdditionalInformationForm;
