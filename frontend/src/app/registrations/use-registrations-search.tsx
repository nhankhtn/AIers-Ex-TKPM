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
  const [filter, setFilter] = useState<Partial<Class>>({});
  const getClassesApi = useFunction(ClassApi.getClasses, {
    disableResetOnCall: true,
  });
  const pagination = usePagination({
    count: getClassesApi.data?.total || 0,
    initialRowsPerPage: 10,
  });
  const classes = useMemo(
    () => getClassesApi.data?.data || [],
    [getClassesApi.data]
  );

  const classFilterConfig = useMemo(
    () => [
      {
        label: "Học kì",
        key: "semester",
        options: [
          {
            value: "",
            label: "Tất cả",
          },
          {
            value: "1",
            label: "Học kì 1",
          },
          {
            value: "2",
            label: "Học kì 2",
          },
          {
            value: "3",
            label: "Học kì 3",
          },
        ],
        xs: 6,
      },
      {
        label: "Năm học",
        key: "academicYear",
        options: [
          {
            value: "2025",
            label: "2025",
          },
          {
            value: "1",
            label: "Học kì 1",
          },
          {
            value: "2",
            label: "Học kì 2",
          },
          {
            value: "3",
            label: "Học kì 3",
          },
        ],
        xs: 6,
      },
    ],
    []
  );

  useEffect(() => {
    getClassesApi.call({
      page: pagination.page,
      limit: pagination.rowsPerPage,
      semester: filter.semester,
    });
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [pagination.page, pagination.rowsPerPage]);

  const students = useMemo(
    () => getStudentsApi.data?.data || [],
    [getStudentsApi.data]
  );

  useEffect(() => {
    getStudentsApi.call({
      page: 1,
      limit: 10,
    });
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, []);

  return {
    students,
    getStudentsApi,
    pagination,
    getClassesApi,
    classes,
    filter,
    setFilter,
  };
};

export default useRegistrationsSearch;
