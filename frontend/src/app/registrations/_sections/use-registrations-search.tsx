import { ClassApi } from "@/api/class";
import { StudentApi } from "@/api/students";
import useFunction from "@/hooks/use-function";
import usePagination from "@/hooks/use-pagination";
import { Class } from "@/types/class";
import { useEffect, useMemo, useState } from "react";

const useRegistrationsSearch = () => {
  const getStudentsApi = useFunction(StudentApi.getStudents, {
    disableResetOnCall: true,
  });
  const [filter, setFilter] = useState({
    semester: "",
    academicYear: "2025",
  });
  const getRegisterableClassApi = useFunction(StudentApi.getRegisterableClass, {
    disableResetOnCall: true,
  });
  const paginationRegisterClass = usePagination({
    count: getRegisterableClassApi.data?.data?.length || 0,
    initialRowsPerPage: 10,
  });
  const [classes, setClasses] = useState<Class[]>([]);

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

  const getStudentClassApi = useFunction(StudentApi.getStudentClass, {
    disableResetOnCall: true,
  });
  const paginationStudentClass = usePagination({
    count: getStudentClassApi.data?.data?.length || 0,
    initialRowsPerPage: 10,
  });

  return {
    students,
    getStudentsApi,
    paginationRegisterClass,
    getRegisterableClassApi,
    classes,
    filter,
    setFilter,
    setClasses,
    paginationStudentClass,
    getStudentClassApi,
  };
};

export default useRegistrationsSearch;
