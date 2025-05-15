import { useEffect, useMemo, useState, useCallback } from "react";
import useFunction from "@/hooks/use-function";
import usePagination from "@/hooks/use-pagination";
import { ClassApi, ClassResponse, GetClassRequest } from "@/api/class";
import type { Class } from "@/types/class";
import { useRouter, useSearchParams } from "next/navigation";
import { useTranslations } from "next-intl";

export const useClassPagination = () => {
  const router = useRouter();
  const searchParams = useSearchParams();
  const t = useTranslations();

  const [filter, setFilter] = useState({
    classId: "",
    semester: "",
  });

  const getClassesApi = useFunction(ClassApi.getClasses, {
    disableResetOnCall: true,
  });

  const classes = useMemo(
    () => getClassesApi.data?.data || [],
    [getClassesApi.data]
  );

  const pagination = usePagination({
    count: getClassesApi.data?.total || 0,
    initialRowsPerPage: 10,
  });

  const deleteClassApi = useFunction(ClassApi.deleteClass, {
    successMessage: t("classes.messages.deleteSuccess"),
    onSuccess: ({ result }) => {
      if (result) {
        getClassesApi.setData({
          data:
            getClassesApi.data?.data.filter(
              (classData) => classData.classId !== result.classId
            ) || [],
          total: (getClassesApi.data?.total || 0) - 1,
        });
      }
    },
  });

  const handleFilterChange = useCallback((newFilter: typeof filter) => {
    setFilter(newFilter);
  }, []);

  const updateUrlParams = useCallback(() => {
    const currentParams = new URLSearchParams();

    Object.entries(filter).forEach(([key, value]) => {
      if (value) {
        currentParams.set(key, value);
      }
    });

    router.replace(`?${currentParams.toString()}`);
  }, [filter, router]);

  const fetchClasses = useCallback(() => {
    const classId = searchParams.get("classId") || "";
    const semester = searchParams.get("semester") || "";
    handleFilterChange({
      classId: classId || "",
      semester: semester || "",
    });
    const semesterNumber =
      semester === t("classes.filters.semester1")
        ? 1
        : semester === t("classes.filters.semester2")
        ? 2
        : semester === t("classes.filters.semester3")
        ? 3
        : 0;
    getClassesApi.call({
      page: pagination.page + 1,
      limit: pagination.rowsPerPage,
      ...(classId ? { classId } : {}),
      ...(semester && semesterNumber > 0 ? { semester: semesterNumber } : {}),
    });
  }, [
    searchParams,
    handleFilterChange,
    getClassesApi,
    pagination.page,
    pagination.rowsPerPage,
    t,
  ]);

  useEffect(() => {
    fetchClasses();
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [pagination.page, pagination.rowsPerPage, searchParams]);

  useEffect(() => {
    updateUrlParams();
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [filter]);

  return {
    getClassesApi,
    deleteClassApi,
    classes,
    pagination,
    filter,
    setFilter: handleFilterChange,
  };
};
