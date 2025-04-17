import { useMemo } from "react";
import SelectFilter from "../dashboard/_components/select-filter";

interface ClassFilterProps {
  filter: Record<string, string>;
  onChange: (key: string, value: string) => void;
}

const ClassFilter = ({ filter, onChange }: ClassFilterProps) => {
  const classFilterConfig = useMemo(
    () => [
      {
        label: "Học kì",
        key: "semester",
        options: [
          {
            value: "Tất cả",
            label: "Tất cả",
          },
          {
            value: "1",
            label: "Học kì 1",
          },
          {
            value: "2",
            label: "Học kì 2",
          },
          {
            value: "3",
            label: "Học kì 3",
          },
        ],
        xs: 6,
      },
      {
        label: "Năm học",
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
    []
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
