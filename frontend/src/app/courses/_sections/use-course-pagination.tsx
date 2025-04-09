import { useEffect, useMemo, useState } from "react";
import useFunction from "@/hooks/use-function";
import usePagination from "@/hooks/use-pagination";
import { CourseApi, CourseResponse, GetCourseRequest } from "@/api/course";
import type { Course } from "@/types/course";
import { useRouter, useSearchParams } from "next/navigation";

export const useCoursePagination = () => {
  const router = useRouter();
  const searchParams = useSearchParams();
  const [filter, setFilter] = useState({
    key: "",
    faculty: "",
  });

  const getCoursesApi = useFunction<GetCourseRequest, CourseResponse>(
    (params) => CourseApi.getCourses(params),
    {
      disableResetOnCall: true,
    }
  );

  const courses = useMemo(
    () => getCoursesApi.data?.data || [],
    [getCoursesApi.data]
  );

  const pagination = usePagination({
    count: getCoursesApi.data?.total || 0,
    initialRowsPerPage: 10,
  });

  const addCourseApi = useFunction<Omit<Course, "id">, Course>(
    (course) => CourseApi.createCourse(course),
    {
      successMessage: "Thêm khóa học thành công",
      onSuccess: ({ result }) => {
        if (result) {
          getCoursesApi.call({
            page: pagination.page + 1,
            limit: pagination.rowsPerPage,
            ...(filter.key ? { key: filter.key } : {}),
            ...(filter.faculty ? { faculty: filter.faculty } : {}),
          });
        }
      },
    }
  );

  const updateCourseApi = useFunction<
    { id: string; course: Partial<Course> },
    Course
  >(({ id, course }) => CourseApi.updateCourse({ id, course }), {
    successMessage: "Cập nhật khóa học thành công",
    onSuccess: ({ result }) => {
      if (result) {
        getCoursesApi.call({
          page: pagination.page + 1,
          limit: pagination.rowsPerPage,
          ...(filter.key ? { key: filter.key } : {}),
          ...(filter.faculty ? { faculty: filter.faculty } : {}),
        });
      }
    },
  });

  const deleteCourseApi = useFunction<string, Course>(
    (id) => CourseApi.deleteCourse(id),
    {
      successMessage: "Xóa khóa học thành công",
      onSuccess: ({ result }) => {
        if (result) {
          getCoursesApi.call({
            page: pagination.page + 1,
            limit: pagination.rowsPerPage,
            ...(filter.key ? { key: filter.key } : {}),
            ...(filter.faculty ? { faculty: filter.faculty } : {}),
          });
        }
      },
    }
  );

  useEffect(() => {
    const key = searchParams.get("key") || "";
    const faculty = searchParams.get("faculty") || "";

    setFilter((prev) => ({
      ...prev,
      key: key || "",
      faculty: faculty || "",
    }));

    getCoursesApi.call({
      page: pagination.page + 1,
      limit: pagination.rowsPerPage,
      ...(key ? { key } : {}),
      ...(faculty ? { faculty } : {}),
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
    addCourseApi,
    updateCourseApi,
    deleteCourseApi,
    courses,
    pagination,
    filter,
    setFilter,
  };
};
