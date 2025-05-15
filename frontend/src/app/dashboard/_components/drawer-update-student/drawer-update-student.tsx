"use client";
import React, { useCallback, useEffect, useMemo } from "react";
import { Button, Typography, Drawer, Stack, Divider } from "@mui/material";
import {
  COUNTRY_CODE_DEFAULT,
  COUNTRY_DEFAULT,
  Faculty,
  Gender,
  getValidationStudent,
  Program,
  Status,
  Student,
} from "../../../../types/student";
import { useFormik } from "formik";
import RowStack from "@/components/row-stack";
import AddressStudentForm from "./address-student-form";
import AdditionalInformationForm from "./addtional-infomation-form";
import BasicInfomationForm from "./basic-infomation-form";
import {
  getOriginPhoneNumber,
  getPhoneNumberFormat,
} from "@/utils/phone-helper";
import useFunction from "@/hooks/use-function";
import { useMainContext } from "@/context/main/main-context";
import { useTranslations } from "next-intl";

const formatDate = (date: Date) => {
  return date && !isNaN(new Date(date).getTime())
    ? new Date(date).toISOString().split("T")[0]
    : "";
};

export const parseStringToAddress = (addressString?: string) => {
  if (!addressString)
    return {
      detail: "",
      ward: "",
      district: "",
      province: "",
      country: COUNTRY_DEFAULT,
    };
  try {
    return JSON.parse(addressString);
  } catch (e) {
    return {
      detail: addressString,
      ward: "",
      district: "",
      province: "",
      country: COUNTRY_DEFAULT,
    };
  }
};

interface DrawerUpdateStudentProps {
  student: Student | null;
  open: boolean;
  onClose: () => void;
  addStudent: (student: Student) => Promise<void>;
  updateStudent: (student: Student | Omit<Student, "email">) => Promise<void>;
  faculties: Faculty[];
  statuses: Status[];
  programs: Program[];
}

export const IDENTITY_TYPES = [
  {
    key: "CCCD",
    translationKey: "CCCD",
  },
  {
    key: "CMND",
    translationKey: "CMND",
  },
  {
    key: "Passport",
    translationKey: "Passport",
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
  const t = useTranslations("dashboard.drawer");
  const { countries, settings } = useMainContext();
  useEffect(() => {
    if (!open) {
      formik.resetForm();
    }
    //  eslint-disable-next-line react-hooks/exhaustive-deps
  }, [open]);

  const permanentAddress = parseStringToAddress(student?.permanentAddress);
  const temporaryAddress = parseStringToAddress(student?.temporaryAddress);
  const mailingAddress = parseStringToAddress(student?.mailingAddress);

  const handleSubmit = useCallback(
    async (values: any) => {
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
        countryIssue: values.identityCountry,
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
          : "",
        mailingAddress: values.useMailingAddress
          ? JSON.stringify(mailingAddress)
          : "",
        faculty: values.faculty,
        course: values.course,
        program: values.program,
        phone: getPhoneNumberFormat(values.phone, values.phoneCode),
        status: values.status,
        identity: identity,
        nationality: values.nationality,
      };

      if (student) {
        await updateStudent(studentData);
      } else {
        await addStudent(studentData);
      }
    },
    [updateStudent, addStudent, student]
  );

  const initialValues = useMemo(
    () => ({
      id: student?.id || "",
      name: student?.name || "",
      dateOfBirth: student?.dateOfBirth.split("T")[0] || "",
      gender: student?.gender || Gender.Male,
      email: student?.email || "",
      phone: getOriginPhoneNumber(student?.phone || "")?.originNumber || "",
      phoneCode:
        getOriginPhoneNumber(student?.phone || "")?.countryCode ||
        COUNTRY_CODE_DEFAULT,
      // Permanent address
      permanentCountry: permanentAddress.country || COUNTRY_DEFAULT,
      permanentProvince: permanentAddress.province || "",
      permanentDistrict: permanentAddress.district || "",
      permanentWard: permanentAddress.ward || "",
      permanentDetail: permanentAddress.detail || "",

      // Temporary address
      useTemporaryAddress: !!student?.temporaryAddress,
      temporaryCountry: temporaryAddress.country || COUNTRY_DEFAULT,
      temporaryProvince: temporaryAddress.province || "",
      temporaryDistrict: temporaryAddress.district || "",
      temporaryWard: temporaryAddress.ward || "",
      temporaryDetail: temporaryAddress.detail || "",

      // Mailing address
      useMailingAddress: !!student?.mailingAddress,
      mailingCountry: mailingAddress.country || COUNTRY_DEFAULT,
      mailingProvince: mailingAddress.province || "",
      mailingDistrict: mailingAddress.district || "",
      mailingWard: mailingAddress.ward || "",
      mailingDetail: mailingAddress.detail || "",

      // Academic info
      faculty: faculties.find((f) => f.id === student?.faculty)?.id || "",
      course: student?.course || 0,
      program: programs.find((p) => p.id === student?.program)?.id || "",
      status: statuses.find((s) => s.id === student?.status)?.id || "",

      // Identity info
      identityType: student?.identity.type || 0,
      identityDocumentNumber: student?.identity.documentNumber || "",
      identityIssueDate: formatDate(student?.identity.issueDate || new Date()),
      identityIssuePlace: student?.identity.issuePlace || "",
      identityExpiryDate: formatDate(
        student?.identity.expiryDate || new Date()
      ),
      identityCountry: student?.identity.countryIssue || COUNTRY_DEFAULT,
      identityIsChip: !!student?.identity.isChip,
      identityNotes: student?.identity.notes || "",
      nationality: student?.nationality || COUNTRY_DEFAULT,
    }),
    // eslint-disable-next-line react-hooks/exhaustive-deps
    [student, faculties, programs, statuses]
  );
  const formik = useFormik({
    initialValues,
    enableReinitialize: true,
    validateOnChange: false,
    validationSchema: getValidationStudent({
      allowedEmailDomain: settings.allowedEmailDomains,
    }),
    onSubmit: handleSubmit,
  });

  return (
    <Drawer
      open={open}
      onClose={onClose}
      anchor="right"
      slotProps={{
        paper: {
          sx: {
            width: { xs: "100%", md: "50%" },
            pb: 2,
          },
        },
      }}
    >
      <Stack px={3} py={2} spacing={2} sx={{ overflowY: "auto" }}>
        <RowStack justifyContent="space-between" alignItems="center">
          <Typography variant="h5">
            {student ? t("editTitle") : t("addTitle")}
          </Typography>
          <RowStack gap={1.5}>
            <Button onClick={onClose} color="inherit" variant="outlined">
              {t("cancel")}
            </Button>
            <Button
              onClick={() => formik.handleSubmit()}
              color="primary"
              variant="contained"
              disabled={formik.isSubmitting}
            >
              {student ? t("update") : t("add")}
            </Button>
          </RowStack>
        </RowStack>

        <Divider />

        <BasicInfomationForm formik={formik} countries={countries} />

        <Divider />

        <AddressStudentForm formik={formik} open={open} countries={countries} />

        <Divider />

        <AdditionalInformationForm
          formik={formik}
          countries={countries}
          programs={programs}
          faculties={faculties}
          statuses={statuses}
        />
      </Stack>
    </Drawer>
  );
}

export default DrawerUpdateStudent;
