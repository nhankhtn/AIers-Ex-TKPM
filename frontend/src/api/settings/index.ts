import { apiGet, apiPut } from "@/utils/api-request";

export interface SettingsResponse {
  domain: string;
}

export class SettingApi {
  static async getSettings(): Promise<SettingsResponse> {
    return await apiGet("/settings/email-domain");
  }

  static async updateSettings(email: string): Promise<void> {
    return await apiPut("/settings/email-domain", {
      email,
    });
  }
}
