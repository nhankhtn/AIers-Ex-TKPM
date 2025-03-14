"use client";
import React, { useEffect, useState } from "react";
import { mockData, Student } from "@/types/student";
import { Box, Typography, Paper, Button } from "@mui/material";
import { People as PeopleIcon, Add as AddIcon } from "@mui/icons-material";
import Grid from "@mui/material/Grid2";
import Table from "@/components/table";
import { StudentCols } from "@/constants";
import SearchBar from "@/app/dashboard/_components/search-bar";
import Dialog from "@/app/dashboard/_components/dialog";
import RowStack from "@/components/row-stack";
import { useDialog } from "@/hooks/use-dialog";
import usePagination from "@/hooks/use-pagination";
import DialogConfirmDelete from "../_components/dialog-confirm-delete";
import { useMutation, useQuery, useQueryClient } from "@tanstack/react-query";
import { StudentApi, StudentResponse } from "@/api/students";

const Content = () => {
  const [filter, setFilter] = useState<{
    key: string;
  }>({
    key: "",
  });
  const pagination = usePagination({
    count: 0,
    initialRowsPerPage: 10,
  });
  const { data, error, isLoading } = useQuery({
    queryKey: ["students", pagination.page, pagination.rowsPerPage, filter.key],
    queryFn: () =>
      StudentApi.getStudents({
        page: pagination.page + 1,
        limit: pagination.rowsPerPage,
        ...(filter.key && { key: filter.key }),
      }),
  });
  console.log("data", data);
  useEffect(() => {
    if (data) {
      pagination.setTotal(data.total || 0);
    }
  }, [data, pagination]);

  const queryClient = useQueryClient();
  const mutationCreate = useMutation({
    mutationFn: StudentApi.createStudent,
    onSuccess: (newStudent) => {
      queryClient.setQueryData<StudentResponse>(["students"], (oldData) => {
        if (!oldData) return { total: 1, data: [newStudent] };
        return {
          ...oldData,
          data: [...oldData.data, newStudent],
          total: (oldData.total || 0) + 1,
        };
      });
    },
  });

  const mutationUpdate = useMutation({
    mutationFn: (student: Student) =>
      StudentApi.updateStudent(student.id, student),
    onSuccess: (_, updatedStudent) => {
      queryClient.setQueryData<StudentResponse>(["students"], (oldData) => {
        if (!oldData) return { total: 1, data: [updatedStudent] };
        return {
          ...oldData,
          data: oldData.data.map((student) =>
            student.id === updatedStudent.id ? updatedStudent : student
          ),
        };
      });
    },
  });

  const mutationDelete = useMutation({
    mutationFn: (id: string) => StudentApi.deleteStudent(id),
    onSuccess: (_, deletedId) => {
      queryClient.setQueryData<StudentResponse>(["students"], (oldData) => {
        if (!oldData) return oldData;

        return {
          ...oldData,
          total: (oldData.total || 0) - 1,
          data: oldData.data.filter((student) => student.id !== deletedId),
        };
      });
    },
  });

  const dialog = useDialog<Student>();
  const dialogConfirmDelete = useDialog<Student>();

  return (
    <Box sx={{ p: 3, maxWidth: "100%" }}>
      <RowStack
        sx={{
          justifyContent: "space-between",
          mb: 3,
        }}
      >
        <Typography variant='h5' fontWeight='bold'>
          Danh sách sinh viên
        </Typography>
        <RowStack sx={{ gap: 1 }}>
          <Button
            variant='contained'
            color='success'
            startIcon={<AddIcon />}
            sx={{ borderRadius: "20px" }}
            onClick={() => dialog.handleOpen()}
          >
            Thêm sinh viên
          </Button>
        </RowStack>
      </RowStack>
      <Grid
        container
        direction='row'
        sx={{
          justifyContent: "space-between",
          alignItems: "center",
        }}
      >
        <Paper
          sx={{
            p: 1,
            display: "flex",
            alignItems: "center",
            border: "1px solid #f0f7ff",
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
              {data?.total}
            </Typography>
          </Box>
        </Paper>
        <Grid size={{ xs: 4, md: 2 }}>
          <SearchBar
            onSearch={(value: string) =>
              setFilter((prev) => ({
                ...prev,
                key: value,
              }))
            }
          />
        </Grid>
      </Grid>
      <Grid container spacing={3} sx={{ mb: 3 }}>
        <Grid size={{ xs: 12, sm: 6, md: 2 }}></Grid>
      </Grid>
      <Table
        data={data?.data || []}
        fieldCols={StudentCols}
        onClickEdit={dialog.handleOpen}
        deleteStudent={dialogConfirmDelete.handleOpen}
        pagination={pagination}
      />
      <Dialog
        isOpen={dialog.open}
        student={dialog.data || null}
        onClose={dialog.handleClose}
        addStudent={mutationCreate.mutate}
        updateStudent={mutationUpdate.mutate}
      />
      {dialogConfirmDelete.data && (
        <DialogConfirmDelete
          open={dialogConfirmDelete.open}
          onClose={dialogConfirmDelete.handleClose}
          onConfirm={() => {
            mutationDelete.mutate(dialogConfirmDelete.data?.id as string);
            dialogConfirmDelete.handleClose();
          }}
          data={dialogConfirmDelete.data}
        />
      )}
    </Box>
  );
};

export default Content;
