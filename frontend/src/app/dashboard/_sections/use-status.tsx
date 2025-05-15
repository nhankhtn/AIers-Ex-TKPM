import { useEffect, useMemo } from "react";
import { useDialog } from "@/hooks/use-dialog";
import useFunction from "@/hooks/use-function";
import { StatusApi } from "@/api/status";
import { useMainContext } from "@/context/main/main-context";
import { useTranslations } from "next-intl";

export const useStatus = () => {
  const dialog = useDialog();
  const t = useTranslations("dashboard.messages");

  const getStatusApi = useFunction(StatusApi.getStatus, {
    disableResetOnCall: true,
  });

  const statuses = useMemo(
    () => getStatusApi.data?.data || [],
    [getStatusApi.data]
  );

  useEffect(() => {
    getStatusApi.call({});
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, []);

  const addStatusApi = useFunction(StatusApi.addStatus, {
    successMessage: t("addStatusSuccess"),
    onSuccess: ({ result }) => {
      getStatusApi.setData({
        data: [...(getStatusApi.data?.data || []), result.data],
      });
    },
  });

  const updateStatusApi = useFunction(StatusApi.updateStatus, {
    successMessage: t("updateStatusSuccess"),
    onSuccess: ({ result }) => {
      getStatusApi.setData({
        data: (getStatusApi.data?.data || []).map((f) =>
          f.id === result.data.id ? result.data : f
        ),
      });
    },
  });

  const deleteStatusApi = useFunction(StatusApi.deleteStatus, {
    successMessage: t("deleteStatusSuccess"),
    onSuccess: ({ payload }) => {
      getStatusApi.setData({
        data: (getStatusApi.data?.data || []).filter((f) => f.id !== payload),
      });
    },
  });

  return {
    dialog,
    getStatusApi,
    addStatusApi,
    updateStatusApi,
    deleteStatusApi,
    statuses,
  };
};
