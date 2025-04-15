import { ClassApi } from "@/api/class";
import useFunction from "@/hooks/use-function";

const useGradesSearch = () => {
  const getClassesApi = useFunction(ClassApi.getClass);
};

export default useGradesSearch;
