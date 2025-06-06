"use client";
import { Box, Typography } from "@mui/material";
import { ClassForm } from "../_components/class-form";
import { ClassApi } from "@/api/class";
import { notFound, useParams, useRouter } from "next/navigation";
import { useClassSearch } from "../_sections/use-class-search";
import { useEffect } from "react";
import { useTranslations } from "next-intl";

export default function EditClassPage() {
  const router = useRouter();
  const { getClassesApi } = useClassSearch();
  const { id } = useParams();
  const t = useTranslations();

  useEffect(() => {
    const fetchClass = async () => {
      if (!id) return;
      const response = await getClassesApi.call({
        page: 1,
        limit: 1,
        classId: id as string,
      });
      if (!response.data || response.data.data.length === 0) {
        router.replace("/noClassFound");
      }
    };
    fetchClass();
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [id]);
  return (
    <Box sx={{ display: "flex", flexDirection: "column", gap: 3 }}>
      <Typography variant="h4" component="h1" fontWeight="bold">
        {t("classes.form.editTitle")}
      </Typography>
      {getClassesApi.loading && (
        <Box
          sx={{
            display: "flex",
            justifyContent: "center",
            alignItems: "center",
            height: "100%",
          }}
        >
          <Typography variant="h6">{t("classes.loading")}</Typography>
        </Box>
      )}
      {getClassesApi.data && (
        <ClassForm classData={getClassesApi.data.data[0]} />
      )}
    </Box>
  );
}
