import { getRequestConfig } from "next-intl/server";

export default getRequestConfig(async () => {
  // Default to Vietnamese on the server
  const locale = "vi";

  return {
    locale,
    messages: (await import(`../../messages/${locale}.json`)).default,
  };
});
