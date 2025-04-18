import { StudentApi } from "@/api/students";
import useFunction from "@/hooks/use-function";
import usePagination from "@/hooks/use-pagination";
import { useEffect, useMemo, useState } from "react";

const useUnregistrationsSearch = () => {
  const getStudentsApi = useFunction(StudentApi.getStudents, {
    disableResetOnCall: true,
  });

  const getUnregisterClassApi = useFunction(StudentApi.getUnregisterClass, {
    disableResetOnCall: true,
  });

  const [filter, setFilter] = useState({
    semester: "",
    academicYear: "2025",
  });

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

  const paginationUnregisterClass = usePagination({
    count: getUnregisterClassApi.data?.data?.length || 0,
    initialRowsPerPage: 10,
  });

  return {
    students,
    getStudentsApi,
    filter,
    setFilter,
    paginationUnregisterClass,
    getUnregisterClassApi,
  };
};

export default useUnregistrationsSearch;
