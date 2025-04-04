import { AddressApi } from "@/api/address";
import { SettingApi } from "@/api/settings";
import useFunction, {
  DEFAULT_FUNCTION_RETURN,
  UseFunctionReturnType,
} from "@/hooks/use-function";
import { Country, Province } from "@/types/address";
import {
  createContext,
  ReactNode,
  useCallback,
  useContext,
  useEffect,
  useMemo,
  useState,
} from "react";

export interface SettingsProps {
  allowedEmailDomains: string[];
}

export const initialSettings: SettingsProps = {
  allowedEmailDomains: [],
};

interface ContextValue {
  getCountriesApi: UseFunctionReturnType<void, Country[]>;
  updateSettingsApi: UseFunctionReturnType<string, void>;

  countries: Country[];
  provinces: Province[];
  settings: SettingsProps;
}

export const MainContext = createContext<ContextValue>({
  getCountriesApi: DEFAULT_FUNCTION_RETURN,
  updateSettingsApi: DEFAULT_FUNCTION_RETURN,

  countries: [],
  provinces: [],
  settings: initialSettings,
});

const MainProvider = ({ children }: { children: ReactNode }) => {
  const getCountriesApi = useFunction(AddressApi.getCountries);
  const getProvincesApi = useFunction(AddressApi.getProvinces);
  const getSettingsApi = useFunction(SettingApi.getSettings);

  const [settings, setSettings] = useState<SettingsProps>(initialSettings);

  const countries = useMemo(
    () => getCountriesApi.data || [],
    [getCountriesApi.data]
  );

  const provinces = useMemo(
    () => getProvincesApi.data || [],
    [getProvincesApi.data]
  );

  const updateSettings = useCallback(
    async (email: string) => {
      await SettingApi.updateSettings(email);
      getSettingsApi.setData({
        domain: email,
      });
    },
    [getSettingsApi]
  );
  const updateSettingsApi = useFunction(updateSettings, {
    successMessage: "Cập nhật thành công",
  });

  useEffect(() => {
    if (getSettingsApi.data) {
      const email = getSettingsApi.data.domain;
      setSettings({
        allowedEmailDomains: email.split(",").map((domain) => domain.trim()),
      });
    }
  }, [getSettingsApi.data]);

  useEffect(() => {
    getCountriesApi.call({});
    getProvincesApi.call({});
    getSettingsApi.call({});
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, []);

  return (
    <MainContext.Provider
      value={{
        getCountriesApi,
        countries,
        provinces,
        settings,
        updateSettingsApi,
      }}
    >
      {children}
    </MainContext.Provider>
  );
};

export const useMainContext = () => useContext(MainContext);

export default MainProvider;
