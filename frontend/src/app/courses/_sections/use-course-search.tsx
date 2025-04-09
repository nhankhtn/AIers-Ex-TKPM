import { useEffect, useMemo } from "react";
import useFunction from "@/hooks/use-function";
import { CourseApi, CourseResponse, GetCourseRequest } from "@/api/course";
import type { Course } from "@/types/course";

export const useCourseSearch = () => {
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

  const searchCourses = (params: GetCourseRequest) => {
    return getCoursesApi.call(params);
  };

  return {
    getCoursesApi,
    courses,
    searchCourses,
  };
};
