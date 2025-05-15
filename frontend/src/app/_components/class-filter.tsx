"use client";

import { useMemo } from "react";
import SelectFilter from "../dashboard/_components/select-filter";
import { useTranslations } from "next-intl";

interface ClassFilterProps {
  filter: Record<string, string>;
  onChange: (key: string, value: string) => void;
}

const ClassFilter = ({ filter, onChange }: ClassFilterProps) => {
  const t = useTranslations();

  const classFilterConfig = useMemo(
    () => [
      {
        label: t("classes.filters.semester"),
        key: "semester",
        options: [
          {
            value: t("common.filters.all"),
            label: t("common.filters.all"),
          },
          {
            value: "1",
            label: t("classes.filters.semester1"),
          },
          {
            value: "2",
            label: t("classes.filters.semester2"),
          },
          {
            value: "3",
            label: t("classes.filters.semester3"),
          },
        ],
        xs: 6,
      },
      {
        label: t("classes.filters.academicYear"),
        key: "academicYear",
        options: [
          {
            value: "2025",
            label: "2025",
          },
          {
            value: "2024",
            label: "2024",
          },
          {
            value: "2023",
            label: "2023",
          },
          {
            value: "2022",
            label: "2022",
          },
        ],
        xs: 6,
      },
    ],
    [t]
  );
  return (
    <SelectFilter
      configs={classFilterConfig}
      filter={filter}
      onChange={onChange}
    />
  );
};

export default ClassFilter;
