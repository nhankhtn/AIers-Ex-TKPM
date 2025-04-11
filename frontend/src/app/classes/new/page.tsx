import { ClassForm } from "@/app/classes/_components/class-form";
import { Box, Typography } from "@mui/material";

export default function NewClassPage() {
  return (
    <Box sx={{ display: "flex", flexDirection: "column", gap: 3 }}>
      <Typography variant="h4" fontWeight="bold">
        Mở lớp học mới
      </Typography>
      <ClassForm />
    </Box>
  );
}
