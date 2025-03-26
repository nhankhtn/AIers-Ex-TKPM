"use client";
import React, { useCallback, useState } from "react";
import { Student } from "@/types/student";
import {
  Box,
  Typography,
  Paper,
  Button,
  IconButton,
  Menu,
  MenuItem,
} from "@mui/material";
import { Add as AddIcon } from "@mui/icons-material";
import RowStack from "@/components/row-stack";
import {
  FileDownload as FileDownloadIcon,
  FileUpload as FileUploadIcon,
  MoreVert as MoreVertIcon,
} from "@mui/icons-material";
import { importFromExcel } from "@/utils/export-helper";
import { useDialog } from "@/hooks/use-dialog";
import DialogExportFile from "./dialog-export-file";
import DialogImportFile from "./dialog-import-file";
import useAppSnackbar from "@/hooks/use-app-snackbar";
import DialogManagement from "./dialog-management";
import { useFaculty } from "../_sections/use-faculty";
import { useProgram } from "../_sections/use-program";
import { useStatus } from "../_sections/use-status";
import DrawerUpdateStudent from "./drawer-update-student/drawer-update-student";
import { exportFiles } from "@/utils/files-helper";
import useDashboardSearch from "../_sections/use-dashboard-search";

interface UtilityButtonsProps {
  students: Student[];
  handleAddStudent: (student: Student) => Promise<void>; 
  handleUpdateStudent: (student: Student | Omit<Student, "email">) => Promise<void>;
  dialog: any;
  createStudentsApi: any;
}
function UtilityButtons({ students, handleAddStudent, handleUpdateStudent, dialog , createStudentsApi}: UtilityButtonsProps) {
  const {
    dialog: dialogFaculty,
    deleteFacultyApi,
    addFacultyApi,
    updateFacultyApi,
    faculties,
  } = useFaculty();
  const {
    dialog: dialogProgram,
    deleteProgramApi,
    addProgramApi,
    updateProgramApi,
    programs,
  } = useProgram();
  const {
    dialog: dialogStatus,
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

  const handleUpload = useCallback(
    async (file: File) => {
      const studentArr = await importFromExcel(file);
      if (studentArr.length === 0) {
        showSnackbarError("File không có dữ liệu");
        return;
      }
      createStudentsApi.call(studentArr as Student[]);
    },
    [showSnackbarError, createStudentsApi]
  );

  const hanldeExport = useCallback(
    async ({ format, rows }: { format: string; rows: number }) => {
      exportFiles(format, students, rows);
      showSnackbarSuccess("Xuất file thành công");
    },
    [students, showSnackbarSuccess]
  );
  return (
    <>
      <RowStack sx={{ gap: 1 }}>
        <Button
          variant="outlined"
          color="success"
          startIcon={<AddIcon />}
          sx={{ borderRadius: "20px" }}
          onClick={() => dialogFaculty.handleOpen()}
        >
          Thêm khoa
        </Button>
        <Button
          variant="contained"
          color="primary"
          startIcon={<AddIcon />}
          sx={{ borderRadius: "20px" }}
          onClick={() => dialogProgram.handleOpen()}
        >
          Thêm chương trình
        </Button>
        <Button
          variant="contained"
          color="secondary"
          startIcon={<AddIcon />}
          sx={{ borderRadius: "20px" }}
          onClick={() => dialogStatus.handleOpen()}
        >
          Thêm trạng thái
        </Button>
        <Button
          variant="contained"
          color="success"
          startIcon={<AddIcon />}
          sx={{ borderRadius: "20px" }}
          onClick={() => dialog.handleOpen()}
        >
          Thêm sinh viên
        </Button>
        <IconButton
          color="primary"
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
            <FileUploadIcon fontSize="small" sx={{ mr: 1 }} />
            <Typography> Import danh sách</Typography>
          </MenuItem>
          <MenuItem
            onClick={() => {
              dialogExport.handleOpen();
              setMoreMenuAnchorEl(null);
            }}
          >
            <FileDownloadIcon fontSize="small" sx={{ mr: 1 }} />
            <Typography>Export danh sách</Typography>
          </MenuItem>
        </Menu>
      </RowStack>
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
    </>
  );
}

export default UtilityButtons;
