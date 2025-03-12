"use client";
import React from "react";
import { Status, Student } from "../types/student";
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
  Chip,
  Stack,
} from "@mui/material";
import { DeleteOutline as DeleteIcon } from "@mui/icons-material";
import RowStack from "./row-stack";
import { UsePaginationResult } from "@/hooks/use-pagination";
import CustomPagination from "./custom-pagination";
function Table({
  data,
  fieldCols,
  onClickEdit,
  deleteStudent,
  pagination,
}: {
  data: Student[];
  fieldCols: any[];
  onClickEdit: (student: Student) => void;
  deleteStudent: (student: Student) => void;
  pagination: UsePaginationResult;
}) {
  const getChip = (status: Status) => {
    switch (status) {
      case Status.Studying:
        return (
          <Chip
            label='Đang học'
            sx={{ bgcolor: "#e0f7fa", color: "#00796b" }}
          />
        );
      case Status.Graduated:
        return (
          <Chip
            label='Đã tốt nghiệp'
            sx={{ bgcolor: "#e8f5e9", color: "#2e7d32" }}
          />
        );
      case Status.DroppedOut:
        return (
          <Chip
            label='Đã thôi học'
            sx={{ bgcolor: "#ffebee", color: "#c62828" }}
          />
        );
      case Status.Suspended:
        return (
          <Chip
            label='Tạm dừng học'
            sx={{ bgcolor: "#fff3e0", color: "#ef6c00" }}
          />
        );
      default:
        return null;
    }
  };

  return (
    <Stack>
      <TableContainer
        component={Paper}
        sx={{
          maxHeight: 500,
          overflow: "auto",
        }}
      >
        <MuiTable sx={{ minWidth: 650 }} stickyHeader>
          <TableHead>
            <TableRow sx={{ bgcolor: "#f5f5f5" }}>
              {fieldCols.map(
                (field, index) =>
                  (field.type == "nested" && (
                    <TableCell
                      key={index}
                      sx={{
                        whiteSpace: "nowrap",
                      }}
                    >
                      {field.first.label} <br />
                      <Typography variant='caption' color='text.secondary'>
                        {field.second[0].label} - {field.second[1].label}
                      </Typography>
                    </TableCell>
                  )) ||
                  (field.name !== "delete" ? (
                    <TableCell
                      key={index}
                      sx={{
                        whiteSpace: "nowrap",
                      }}
                    >
                      {field.label}
                    </TableCell>
                  ) : (
                    <TableCell
                      key={index}
                      align='center'
                      sx={{
                        whiteSpace: "nowrap",
                      }}
                    ></TableCell>
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
                            component='span'
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
                        <RowStack justifyContent={"center"} gap={1}>
                          <Button
                            variant='outlined'
                            size='small'
                            sx={{ borderRadius: "20px", whiteSpace: "nowrap" }}
                            onClick={() => {
                              onClickEdit(student);
                            }}
                          >
                            Chỉnh sửa
                          </Button>
                          <IconButton
                            size='small'
                            color='error'
                            onClick={() => deleteStudent(student)}
                          >
                            <DeleteIcon fontSize='small' />
                          </IconButton>
                        </RowStack>
                      </TableCell>
                    )) ||
                    (field.type === "nested" && (
                      <TableCell key={index}>
                        <Box>
                          <Typography variant='body2'>
                            {student[field.first.name]}
                          </Typography>
                          <Typography variant='caption' color='text.secondary'>
                            {student[field.second[0].name]} -{" "}
                            {student[field.second[1].name]}
                          </Typography>
                        </Box>
                      </TableCell>
                    )) || (
                      <TableCell key={index}>{student[field.name]}</TableCell>
                    )
                )}
              </TableRow>
            ))}
          </TableBody>
        </MuiTable>
      </TableContainer>

      <CustomPagination
        pagination={pagination}
        justifyContent='end'
        px={2}
        pt={2}
        borderTop={1}
        borderColor={"divider"}
        rowsPerPageOptions={[5, 10, 15]}
      />
    </Stack>
  );
}

export default Table;
