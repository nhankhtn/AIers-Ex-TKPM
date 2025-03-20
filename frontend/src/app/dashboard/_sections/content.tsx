"use client";
import React, { useCallback, useEffect, useState } from "react";
import { mappingFiledStudent, Student } from "@/types/student";
import {
  Box,
  Typography,
  Paper,
  Button,
  IconButton,
  Menu,
  MenuItem,
  Stack,
} from "@mui/material";
import { People as PeopleIcon, Add as AddIcon } from "@mui/icons-material";
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
import SelectFilter from "../_components/select-filter";
import { CustomTable } from "@/components/custom-table";
import { getTableConfig } from "./table-config";
import CustomPagination from "@/components/custom-pagination";
import DialogManagement from "../_components/dialog-management";
import { useFaculty } from "./use-faculty";
import { useProgram } from "./use-program";
import { useStatus } from "./use-status";
import DrawerUpdateStudent from "../_components/drawer-update-student/drawer-update-student";

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
  const {
    dialog: dialogFaculty,
    getFacultiesApi,
    deleteFacultyApi,
    addFacultyApi,
    updateFacultyApi,
    faculties,
  } = useFaculty();
  const {
    dialog: dialogProgram,
    getProgramApi,
    deleteProgramApi,
    addProgramApi,
    updateProgramApi,
    programs,
  } = useProgram();
  const {
    dialog: dialogStatus,
    getStatusApi,
    deleteStatusApi,
    addStatusApi,
    updateStatusApi,
    statuses,
  } = useStatus();
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
      // createStudentsApi.call(studentsImported);
    },
    [showSnackbarError]
  );

  const hanldeExport = useCallback(
    async ({ format, rows }: { format: string; rows: number }) => {
      // const data = students.slice(0, rows).map((student) => {
      //   const mappedStudent: Record<string, any> = {};
      //   Object.entries(mappingFiledStudent).forEach(([key, value]) => {
      //     mappedStudent[value] = student[key];
      //   });
      //   return mappedStudent;
      // });
      // switch (format) {
      //   case "csv": {
      //     exportToCSV(data, "students");
      //     break;
      //   }
      //   case "excel": {
      //     exportToExcel(data, "students");
      //     break;
      //   }
      //   case "pdf": {
      //     exportToPDF(data, "students");
      //   }
      //   default:
      //     break;
      // }
      // showSnackbarSuccess("Xuất file thành công");
    },
    []
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
            onClick={() => dialogFaculty.handleOpen()}
          >
            Thêm khoa
          </Button>
          <Button
            variant='contained'
            color='primary'
            startIcon={<AddIcon />}
            sx={{ borderRadius: "20px" }}
            onClick={() => dialogProgram.handleOpen()}
          >
            Thêm chương trình
          </Button>
          <Button
            variant='contained'
            color='secondary'
            startIcon={<AddIcon />}
            sx={{ borderRadius: "20px" }}
            onClick={() => dialogStatus.handleOpen()}
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
          </Button>
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
          configs={getTableConfig()}
          rows={students}
          loading={getStudentsApi.loading}
          emptyState={<Typography>Không có dữ liệu</Typography>}
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
      <DrawerUpdateStudent
        open={dialog.open}
        student={dialog.data || null}
        onClose={dialog.handleClose}
        addStudent={handleAddStudent}
        updateStudent={handleUpdateStudent}
        faculties={faculties}
        programs={programs}
        statuses={statuses}
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
      <DialogManagement
        type={"faculty"}
        open={dialogFaculty.open}
        onClose={dialogFaculty.handleClose}
        handleAddItem={addFacultyApi.call}
        handleDeleteItem={deleteFacultyApi.call}
        handleUpdateItem={updateFacultyApi.call}
        items={faculties}
        handleEditItem={(item) => updateFacultyApi.call(item)}
      />
      <DialogManagement
        type={"program"}
        open={dialogProgram.open}
        onClose={dialogProgram.handleClose}
        handleAddItem={addProgramApi.call}
        handleDeleteItem={deleteProgramApi.call}
        handleUpdateItem={updateProgramApi.call}
        items={programs}
        handleEditItem={(item) => updateProgramApi.call(item)}
      />
      <DialogManagement
        type={"status"}
        open={dialogStatus.open}
        onClose={dialogStatus.handleClose}
        handleAddItem={addStatusApi.call}
        handleDeleteItem={deleteStatusApi.call}
        handleUpdateItem={updateStatusApi.call}
        items={statuses}
        handleEditItem={(item) => updateStatusApi.call(item)}
      />
    </Box>
  );
};

export default Content;
