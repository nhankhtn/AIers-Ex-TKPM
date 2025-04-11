"use client";

import { useState, type MouseEvent, useCallback, useMemo } from "react";
import Link from "next/link";
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
  Typography,
  type SelectChangeEvent,
  Stack,
} from "@mui/material";
import SearchIcon from "@mui/icons-material/Search";
import EditIcon from "@mui/icons-material/Edit";
import DeleteIcon from "@mui/icons-material/Delete";
import MoreVertIcon from "@mui/icons-material/MoreVert";
import CalendarMonthIcon from "@mui/icons-material/CalendarMonth";
import PeopleIcon from "@mui/icons-material/People";
import { mockClasses } from "@/lib/mock-data";
import type { Class } from "@/types/class";
import type { JSX } from "react/jsx-runtime";
import { CustomTable } from "@/components/custom-table";
import { getTableConfig } from "../_sections/table-config";
import { useClassPagination } from "../_sections/use-class-pagination";
import CustomPagination from "@/components/custom-pagination";
import RowStack from "@/components/row-stack";
import SelectFilter from "@/app/dashboard/_components/select-filter";
import { paths } from "@/paths";
import { useDialog } from "@/hooks/use-dialog";
import DialogConfirmDeleteClass from "./dialog-confirm-delete-class";

export function ClassList(): JSX.Element {
  const [searchQuery, setSearchQuery] = useState<string>("");
  const deleteDialog = useDialog<Class>();
  const [menuAnchorEl, setMenuAnchorEl] = useState<HTMLElement | null>(null);
  const [menuClass, setMenuClass] = useState<Class | null>(null);

  const { classes, deleteClassApi, pagination, filter, setFilter } =
    useClassPagination();

  const filterConfig = useMemo(
    () => [
      {
        label: "Học kỳ",
        key: "semester",
        options: [
          { value: "", label: "Tất cả" },
          { value: "1", label: "Học kỳ 1" },
          { value: "2", label: "Học kỳ 2" },
          { value: "3", label: "Học kỳ hè" },
        ],
        xs: 6,
      },
    ],
    []
  );

  const handleMenuOpen = useCallback(
    (event: MouseEvent<HTMLButtonElement>, classData: Class): void => {
      setMenuAnchorEl(event.currentTarget);
      setMenuClass(classData);
    },
    []
  );

  const handleMenuClose = useCallback((): void => {
    setMenuAnchorEl(null);
    setMenuClass(null);
  }, []);

  const handleDeleteClick = useCallback(
    (classData: Class) => {
      deleteDialog.handleOpen(classData);
    },
    [deleteDialog]
  );

  const handleConfirmDelete = useCallback(() => {
    if (deleteDialog.data) {
      deleteClassApi.call(deleteDialog.data.id);
      deleteDialog.handleClose();
      setMenuClass(null);
      setMenuAnchorEl(null);
    }
  }, [deleteDialog, deleteClassApi]);

  const renderRowActions = useCallback(
    (classData: Class) => (
      <IconButton
        aria-label="more"
        onClick={(e) => handleMenuOpen(e, classData)}
        size="small"
      >
        <MoreVertIcon fontSize="small" />
      </IconButton>
    ),
    [handleMenuOpen]
  );

  const tableRows = useMemo(
    () =>
      classes.map((classData: Class) => ({
        ...classData,
        id: classData.id,
      })),
    [classes]
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
          Danh sách lớp học
        </Typography>
      </RowStack>

      <RowStack mb={3} gap={2} justifyContent="space-between">
        <Stack flex={1}>
          <TextField
            placeholder="Tìm kiếm lớp học..."
            variant="outlined"
            fullWidth
            value={searchQuery}
            onChange={(e) => {
              setSearchQuery(e.target.value);
              if (e.target.value === "") {
                setFilter({ ...filter, courseId: "" });
              }
            }}
            onKeyDown={(e) => {
              if (e.key === "Enter") {
                setFilter({ ...filter, courseId: searchQuery });
              }
            }}
            InputProps={{
              startAdornment: (
                <InputAdornment
                  position="start"
                  sx={{ cursor: "pointer" }}
                  onClick={() => setFilter({ ...filter, courseId: searchQuery })}
                >
                  <SearchIcon />
                </InputAdornment>
              ),
            }}
          />
        </Stack>

        <Stack width={300}>
          <SelectFilter
            configs={filterConfig}
            filter={filter}
            onChange={(key: string, value: string) => {
              setFilter({
                ...filter,
                [key]: value,
              });
            }}
          />
        </Stack>
      </RowStack>

      <Stack>
        <CustomTable
          rows={tableRows}
          configs={getTableConfig()}
          renderRowActions={renderRowActions}
          loading={false}
          emptyState={
            <Box sx={{ py: 3, textAlign: "center" }}>
              Không tìm thấy lớp học nào.
            </Box>
          }
        />
        {classes.length > 0 && (
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
          href={paths.classes.edit.replace(
            ":id",
            menuClass?.id.toString() ?? ""
          )}
          onClick={handleMenuClose}
        >
          <ListItemIcon>
            <EditIcon fontSize="small" />
          </ListItemIcon>
          Chỉnh sửa
        </MenuItem>
        <MenuItem
          onClick={() => menuClass && handleDeleteClick(menuClass)}
          sx={{ color: "error.main" }}
        >
          <ListItemIcon>
            <DeleteIcon fontSize="small" color="error" />
          </ListItemIcon>
          Xóa
        </MenuItem>
      </Menu>

      {deleteDialog.data && (
        <DialogConfirmDeleteClass
          open={deleteDialog.open}
          onClose={deleteDialog.handleClose}
          onConfirm={handleConfirmDelete}
          data={deleteDialog.data}
        />
      )}
    </Box>
  );
}
