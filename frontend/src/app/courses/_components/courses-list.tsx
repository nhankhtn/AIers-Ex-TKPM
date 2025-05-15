"use client";

import { useMemo, useState, type MouseEvent } from "react";
import Link from "next/link";
import {
  Box,
  TextField,
  InputAdornment,
  IconButton,
  Menu,
  MenuItem,
  ListItemIcon,
  Typography,
  Stack,
} from "@mui/material";
import SearchIcon from "@mui/icons-material/Search";
import EditIcon from "@mui/icons-material/Edit";
import DeleteIcon from "@mui/icons-material/Delete";
import MoreVertIcon from "@mui/icons-material/MoreVert";
import AddCircleIcon from "@mui/icons-material/AddCircle";
import type { Course } from "@/types/course";
import { CustomTable } from "@/components/custom-table";
import { GetTableConfig } from "../_sections/table-config";
import { useCoursePagination } from "../_sections/use-course-pagination";
import CustomPagination from "@/components/custom-pagination";
import RowStack from "@/components/row-stack";
import SelectFilter from "@/app/dashboard/_components/select-filter";
import { paths } from "@/paths";
import { useDialog } from "@/hooks/use-dialog";
import DialogConfirmDeleteCourse from "./dialog-confirm-delete-course";
import { useMainContext } from "@/context/main/main-context";
import { useLocale, useTranslations } from "next-intl";

export function CourseList() {
  const [searchQuery, setSearchQuery] = useState("");
  const deleteDialog = useDialog<Course>();
  const [menuAnchorEl, setMenuAnchorEl] = useState<HTMLElement | null>(null);
  const [menuCourse, setMenuCourse] = useState<Course | null>(null);
  const t = useTranslations();
  const locale = useLocale() as "en" | "vi";
  const { courses, deleteCourseApi, pagination, filter, setFilter } =
    useCoursePagination();
  const { faculties } = useMainContext();

  const filterConfig = useMemo(
    () => [
      {
        label: t("courses.filters.faculty"),
        key: "faculty",
        options: [
          {
            value: t("common.filters.all"),
            label: t("common.filters.all"),
          },
          ...faculties.map((f) => ({
            value: f.name.vi,
            label: f.name[locale],
          })),
        ],
        xs: 6,
      },
      {
        label: t("courses.filters.status"),
        key: "status",
        options: [
          { value: t("common.filters.all"), label: t("common.filters.all") },
          { value: "active", label: t("courses.filters.active") },
          { value: "inactive", label: t("courses.filters.inactive") },
        ],
        xs: 6,
      },
    ],

    // eslint-disable-next-line react-hooks/exhaustive-deps
    [faculties, t]
  );

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

  const handleDeleteClick = (course: Course) => {
    deleteDialog.handleOpen(course);
  };

  const handleConfirmDelete = () => {
    if (deleteDialog.data) {
      deleteCourseApi.call(deleteDialog.data.courseId);
      deleteDialog.handleClose();
      setMenuCourse(null);
      setMenuAnchorEl(null);
    }
  };

  const renderRowActions = (course: Course) => (
    <IconButton
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
          {t("courses.list.title")}
        </Typography>
      </RowStack>

      <RowStack mb={3} gap={2} justifyContent="space-between">
        <Stack flex={1}>
          <TextField
            placeholder={t("courses.search.placeholder")}
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
            filter={filter}
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
          rows={courses.map((course: Course) => ({
            ...course,
            id: course.courseId,
          }))}
          configs={GetTableConfig()}
          renderRowActions={renderRowActions}
          loading={false}
          emptyState={
            <Box sx={{ py: 3, textAlign: "center" }}>
              {t("courses.list.noResults")}
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
            borderColor="divider"
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
          href={paths.courses.edit.replace(":id", menuCourse?.courseId ?? "")}
          onClick={handleMenuClose}
        >
          <ListItemIcon>
            <EditIcon fontSize="small" />
          </ListItemIcon>
          {t("common.actions.edit")}
        </MenuItem>
        <MenuItem
          component={Link}
          href={
            paths.classes.create.replace(":id", menuCourse?.courseId ?? "") +
            "?courseId=" +
            menuCourse?.courseId
          }
          onClick={handleMenuClose}
        >
          <ListItemIcon>
            <AddCircleIcon fontSize="small" />
          </ListItemIcon>
          {t("courses.actions.openClass")}
        </MenuItem>
        <MenuItem
          onClick={() => menuCourse && handleDeleteClick(menuCourse)}
          sx={{ color: "error.main" }}
        >
          <ListItemIcon>
            <DeleteIcon fontSize="small" color="error" />
          </ListItemIcon>
          {t("common.actions.delete")}
        </MenuItem>
      </Menu>

      {deleteDialog.data && (
        <DialogConfirmDeleteCourse
          open={deleteDialog.open}
          onClose={deleteDialog.handleClose}
          onConfirm={handleConfirmDelete}
          courseName={`${deleteDialog.data.courseName.vi} (${deleteDialog.data.courseName.en})`}
        />
      )}
    </Box>
  );
}
