"use client";
import { IconButton, InputAdornment, TextField } from "@mui/material";
import { Search as SearchIcon } from "@mui/icons-material";
import React, { useCallback, useState } from "react";
import { useTranslations } from "next-intl";

function SearchBar({ onSearch }: { onSearch: (value: string) => void }) {
  const t = useTranslations("dashboard.search");
  const [searchQuery, setSearchQuery] = useState("");
  const handleSearchChange = useCallback(
    (event: React.ChangeEvent<HTMLInputElement>) => {
      setSearchQuery(event.target.value);
    },
    []
  );

  const handleSearchClick = useCallback(() => {
    onSearch(searchQuery);
  }, [searchQuery, onSearch]);

  return (
    <TextField
      fullWidth
      id="outlined-basic"
      label={t("label")}
      placeholder={t("placeholder")}
      variant="outlined"
      size="small"
      onChange={handleSearchChange}
      slotProps={{
        input: {
          endAdornment: (
            <InputAdornment position="start">
              <IconButton onClick={handleSearchClick}>
                <SearchIcon />
              </IconButton>
            </InputAdornment>
          ),
        },
      }}
    />
  );
}

export default SearchBar;
