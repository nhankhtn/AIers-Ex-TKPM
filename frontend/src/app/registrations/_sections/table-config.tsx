import { CustomTableConfig } from "@/components/custom-table";
import { Class } from "@/types/class";
import { UnregisterClass } from "@/types/registration";
import { StudentClass } from "@/types/student";
import { Typography } from "@mui/material";
import { useLocale, useTranslations } from "next-intl";

export const GetClassesTableConfig = (): CustomTableConfig<string, Class>[] => {
  const t = useTranslations("registrations.table");
  return [
    {
      key: "code",
      headerLabel: t("classCode"),
      renderCell: (data) => (
        <Typography variant="body2">{data.classId}</Typography>
      ),
    },
    {
      key: "name",
      headerLabel: t("courseName"),
      renderCell: (data) => (
        <Typography variant="body2">{data.courseId}</Typography>
      ),
    },
    {
      key: "teacherName",
      headerLabel: t("teacher"),
      renderCell: (data) => (
        <Typography variant="body2">{data.teacherName}</Typography>
      ),
    },
    {
      key: "dayOfWeek",
      headerLabel: t("schedule"),
      renderCell: (data) => (
        <Typography variant="body2">{data.dayOfWeek}</Typography>
      ),
    },
    {
      key: "time",
      headerLabel: t("capacity"),
      renderCell: (data) => (
        <Typography variant="body2">{data.maxStudents}</Typography>
      ),
    },
    {
      key: "room",
      headerLabel: t("room"),
      renderCell: (data) => (
        <Typography variant="body2">{data.room}</Typography>
      ),
    },
  ];
};

export const GetRegisteredClassesTableConfig = (): CustomTableConfig<
  string,
  StudentClass
>[] => {
  const t = useTranslations("registrations.studentInfo");
  const locale = useLocale() as "en" | "vi";
  return [
    {
      key: "studentId",
      headerLabel: t("studentId"),
      renderCell: (data) => (
        <Typography variant="body2">{data.studentId}</Typography>
      ),
    },
    {
      key: "studentName",
      headerLabel: t("name"),
      renderCell: (data) => (
        <Typography variant="body2">{data.studentName}</Typography>
      ),
    },
    {
      key: "code",
      headerLabel: t("classCode"),
      renderCell: (data) => (
        <Typography variant="body2">{data.classId}</Typography>
      ),
    },
    {
      key: "name",
      headerLabel: t("courseName"),
      renderCell: (data) => (
        <Typography variant="body2">{data.courseName[locale]}</Typography>
      ),
    },
  ];
};

export const GetUnregisteredClassesTableConfig = (): CustomTableConfig<
  number,
  UnregisterClass
>[] => {
  const t = useTranslations("registrations.table");
  const locale = useLocale() as "en" | "vi";
  return [
    {
      key: "classId",
      headerLabel: t("classCode"),
      renderCell: (data) => (
        <Typography variant="body2">{data.classId}</Typography>
      ),
    },
    {
      key: "courseName",
      headerLabel: t("courseName"),
      renderCell: (data) => (
        <Typography variant="body2">{data.courseName}</Typography>
      ),
    },
    {
      key: "semester",
      headerLabel: t("semester"),
      renderCell: (data) => (
        <Typography variant="body2">{data.semester}</Typography>
      ),
    },
    {
      key: "academicYear",
      headerLabel: t("academicYear"),
      renderCell: (data) => (
        <Typography variant="body2">{data.academicYear}</Typography>
      ),
    },
    {
      key: "time",
      headerLabel: t("requestTime"),
      renderCell: (data) => (
        <Typography variant="body2">{data.time.split("T")[0]}</Typography>
      ),
    },
  ];
};
