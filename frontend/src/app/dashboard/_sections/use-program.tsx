import { useEffect, useMemo } from "react";
import { useDialog } from "@/hooks/use-dialog";
import useFunction from "@/hooks/use-function";
import { ProgramApi } from "@/api/program";
import { useMainContext } from "@/context/main/main-context";
import { useTranslations } from "next-intl";

export const useProgram = () => {
  const dialog = useDialog();
  const t = useTranslations("dashboard.messages");

  const getProgramApi = useFunction(ProgramApi.getProgram, {
    disableResetOnCall: true,
  });

  const programs = useMemo(
    () => getProgramApi.data?.data || [],
    [getProgramApi.data]
  );

  useEffect(() => {
    getProgramApi.call({});
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, []);

  const addProgramApi = useFunction(ProgramApi.addProgram, {
    successMessage: t("addProgramSuccess"),
    onSuccess: ({ result }) => {
      getProgramApi.setData({
        data: [...(getProgramApi.data?.data || []), result.data],
      });
    },
  });

  const updateProgramApi = useFunction(ProgramApi.updateProgram, {
    successMessage: t("updateProgramSuccess"),
    onSuccess: ({ result }) => {
      getProgramApi.setData({
        data: (getProgramApi.data?.data || []).map((f) =>
          f.id === result.data.id ? result.data : f
        ),
      });
    },
  });

  const deleteProgramApi = useFunction(ProgramApi.deleteProgram, {
    successMessage: t("deleteProgramSuccess"),
    onSuccess: ({ payload }) => {
      getProgramApi.setData({
        data: (getProgramApi.data?.data || []).filter((f) => f.id !== payload),
      });
    },
  });

  return {
    dialog,
    getProgramApi,
    programs,
    addProgramApi,
    updateProgramApi,
    deleteProgramApi,
  };
};
