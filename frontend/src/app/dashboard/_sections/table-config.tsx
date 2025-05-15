import { CustomTableConfig } from "@/components/custom-table";
import {
  Faculty,
  mappingGender,
  Program,
  Status,
  Student,
} from "@/types/student";
import { Typography } from "@mui/material";
import { parseStringToAddress } from "../_components/drawer-update-student/drawer-update-student";
import { formatDate } from "@/utils/format-date";
import { useLocale, useTranslations } from "next-intl";

export function objectToAddress(address: any) {
  return Object.entries(address)
    .map(([, value]) => value)
    .filter(Boolean)
    .reverse()
    .join(", ");
}

export const GetTableConfig = ({
  programs,
  statuses,
  faculties,
}: {
  programs: Program[];
  statuses: Status[];
  faculties: Faculty[];
}): CustomTableConfig<Student["id"], Student>[] => {
  const t = useTranslations("dashboard.list");
  const commonT = useTranslations("common");
  const locale = useLocale() as "en" | "vi";
  return [
    {
      key: "mssv",
      headerLabel: t("studentId"),
      type: "string",
      headerCellProps: {
        sx: {
          position: "sticky",
          backgroundColor: "white",
        },
      },
      renderCell: (data) => <Typography variant="body2">{data.id}</Typography>,
    },
    {
      key: "name",
      headerLabel: t("studentName"),
      type: "string",
      headerCellProps: {
        sx: {
          position: "sticky",
          backgroundColor: "white",
        },
      },
      renderCell: (data) => (
        <Typography variant="body2">{data.name}</Typography>
      ),
    },
    {
      key: "dob",
      headerLabel: t("dateOfBirth"),
      type: "string",
      renderCell: (data) => (
        <Typography variant="body2">{formatDate(data.dateOfBirth)}</Typography>
      ),
    },
    {
      key: "gender",
      headerLabel: t("gender"),
      type: "string",
      renderCell: (data) => (
        <Typography variant="body2">{mappingGender[data.gender]}</Typography>
      ),
    },
    {
      key: "email",
      headerLabel: t("email"),
      type: "string",
      renderCell: (data) => (
        <Typography variant="body2">{data.email}</Typography>
      ),
    },
    {
      key: "phone",
      headerLabel: t("phone"),
      type: "string",
      renderCell: (data) => (
        <Typography variant="body2">{data.phone}</Typography>
      ),
    },
    {
      key: "permanent_address",
      headerLabel: t("permanentAddress"),
      type: "string",
      renderCell: (data) => {
        const address = parseStringToAddress(data.permanentAddress);
        return (
          <Typography variant="body2" width={300} whiteSpace={"normal"}>
            {objectToAddress(address)}
          </Typography>
        );
      },
    },
    {
      key: "temporary_address",
      headerLabel: t("temporaryAddress"),
      type: "string",
      renderCell: (data) => {
        const address = parseStringToAddress(data.temporaryAddress);
        return (
          <Typography variant="body2" width={300} whiteSpace={"normal"}>
            {data.temporaryAddress ? objectToAddress(address) : t("empty")}
          </Typography>
        );
      },
    },
    {
      key: "mailing_address",
      headerLabel: t("mailingAddress"),
      type: "string",
      renderCell: (data) => {
        const address = parseStringToAddress(data.mailingAddress);
        return (
          <Typography variant="body2">
            {data.mailingAddress ? objectToAddress(address) : t("empty")}
          </Typography>
        );
      },
    },
    {
      key: "faculty",
      headerLabel: t("faculty"),
      type: "string",
      renderCell: (data) => (
        <Typography variant="body2">
          {faculties.find((f) => f.id === data.faculty)?.name[locale]}
        </Typography>
      ),
    },
    {
      key: "course",
      headerLabel: t("course"),
      type: "string",
      renderCell: (data) => (
        <Typography variant="body2">{data.course}</Typography>
      ),
    },
    {
      key: "program",
      headerLabel: t("program"),
      type: "string",
      renderCell: (data) => (
        <Typography variant="body2">
          {programs.find((p) => p.id === data.program)?.name[locale]}
        </Typography>
      ),
    },
    {
      key: "status",
      headerLabel: t("status"),
      type: "string",
      renderCell: (data) => (
        <Typography variant="body2">
          {statuses.find((s) => s.id === data.status)?.name[locale]}
        </Typography>
      ),
    },
  ];
};
