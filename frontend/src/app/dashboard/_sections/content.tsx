"use client";
import React, { useCallback, useEffect, useMemo, useState } from "react";
import { Student } from "@/types/student";
import { Box, Typography, Paper, Button, DialogTitle } from "@mui/material";
import { People as PeopleIcon, Add as AddIcon } from "@mui/icons-material";
import Grid from "@mui/material/Grid2";
import Table from "@/components/table";
import { StudentCols } from "@/constants";
import SearchBar from "@/app/dashboard/_components/search-bar";
import {
  Dialog as MuiDialog ,
  DialogContent,
  DialogActions,

} from "@mui/material";
import Dialog from "@/app/dashboard/_components/dialog";
import RowStack from "@/components/row-stack";
import { useDialog } from "@/hooks/use-dialog";
import usePagination from "@/hooks/use-pagination";
import DialogConfirmDelete from "../_components/dialog-confirm-delete";
import { StudentApi } from "@/api/students";
import useFunction from "@/hooks/use-function";

const Content = () => {
  const getStudentsApi = useFunction(StudentApi.getStudents);
  const deleteStudentsApi = useFunction(StudentApi.deleteStudent);
  const createStudentsApi = useFunction(StudentApi.createStudent);
  const updatetudentsApi = useFunction(StudentApi.updateStudent);

  const students = useMemo(
    () => getStudentsApi.data?.data || [],
    [getStudentsApi.data]
  );

  const [filter, setFilter] = useState<{
    key: string;
  }>({
    key: "",
  });
  const pagination = usePagination({
    count: 0,
    initialRowsPerPage: 10,
  });

  useEffect(() => {
    getStudentsApi.call({
      page: pagination.page + 1,
      limit: pagination.rowsPerPage,
      key: filter.key,
    });
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [pagination.page, pagination.rowsPerPage, filter.key]);

  const handleAddStudent = useCallback(
    async (student: Student) => {
      for (let i = 0; i < students.length; i++) {
        if (students[i].email === student.email) {
          return "Email đã tồn tại";
        }
      }
      const res = await createStudentsApi.call(student);
      if (!createStudentsApi.error && !createStudentsApi.loading) {
        getStudentsApi.setData({
          data: [...students, res?.data ? res.data : student],
          total: getStudentsApi.data?.total ? getStudentsApi.data.total + 1 : 1,
        });
      }
    },
    [createStudentsApi, getStudentsApi, students]
  );

  const handleUpdateStudent = useCallback(
    async (student: Student) => {
      await updatetudentsApi.call({
        id: student.id as string,
        student,
      });
      if (!updatetudentsApi.error && !updatetudentsApi.loading) {
        getStudentsApi.setData({
          data: students.map((s) => (s.id === student.id ? student : s)),
          total: getStudentsApi.data?.total || 0,
        });
      }
    },
    [updatetudentsApi, getStudentsApi, students]
  );

  const handleDeleteStudent = useCallback(
    async (studentId: string) => {
      await deleteStudentsApi.call(studentId);
      if (!deleteStudentsApi.error && !deleteStudentsApi.loading) {
        getStudentsApi.setData({
          data: students.filter((s) => s.id !== studentId),
          total: getStudentsApi.data?.total ? getStudentsApi.data.total - 1 : 0,
        });
      }
    },
    [deleteStudentsApi, getStudentsApi, students]
  );

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
        <Typography variant="h5" fontWeight="bold">
          Danh sách sinh viên
        </Typography>
        <RowStack sx={{ gap: 1 }}>
          <Button
            variant="contained"
            color="success"
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
        direction="row"
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
            <Typography variant="body2" color="text.secondary">
              Tổng số sinh viên
            </Typography>
            <Typography variant="h5" fontWeight="bold">
              {getStudentsApi.data?.total || 0}
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
        data={students || []}
        fieldCols={StudentCols}
        onClickEdit={dialog.handleOpen}
        deleteStudent={dialogConfirmDelete.handleOpen}
        pagination={pagination}
      />
      <Dialog
        isOpen={dialog.open}
        student={dialog.data || null}
        onClose={dialog.handleClose}
        addStudent={handleAddStudent}
        updateStudent={handleUpdateStudent}
      />
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
