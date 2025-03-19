import { useEffect, useMemo, useState } from "react";
import { useDialog } from "@/hooks/use-dialog";
import useFunction from "@/hooks/use-function";
import { ManagementApi } from "@/api/managements";

export const useStatus = () => {
  const dialog = useDialog();

  const getStatusApi = useFunction(ManagementApi.getStatus, {
    disableResetOnCall: true,
  });

  useEffect(() => {
    getStatusApi.call({});
  }, []);

  const addStatusApi = useFunction(ManagementApi.addStatus, {
    successMessage: "Thêm trạng thái thành công",
    onSuccess: ({ result }) => {
      getStatusApi.setData(
        [...(getStatusApi.data || []), result],
    );
    },
  });

  const updateStatusApi = useFunction(ManagementApi.updateStatus, {
    successMessage: "Cập nhật trạng thái thành công",
    onSuccess: ({ result }) => {
      getStatusApi.setData(
        (getStatusApi.data || []).map((f) =>
          f.id === result.id ? result : f
        )
      );
    },
  });

  const deleteStatusApi = useFunction(ManagementApi.deleteStatus, {
    successMessage: "Xóa trạng thái thành công",
    onSuccess: ({ payload }) => {
      getStatusApi.setData(
        (getStatusApi.data || []).filter((f) => f.id !== payload)
      );
    },
  });

  return {
    dialog,
    getStatusApi,
    addStatusApi,
    updateStatusApi,
    deleteStatusApi,
  };
};