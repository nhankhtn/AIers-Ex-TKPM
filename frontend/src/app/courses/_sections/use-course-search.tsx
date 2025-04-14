import { useEffect, useMemo } from "react";
import useFunction from "@/hooks/use-function";
import { CourseApi, CourseResponse, GetCourseRequest } from "@/api/course";
import type { Course } from "@/types/course";
import { useRouter } from "next/navigation";
import { paths } from "@/paths";
export const useCourseSearch = () => {
  const router = useRouter();
  const getCoursesApi = useFunction(CourseApi.getCourses, {
    disableResetOnCall: true,
  });
  const courses = useMemo(
    () => getCoursesApi.data?.data || [],
    [getCoursesApi.data]
  );
  const getCourseApi = useFunction(CourseApi.getCourse, {
    disableResetOnCall: true,
  });
  const searchCourses = (params: GetCourseRequest) => {
    return getCoursesApi.call(params);
  };
  const addCourseApi = useFunction(CourseApi.createCourse, {
    successMessage: "Thêm khóa học thành công",
    onSuccess: () => {
        router.push(paths.courses.index);
      },
    }
  );
  const updateCourseApi = useFunction(CourseApi.updateCourse, {
    successMessage: "Cập nhật khóa học thành công",
    onSuccess: ({ result }) => {
      router.push(paths.courses.index);
    },
  });
  return {
    getCoursesApi,
    courses,
    searchCourses,
    addCourseApi,
    getCourseApi,
    updateCourseApi,
  };
};
