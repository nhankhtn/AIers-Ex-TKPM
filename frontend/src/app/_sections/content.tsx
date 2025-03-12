"use client";
import React, { useEffect, useState } from "react";
import { mockData, Student } from "@/types/student";
import { Box, Typography, Paper, Button } from "@mui/material";
import { People as PeopleIcon, Add as AddIcon } from "@mui/icons-material";
import Grid from "@mui/material/Grid2";
import Table from "@/components/table";
import { StudentCols } from "@/constants";
import SearchBar from "@/app/_components/search-bar";
import Dialog from "@/app/_components/dialog";
import RowStack from "@/components/row-stack";
import { useDialog } from "@/hooks/use-dialog";
import usePagination from "@/hooks/use-pagination";
import DialogConfirmDelete from "../_components/dialog-confirm-delete";

const Content = () => {
  const [total, setTotal] = useState(10);
  const pagination = usePagination({
    count: total,
    initialRowsPerPage: 10,
  });
  useEffect(() => {
    getStudents(pagination.page, pagination.rowsPerPage);
  }, [pagination.page, pagination.rowsPerPage]);
  const [students, setStudents] = useState<Student[]>([]);

  const dialog = useDialog<Student>();
  const dialogConfirmDelete = useDialog<Student>();

  const addStudent = (student: Student) => {
    setStudents([...students, student]);
    setTotal(total + 1);
  };
  const deleteStudent = (id: string) => {
    setStudents(students.filter((student) => student.id !== id));
    setTotal(total - 1);
  };
  const updateStudent = (student: Student) => {
    setStudents(students.map((s) => (s.id === student.id ? student : s)));
  };
  const getStudents = async (page: number, rowsPerPage: number) => {
    console.log(page, rowsPerPage);
    await setTimeout(() => {}, 1000);
    setStudents(mockData.slice(page * rowsPerPage, (page + 1) * rowsPerPage));
  };
  const searchStudent = async (query: string) => {
    await setTimeout(() => {}, 1000);
    setStudents(
      mockData.filter(
        (student) => student.id.includes(query) || student.name.includes(query)
      )
    );
  };
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
              {total}
            </Typography>
          </Box>
        </Paper>
        <Grid size={{ xs: 4, md: 2 }}>
          <SearchBar search={searchStudent} />
        </Grid>
      </Grid>
      <Grid container spacing={3} sx={{ mb: 3 }}>
        <Grid size={{ xs: 12, sm: 6, md: 2 }}></Grid>
      </Grid>
      <Table
        data={students}
        fieldCols={StudentCols}
        onClickEdit={dialog.handleOpen}
        deleteStudent={dialogConfirmDelete.handleOpen}
        pagination={pagination}
      />
      <Dialog
        isOpen={dialog.open}
        student={dialog.data || null}
        onClose={dialog.handleClose}
        addStudent={addStudent}
        updateStudent={updateStudent}
      />
      {dialogConfirmDelete.data && (
        <DialogConfirmDelete
          open={dialogConfirmDelete.open}
          onClose={dialogConfirmDelete.handleClose}
          onConfirm={() => {
            deleteStudent(dialogConfirmDelete.data?.id as string);
            dialogConfirmDelete.handleClose();
          }}
          data={dialogConfirmDelete.data}
        />
      )}
    </Box>
  );
};

export default Content;
