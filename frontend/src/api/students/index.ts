import { ResponseWithTotal } from "@/types";
import { Student } from "@/types/student";
import { apiGet } from "@/utils/api-request";

export type GetStudentRequest = { page: number; limit: number };
export type StudentResponse = ResponseWithTotal<Student[]>;

export class StudentApi {
  static async getStudents({
    page,
    limit,
  }: GetStudentRequest): Promise<StudentResponse> {
    return await apiGet("/students", {
      page,
      limit,
    });
  }
}
