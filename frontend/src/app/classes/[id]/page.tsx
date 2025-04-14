"use client";
import { Box, Typography } from "@mui/material";
import { ClassForm } from "../_components/class-form";
import { ClassApi } from "@/api/class";
import { notFound, useParams, useRouter } from "next/navigation";
import { useClassSearch } from "../_sections/use-class-search";
import { useEffect } from "react";

export default function EditClassPage() {
  const router = useRouter();
  const { getClassApi } = useClassSearch();
  const { id } = useParams();
  useEffect(() => {
    const fetchClass = async () => {
      if (!id) return;
      const response = await getClassApi.call(id as string);
      if (!response.data || Object.keys(response.data).length === 0) {
        router.replace("/noClassFound");
      }
    };
    fetchClass();
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [id]);
  return (
    <Box sx={{ display: "flex", flexDirection: "column", gap: 3 }}>
      <Typography variant="h4" component="h1" fontWeight="bold">
        Sửa lớp học
      </Typography>
      {getClassApi.loading && (
        <Box
          sx={{
            display: "flex",
            justifyContent: "center",
            alignItems: "center",
            height: "100%",
          }}
        >
          <Typography variant="h6">Loading...</Typography>
        </Box>
      )}
      {getClassApi.data && <ClassForm classData={getClassApi.data} />}
    </Box>
  );
}
