import { Box, Typography } from "@mui/material";
import { ClassForm } from "../_components/class-form";
import { ClassApi } from "@/api/class";
import { notFound } from "next/navigation";

export default async function EditClassPage({
  params,
}: {
  params: { id: string };
}) {
  const { id } = await params;
  const response = await ClassApi.getClass(parseInt(id));
  if (!response || Object.keys(response).length === 0) {
    notFound();
  }

  return (
    <Box sx={{ display: "flex", flexDirection: "column", gap: 3 }}>
      <Typography variant="h4" component="h1" fontWeight="bold">
        Sửa lớp học
      </Typography>
      <ClassForm classData={response.data} />
    </Box>
  );
}
