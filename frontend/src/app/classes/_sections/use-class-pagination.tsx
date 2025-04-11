import { useEffect, useMemo, useState, useCallback } from "react";
import useFunction from "@/hooks/use-function";
import usePagination from "@/hooks/use-pagination";
import { ClassApi, ClassResponse, GetClassRequest, ClassDeleted } from "@/api/class";
import type { Class } from "@/types/class";
import { useRouter, useSearchParams } from "next/navigation";

export const useClassPagination = () => {
  const router = useRouter();
  const searchParams = useSearchParams();
  const [filter, setFilter] = useState({
    courseId: "",
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

  const deleteClassApi = useFunction<number, ClassDeleted>(
    (id) => ClassApi.deleteClass(id),
    {
      successMessage: "Xóa lớp học thành công",
      onSuccess: ({ result }) => {
        if (result) {
          getClassesApi.setData({
            data:
              getClassesApi.data?.data.filter(
                (classData) => classData.id !== result.data.id
              ) || [],
            total: (getClassesApi.data?.total || 0) - 1,
          });
        }
      },
    }
  );

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
    const courseId = searchParams.get("courseId") || "";
    const semester = searchParams.get("semester") || "";

    handleFilterChange({
      courseId: courseId || "",
      semester: semester || "",
    });

    getClassesApi.call({
      page: pagination.page + 1,
      limit: pagination.rowsPerPage,
      ...(courseId ? { courseId } : {}),
      ...(semester
        ? { semester }
        : {}),
    });
  }, [
    searchParams,
    handleFilterChange,
    getClassesApi,
    pagination.page,
    pagination.rowsPerPage,
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
