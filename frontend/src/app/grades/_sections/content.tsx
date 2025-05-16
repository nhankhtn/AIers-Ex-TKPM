"use client";

import { Box, Typography } from "@mui/material";
import { GradeEntryForm } from "./grade-entity-form";
import { useTranslations } from "next-intl";

export default function GradesContent() {
  const t = useTranslations("grades");
  return (
    <Box sx={{ display: "flex", flexDirection: "column", gap: 3 }}>
      <Typography variant='h4' component='h1' fontWeight='bold'>
        {t("title")}
      </Typography>
      <GradeEntryForm />
    </Box>
  );
}
