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
    console.log("createStudent ", student);
    const res = await apiPost("/students", student);
    console.log("createStudent res: ", res);
    return res.data;
  }

  static async updateStudent(
    id: Student["id"],
    student: Partial<Student>
  ): Promise<void> {
    //console.log("updateStudent ", id, student);
    const res= await apiPut(`/students/${id}`, student);
    console.log("updateStudent res: ", res);
    return res.data;
  }

  static async deleteStudent(id: Student["id"]): Promise<void> {
    const res= await apiDelete(`/students/${id}`, {});
    console.log("deleteStudent res: ", res);
    return res.data;
  }
}
