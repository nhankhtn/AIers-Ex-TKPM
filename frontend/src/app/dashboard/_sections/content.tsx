"use client";
import React, { useCallback, useState } from "react";
import { mappingFiledStudent, Student } from "@/types/student";
import {
  Box,
  Typography,
  Paper,
  Button,
  IconButton,
  Menu,
  MenuItem,
} from "@mui/material";
import { People as PeopleIcon, Add as AddIcon } from "@mui/icons-material";
import Grid from "@mui/material/Grid2";
import Table from "@/components/table";
import { StudentCols } from "@/constants";
import SearchBar from "@/app/dashboard/_components/search-bar";
import Dialog from "@/app/dashboard/_components/dialog";
import RowStack from "@/components/row-stack";
import DialogConfirmDelete from "../_components/dialog-confirm-delete";
import {
  FileDownload as FileDownloadIcon,
  FileUpload as FileUploadIcon,
  Search as SearchIcon,
  MoreVert as MoreVertIcon,
} from "@mui/icons-material";
import useDashboardSearch from "./use-dashboard-search";
import {
  exportToCSV,
  exportToExcel,
  exportToPDF,
  importFromCSV,
  importFromExcel,
} from "@/utils/export-helper";
import { useDialog } from "@/hooks/use-dialog";
import DialogExportFile from "../_components/dialog-export-file";
import DialogImportFile from "../_components/dialog-import-file";
import useAppSnackbar from "@/hooks/use-app-snackbar";

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
  } = useDashboardSearch();

  const dialogExport = useDialog();
  const dialogImport = useDialog();
  const { showSnackbarSuccess, showSnackbarError } = useAppSnackbar();

  const [moreMenuAnchorEl, setMoreMenuAnchorEl] = useState<null | HTMLElement>(
    null
  );

  const handleAddStudent = useCallback(
    (student: Student) => {
      createStudentsApi.call([student]);
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

  const handleUpload = useCallback(
    async (file: File) => {
      let studentsImported = null;

      console.log(file.type);
      if (file.type.includes("csv")) {
        const data = await importFromCSV(file);

        studentsImported = data.map((item) => {
          const mappedStudent: Record<string, any> = {};

          Object.entries(mappingFiledStudent).forEach(([key, value]) => {
            mappedStudent[key] = item[value];
          });

          return mappedStudent;
        });
      } else if (
        file.type ===
          "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" ||
        file.type === "application/vnd.ms-excel"
      ) {
        const data = await importFromExcel(file);

        studentsImported = data.map((item) => {
          const mappedStudent: Record<string, any> = {};
          Object.entries(mappingFiledStudent).forEach(([key, value]) => {
            mappedStudent[key] = item[value];
          });
          return mappedStudent;
        });
      }
      if (!studentsImported || studentsImported.length === 0) {
        showSnackbarError("Không có dữ liệu để import");
        return;
      }
      createStudentsApi.call(studentsImported);
    },
    [createStudentsApi, showSnackbarError]
  );

  const hanldeExport = useCallback(
    async ({ format, rows }: { format: string; rows: number }) => {
      const data = students.slice(0, rows).map((student) => {
        const mappedStudent: Record<string, any> = {};

        Object.entries(mappingFiledStudent).forEach(([key, value]) => {
          mappedStudent[value] = student[key];
        });

        return mappedStudent;
      });

      switch (format) {
        case "csv": {
          exportToCSV(data, "students");
          break;
        }
        case "excel": {
          exportToExcel(data, "students");
          break;
        }
        case "pdf": {
          exportToPDF(data, "students");
        }
        default:
          break;
      }
      showSnackbarSuccess("Xuất file thành công");
    },
    [students, showSnackbarSuccess]
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
          <Button
            variant='outlined'
            color='success'
            startIcon={<AddIcon />}
            sx={{ borderRadius: "20px" }}
          >
            Thêm khoa
          </Button>
          <Button
            variant='contained'
            color='primary'
            startIcon={<AddIcon />}
            sx={{ borderRadius: "20px" }}
          >
            Thêm chương trình
          </Button>
          <Button
            variant='contained'
            color='secondary'
            startIcon={<AddIcon />}
            sx={{ borderRadius: "20px" }}
          >
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
          </Button>{" "}
          <IconButton
            color='primary'
            onClick={(event: React.MouseEvent<HTMLButtonElement>) => {
              setMoreMenuAnchorEl(event.currentTarget);
            }}
            sx={{
              border: "1px solid rgba(25, 118, 210, 0.5)",
              borderRadius: "20px",
            }}
          >
            <MoreVertIcon />
          </IconButton>
          <Menu
            anchorEl={moreMenuAnchorEl}
            open={!!moreMenuAnchorEl}
            onClose={() => setMoreMenuAnchorEl(null)}
          >
            <MenuItem
              onClick={() => {
                dialogImport.handleOpen();
                setMoreMenuAnchorEl(null);
              }}
            >
              <FileUploadIcon fontSize='small' sx={{ mr: 1 }} />
              <Typography> Import danh sách</Typography>
            </MenuItem>
            <MenuItem
              onClick={() => {
                dialogExport.handleOpen();
                setMoreMenuAnchorEl(null);
              }}
            >
              <FileDownloadIcon fontSize='small' sx={{ mr: 1 }} />
              <Typography>Export danh sách</Typography>
            </MenuItem>
          </Menu>
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
      <DialogExportFile
        open={dialogExport.open}
        onClose={dialogExport.handleClose}
        onExport={hanldeExport}
        totalRows={students.length}
      />
      <DialogImportFile
        open={dialogImport.open}
        onClose={dialogImport.handleClose}
        onUpload={handleUpload}
      />
    </Box>
  );
};

export default Content;
