import { StudentApi } from "@/api/students";
import { useDialog } from "@/hooks/use-dialog";
import useFunction from "@/hooks/use-function";
import usePagination from "@/hooks/use-pagination";
import { mockData, Student, StudentFilter } from "@/types/student";
import { useEffect, useMemo, useState } from "react";

const useDashboardSearch = () => {
  const [filter, setFilter] = useState<StudentFilter>({
    key: "",
  });
  const students = useMemo(() => mockData, []);

  const dialog = useDialog<Student>();
  const dialogConfirmDelete = useDialog<Student>();

  const getStudentsApi = useFunction(StudentApi.getStudents, {
    disableResetOnCall: true,
  });

  const pagination = usePagination({
    count: getStudentsApi.data?.total || 0,
    initialRowsPerPage: 10,
  });

  const deleteStudentsApi = useFunction(StudentApi.deleteStudent, {
    successMessage: "Xóa sinh viên thành công",
    onSuccess: ({ payload }) => {
      getStudentsApi.setData({
        data: students.filter((s) => s.id !== payload),
        total: getStudentsApi.data?.total ? getStudentsApi.data.total - 1 : 0,
      });
    },
  });
  const createStudentsApi = useFunction(StudentApi.createStudent, {
    successMessage: "Thêm sinh viên thành công",
    onSuccess: ({ result }: { result: Student[] }) => {
      getStudentsApi.setData({
        data: [...students, ...result],
        total: (getStudentsApi.data?.total || 0) + result.length,
      });
      dialog.handleClose();
    },
  });
  const updateStudentsApi = useFunction(StudentApi.updateStudent, {
    successMessage: "Cập nhật sinh viên thành công",
    onSuccess: ({
      payload,
    }: {
      payload: {
        id: Student["id"];
        student: Partial<Student>;
      };
    }) => {
      getStudentsApi.setData({
        data: students.map((s) =>
          s.id === payload.id
            ? {
                ...s,
                ...payload.student,
              }
            : s
        ),
        total: getStudentsApi.data?.total || 0,
      });
      dialog.handleClose();
    },
  });

  useEffect(() => {
    getStudentsApi.call({
      page: pagination.page + 1,
      limit: pagination.rowsPerPage,
      key: filter.key,
    });
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [pagination.page, pagination.rowsPerPage, filter.key]);

  return {
    dialog,
    dialogConfirmDelete,
    getStudentsApi,
    deleteStudentsApi,
    createStudentsApi,
    updateStudentsApi,
    students,
    setFilter,
    pagination,
  };
};

export default useDashboardSearch;
