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
    const res = await apiPost("/students", student);
    return res.data;
  }

  static async updateStudent({
    id,
    student,
  }: {
    id: Student["id"];
    student: Partial<Student>;
  }): Promise<void> {
    const res = await apiPut(`/students/${id}`, student);
    return res.data;
  }

  static async deleteStudent(id: Student["id"]): Promise<void> {
    const res = await apiDelete(`/students/${id}`, {});
    return res.data;
  }
}
