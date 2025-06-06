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
  Button,
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
import { useTranslations } from "next-intl";

export function ClassList(): JSX.Element {
  const [searchQuery, setSearchQuery] = useState<string>("");
  const deleteDialog = useDialog<Class>();
  const { classes, deleteClassApi, pagination, filter, setFilter } =
    useClassPagination();
  const t = useTranslations();

  const filterConfig = useMemo(
    () => [
      {
        label: t("classes.filters.semester"),
        key: "semester",
        options: [
          { value: t("common.filters.all"), label: t("common.filters.all") },
          { value: "1", label: t("classes.filters.semester1") },
          { value: "2", label: t("classes.filters.semester2") },
          { value: "3", label: t("classes.filters.semester3") },
        ],
        xs: 6,
      },
    ],
    [t]
  );

  const handleDeleteClick = useCallback(
    (classData: Class) => {
      deleteDialog.handleOpen(classData);
    },
    [deleteDialog]
  );

  const handleConfirmDelete = useCallback(() => {
    if (deleteDialog.data) {
      deleteClassApi.call(deleteDialog.data.classId);
      deleteDialog.handleClose();
    }
  }, [deleteDialog, deleteClassApi]);

  const renderRowActions = useCallback(
    (classData: Class) => (
      <RowStack gap={1}>
        <Button
          variant="outlined"
          size="small"
          sx={{ borderRadius: "20px", whiteSpace: "nowrap" }}
          component={Link}
          href={paths.classes.edit.replace(":id", classData.classId)}
        >
          {t("classes.list.edit")}
        </Button>
        <IconButton
          size="small"
          color="error"
          onClick={() => handleDeleteClick(classData)}
        >
          <DeleteIcon fontSize="small" />
        </IconButton>
      </RowStack>
    ),
    [handleDeleteClick, t]
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
          {t("classes.list.title")}
        </Typography>
      </RowStack>

      <RowStack mb={3} gap={2} justifyContent="space-between">
        <Stack flex={1}>
          <TextField
            placeholder={t("classes.search.placeholder")}
            variant="outlined"
            fullWidth
            value={searchQuery}
            onChange={(e) => {
              setSearchQuery(e.target.value);
              if (e.target.value === "") {
                setFilter({ ...filter, classId: "" });
              }
            }}
            onKeyDown={(e) => {
              if (e.key === "Enter") {
                setFilter({ ...filter, classId: searchQuery });
              }
            }}
            InputProps={{
              startAdornment: (
                <InputAdornment
                  position="start"
                  sx={{ cursor: "pointer" }}
                  onClick={() => setFilter({ ...filter, classId: searchQuery })}
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
          rows={classes.map((classData) => ({
            ...classData,
            id: classData.classId,
          }))}
          configs={getTableConfig(t)}
          renderRowActions={renderRowActions}
          loading={false}
          emptyState={
            <Box sx={{ py: 3, textAlign: "center" }}>
              {t("classes.list.noResults")}
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
