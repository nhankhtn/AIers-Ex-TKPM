"use client";

import { useState, type MouseEvent } from "react";
import {
  Box,
  Paper,
  Table,
  TableBody,
  TableCell,
  TableContainer,
  TableHead,
  TableRow,
  TextField,
  InputAdornment,
  IconButton,
  Menu,
  MenuItem,
  ListItemIcon,
  Chip,
  FormControl,
  InputLabel,
  Select,
  Dialog,
  DialogActions,
  DialogContent,
  DialogContentText,
  DialogTitle,
  Button,
  type SelectChangeEvent,
  Typography,
  Card,
  CardContent,
  Grid,
} from "@mui/material";
import SearchIcon from "@mui/icons-material/Search";
import MoreVertIcon from "@mui/icons-material/MoreVert";
import DescriptionIcon from "@mui/icons-material/Description";
import CancelIcon from "@mui/icons-material/Cancel";
import WarningIcon from "@mui/icons-material/Warning";
import { Delete as DeleteIcon, Edit as EditIcon } from "@mui/icons-material";
import {
  mockRegistrations,
  mockStudents,
  mockClasses,
  mockCourses,
} from "@/lib/mock-data";
import type { Registration } from "@/types/registration";
import { Student } from "@/types/student";
import { Class } from "@/types/class";
import { Course } from "@/types/course";

interface RegistrationListProps {
  registrations: Registration[];
  onEdit: (registration: Registration) => void;
  onDelete: (registration: Registration) => void;
}

export default function RegistrationList({
  registrations,
  onEdit,
  onDelete,
}: RegistrationListProps) {
  const [searchTerm, setSearchTerm] = useState<string>("");
  const [statusFilter, setStatusFilter] = useState<string>("all");
  const [cancelDialogOpen, setCancelDialogOpen] = useState<boolean>(false);
  const [selectedRegistration, setSelectedRegistration] =
    useState<Registration | null>(null);
  const [menuAnchorEl, setMenuAnchorEl] = useState<HTMLElement | null>(null);
  const [menuRegistration, setMenuRegistration] = useState<Registration | null>(
    null
  );

  // Filter registrations based on search term and filters
  const filteredRegistrations = registrations.filter((registration) => {
    const student = mockStudents.find((s) => s.id === registration.studentId);
    const classItem = mockClasses.find((c) => c.id === registration.classId);
    const course = mockCourses.find(
      (c) => c.id.toString() === classItem?.courseId.toString()
    );

    if (!student || !classItem || !course) return false;

    const searchLower = searchTerm.toLowerCase();
    return (
      student.code.toLowerCase().includes(searchLower) ||
      student.name.toLowerCase().includes(searchLower) ||
      course.courseId.toLowerCase().includes(searchLower) ||
      course.courseName.toLowerCase().includes(searchLower) ||
      classItem.code.toLowerCase().includes(searchLower)
    );
  });

  const handleMenuOpen = (
    event: MouseEvent<HTMLButtonElement>,
    registration: Registration
  ): void => {
    setMenuAnchorEl(event.currentTarget);
    setMenuRegistration(registration);
  };

  const handleMenuClose = (): void => {
    setMenuAnchorEl(null);
    setMenuRegistration(null);
  };

  const handleCancelClick = (registration: Registration): void => {
    setSelectedRegistration(registration);
    setCancelDialogOpen(true);
    handleMenuClose();
  };

  const handleCancelConfirm = (): void => {
    // In a real app, this would call an API to cancel the registration
    setCancelDialogOpen(false);
  };

  const handleStatusFilterChange = (event: SelectChangeEvent): void => {
    setStatusFilter(event.target.value);
  };

  return (
    <Card>
      <CardContent>
        <Box sx={{ mb: 2 }}>
          <TextField
            label="Tìm kiếm"
            variant="outlined"
            fullWidth
            value={searchTerm}
            onChange={(e) => setSearchTerm(e.target.value)}
          />
        </Box>

        <TableContainer>
          <Table>
            <TableHead>
              <TableRow>
                <TableCell>Mã sinh viên</TableCell>
                <TableCell>Tên sinh viên</TableCell>
                <TableCell>Mã môn học</TableCell>
                <TableCell>Tên môn học</TableCell>
                <TableCell>Mã lớp</TableCell>
                <TableCell>Ngày đăng ký</TableCell>
                <TableCell>Trạng thái</TableCell>
                <TableCell>Thao tác</TableCell>
              </TableRow>
            </TableHead>
            <TableBody>
              {filteredRegistrations.map((registration) => {
                const student = mockStudents.find(
                  (s) => s.id === registration.studentId
                );
                const classItem = mockClasses.find(
                  (c) => c.id === registration.classId
                );
                const course = mockCourses.find(
                  (c) => c.id.toString() === classItem?.courseId.toString()
                );

                if (!student || !classItem || !course) return null;

                return (
                  <TableRow key={registration.id}>
                    <TableCell>{student.code}</TableCell>
                    <TableCell>{student.name}</TableCell>
                    <TableCell>{course.courseId}</TableCell>
                    <TableCell>{course.courseName}</TableCell>
                    <TableCell>{classItem.code}</TableCell>
                    <TableCell>{registration.registrationDate}</TableCell>
                    <TableCell>{registration.status}</TableCell>
                    <TableCell>
                      <IconButton
                        color="primary"
                        onClick={() => onEdit(registration)}
                      >
                        <EditIcon />
                      </IconButton>
                      <IconButton
                        color="error"
                        onClick={() => onDelete(registration)}
                      >
                        <DeleteIcon />
                      </IconButton>
                    </TableCell>
                  </TableRow>
                );
              })}
            </TableBody>
          </Table>
        </TableContainer>
      </CardContent>
    </Card>
  );
}
