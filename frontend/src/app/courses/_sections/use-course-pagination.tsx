import { useEffect, useMemo, useState } from "react";
import useFunction from "@/hooks/use-function";
import usePagination from "@/hooks/use-pagination";
import { CourseApi } from "@/api/course";
import { useRouter, useSearchParams } from "next/navigation";
import { useMainContext } from "@/context/main/main-context";
import { useTranslations } from "next-intl";

export const useCoursePagination = () => {
  const router = useRouter();
  const searchParams = useSearchParams();
  const { faculties } = useMainContext();
  const t = useTranslations();
  const [filter, setFilter] = useState({
    key: "",
    faculty: "",
    status: "",
  });

  const getCoursesApi = useFunction(CourseApi.getCourses, {
    disableResetOnCall: true,
  });
  const courses = useMemo(
    () => getCoursesApi.data?.data || [],
    [getCoursesApi.data]
  );

  const pagination = usePagination({
    count: getCoursesApi.data?.total || 0,
    initialRowsPerPage: 10,
  });

  const deleteCourseApi = useFunction(CourseApi.deleteCourse, {
    successMessage: t("courses.messages.deleteCourseSuccess"),
    onSuccess: ({ payload }) => {
      if (payload) {
        getCoursesApi.setData({
          data:
            getCoursesApi.data?.data.filter(
              (course) => course.courseId !== payload
            ) || [],
          total: (getCoursesApi.data?.total || 0) - 1,
        });
      }
    },
    onError: (error, payload) => {
      const err = error as Error;
      console.log(err.message);
      if (
        err.message ==
        "Lỗi: Course already has class. Status will be changed to Deactivated"
      ) {
        getCoursesApi.setData({
          data:
            getCoursesApi.data?.data.map((course) =>
              course.courseId === payload
                ? { ...course, deletedAt: new Date().toISOString() }
                : course
            ) || [],
          total: getCoursesApi.data?.total || 0,
        });
      }
    },
  });

  useEffect(() => {
    const key = searchParams.get("key") || "";
    const faculty = searchParams.get("faculty") || "";
    const status = searchParams.get("status") || "";
    setFilter((prev) => ({
      ...prev,
      key: key || "",
      faculty: faculty || "",
      status: status || "",
    }));

    getCoursesApi.call({
      page: pagination.page + 1,
      limit: pagination.rowsPerPage,
      ...(key ? { courseId: key } : {}),
      ...(faculty
        ? { facultyId: faculties.find((f) => f.name.vi === faculty)?.id }
        : {}),
      ...(status
        ? { isDeleted: status === "Đang hoạt động" ? false : true }
        : {}),
    });
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [pagination.page, pagination.rowsPerPage, searchParams]);

  useEffect(() => {
    const currentParams = new URLSearchParams();

    Object.entries(filter).forEach(([key, value]) => {
      if (value) {
        currentParams.set(key, value);
      }
    });

    router.replace(`?${currentParams.toString()}`);
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [filter]);

  return {
    getCoursesApi,
    deleteCourseApi,
    courses,
    pagination,
    filter,
    setFilter,
  };
};
