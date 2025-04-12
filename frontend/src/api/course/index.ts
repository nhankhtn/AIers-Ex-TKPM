import { ResponseWithTotal } from "@/types";
import { Course } from "@/types/course";
import { apiDelete, apiGet, apiPost, apiPut, getFormData } from "@/utils/api-request";

export type GetCourseRequest = {
  page: number;
  limit: number;
  courseId?: string;
  facultyId?: string;
  isDeleted?: boolean;
};

export type CourseResponse = ResponseWithTotal<Course[]>;
export type CourseByIdResponse = {
  data: Course
};
export type CourseDeleted = {
  data: string;
}
export class CourseApi {
  static async getCourses(params: GetCourseRequest): Promise<CourseResponse> {
    return await apiGet(
      "/course",
      getFormData(params)
    );
  }

  static async getCourse(id: Course["courseId"]): Promise<CourseByIdResponse> {
    return await apiGet(`/course/${id}`, {});
}

  static async createCourse(course: Omit<Course, "courseId">): Promise<Course> {
    return await apiPost("/course", course);
  }

  static async updateCourse({
    id,
    course,
  }: {
    id: Course["courseId"];
    course: Partial<Course>;
  }): Promise<Course> {
    return await apiPut(`/course/${id}`, course);
  }

  static async deleteCourse(id: Course["courseId"]): Promise<CourseDeleted> {
    return await apiDelete(`/course/${id}`, {});
  }
} 