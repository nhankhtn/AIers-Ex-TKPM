import RowStack from "@/components/row-stack";
import {
  FormControl,
  Grid2,
  InputLabel,
  MenuItem,
  Select,
  SelectChangeEvent,
} from "@mui/material";
import { useCallback } from "react";
import { useTranslations } from "next-intl";

interface SelectFilterProps {
  configs: {
    label: string;
    xs: number;
    key: string;
    options: {
      value: string;
      label: string;
    }[];
  }[];
  filter: {
    [key: string]: string;
  };
  onChange: (key: string, value: string) => void;
}

const SelectFilter = ({ configs, filter, onChange }: SelectFilterProps) => {
  const t = useTranslations("dashboard.filters");
  const handleChange = useCallback(
    (key: string, event: SelectChangeEvent<string>) => {
      if (event.target.value === t("all")) {
        onChange(key, "");
      } else onChange(key, event.target.value);
    },
    [onChange, t]
  );

  return (
    <RowStack spacing={1.5}>
      {configs.map(({ label, xs, key }, index) => (
        <Grid2 size={{ xs: xs }} key={key} sx={{ width: "100%" }}>
          <FormControl fullWidth>
            <InputLabel>{t(key)}</InputLabel>
            <Select
              margin="dense"
              id={key}
              label={t(key)}
              fullWidth
              variant="outlined"
              value={filter[key] === "" ? t("all") : filter[key]}
              onChange={(e) => handleChange(key, e)}
            >
              {configs[index].options.map(({ value, label }) => (
                <MenuItem key={value} value={value}>
                  {label}
                </MenuItem>
              ))}
            </Select>
          </FormControl>
        </Grid2>
      ))}
    </RowStack>
  );
};

export default SelectFilter;
