"use client";

import { useState, useEffect } from "react";
import {
  Dialog,
  DialogTitle,
  DialogContent,
  DialogActions,
  Button,
  TextField,
  IconButton,
  Box,
  List,
  ListItem,
  ListItemText,
  Typography,
  Alert,
} from "@mui/material";
import {
  Close as CloseIcon,
  Add as AddIcon,
  Edit as EditIcon,
  Delete as DeleteIcon,
  DragIndicator as DragIndicatorIcon,
  Check as CheckIcon,
  Close,
} from "@mui/icons-material";
import useFunction from "@/hooks/use-function";
import { Faculty, Program, Status } from "@/types/student";
import RowStack from "@/components/row-stack";
import { useTranslations } from "next-intl";

type Item = Status | Program | Faculty;

interface DialogManagementProps {
  open: boolean;
  onClose: () => void;
  type: string;
  items: Item[];
  handleAddItem: (item: Item) => void;
  handleEditItem: (item: Item) => void;
  handleDeleteItem: (itemId: string) => void;
  handleUpdateItem: (item: Item) => void;
}

export default function DialogManagement({
  open,
  onClose,
  type,
  items,
  handleAddItem,
  handleEditItem,
  handleDeleteItem,
}: DialogManagementProps) {
  const t = useTranslations("dashboard.dialogs.management");
  const [newItemNameVi, setNewItemNameVi] = useState<string | null>(null);
  const [newItemNameEn, setNewItemNameEn] = useState<string | null>(null);
  const [editingItem, setEditingItem] = useState<Item | null>(null);
  const [confirmDialogOpen, setConfirmDialogOpen] = useState(false);
  const [itemToDelete, setItemToDelete] = useState<string | null>(null);
  const [itemToEdit, setItemToEdit] = useState<Item | null>(null);

  const getTitle = () => {
    switch (type) {
      case "faculty":
        return t("faculty");
      case "program":
        return t("program");
      case "status":
        return t("status");
      default:
        return "";
    }
  };

  const handleEditClick = (item: Item) => {
    setItemToEdit(item);
    setEditingItem(item);
  };

  const handleConfirmEdit = () => {
    if (!editingItem) return;
    handleEditItem(editingItem);
    setConfirmDialogOpen(false);
    setEditingItem(null);
    setItemToEdit(null);
  };

  const handleSaveEdit = () => {
    setConfirmDialogOpen(true);
  };

  const handleCancelAction = () => {
    setConfirmDialogOpen(false);
    setItemToDelete(null);
    setItemToEdit(null);
    setEditingItem(null);
  };

  const handleDeleteClick = (itemId: string) => {
    setItemToDelete(itemId);
    setConfirmDialogOpen(true);
  };

  const handleConfirmDelete = () => {
    if (itemToDelete) {
      handleDeleteItem(itemToDelete);
      setConfirmDialogOpen(false);
      setItemToDelete(null);
    }
  };
  const resetAll = () => {
    setNewItemNameVi(null);
    setNewItemNameEn(null);
    setEditingItem(null);
    setConfirmDialogOpen(false);
    setItemToDelete(null);
    setItemToEdit(null);
  };
  useEffect(() => {
    if (!open) {
      resetAll();
    }
  }, [open]);
  const handleAddClick = () => {
    if (newItemNameVi == null || newItemNameVi.trim() === "") return;

    const newItem = {
      id: "",
      name: {
        vi: newItemNameVi,
        en: newItemNameEn || "",
      },
    };
    handleAddItem(newItem);
    setNewItemNameVi(null);
    setNewItemNameEn(null);
  };

  return (
    <>
      <Dialog open={open} onClose={onClose} maxWidth="sm" fullWidth>
        <DialogTitle>
          <RowStack justifyContent="space-between" alignItems="center">
            {getTitle()}
            <IconButton onClick={onClose}>
              <CloseIcon />
            </IconButton>
          </RowStack>
        </DialogTitle>
        <DialogContent>
          <Box sx={{ mt: 2 }}>
            <RowStack spacing={2}>
              <TextField
                fullWidth
                label={t("nameVi")}
                value={editingItem ? editingItem.name.vi : newItemNameVi || ""}
                onChange={(e) =>
                  editingItem
                    ? setEditingItem({
                        ...editingItem,
                        name: { ...editingItem.name, vi: e.target.value },
                      })
                    : setNewItemNameVi(e.target.value)
                }
              />
              <TextField
                fullWidth
                label={t("nameEn")}
                value={editingItem ? editingItem.name.en : newItemNameEn || ""}
                onChange={(e) =>
                  editingItem
                    ? setEditingItem({
                        ...editingItem,
                        name: { ...editingItem.name, en: e.target.value },
                      })
                    : setNewItemNameEn(e.target.value)
                }
              />
              {!editingItem && (
                <Button
                  variant="contained"
                  onClick={() => {
                    if (newItemNameVi && newItemNameEn) {
                      const newItem = {
                        id: "",
                        name: {
                          vi: newItemNameVi,
                          en: newItemNameEn,
                        },
                        ...(type === "status" ? { order: items.length } : {}),
                      };
                      handleAddItem(newItem);
                      setNewItemNameVi(null);
                      setNewItemNameEn(null);
                    }
                  }}
                  disabled={!newItemNameVi || !newItemNameEn}
                >
                  <AddIcon />
                </Button>
              )}
              {editingItem && (
                <RowStack spacing={1}>
                  <IconButton
                    color="primary"
                    onClick={handleSaveEdit}
                    disabled={!editingItem.name.vi || !editingItem.name.en}
                  >
                    <CheckIcon />
                  </IconButton>
                  <IconButton
                    color="error"
                    onClick={() => {
                      setEditingItem(null);
                      setItemToEdit(null);
                    }}
                  >
                    <Close />
                  </IconButton>
                </RowStack>
              )}
            </RowStack>
            <List>
              {items.map((item) => (
                <ListItem
                  key={item.id}
                  secondaryAction={
                    <RowStack spacing={1}>
                      <IconButton
                        edge="end"
                        onClick={() => handleEditClick(item)}
                      >
                        <EditIcon />
                      </IconButton>
                      <IconButton
                        edge="end"
                        onClick={() => handleDeleteClick(item.id)}
                      >
                        <DeleteIcon />
                      </IconButton>
                    </RowStack>
                  }
                >
                  <ListItemText
                    primary={item.name.vi}
                    secondary={item.name.en}
                  />
                </ListItem>
              ))}
            </List>
          </Box>
        </DialogContent>
      </Dialog>

      <Dialog open={confirmDialogOpen} onClose={handleCancelAction}>
        <DialogTitle>
          {itemToDelete ? t("confirmDelete") : t("confirmEdit")}
        </DialogTitle>
        <DialogContent>
          {itemToDelete && (
            t("deleteWarning")
          )}
          {itemToEdit && (
            t("editConfirm")
          )}
        </DialogContent>
        <DialogActions>
          <Button onClick={handleCancelAction}>{t("cancel")}</Button>
          <Button
            onClick={itemToDelete ? handleConfirmDelete : handleConfirmEdit}
            autoFocus
          >
            {itemToDelete ? t("confirmDelete") : t("confirmEdit")}
          </Button>
        </DialogActions>
      </Dialog>
    </>
  );
}
