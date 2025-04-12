import { useEffect, useMemo } from "react";
import useFunction from "@/hooks/use-function";
import { ClassApi, ClassResponse, GetClassRequest, ClassDeleted } from "@/api/class";
import type { Class } from "@/types/class";
import { useRouter } from "next/navigation";

export const useClassSearch = () => {
  const router = useRouter();
  const getClassesApi = useFunction<GetClassRequest, ClassResponse>(
    (params) => ClassApi.getClasses(params),
    {
      disableResetOnCall: true,
    }
  );

  const classes = useMemo(
    () => getClassesApi.data?.data || [],
    [getClassesApi.data]
  );

  const searchClasses = (params: GetClassRequest) => {
    return getClassesApi.call(params);
  };

  const addClassApi = useFunction<Class, Class>(
    (classData) => ClassApi.createClass(classData),
    {
      successMessage: "Thêm lớp học thành công",
      onSuccess: () => {
        router.push("/classes");
      },
    }
  );

  const updateClassApi = useFunction<
    { classId: string; class: Partial<Class> },
    Class
  >(
    (params) =>
      ClassApi.updateClass({
        classId: params.classId,
        classData: params.class,
      }),
    {
      successMessage: "Cập nhật lớp học thành công",
      onSuccess: () => {
        router.push("/classes");
      },
    }
  );


  return {
    getClassesApi,
    classes,
    searchClasses,
    addClassApi,
    updateClassApi,
  };
};
