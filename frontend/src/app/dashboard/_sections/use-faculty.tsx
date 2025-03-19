import { useEffect, useMemo, useState } from "react";
import { useDialog } from "@/hooks/use-dialog";
import useFunction from "@/hooks/use-function";
import { ManagementApi } from "@/api/managements";

export const useFaculty = () => {
  const dialog = useDialog();

  const getFacultiesApi = useFunction(ManagementApi.getFaculty, {
    disableResetOnCall: true,
  });

  useEffect(() => {
    getFacultiesApi.call({});
  }, []);

  const addFacultyApi = useFunction(ManagementApi.addFaculty, {
    successMessage: "Thêm khoa thành công",
    onSuccess: ({ result }) => {
      getFacultiesApi.setData(
        [...(getFacultiesApi.data || []), result],
    );
    },
  });

  const updateFacultyApi = useFunction(ManagementApi.updateFaculty, {
    successMessage: "Cập nhật khoa thành công",
    onSuccess: ({ result }) => {
      getFacultiesApi.setData(
        (getFacultiesApi.data || []).map((f) =>
          f.id === result.id ? result : f
        )
      );
    },
  });

  const deleteFacultyApi = useFunction(ManagementApi.deleteFaculty, {
    successMessage: "Xóa khoa thành công",
    onSuccess: ({ payload }) => {
      getFacultiesApi.setData(
        (getFacultiesApi.data || []).filter((f) => f.id !== payload)
      );
    },
  });

  return {
    dialog,
    getFacultiesApi,
    addFacultyApi,
    updateFacultyApi,
    deleteFacultyApi,
  };
};