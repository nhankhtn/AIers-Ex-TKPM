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
import SelectFilter from "@/app/dashboard/_components/select-filter";

export function CourseList() {
  const [searchQuery, setSearchQuery] = useState("");
  const [deleteDialogOpen, setDeleteDialogOpen] = useState<boolean>(false);
  const [selectedCourse, setSelectedCourse] = useState<Course | null>(null);
  const [menuAnchorEl, setMenuAnchorEl] = useState<HTMLElement | null>(null);
  const [menuCourse, setMenuCourse] = useState<Course | null>(null);

  const { courses, deleteCourseApi, pagination, filter, setFilter } =
    useCoursePagination();
  const { faculties } = useFaculty();

  const filterConfig = [
    {
      label: "Khoa",
      key: "faculty",
      options: [
        {
          value: "",
          label: "Tất cả",
        },
        ...faculties.map((f) => ({
          value: f.name,
          label: f.name,
        })),
      ],
      xs: 6,
    },
    {
      label: "Trạng thái",
      key: "status",
      options: [
        { value: "", label: "Tất cả" },
        { value: "active", label: "Đang hoạt động" },
        { value: "inactive", label: "Không hoạt động" },
      ],
      xs: 6,
    },
  ];

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
      await deleteCourseApi.call(selectedCourse.courseId);
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

      <RowStack mb={3} gap={2} justifyContent="space-between">
        <Stack flex={1}>
          <TextField
            placeholder="Tìm kiếm khóa học..."
            variant="outlined"
            fullWidth
            value={searchQuery}
            onChange={(e) => {
              setSearchQuery(e.target.value);
              if (e.target.value === "") {
                setFilter((prev) => ({ ...prev, key: "" }));
              }
            }}
            onKeyDown={(e) => {
              if (e.key === "Enter") {
                setFilter((prev) => ({ ...prev, key: searchQuery }));
              }
            }}
            InputProps={{
              startAdornment: (
                <InputAdornment
                  position="start"
                  sx={{ cursor: "pointer" }}
                  onClick={() =>
                    setFilter((prev) => ({ ...prev, key: searchQuery }))
                  }
                >
                  <SearchIcon />
                </InputAdornment>
              ),
            }}
          />
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
      </RowStack>

      <Stack height={300}>
        <CustomTable
          rows={courses.map((course) => ({
            ...course,
            id: course.courseId,
          }))}
          configs={getTableConfig()}
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
          href={`/courses/${menuCourse?.courseId}`}
          onClick={handleMenuClose}
        >
          <ListItemIcon>
            <EditIcon fontSize="small" />
          </ListItemIcon>
          Chỉnh sửa
        </MenuItem>
        <MenuItem
          component={Link}
          href={`/classes/new?courseId=${menuCourse?.courseId}`}
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
                  {selectedCourse.courseId}: {selectedCourse.courseName}
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
