import { apiGet } from "@/utils/api-request";

export interface SettingsResponse {
  domain: string;
}

export class SettingApi {
  static async getSettings(): Promise<SettingsResponse> {
    return await apiGet("/settings/email-domain");
  }
}
