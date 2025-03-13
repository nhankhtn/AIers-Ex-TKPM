import { ResponseWithTotal } from "@/types";
import { Student } from "@/types/student";
import {
  apiDelete,
  apiGet,
  apiPost,
  apiPut,
  getFormData,
} from "@/utils/api-request";

export type GetStudentRequest = { page: number; limit: number; key?: string };
export type StudentResponse = ResponseWithTotal<Student[]>;

export class StudentApi {
  static async getStudents({
    page,
    limit,
    key,
  }: GetStudentRequest): Promise<StudentResponse> {
    return await apiGet(
      "/students",
      getFormData({
        page,
        limit,
        key,
      })
    );
  }

  static async createStudent(student: Omit<Student, "id">): Promise<Student> {
    return await apiPost("/students", student);
  }

  static async updateStudent(
    id: Student["id"],
    student: Partial<Student>
  ): Promise<void> {
    return await apiPut(`/students/${id}`, student);
  }

  static async deleteStudent(id: Student["id"]): Promise<void> {
    return await apiDelete(`/students/${id}`, {});
  }
}
