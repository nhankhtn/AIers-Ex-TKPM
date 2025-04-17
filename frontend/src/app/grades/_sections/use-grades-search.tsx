import { ClassApi } from "@/api/class";
import useFunction from "@/hooks/use-function";
import usePagination from "@/hooks/use-pagination";
import { useEffect, useMemo, useState } from "react";

const useGradesSearch = () => {
  const getClassesApi = useFunction(ClassApi.getClasses);
  const getClassScoresApi = useFunction(ClassApi.getClassScores, {
    disableResetOnCall: true,
  });

  const pagination = usePagination({
    count: getClassScoresApi.data?.length || 0,
    initialRowsPerPage: 10,
  });

  const [filter, setFilter] = useState({
    semester: "",
    academicYear: "2025",
  });

  const classes = useMemo(
    () => getClassesApi.data?.data || [],
    [getClassesApi.data]
  );

  useEffect(() => {
    getClassesApi.call({
      page: null,
      limit: null,
      ...(filter.semester ? { semester: Number(filter.semester) } : {}),
      ...(filter.academicYear ? { year: Number(filter.academicYear) } : {}),
    });
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [filter.academicYear, filter.semester]);

  return {
    getClassesApi,
    filter,
    setFilter,
    classes,
    getClassScoresApi,
    pagination,
  };
};

export default useGradesSearch;
