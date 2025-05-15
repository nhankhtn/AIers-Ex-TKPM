import { useEffect, useMemo } from "react";
import useFunction from "@/hooks/use-function";
import { ClassApi, ClassResponse, GetClassRequest } from "@/api/class";
import type { Class } from "@/types/class";
import { useRouter } from "next/navigation";
import { paths } from "@/paths";
import { useTranslations } from "next-intl";

export const useClassSearch = () => {
  const router = useRouter();
  const t = useTranslations();

  const getClassesApi = useFunction(ClassApi.getClasses, {
    disableResetOnCall: true,
  });

  const getClassApi = useFunction(ClassApi.getClass, {
    disableResetOnCall: true,
  });

  const classes = useMemo(
    () => getClassesApi.data?.data || [],
    [getClassesApi.data]
  );

  const searchClasses = (params: GetClassRequest) => {
    return getClassesApi.call(params);
  };

  const addClassApi = useFunction(ClassApi.createClass, {
    successMessage: t("classes.messages.addSuccess"),
    onSuccess: () => {
      router.push(paths.classes.index);
    },
  });

  const updateClassApi = useFunction(ClassApi.updateClass, {
    successMessage: t("classes.messages.updateSuccess"),
    onSuccess: () => {
      router.push(paths.classes.index);
    },
  });

  return {
    getClassesApi,
    classes,
    searchClasses,
    addClassApi,
    getClassApi,
    updateClassApi,
  };
};
