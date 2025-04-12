import { useEffect, useMemo, useState } from "react";
import useFunction from "@/hooks/use-function";
import usePagination from "@/hooks/use-pagination";
import { CourseApi} from "@/api/course";
import type { Course } from "@/types/course";
import { useRouter, useSearchParams } from "next/navigation";
import { useFaculty } from "@/app/dashboard/_sections/use-faculty";

export const useCoursePagination = () => {
  const router = useRouter();
  const searchParams = useSearchParams();
  const { faculties } = useFaculty();
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

  const deleteCourseApi = useFunction(CourseApi.deleteCourse,
    {
      successMessage: "Xóa khóa học thành công",
      onSuccess: ({ payload }) => {
        if (payload) {
          getCoursesApi.setData(
            {
              data: getCoursesApi.data?.data.filter((course) => course.courseId !== payload) || [],
              total: (getCoursesApi.data?.total || 0) - 1
            }
          );
        }
      },
    }
  );

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
        ? { facultyId: faculties.find((f) => f.name === faculty)?.id }
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
