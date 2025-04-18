"use client";

import { Box, Typography, Button } from "@mui/material";
import AddCircleIcon from "@mui/icons-material/AddCircle";
import { ClassList } from "../_components/class-list";
import Link from "next/link";
import { paths } from "@/paths";

export default function Content() {
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
          Quản lý lớp học
        </Typography>
        <Button
          variant="contained"
          startIcon={<AddCircleIcon />}
          component={Link}
          href={paths.classes.create}
        >
          Mở lớp học mới
        </Button>
      </Box>

      <ClassList />
    </Box>
  );
}
