"use client";
import { IconButton, InputAdornment, TextField } from "@mui/material";
import { Search as SearchIcon } from "@mui/icons-material";
import React, { useState } from "react";

function SearchBar({ search }: { search: (query: string) => void }) {
  const [searchQuery, setSearchQuery] = useState("");
  const handleSearchChange = (event: React.ChangeEvent<HTMLInputElement>) => {
    setSearchQuery(event.target.value);
    if (event.target.value === "") {
      search("");
    }
  };

  const handleSearchClick = () => {
    search(searchQuery);
  };
  return (
    <TextField
      fullWidth
      id='outlined-basic'
      label='Tìm học sinh'
      placeholder='Nhập MSSV hoặc họ tên'
      variant='outlined'
      size='small'
      onChange={handleSearchChange}
      slotProps={{
        input: {
          endAdornment: (
            <InputAdornment position='start'>
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
