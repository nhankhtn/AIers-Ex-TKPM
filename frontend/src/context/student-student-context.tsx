import { GetStudentRequest, StudentApi, StudentResponse } from "@/api/students";
import useFunction, {
  DEFAULT_FUNCTION_RETURN,
  UseFunctionReturnType,
} from "@/hooks/use-function";
import usePagination, { UsePaginationResult } from "@/hooks/use-pagination";
import { Student } from "@/types/student";
import { useContext, createContext } from "react";

interface ContextValue {
  getStudentsApi: UseFunctionReturnType<GetStudentRequest, StudentResponse>;

  pagination: UsePaginationResult;
}

const StudentContext = createContext<ContextValue>({
  getStudentsApi: DEFAULT_FUNCTION_RETURN,

  pagination: {
    count: 0,
    page: 1,
    rowsPerPage: 10,
    totalPages: 0,
    onPageChange: () => {},
    onRowsPerPageChange: () => {},
  },
});

const StudentProvider = ({ children }: { children: React.ReactNode }) => {
  const getStudentsApi = useFunction(StudentApi.getStudents, {
    disableResetOnCall: true,
  });

  const pagination = usePagination({
    count: getStudentsApi.data?.total || 0,
    initialRowsPerPage: 10,
  });

  return (
    <StudentContext.Provider
      value={{
        getStudentsApi,
        pagination,
      }}
    >
      {children}
    </StudentContext.Provider>
  );
};
export default StudentProvider;

export const useStudentContext = () => useContext(StudentContext);
