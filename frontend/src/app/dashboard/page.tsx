"use client";

import { useTranslations } from "next-intl";
import { Typography } from "@mui/material";
import StudentProvider from "@/context/student-student-context";
import Content from "./_sections/content";

export default function DashboardPage() {
  const t = useTranslations("dashboard");

  return (
    <StudentProvider>
      <Content />
    </StudentProvider>
  );
}
