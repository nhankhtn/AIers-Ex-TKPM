"use client";

import React, { useCallback } from "react";
import { Student } from "@/types/student";
import {
  Box,
  Typography,
  Paper,
  Button,
  IconButton,
  Stack,
} from "@mui/material";
import { People as PeopleIcon } from "@mui/icons-material";
import SearchBar from "@/app/dashboard/_components/search-bar";
import RowStack from "@/components/row-stack";
import DialogConfirmDelete from "../_components/dialog-confirm-delete";
import useDashboardSearch from "./use-dashboard-search";
import { CustomTable } from "@/components/custom-table";
import CustomPagination from "@/components/custom-pagination";
import { useProgram } from "./use-program";
import { useStatus } from "./use-status";
import SelectFilter from "../_components/select-filter";
import { getTableConfig } from "./table-config";
import DeleteIcon from "@mui/icons-material/Delete";
import UtilityButtons from "./utility-buttons";
import { useMainContext } from "@/context/main/main-context";

const Content = () => {
  const {
    dialog,
    dialogConfirmDelete,
    getStudentsApi,
    deleteStudentsApi,
    createStudentsApi,
    updateStudentsApi,
    students,
    setFilter,
    pagination,
    filterConfig,
    filter,
  } = useDashboardSearch();
  const { faculties } = useMainContext();
  const { programs } = useProgram();
  const { statuses } = useStatus();
  const handleDeleteStudent = useCallback(
    (studentId: string) => {
      deleteStudentsApi.call(studentId);
    },
    [deleteStudentsApi]
  );

  return (
    <Box sx={{ maxWidth: "100%" }}>
      <RowStack
        sx={{
          justifyContent: "space-between",
          mb: 3,
        }}
      >
        <Typography variant='h5' fontWeight='bold'>
          Danh sách sinh viên
        </Typography>
        <UtilityButtons
          students={students}
          createStudentsApi={createStudentsApi}
          updateStudentsApi={updateStudentsApi}
          dialog={dialog}
        />
      </RowStack>
      <RowStack mb={3} gap={2}>
        <Stack>
          <Paper
            sx={{
              p: 1,
              display: "flex",
              alignItems: "center",
              border: "1px solid #f0f7ff",
              width: 250,
            }}
          >
            <RowStack
              sx={{
                bgcolor: "#f0f7ff",
                borderRadius: "50%",
                width: 50,
                height: 50,
                justifyContent: "center",
                mr: 2,
              }}
            >
              <PeopleIcon sx={{ color: "#1976d2", fontSize: 30 }} />
            </RowStack>
            <Box>
              <Typography variant='body2' color='text.secondary'>
                Tổng số sinh viên
              </Typography>
              <Typography variant='h5' fontWeight='bold'>
                {getStudentsApi.data?.total || 0}
              </Typography>
            </Box>
          </Paper>
        </Stack>
        <Stack width={500}>
          <SelectFilter
            configs={filterConfig}
            filter={filter as any}
            onChange={(key: string, value: string) => {
              setFilter((prev) => ({
                ...prev,
                [key]: value,
              }));
            }}
          />
        </Stack>

        <Stack flex={1}>
          <SearchBar
            onSearch={(value: string) =>
              setFilter((prev) => ({
                ...prev,
                key: value,
              }))
            }
          />
        </Stack>
      </RowStack>
      <Stack height={300}>
        <CustomTable
          configs={getTableConfig({
            statuses,
            faculties,
            programs,
          })}
          rows={students}
          loading={getStudentsApi.loading}
          emptyState={<Typography>Không có dữ liệu</Typography>}
          renderRowActions={(row: Student) => {
            return (
              <RowStack gap={1}>
                <Button
                  variant='outlined'
                  size='small'
                  sx={{ borderRadius: "20px", whiteSpace: "nowrap" }}
                  onClick={() => dialog.handleOpen(row)}
                >
                  Chỉnh sửa
                </Button>
                <IconButton
                  size='small'
                  color='error'
                  onClick={() => dialogConfirmDelete.handleOpen(row)}
                >
                  <DeleteIcon fontSize='small' />
                </IconButton>
              </RowStack>
            );
          }}
        />
        {students.length > 0 && (
          <CustomPagination
            pagination={pagination}
            justifyContent='end'
            px={2}
            pt={2}
            borderTop={1}
            borderColor={"divider"}
            rowsPerPageOptions={[10, 15, 20]}
          />
        )}
      </Stack>
      {dialogConfirmDelete.data && (
        <DialogConfirmDelete
          open={dialogConfirmDelete.open}
          onClose={dialogConfirmDelete.handleClose}
          onConfirm={() => {
            handleDeleteStudent(dialogConfirmDelete.data?.id as string);
            dialogConfirmDelete.handleClose();
          }}
          data={dialogConfirmDelete.data}
        />
      )}
    </Box>
  );
};

export default Content;
