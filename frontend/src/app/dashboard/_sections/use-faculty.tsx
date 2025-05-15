import { useDialog } from "@/hooks/use-dialog";
import useFunction from "@/hooks/use-function";
import { FacultyApi } from "@/api/faculty";
import { useMainContext } from "@/context/main/main-context";
import { useTranslations } from "next-intl";

export const useFaculty = () => {
  const dialog = useDialog();
  const t = useTranslations("dashboard.messages");
  const { getFacultiesApi } = useMainContext();

  const addFacultyApi = useFunction(FacultyApi.addFaculty, {
    successMessage: t("addFacultySuccess"),
    onSuccess: ({ result }) => {
      getFacultiesApi.setData({
        data: [...(getFacultiesApi.data?.data || []), result.data],
      });
    },
  });

  const updateFacultyApi = useFunction(FacultyApi.updateFaculty, {
    successMessage: t("updateFacultySuccess"),
    onSuccess: ({ result }) => {
      getFacultiesApi.setData({
        data: (getFacultiesApi.data?.data || []).map((f) =>
          f.id === result.data.id ? result.data : f
        ),
      });
    },
  });

  const deleteFacultyApi = useFunction(FacultyApi.deleteFaculty, {
    successMessage: t("deleteFacultySuccess"),
    onSuccess: ({ payload }) => {
      getFacultiesApi.setData({
        data: (getFacultiesApi.data?.data || []).filter(
          (f) => f.id !== payload
        ),
      });
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
