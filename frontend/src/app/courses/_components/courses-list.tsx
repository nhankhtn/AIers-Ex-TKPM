"use client";

import { useState, type MouseEvent } from "react";
import Link from "next/link";
import {
  Box,
  Paper,
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
  type SelectChangeEvent,
  Typography,
  Stack,
} from "@mui/material";
import SearchIcon from "@mui/icons-material/Search";
import EditIcon from "@mui/icons-material/Edit";
import DeleteIcon from "@mui/icons-material/Delete";
import MoreVertIcon from "@mui/icons-material/MoreVert";
import WarningIcon from "@mui/icons-material/Warning";
import AddCircleIcon from "@mui/icons-material/AddCircle";
import { Button } from "@mui/material";
import type { Course } from "@/types/course";
import { CustomTable } from "@/components/custom-table";
import { getTableConfig } from "../_sections/table-config";
import { useCoursePagination } from "../_sections/use-course-pagination";
import CustomPagination from "@/components/custom-pagination";
import RowStack from "@/components/row-stack";
import { useFaculty } from "@/app/dashboard/_sections/use-faculty";
export function CourseList() {
  const [deleteDialogOpen, setDeleteDialogOpen] = useState<boolean>(false);
  const [selectedCourse, setSelectedCourse] = useState<Course | null>(null);
  const [menuAnchorEl, setMenuAnchorEl] = useState<HTMLElement | null>(null);
  const [menuCourse, setMenuCourse] = useState<Course | null>(null);

  const { courses, deleteCourseApi, pagination, filter, setFilter } =
    useCoursePagination();
  const { faculties } = useFaculty();
  const handleMenuOpen = (
    event: MouseEvent<HTMLButtonElement>,
    course: Course
  ): void => {
    setMenuAnchorEl(event.currentTarget);
    setMenuCourse(course);
  };

  const handleMenuClose = (): void => {
    setMenuAnchorEl(null);
    setMenuCourse(null);
  };

  const handleDeleteClick = (course: Course): void => {
    setSelectedCourse(course);
    setDeleteDialogOpen(true);
    handleMenuClose();
  };

  const handleDeleteConfirm = async (): Promise<void> => {
    if (selectedCourse) {
      await deleteCourseApi.call(selectedCourse.id);
      setDeleteDialogOpen(false);
    }
  };

  const renderRowActions = (course: Course) => (
    <IconButton
      aria-label="more"
      onClick={(e) => handleMenuOpen(e, course)}
      size="small"
    >
      <MoreVertIcon fontSize="small" />
    </IconButton>
  );

  return (
    <Box sx={{ maxWidth: "100%" }}>
      <RowStack
        sx={{
          justifyContent: "space-between",
          mb: 3,
        }}
      >
        <Typography variant="h5" fontWeight="bold">
          Danh sách khóa học
        </Typography>
      </RowStack>

      <RowStack mb={3} gap={2}>
        <Stack flex={1}>
          <TextField
            placeholder="Tìm kiếm khóa học..."
            variant="outlined"
            fullWidth
            value={filter.key}
            onChange={(e) =>
              setFilter((prev) => ({ ...prev, key: e.target.value }))
            }
            InputProps={{
              startAdornment: (
                <InputAdornment position="start">
                  <SearchIcon />
                </InputAdornment>
              ),
            }}
          />
        </Stack>

        <FormControl sx={{ minWidth: 180 }}>
          <InputLabel id="department-filter-label">Khoa</InputLabel>
          <Select
            labelId="department-filter-label"
            id="department-filter"
            value={filter.faculty}
            label="Khoa"
            onChange={(e) =>
              setFilter((prev) => ({ ...prev, faculty: e.target.value }))
            }
          >
            {faculties.map((faculty) => (
              <MenuItem key={faculty.id} value={faculty.id}>
                {faculty.name}
              </MenuItem>
            ))}
          </Select>
        </FormControl>
      </RowStack>

      <Stack height={300}>
        <CustomTable
          rows={courses}
          configs={getTableConfig(faculties)}
          renderRowActions={renderRowActions}
          loading={false}
          emptyState={
            <Box sx={{ py: 3, textAlign: "center" }}>
              Không tìm thấy khóa học nào.
            </Box>
          }
        />
        {courses.length > 0 && (
          <CustomPagination
            pagination={pagination}
            justifyContent="end"
            px={2}
            pt={2}
            borderTop={1}
            borderColor={"divider"}
            rowsPerPageOptions={[10, 15, 20]}
          />
        )}
      </Stack>

      {/* Menu for row actions */}
      <Menu
        anchorEl={menuAnchorEl}
        open={Boolean(menuAnchorEl)}
        onClose={handleMenuClose}
      >
        <MenuItem
          component={Link}
          href={`/courses/${menuCourse?.id}`}
          onClick={handleMenuClose}
        >
          <ListItemIcon>
            <EditIcon fontSize="small" />
          </ListItemIcon>
          Chỉnh sửa
        </MenuItem>
        <MenuItem
          component={Link}
          href={`/classes/new?courseId=${menuCourse?.id}`}
          onClick={handleMenuClose}
        >
          <ListItemIcon>
            <AddCircleIcon fontSize="small" />
          </ListItemIcon>
          Mở lớp học
        </MenuItem>
        <MenuItem
          onClick={() => menuCourse && handleDeleteClick(menuCourse)}
          sx={{ color: "error.main" }}
        >
          <ListItemIcon>
            <DeleteIcon fontSize="small" color="error" />
          </ListItemIcon>
          Xóa
        </MenuItem>
      </Menu>

      {/* Delete confirmation dialog */}
      <Dialog
        open={deleteDialogOpen}
        onClose={() => setDeleteDialogOpen(false)}
      >
        <DialogTitle sx={{ display: "flex", alignItems: "center", gap: 1 }}>
          <WarningIcon color="error" />
          Xác nhận xóa khóa học
        </DialogTitle>
        <DialogContent>
          <DialogContentText>
            {selectedCourse && (
              <>
                Bạn có chắc chắn muốn xóa khóa học{" "}
                <strong>
                  {selectedCourse.id}: {selectedCourse.name}
                </strong>
                ?
                <br />
                <br />
                Lưu ý: Chỉ có thể xóa khóa học trong vòng 30 phút sau khi tạo và
                chưa có lớp học nào được mở.
              </>
            )}
          </DialogContentText>
        </DialogContent>
        <DialogActions>
          <Button onClick={() => setDeleteDialogOpen(false)}>Hủy</Button>
          <Button
            onClick={handleDeleteConfirm}
            color="error"
            variant="contained"
            disabled={deleteCourseApi.loading}
          >
            {deleteCourseApi.loading ? "Đang xóa..." : "Xóa khóa học"}
          </Button>
        </DialogActions>
      </Dialog>
    </Box>
  );
}
