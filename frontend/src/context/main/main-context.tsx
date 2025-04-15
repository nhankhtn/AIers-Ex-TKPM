import { AddressApi } from "@/api/address";
import { FacultyApi } from "@/api/faculty";
import { SettingApi } from "@/api/settings";
import useFunction, {
  DEFAULT_FUNCTION_RETURN,
  UseFunctionReturnType,
} from "@/hooks/use-function";
import { ResponseWithData } from "@/types";
import { Country, Province } from "@/types/address";
import { Faculty } from "@/types/student";
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
  getFacultiesApi: UseFunctionReturnType<void, ResponseWithData<Faculty[]>>;

  countries: Country[];
  provinces: Province[];
  settings: SettingsProps;
  faculties: Faculty[];
}

export const MainContext = createContext<ContextValue>({
  getCountriesApi: DEFAULT_FUNCTION_RETURN,
  updateSettingsApi: DEFAULT_FUNCTION_RETURN,
  getFacultiesApi: DEFAULT_FUNCTION_RETURN,

  countries: [],
  provinces: [],
  settings: initialSettings,
  faculties: [],
});

const MainProvider = ({ children }: { children: ReactNode }) => {
  const getCountriesApi = useFunction(AddressApi.getCountries);
  const getProvincesApi = useFunction(AddressApi.getProvinces);
  const getSettingsApi = useFunction(SettingApi.getSettings);
  const getFacultiesApi = useFunction(FacultyApi.getFaculty, {
    disableResetOnCall: true,
  });

  const [settings, setSettings] = useState<SettingsProps>(initialSettings);

  const countries = useMemo(
    () => getCountriesApi.data || [],
    [getCountriesApi.data]
  );

  const provinces = useMemo(
    () => getProvincesApi.data || [],
    [getProvincesApi.data]
  );

  const faculties = useMemo(
    () => getFacultiesApi.data?.data || [],
    [getFacultiesApi.data]
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
    getFacultiesApi.call({});
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, []);

  return (
    <MainContext.Provider
      value={{
        getCountriesApi,
        getFacultiesApi,
        countries,
        provinces,
        settings,
        updateSettingsApi,
        faculties,
      }}
    >
      {children}
    </MainContext.Provider>
  );
};

export const useMainContext = () => useContext(MainContext);

export default MainProvider;
