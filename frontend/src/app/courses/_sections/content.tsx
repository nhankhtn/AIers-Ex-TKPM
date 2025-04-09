"use client";

import { Box, Typography, Button } from "@mui/material";
import AddCircleIcon from "@mui/icons-material/AddCircle";
import { CourseList } from "../_components/courses-list";
import Link from "next/link";

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
          Quản lý khóa học
        </Typography>
        <Button
          variant="contained"
          startIcon={<AddCircleIcon />}
          component={Link}
          href="/courses/new"
        >
          Thêm khóa học mới
        </Button>
      </Box>

      <CourseList />
    </Box>
  );
}
