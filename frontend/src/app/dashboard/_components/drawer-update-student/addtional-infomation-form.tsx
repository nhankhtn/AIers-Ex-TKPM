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
import { useLocale, useTranslations } from "next-intl";

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
  const t = useTranslations("dashboard.list");
  const t2 = useTranslations("dashboard.drawer.identityTypes");
  const locale = useLocale() as "en" | "vi";
  return (
    <>
      {/* Academic Information */}
      <Typography variant="h6">{t("academicInfo")}</Typography>
      <Grid2 container spacing={2}>
        <Grid2
          size={{
            xs: 12,
            md: 4,
          }}
        >
          <FormControl fullWidth>
            <InputLabel>{t("faculty")}</InputLabel>
            <Select
              id="faculty"
              label={t("faculty")}
              value={formik.values.faculty}
              onChange={(event) =>
                formik.setFieldValue("faculty", event.target.value)
              }
              error={formik.touched.faculty && Boolean(formik.errors.faculty)}
            >
              {faculties.map((faculty) => (
                <MenuItem key={faculty.id} value={faculty.id}>
                  {faculty.name[locale]}
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
            <InputLabel>{t("program")}</InputLabel>
            <Select
              id="program"
              label={t("program")}
              value={formik.values.program}
              onChange={(event) =>
                formik.setFieldValue("program", event.target.value)
              }
              error={formik.touched.program && Boolean(formik.errors.program)}
            >
              {programs.map((program) => (
                <MenuItem key={program.id} value={program.id}>
                  {program.name[locale]}
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
            id="course"
            label={t("course")}
            type="number"
            fullWidth
            variant="outlined"
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
            <InputLabel>{t("status")}</InputLabel>
            <Select
              id="status"
              label={t("status")}
              value={formik.values.status}
              onChange={(event) =>
                formik.setFieldValue("status", event.target.value)
              }
              error={formik.touched.status && Boolean(formik.errors.status)}
            >
              {statuses.map((status) => (
                <MenuItem key={status.id} value={status.id}>
                  {status.name[locale]}
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
      <Typography variant="h6">{t("identityInfo")}</Typography>
      <Grid2 container spacing={2}>
        <Grid2
          size={{
            xs: 12,
            md: 4,
          }}
        >
          <FormControl fullWidth>
            <InputLabel>{t("identityType")}</InputLabel>
            <Select
              id="identityType"
              label={t("identityType")}
              value={formik.values.identityType}
              onChange={(event) =>
                formik.setFieldValue("identityType", event.target.value)
              }
            >
              {IDENTITY_TYPES.map(({ key, translationKey }) => (
                <MenuItem key={key} value={key}>
                  {t2(translationKey)}
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
            id="documentNumber"
            label={t("documentNumber")}
            fullWidth
            variant="outlined"
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
            id="issuePlace"
            label={t("issuePlace")}
            fullWidth
            variant="outlined"
            value={formik.values.identityIssuePlace}
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
            id="issueDate"
            label={t("issueDate")}
            type="date"
            fullWidth
            variant="outlined"
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
            id="expiryDate"
            label={t("expiryDate")}
            type="date"
            fullWidth
            variant="outlined"
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
              <InputLabel>{t("issueCountry")}</InputLabel>
              <Select
                id="identityCountry"
                label={t("issueCountry")}
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
                    name="isChip"
                  />
                }
                label={t("isChip")}
              />
            </Grid2>
            <Grid2 size={12}>
              <TextField
                id="identityNotes"
                label={t("notes")}
                fullWidth
                multiline
                rows={2}
                variant="outlined"
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
