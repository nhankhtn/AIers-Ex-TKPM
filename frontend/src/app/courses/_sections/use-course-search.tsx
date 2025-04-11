import { useEffect, useMemo } from "react";
import useFunction from "@/hooks/use-function";
import { CourseApi, CourseResponse, GetCourseRequest } from "@/api/course";
import type { Course } from "@/types/course";
import { useRouter } from "next/navigation";
export const useCourseSearch = () => {
  const router = useRouter();
  const getCoursesApi = useFunction(CourseApi.getCourses, {
    disableResetOnCall: true,
  });
  const courses = useMemo(
    () => getCoursesApi.data?.data || [],
    [getCoursesApi.data]
  );

  const searchCourses = (params: GetCourseRequest) => {
    return getCoursesApi.call(params);
  };
  const addCourseApi = useFunction<Course, Course>(
    (course) => CourseApi.createCourse(course),
    {
      successMessage: "Thêm khóa học thành công",
      onSuccess: () => {
        router.push("/courses");
      },
    }
  );
  const updateCourseApi = useFunction<
    { id: string; course: Partial<Course> },
    Course
  >(({ id, course }) => CourseApi.updateCourse({ id, course }), {
    successMessage: "Cập nhật khóa học thành công",
    onSuccess: ({ result }) => {
      router.push("/courses");
    },
  });
  return {
    getCoursesApi,
    courses,
    searchCourses,
    addCourseApi,
    updateCourseApi,
  };
};
