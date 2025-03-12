"use client";
import React, { useEffect, useState } from "react";
import { Student } from "../types/student";
import {
  Box,
  Typography,
  Button,
  Paper,
  Table as MuiTable,
  TableBody,
  TableCell,
  TableContainer,
  TableHead,
  TableRow,
  IconButton,
  FormControl,
  Select,
  MenuItem,
  Stack,
  Chip,
  SelectChangeEvent,
} from "@mui/material";
import {
  DeleteOutline as DeleteIcon,
  Edit as EditIcon,
  NavigateBefore as NavigateBeforeIcon,
  NavigateNext as NavigateNextIcon,
} from "@mui/icons-material";
import { useRouter } from "next/navigation";
import { get } from "http";
function Table({
  data,
  fieldCols,
  updateStudent,
  setIsOpen,
  deleteStudent,
  getStudents,
  total,
  page,
  rowsPerPage,
  setPage,
  setRowsPerPage,
}: {
  data: Student[];
  fieldCols: any[];
  updateStudent: (student: Student | null) => void;
  setIsOpen: (isOpen: boolean) => void;
  deleteStudent: (id: string) => void;
  getStudents: (page: number, rowsPerPage: number) => void;
  total: number;
  page: number;
  rowsPerPage: number;
  setPage: (page: number) => void;
  setRowsPerPage: (rowsPerPage: number) => void;
}) {
  const handleChangePage = (
    newPage: number
  ) => {
    setPage(newPage);
    getStudents(newPage, rowsPerPage);
  };

  const handleChangeRowsPerPage = (event: SelectChangeEvent) => {
    const newRowsPerPage = Number.parseInt(event.target.value, 10);
    setRowsPerPage(newRowsPerPage);
    setPage(1);
    getStudents(1, newRowsPerPage);
  };
  const getChip = (status: string) => {
    switch (status) {
      case "Đang học":
        return (
          <Chip
            label="Đang học"
            sx={{ bgcolor: "#e0f7fa", color: "#00796b" }}
          />
        );
      case "Đã tốt nghiệp":
        return (
          <Chip
            label="Đã tốt nghiệp"
            sx={{ bgcolor: "#e8f5e9", color: "#2e7d32" }}
          />
        );
      case "Đã thôi học":
        return (
          <Chip
            label="Đã thôi học"
            sx={{ bgcolor: "#ffebee", color: "#c62828" }}
          />
        );
      case "Tạm dừng học":
        return (
          <Chip
            label="Tạm dừng học"
            sx={{ bgcolor: "#fff3e0", color: "#ef6c00" }}
          />
        );
      default:
        return null;
    }
  };

  return (
    <TableContainer component={Paper}>
      <MuiTable sx={{ minWidth: 650 }}>
        <TableHead>
          <TableRow sx={{ bgcolor: "#f5f5f5" }}>
            {fieldCols.map(
              (field, index) =>
                (field.type == "nested" && (
                  <TableCell key={index}>
                    {field.first.label} <br />
                    <Typography variant="caption" color="text.secondary">
                      {field.second[0].label} - {field.second[1].label}
                    </Typography>
                  </TableCell>
                )) ||
                (field.name !== "delete" ? (
                  <TableCell key={index}>{field.label}</TableCell>
                ) : (
                  <TableCell key={index} align="center"></TableCell>
                ))
            )}
          </TableRow>
        </TableHead>
        <TableBody>
          {data.map((student, index) => (
            <TableRow key={index}>
              {fieldCols.map(
                (field, index) =>
                  (field.type === "chip" && (
                    <TableCell key={index}>
                      {getChip(student[field.name])}
                    </TableCell>
                  )) ||
                  (field.type === "icon" && (
                    <TableCell key={index}>
                      <Box sx={{ display: "flex", alignItems: "center" }}>
                        <Box
                          component="span"
                          sx={{
                            width: 30,
                            height: 30,
                            borderRadius: "8px",
                            bgcolor: "#f0f0f0",
                            display: "inline-flex",
                            alignItems: "center",
                            justifyContent: "center",
                            mr: 1,
                          }}
                        >
                          👤
                        </Box>
                        {student[field.name]}
                      </Box>
                    </TableCell>
                  )) ||
                  (field.type === "action" && (
                    <TableCell key={index}>
                      <Box
                        sx={{
                          display: "flex",
                          justifyContent: "center",
                          gap: 1,
                        }}
                      >
                        <Button
                          variant="outlined"
                          size="small"
                          sx={{ borderRadius: "20px" }}
                          onClick={() => {
                            updateStudent(student);
                            setIsOpen(true);
                          }}
                        >
                          Chỉnh sửa
                        </Button>
                        <IconButton size="small" color="error" onClick={() => deleteStudent(student.id)}>
                          <DeleteIcon fontSize="small" />
                        </IconButton>
                      </Box>
                    </TableCell>
                  )) ||
                  (field.type === "nested" && (
                    <TableCell key={index}>
                      <Box>
                        <Typography variant="body2">
                          {student[field.first.name]}
                        </Typography>
                        <Typography variant="caption" color="text.secondary">
                          {student[field.second[0].name]} -{" "}
                          {student[field.second[1].name]}
                        </Typography>
                      </Box>
                    </TableCell>
                  )) || <TableCell key={index}>{student[field.name]}</TableCell>
              )}
            </TableRow>
          ))}
        </TableBody>
      </MuiTable>
      <Box
        sx={{
          display: "flex",
          justifyContent: "space-between",
          alignItems: "center",
          p: 2,
        }}
      >
        <Box sx={{ display: "flex", alignItems: "center" }}>
          <Typography variant="body2" sx={{ mr: 2 }}>
            Số hàng hiển thị
          </Typography>
          <FormControl size="small" sx={{ minWidth: 70 }}>
            <Select
              value={rowsPerPage.toString()}
              onChange={handleChangeRowsPerPage}
            >
              <MenuItem value={5}>5</MenuItem>
              <MenuItem value={10}>10</MenuItem>
              <MenuItem value={25}>25</MenuItem>
            </Select>
          </FormControl>
          <Typography variant="body2" sx={{ ml: 2 }}>
            {page * rowsPerPage - rowsPerPage + 1} -{" "}
            {page * rowsPerPage > total
              ? total
              : page * rowsPerPage}{" "}
            trên {total}
          </Typography>
        </Box>
        <Stack direction="row" spacing={1}>
          <IconButton
            size="small"
            disabled={page === 1}
            onClick={() => handleChangePage(page - 1)}
          >
            <NavigateBeforeIcon />
          </IconButton>
          <IconButton
            size="small"
            disabled={page * rowsPerPage >= total}
            onClick={() => handleChangePage(page + 1)}
          >
            <NavigateNextIcon />
          </IconButton>
        </Stack>
      </Box>
    </TableContainer>
  );
}

export default Table;
