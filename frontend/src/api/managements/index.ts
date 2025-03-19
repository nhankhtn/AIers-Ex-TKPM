import {
  apiDelete,
  apiGet,
  apiPost,
  apiPut,
  getFormData,
} from "@/utils/api-request";

export type Item = {
  id: string;
  name: string;
};


export class ManagementApi {
  static async getFaculty(): Promise<Item[]> {
    const res = await apiGet("/faculty");
    return res.data;
  }

  static async getProgram(): Promise<Item[]> {
    const res = await apiGet("/program");
    return res.data;
  }
  static async getStatus(): Promise<Item[]> {
    const res=  await apiGet("/studentstatus");
    return res.data;
  }

  static async addFaculty(faculty: Item): Promise<Item> {
    const res= await apiPost("/faculty", faculty);
    return res.data;
  }
  static async addProgram(program: Omit<Item, "id">): Promise<Item> {
    const res= await apiPost("/program", program);
    return res.data;
  }
  static async addStatus(status: Omit<Item, "id">): Promise<Item> {
    const res= await apiPost("/studentstatus", status);
    return res.data;
  }
  static async updateFaculty(faculty: Item): Promise<Item> {
    const res= await apiPut(`/faculty/${faculty.id}`, faculty);
    return res.data;
  }
  static async updateProgram(program: Item): Promise<Item> {
    const res= await apiPut(`/program/${program.id}`, program);
    return res.data;
  }
  static async updateStatus(status: Item): Promise<Item> {
    const res= await apiPut(`/studentstatus/${status.id}`, status);
    return res.data
  }
  static async deleteFaculty(id: string): Promise<void> {
    await apiDelete(`/faculty/id/${id}`, {});
  }
  static async deleteProgram(id: string): Promise<void> {
    await apiDelete(`/program//id/${id}`, {});
  }
  static async deleteStatus(id: string): Promise<void> {
    await apiDelete(`/studentstatus/id/${id}`, {});
  }

}
