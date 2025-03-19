import { useEffect, useMemo, useState } from "react";
import { useDialog } from "@/hooks/use-dialog";
import useFunction from "@/hooks/use-function";
import { ManagementApi } from "@/api/managements";

export const useProgram = () => {
  const dialog = useDialog();

  const getProgramApi = useFunction(ManagementApi.getProgram, {
    disableResetOnCall: true,
  });

  useEffect(() => {
    getProgramApi.call({});
  }, []);

  const addProgramApi = useFunction(ManagementApi.addProgram, {
    successMessage: "Thêm chương trình thành công",
    onSuccess: ({ result }) => {
      getProgramApi.setData(
        [...(getProgramApi.data || []), result],
    );
    },
  });

  const updateProgramApi = useFunction(ManagementApi.updateProgram, {
    successMessage: "Cập nhật chương trình thành công",
    onSuccess: ({ result }) => {
      getProgramApi.setData(
        (getProgramApi.data || []).map((f) =>
          f.id === result.id ? result : f
        )
      );
    },
  });

  const deleteProgramApi = useFunction(ManagementApi.deleteProgram, {
    successMessage: "Xóa chương trình thành công",
    onSuccess: ({ payload }) => {
      getProgramApi.setData(
        (getProgramApi.data || []).filter((f) => f.id !== payload)
      );
    },
  });

  return {
    dialog,
    getProgramApi,
    addProgramApi,
    updateProgramApi,
    deleteProgramApi,
  };
};