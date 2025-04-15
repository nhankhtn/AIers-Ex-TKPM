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
  const pagination = usePagination({
    count: getRegisterableClassApi.data?.data?.length || 0,
    initialRowsPerPage: 10,
  });
  const classes = useMemo(
    () =>
      (getRegisterableClassApi.data?.data || []).slice(
        (pagination.page - 1) * pagination.rowsPerPage,
        pagination.page * pagination.rowsPerPage
      ),
    [getRegisterableClassApi.data, pagination.page, pagination.rowsPerPage]
  );

  const classFilterConfig = useMemo(
    () => [
      {
        label: "Học kì",
        key: "semester",
        options: [
          {
            value: "Tất cả",
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
            value: "2024",
            label: "2024",
          },
          {
            value: "2023",
            label: "2023",
          },
          {
            value: "2022",
            label: "2022",
          },
        ],
        xs: 6,
      },
    ],
    []
  );

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
    getRegisterableClassApi,
    classes,
    filter,
    setFilter,
    classFilterConfig,
  };
};

export default useRegistrationsSearch;
