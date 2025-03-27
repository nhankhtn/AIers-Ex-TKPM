"use client";
import React, { useCallback, useState } from "react";
import {
  Gender,
  mappingFiledStudent,
  mappingGender,
  Student,
} from "@/types/student";
import { Typography, Button, IconButton, Menu, MenuItem } from "@mui/material";
import { Add as AddIcon } from "@mui/icons-material";
import RowStack from "@/components/row-stack";
import {
  FileDownload as FileDownloadIcon,
  FileUpload as FileUploadIcon,
  MoreVert as MoreVertIcon,
} from "@mui/icons-material";
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
import DialogManagement from "../_components/dialog-management";
import { useFaculty } from "./use-faculty";
import { useProgram } from "./use-program";
import { useStatus } from "./use-status";
import DrawerUpdateStudent from "../_components/drawer-update-student/drawer-update-student";
import { isJSONString } from "@/utils/string-helper";
import { objectToAddress } from "./table-config";

function parseAddress(address: string) {
  const parts = address.split(",").map((part) => part.trim());
  const len = parts.length;
  return {
    detail: parts.slice(0, len - 4).join(", "),
    ward: parts[len - 4] || "",
    district: parts[len - 3] || "",
    provinces: parts[len - 2] || "",
    contry: parts[len - 1] || "",
  };
}

interface UtilityButtonsProps {
  students: Student[];
  handleAddStudent: (student: Student) => Promise<void>;
  handleUpdateStudent: (
    student: Student | Omit<Student, "email">
  ) => Promise<void>;
  dialog: any;
  createStudentsApi: any;
}
function UtilityButtons({
  students,
  handleAddStudent,
  handleUpdateStudent,
  dialog,
  createStudentsApi,
}: UtilityButtonsProps) {
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
      let studentsImported = null;

      if (file.type.includes("csv")) {
        const data = await importFromCSV(file);
        let identity = {};
        studentsImported = data.map((item) => {
          const mappedStudent: Record<string, any> = {};

          Object.entries(mappingFiledStudent).forEach(([key, value]) => {
            if (
              key === "temporaryAddress" ||
              key === "mailingAddress" ||
              key === "permanentAddress"
            ) {
              mappedStudent[key] = parseAddress(item[value]);
              return;
            }
            if (key === "identity") {
              mappedStudent[key] = identity;
              return;
            }
            if (
              key === "type" ||
              key === "documentNumber" ||
              key === "issueDate" ||
              key === "issuePlace" ||
              key === "expiryDate" ||
              key === "country" ||
              key === "isChip" ||
              key === "notes"
            ) {
              identity = {
                ...identity,
                [key]: item[value],
              };
              return;
            }
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
        return [];
      }
      const studentArr = studentsImported.map((student) => {
        const mappedStudent: Student = {
          id: student.id || "",
          name: student.name || "",
          dateOfBirth: student.dateOfBirth || "",
          gender: student.gender,
          email: student.email || "",
          temporaryAddress: student.temporaryAddress || undefined,
          permanentAddress: student.permanentAddress || "",
          mailingAddress: student.mailingAddress || undefined,
          faculty:
            faculties.find(
              (faculty) =>
                faculty.name.trim().toLowerCase() ===
                student.faculty.trim().toLowerCase()
            )?.id || "",
          course: Number(student.course) || 0,
          program:
            programs.find(
              (program) =>
                program.name.trim().toLowerCase() ===
                student.program.trim().toLowerCase()
            )?.id || "",
          phone: student.phone || "",
          status:
            statuses.find(
              (status) =>
                status.name.trim().toLowerCase() ===
                student.status.trim().toLowerCase()
            )?.id || "",
          identity: {
            type: student.type || 0,
            documentNumber: student.documentNumber || "",
            issueDate: student.issueDate,
            issuePlace: student.issuePlace || "",
            expiryDate: student.expiryDate,
            countryIssue: student.countryIssue || "",
            isChip: Boolean(student.isChip),
            notes: student.notes || "",
          },
          nationality: student.nationality || "",
        };
        return mappedStudent;
      });
      if (studentArr.length === 0) {
        showSnackbarError("File không có dữ liệu");
        return;
      }
      createStudentsApi.call(studentArr as Student[]);
    },
    [showSnackbarError, createStudentsApi, faculties, programs, statuses]
  );

  const hanldeExport = useCallback(
    async ({ format, rows }: { format: string; rows: number }) => {
      const data = students.slice(0, rows).map((student) => {
        const mappedStudent: Record<string, any> = {};
        Object.entries(mappingFiledStudent).forEach(([key, value]) => {
          const typedKey = key as keyof Student;
          if (typeof student[typedKey] === "object") {
            Object.entries(student[typedKey]).forEach(([subKey, subValue]) => {
              if (
                subValue === "" ||
                subValue === undefined ||
                subValue === null
              )
                return;
              // if (subKey === "issueDate" || subKey === "expiryDate") {
              //   if (typeof subValue === "string" || typeof subValue === "number" || subValue instanceof Date) {
              //     mappedStudent[mappingFiledStudent[subKey]] = new Date(subValue).toLocaleDateString(
              //       "vi-VN"
              //     );
              //   }
              //   return;
              // }
              if (subKey === "isChip") {
                mappedStudent[mappingFiledStudent[subKey]] = subValue
                  ? "Có"
                  : "Không";
                return;
              }
              mappedStudent[mappingFiledStudent[subKey]] = subValue;
            });
          }
          if (isJSONString(student[typedKey] as string)) {
            mappedStudent[value] = objectToAddress(
              JSON.parse(student[typedKey] as string)
            );
          } else if (typedKey === "gender") {
            mappedStudent[value] = mappingGender[student[typedKey] as Gender];
          } else if (typedKey === "status") {
            mappedStudent[value] = statuses.find(
              (status) => status.id === student[typedKey]
            )?.name;
          } else if (typedKey === "program") {
            mappedStudent[value] = programs.find(
              (program) => program.id === student[typedKey]
            )?.name;
          } else if (typedKey === "faculty") {
            mappedStudent[value] = faculties.find(
              (faculty) => faculty.id === student[typedKey]
            )?.name;
          } else {
            mappedStudent[value] = student[typedKey];
          }
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
    [students, showSnackbarSuccess, faculties, programs, statuses]
  );
  return (
    <>
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
