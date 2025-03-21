import { AddressApi } from "@/api/address";
import useFunction, {
  DEFAULT_FUNCTION_RETURN,
  UseFunctionReturnType,
} from "@/hooks/use-function";
import { Country } from "@/types/address";
import {
  createContext,
  ReactNode,
  useContext,
  useEffect,
  useMemo,
} from "react";

interface ContextValue {
  getCountriesApi: UseFunctionReturnType<void, Country[]>;
  countries: Country[];
}

export const MainContext = createContext<ContextValue>({
  getCountriesApi: DEFAULT_FUNCTION_RETURN,
  countries: [],
});

const MainProvider = ({ children }: { children: ReactNode }) => {
  const getCountriesApi = useFunction(AddressApi.getCountries);

  const countries = useMemo(
    () => getCountriesApi.data || [],
    [getCountriesApi.data]
  );

  useEffect(() => {
    getCountriesApi.call({});
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, []);

  return (
    <MainContext.Provider value={{ getCountriesApi, countries }}>
      {children}
    </MainContext.Provider>
  );
};

export const useMainContext = () => useContext(MainContext);

export default MainProvider;
