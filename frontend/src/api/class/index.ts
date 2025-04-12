import { ResponseWithTotal } from "@/types";
import { Class } from "@/types/class";
import { apiDelete, apiGet, apiPost, apiPut, getFormData } from "@/utils/api-request";

export type GetClassRequest = {
  page: number;
  limit: number;
  classId?: string;
  semester?: number;
};

export type ClassResponse = ResponseWithTotal<Class[]>;

export type ClassByIdResponse = {
  data: Class;
};

export type ClassDeleted = {
  data: Class;
}
export class ClassApi {
  static async getClasses(params: GetClassRequest): Promise<ClassResponse> {
    return await apiGet("/class", getFormData(params));
  }

  static async getClass(classId: Class["classId"]): Promise<ClassByIdResponse> {
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

  static async deleteClass(classId: Class["classId"]): Promise<ClassDeleted> {
    return await apiDelete(`/class/${classId}`, {});
  }
}
