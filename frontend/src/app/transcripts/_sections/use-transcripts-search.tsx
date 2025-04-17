import { StudentApi } from "@/api/students";
import useFunction from "@/hooks/use-function";
import { useEffect, useMemo } from "react";

const useTranscriptsSearch = () => {
  const getStudentsApi = useFunction(StudentApi.getStudents);

  const students = useMemo(
    () => getStudentsApi.data?.data || [],
    [getStudentsApi.data]
  );

  useEffect(() => {
    getStudentsApi.call({
      page: null,
      limit: null,
    });
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, []);

  return {
    students,
  };
};

export default useTranscriptsSearch;
