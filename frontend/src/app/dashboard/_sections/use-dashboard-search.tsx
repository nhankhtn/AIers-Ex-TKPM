"use client";

import { StudentApi } from "@/api/students";
import { useDialog } from "@/hooks/use-dialog";
import useFunction from "@/hooks/use-function";
import usePagination from "@/hooks/use-pagination";
import { Student, StudentFilter } from "@/types/student";
import { useEffect, useMemo, useState } from "react";
import { getFilterConfig } from "./filter-config";
import { useFaculty } from "./use-faculty";
import { useStatus } from "./use-status";
import { useProgram } from "./use-program";
import { normalizeString } from "@/utils/string-helper";

const useDashboardSearch = () => {
  const { faculties } = useFaculty();
  const { statuses } = useStatus();
  const [filter, setFilter] = useState<StudentFilter>({
    key: "",
    status_name: "",
    faculty_name: "",
  });

  const dialog = useDialog<Student>();
  const dialogConfirmDelete = useDialog<Student>();

  const getStudentsApi = useFunction(StudentApi.getStudents, {
    disableResetOnCall: true,
  });

  const students = useMemo(
    () => getStudentsApi.data?.data || [],
    [getStudentsApi.data?.data]
  );

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
    },
  });

  useEffect(() => {
    const searchParams = new URLSearchParams(window.location.search);
    const key = searchParams.get("key");
    const status_name = searchParams.get("status_name");
    const faculty_name = searchParams.get("faculty_name");

    setFilter((prev) => ({
      ...prev,
      key: key || "",
      status_name: status_name || "",
      faculty_name: faculty_name || "",
    }));
    getStudentsApi.call({
      page: pagination.page + 1,
      limit: pagination.rowsPerPage,
      key: filter.key,
      status_name: filter.status_name,
      faculty_name: filter.faculty_name,
    });
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [
    pagination.page,
    pagination.rowsPerPage,
    filter.key,
    filter.status_name,
    filter.faculty_name,
  ]);

  useEffect(() => {
    const searchParams = new URLSearchParams(window.location.search);

    Object.entries(filter).forEach(([key, value]) => {
      if (value) {
        searchParams.set(key, value);
      } else {
        searchParams.delete(key);
      }
    });

    window.history.pushState({}, "", `?${searchParams.toString()}`);
  }, [filter]);

  const filterConfig = getFilterConfig({
    status: [
      {
        value: "",
        label: "Tất cả",
      },
      ...statuses.map((s) => ({
        value: normalizeString(s.name),
        label: s.name,
      })),
    ],
    faculty: [
      {
        value: "",
        label: "Tất cả",
      },
      ...faculties.map((s) => ({
        value: normalizeString(s.name),
        label: s.name,
      })),
    ],
  });

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
    filterConfig,
    filter,
  };
};

export default useDashboardSearch;
