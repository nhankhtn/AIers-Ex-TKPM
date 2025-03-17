"use client";
import React, { useCallback, useEffect, useMemo, useState } from "react";
import { Student } from "@/types/student";
import { Box, Typography, Paper, Button, IconButton, Menu, MenuItem } from "@mui/material";
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
import { StudentApi } from "@/api/students";
import useFunction from "@/hooks/use-function";
import {
  FileDownload as FileDownloadIcon,
  FileUpload as FileUploadIcon,
  Search as SearchIcon,
  MoreVert as MoreVertIcon,
} from "@mui/icons-material"
const Content = () => {
  const dialog = useDialog<Student>();
  const dialogConfirmDelete = useDialog<Student>();

  const getStudentsApi = useFunction(StudentApi.getStudents);
  const deleteStudentsApi = useFunction(StudentApi.deleteStudent, {
    onSuccess: ({ payload }) => {
      getStudentsApi.setData({
        data: students.filter((s) => s.id !== payload),
        total: getStudentsApi.data?.total ? getStudentsApi.data.total - 1 : 0,
      });
    },
  });
  const createStudentsApi = useFunction(StudentApi.createStudent, {
    onSuccess: ({ result }: { result: Student }) => {
      getStudentsApi.setData({
        data: [...students, result],
        total: getStudentsApi.data?.total ? getStudentsApi.data.total + 1 : 1,
      });
      dialog.handleClose();
    },
  });
  const updateStudentsApi = useFunction(StudentApi.updateStudent, {
    onSuccess: ({
      payload,
    }: {
      payload: {
        id: Student["id"];
        student: Partial<Student>;
      };
    }) => {
      getStudentsApi.setData({
        data: students.map((s) =>
          s.id === payload.id
            ? {
                ...s,
                ...payload.student,
              }
            : s
        ),
        total: getStudentsApi.data?.total || 0,
      });
      dialog.handleClose();
    },
  });

  const students = useMemo(
    () => getStudentsApi.data?.data || [],
    [getStudentsApi.data]
  );

  const [moreMenuAnchorEl, setMoreMenuAnchorEl] = useState<null | HTMLElement>(null)
  const openMoreMenu = Boolean(moreMenuAnchorEl)
  const handleMoreMenuClick = (event: React.MouseEvent<HTMLButtonElement>) => {
    setMoreMenuAnchorEl(event.currentTarget)
  }
  const handleMoreMenuClose = () => {
    setMoreMenuAnchorEl(null)
  }
  const [filter, setFilter] = useState<{
    key: string;
  }>({
    key: "",
  });
  const pagination = usePagination({
    count: getStudentsApi.data?.total || 0,
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
    (student: Student) => {
      createStudentsApi.call(student);
    },
    [createStudentsApi]
  );

  const handleUpdateStudent = useCallback(
    (student: Student) => {
      updateStudentsApi.call({
        id: student.id as string,
        student,
      });
    },
    [updateStudentsApi]
  );

  const handleDeleteStudent = useCallback(
    (studentId: string) => {
      deleteStudentsApi.call(studentId);
    },
    [deleteStudentsApi]
  );

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
        <IconButton
            color="primary"
            onClick={handleMoreMenuClick}
            sx={{ border: "1px solid rgba(25, 118, 210, 0.5)", borderRadius: "20px" }}
          >
            <MoreVertIcon />
          </IconButton>
          <Menu anchorEl={moreMenuAnchorEl} open={openMoreMenu} onClose={handleMoreMenuClose}>
            <MenuItem onClick={() => {}}>
              <FileUploadIcon fontSize="small" sx={{ mr: 1 }} />
              Import danh sách
            </MenuItem>
            <MenuItem onClick={() => {}}>
              <FileDownloadIcon fontSize="small" sx={{ mr: 1 }} />
              Export danh sách
            </MenuItem>
          </Menu>
          <Button variant="contained" color="success" startIcon={<AddIcon />} sx={{ borderRadius: "20px" }}>
            Thêm khoa
          </Button>
          <Button variant="contained" color="primary" startIcon={<AddIcon />} sx={{ borderRadius: "20px" }}>
            Thêm chương trình
          </Button>
          <Button variant="contained" color="primary" startIcon={<AddIcon />} sx={{ borderRadius: "20px" }}>
            Thêm trạng thái
          </Button>
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
