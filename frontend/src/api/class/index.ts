import { ResponseWithTotal } from "@/types";
import { Class } from "@/types/class";
import { apiDelete, apiGet, apiPost, apiPut, getFormData } from "@/utils/api-request";

export type GetClassRequest = {
  page: number;
  limit: number;
  courseId?: string;
  semester?: string;
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
    return await apiGet(
      "/class",
      getFormData(params)
    );
  }

  static async getClass(id: Class["id"]): Promise<ClassByIdResponse> {
    return await apiGet(`/class/${id}`, {});
  }

  static async createClass(classData: Class): Promise<Class> {
    return await apiPost("/class", classData);
  }

  static async updateClass({
    id,
    classData,
  }: {
    id: Class["id"];
    classData: Partial<Class>;
  }): Promise<Class> {
    return await apiPut(`/class/${id}`, classData);
  }

  static async deleteClass(id: Class["id"]): Promise<ClassDeleted> {
    return await apiDelete(`/class/${id}`, {});
  }
}
