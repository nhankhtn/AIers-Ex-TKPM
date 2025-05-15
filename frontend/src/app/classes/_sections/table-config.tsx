import { Box, Typography } from "@mui/material";
import type { Class } from "@/types/class";
import type { CustomTableConfig } from "@/components/custom-table/custom-table.types";
import { format } from "date-fns";

export const getTableConfig = (t: any): CustomTableConfig<string, Class>[] => [
  {
    key: "classId",
    headerLabel: t("classes.list.code"),
    textAlign: "left" as const,
    renderCell: (classData) => {
      return (
        <Box>
          <Typography variant="body2" fontWeight="medium">
            {classData.classId}
          </Typography>
        </Box>
      );
    },
  },
  {
    key: "course",
    headerLabel: t("classes.list.course"),
    textAlign: "left" as const,
    renderCell: (classData) => (
      <Box>
        <Typography variant="body2" fontWeight="medium">
          {classData.courseId}
        </Typography>
      </Box>
    ),
  },
  {
    key: "teacher",
    headerLabel: t("classes.list.teacher"),
    textAlign: "left" as const,
    renderCell: (classData) => classData.teacherName,
  },
  {
    key: "year",
    headerLabel: t("classes.list.year"),
    textAlign: "left" as const,
    renderCell: (classData) => classData.academicYear,
  },
  {
    key: "semester",
    headerLabel: t("classes.list.semester"),
    textAlign: "left" as const,
    renderCell: (classData) =>
      classData.semester === 1
        ? t("classes.filters.semester1")
        : classData.semester === 2
        ? t("classes.filters.semester2")
        : t("classes.filters.semester3"),
  },
  {
    key: "room",
    headerLabel: t("classes.list.room"),
    textAlign: "left" as const,
    renderCell: (classData) => classData.room,
  },
  {
    key: "dayOfWeek",
    headerLabel: t("classes.list.dayOfWeek"),
    textAlign: "left" as const,
    renderCell: (classData) => (
      <Typography>
        {classData.dayOfWeek === 2
          ? t("classes.form.monday")
          : classData.dayOfWeek === 3
          ? t("classes.form.tuesday")
          : classData.dayOfWeek === 4
          ? t("classes.form.wednesday")
          : classData.dayOfWeek === 5
          ? t("classes.form.thursday")
          : classData.dayOfWeek === 6
          ? t("classes.form.friday")
          : classData.dayOfWeek === 7
          ? t("classes.form.saturday")
          : t("classes.form.sunday")}
      </Typography>
    ),
  },
  {
    key: "time",
    headerLabel: t("classes.list.time"),
    textAlign: "left" as const,
    renderCell: (classData) => (
      <Typography>
        {classData.startTime}:00 - {classData.endTime}:00
      </Typography>
    ),
  },
  {
    key: "maxStudents",
    headerLabel: t("classes.list.maxStudents"),
    textAlign: "center" as const,
    renderCell: (classData) => <Typography>{classData.maxStudents}</Typography>,
  },
  {
    key: "deadline",
    headerLabel: t("classes.list.deadline"),
    textAlign: "left" as const,
    renderCell: (classData) => {
      const deadline = new Date(classData.deadline);
      const now = new Date();
      const isExpired = deadline < now;

      return (
        <Typography
          color={isExpired ? "error.main" : "success.main"}
          fontWeight={500}
        >
          {format(deadline, "dd/MM/yyyy")}
        </Typography>
      );
    },
  },
];
