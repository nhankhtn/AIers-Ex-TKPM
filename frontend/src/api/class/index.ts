import { ResponseWithTotal } from "@/types";
import { Class } from "@/types/class";
import { StudentScore } from "@/types/student";
import {
  apiDelete,
  apiGet,
  apiPost,
  apiPut,
  getFormData,
} from "@/utils/api-request";

export type GetClassRequest = {
  page: number | null;
  limit: number | null;
  classId?: string;
  semester?: number;
  year?: number;
};

export type ClassResponse = ResponseWithTotal<Class[]>;

export class ClassApi {
  static async getClasses(params: GetClassRequest): Promise<ClassResponse> {
    return await apiGet("/class", getFormData(params));
  }

  static async getClass(classId: Class["classId"]): Promise<Class> {
    return await apiGet(`/class/${classId}`, {});
  }

  static async createClass(classData: Class): Promise<Class> {
    return await apiPost("/class", classData);
  }

  static async updateClass({
    classId,
    classData,
  }: {
    classId: Class["classId"];
    classData: Partial<Class>;
  }): Promise<Class> {
    return await apiPut(`/class/${classId}`, classData);
  }

  static async deleteClass(classId: Class["classId"]): Promise<Class> {
    return await apiDelete(`/class/${classId}`, {});
  }

  static async getClassScores(classId: string): Promise<StudentScore[]> {
    return await apiGet(`/class/scores?classId=${classId}`);
  }

  static async createScoresStudent({
    classId,
    scores,
  }: {
    classId: string;
    scores: {
      studentId: string;
      midTermScore: number;
      finalScore: number;
    }[];
  }): Promise<StudentScore[]> {
    return await apiPut(`/class/scores?classId=${classId}`, scores);
  }
}
