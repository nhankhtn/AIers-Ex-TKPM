"use client";

import { Box, Typography, Button } from "@mui/material";
import AddCircleIcon from "@mui/icons-material/AddCircle";
import { CourseList } from "../_components/courses-list";
import Link from "next/link";
import { paths } from "@/paths";
import { useTranslations } from "next-intl";

export default function Content() {
  const t = useTranslations();
  return (
    <Box sx={{ display: "flex", flexDirection: "column", gap: 3 }}>
      <Box
        sx={{
          display: "flex",
          justifyContent: "space-between",
          alignItems: "center",
        }}
      >
        <Typography variant="h5" component="h5" fontWeight="bold">
          {t("courses.title")}
        </Typography>
        <Button
          variant="contained"
          startIcon={<AddCircleIcon />}
          component={Link}
          href={paths.courses.create}
        >
          {t("courses.form.addTitle")}
        </Button>
      </Box>

      <CourseList />
    </Box>
  );
}
