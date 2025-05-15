"use client";

import { Box, Typography } from "@mui/material";
import { TranscriptForm } from "./transcript-form";
import { useTranslations } from "next-intl";

export default function TranscriptsContent() {
  const t = useTranslations("transcripts");

  return (
    <Box sx={{ display: "flex", flexDirection: "column", gap: 3 }}>
      <Typography variant='h4' component='h1' fontWeight='bold'>
        {t("title")}
      </Typography>
      <TranscriptForm />
    </Box>
  );
}
