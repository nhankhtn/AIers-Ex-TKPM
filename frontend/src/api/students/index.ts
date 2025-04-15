import { ResponseWithData, ResponseWithTotal } from "@/types";
import { Class } from "@/types/class";
import { Student, StudentFilter } from "@/types/student";
import {
  apiDelete,
  apiGet,
  apiPost,
  apiPut,
  removeEmptyKeys,
} from "@/utils/api-request";

export type GetStudentRequest = {
  page: number | null;
  limit: number | null;
  key?: string;
} & Partial<StudentFilter>;
export type StudentResponse = ResponseWithTotal<Student[]>;

export class StudentApi {
  static async getStudents({
    page,
    limit,
    key,
    status,
    faculty,
  }: GetStudentRequest): Promise<StudentResponse> {
    return await apiGet(
      "/students",
      removeEmptyKeys({
        page,
        limit,
        key,
        status,
        faculty,
      })
    );
  }

  static async createStudent(students: Omit<Student, "id">[]): Promise<{
    data: Student[];
    errors?: {
      index: number;
      code: string;
    }[];
  }> {
    return await apiPost("/students", students);
  }

  static async updateStudent({
    id,
    student,
  }: {
    id: Student["id"];
    student: Partial<Student | Omit<Student, "email">>;
  }): Promise<void> {
    return await apiPut(`/students/${id}`, student);
  }

  static async deleteStudent(id: Student["id"]): Promise<void> {
    return await apiDelete(`/students/${id}`, {});
  }

  static async registerClass({
    studentId,
    classIds,
  }: {
    studentId: string;
    classIds: string[];
  }) {
    return await apiPost(`/students/register/${studentId}`, classIds);
  }

  static async getRegisterableClass(
    studentId: string
  ): Promise<ResponseWithData<Class[]>> {
    return await apiGet(
      `/students/registerable-classes?studentId=${studentId}`
    );
  }

  static async deleteRegisterClass({
    studentId,
    classId,
  }: {
    studentId: string;
    classId: string;
  }): Promise<ResponseWithData<Class[]>> {
    return await apiDelete(`/students/register`, {
      studentId,
      classId,
    });
  }

  static async getStudentTranscript(studentId: string) {
    return await apiGet(`/students/transcript?studentId=${studentId}`);
  }
}
// : Promise<
//     ResponseWithData<{ classId: string; courseName: string; studentId: string }>
//   >
