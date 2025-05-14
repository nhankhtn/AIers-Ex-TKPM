"use client";

import { useState, useEffect, useCallback } from "react";
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
  Tooltip,
  MenuItem,
} from "@mui/material";
import {
  Close as CloseIcon,
  Add as AddIcon,
  Edit as EditIcon,
  Delete as DeleteIcon,
  Check as CheckIcon,
  Close,
  InfoOutlined,
} from "@mui/icons-material";
import { Status } from "@/types/student";
import RowStack from "@/components/row-stack";
import { useDialog } from "@/hooks/use-dialog";

interface DialogSettingsStatusProps {
  open: boolean;
  onClose: () => void;
  items: Status[];
  handleAddItem: (item: Status) => void;
  handleEditItem: (item: Status) => void;
  handleDeleteItem: (itemId: string) => void;
  handleUpdateItem: (item: Status) => void;
}

export default function DialogSettingsStatus({
  open,
  onClose,
  items,
  handleAddItem,
  handleEditItem,
  handleDeleteItem,
}: DialogSettingsStatusProps) {
  const [newItem, setNewItem] = useState<Status | null>(null);
  const [editingItem, setEditingItem] = useState<Status | null>(null);
  const dialogConfirm = useDialog();
  const [itemToDelete, setItemToDelete] = useState<string | null>(null);
  const [itemToEdit, setItemToEdit] = useState<Status | null>(null);

  const handleEditClick = (item: Status) => {
    setItemToEdit(item);
    setEditingItem(item);
  };

  const handleConfirmEdit = () => {
    if (!editingItem) return;
    handleEditItem(editingItem);
    dialogConfirm.handleClose();
    setEditingItem(null);
    setItemToEdit(null);
  };

  const handleCancelAction = () => {
    dialogConfirm.handleClose();
    setItemToDelete(null);
    setItemToEdit(null);
    setEditingItem(null);
  };

  const handleDeleteClick = (itemId: string) => {
    setItemToDelete(itemId);
    dialogConfirm.handleOpen();
  };

  const handleConfirmDelete = () => {
    if (itemToDelete) {
      handleDeleteItem(itemToDelete);
      dialogConfirm.handleClose();
      setItemToDelete(null);
    }
  };
  const resetAll = () => {
    setNewItem(null);
    setEditingItem(null);
    dialogConfirm.handleClose();
    setItemToDelete(null);
    setItemToEdit(null);
  };
  useEffect(() => {
    if (!open) {
      resetAll();
    }
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [open]);
  const handleAddClick = useCallback(() => {
    // if (newItem.name == null || newItem.name.trim() === "") return;

    handleAddItem(newItem!);
    setNewItem(null);
  }, [handleAddItem, setNewItem, newItem]);

  return (
    <>
      <Dialog open={open} onClose={onClose} fullWidth maxWidth='sm'>
        <DialogTitle>
          <RowStack
            sx={{
              justifyContent: "space-between",
            }}
          >
            <Typography variant='h6'> Cài đặt trạng thái</Typography>
            <IconButton onClick={onClose} size='small'>
              <CloseIcon />
            </IconButton>
          </RowStack>
        </DialogTitle>
        <DialogContent dividers>
          <RowStack>
            <RowStack width={30}></RowStack>
            <RowStack flex={1}>
              <Typography variant='subtitle2'>Tên</Typography>
            </RowStack>
            <RowStack gap={1} width={100}>
              <Typography variant='subtitle2'>Thứ tự</Typography>
              <Tooltip
                title='Khi cập nhật trạng thái sinh viên chỉ có thể chuyển đến trạng thái có thứ tự lớn hơn hoặc bằng trạng thái hiện tại'
                placement='top'
              >
                <InfoOutlined
                  sx={{
                    color: "action.active",
                  }}
                />
              </Tooltip>
            </RowStack>
            <RowStack width={100}></RowStack>
          </RowStack>
          <List sx={{ width: "100%" }}>
            {items.map((item) => (
              <ListItem
                key={item.id}
                sx={{
                  py: 1,
                  borderRadius: 1,
                }}
              >
                <Box
                  width={"8px"}
                  height={"8px"}
                  borderRadius={"50%"}
                  bgcolor={"text.secondary"}
                  mr={1}
                />
                <RowStack gap={1} flex={1}>
                  {editingItem?.id === item.id ? (
                    <RowStack gap={1} flex={1}>
                      <TextField
                        fullWidth
                        variant='outlined'
                        size='small'
                        label='Tên tiếng Việt'
                        value={editingItem.name.vi}
                        onChange={(e) =>
                          setEditingItem({
                            ...editingItem,
                            name: { ...editingItem.name, vi: e.target.value },
                          })
                        }
                        autoFocus
                      />
                      <TextField
                        fullWidth
                        variant='outlined'
                        size='small'
                        label='Tên tiếng Anh'
                        value={editingItem.name.en}
                        onChange={(e) =>
                          setEditingItem({
                            ...editingItem,
                            name: { ...editingItem.name, en: e.target.value },
                          })
                        }
                      />
                      <TextField
                        sx={{ width: 220 }}
                        variant='outlined'
                        size='small'
                        value={editingItem.order}
                        onChange={(e) =>
                          setEditingItem({
                            ...editingItem,
                            order: parseInt(e.target.value, 10),
                          })
                        }
                        select
                      >
                        {Array.from({ length: 10 }, (_, i) => i + 1).map(
                          (i) => (
                            <MenuItem key={i} value={i}>
                              {i}
                            </MenuItem>
                          )
                        )}
                      </TextField>
                      <IconButton
                        size='small'
                        color='success'
                        onClick={dialogConfirm.handleOpen}
                      >
                        <CheckIcon />
                      </IconButton>

                      <IconButton
                        size='small'
                        color='error'
                        onClick={() => {
                          setEditingItem(null);
                          setItemToEdit(null);
                        }}
                      >
                        <Close />
                      </IconButton>
                    </RowStack>
                  ) : (
                    <RowStack flex={1} gap={1}>
                      <ListItemText
                        primary={item.name.vi}
                        secondary={item.name.en}
                      />
                      <RowStack width={100}>
                        <Typography variant='body2'>{item.order}</Typography>
                      </RowStack>
                      <IconButton
                        size='small'
                        sx={{ color: "primary.main" }}
                        onClick={() => handleEditClick(item)}
                      >
                        <EditIcon fontSize='small' />
                      </IconButton>
                      <IconButton
                        size='small'
                        sx={{ color: "error.main" }}
                        onClick={() => handleDeleteClick(item.id)}
                      >
                        <DeleteIcon fontSize='small' />
                      </IconButton>
                    </RowStack>
                  )}
                </RowStack>
              </ListItem>
            ))}
            {newItem !== null && (
              <ListItem>
                <Box
                  width={"8px"}
                  height={"8px"}
                  borderRadius={"50%"}
                  bgcolor={"text.secondary"}
                  mr={1}
                />
                <RowStack gap={1} flex={1}>
                  <TextField
                    fullWidth
                    variant='outlined'
                    size='small'
                    label='Tên tiếng Việt'
                    value={newItem.name.vi}
                    onChange={(e) =>
                      setNewItem({
                        ...newItem,
                        name: { ...newItem.name, vi: e.target.value },
                      })
                    }
                    autoFocus
                  />
                  <TextField
                    fullWidth
                    variant='outlined'
                    size='small'
                    label='Tên tiếng Anh'
                    value={newItem.name.en}
                    onChange={(e) =>
                      setNewItem({
                        ...newItem,
                        name: { ...newItem.name, en: e.target.value },
                      })
                    }
                  />
                  <TextField
                    sx={{ width: 220 }}
                    variant='outlined'
                    size='small'
                    value={newItem.order}
                    onChange={(e) =>
                      setNewItem({
                        ...newItem,
                        order: parseInt(e.target.value, 10),
                      })
                    }
                    select
                  >
                    {Array.from({ length: 10 }, (_, i) => i + 1).map((i) => (
                      <MenuItem key={i} value={i}>
                        {i}
                      </MenuItem>
                    ))}
                  </TextField>
                  <IconButton
                    size='small'
                    color='success'
                    onClick={handleAddClick}
                    disabled={
                      !newItem.name.vi.trim() || !newItem.name.en.trim()
                    }
                  >
                    <CheckIcon />
                  </IconButton>

                  <IconButton
                    size='small'
                    color='error'
                    onClick={() => setNewItem(null)}
                  >
                    <Close />
                  </IconButton>
                </RowStack>
              </ListItem>
            )}
          </List>
          <RowStack gap={1} sx={{ mt: 3, justifyContent: "flex-end" }}>
            <Button
              variant='outlined'
              startIcon={<AddIcon />}
              onClick={() =>
                setNewItem({
                  id: "",
                  name: {
                    vi: "",
                    en: "",
                  },
                  order: 1,
                })
              }
            >
              Thêm mới
            </Button>
            <Button variant='contained' color='secondary' onClick={onClose}>
              Đóng
            </Button>
          </RowStack>
        </DialogContent>
      </Dialog>

      {/* Confirmation Dialog */}
      <Dialog
        open={dialogConfirm.open}
        onClose={handleCancelAction}
        maxWidth='xs'
        fullWidth
      >
        <DialogTitle>
          {itemToDelete ? "Xác nhận xóa" : "Xác nhận chỉnh sửa"}
        </DialogTitle>
        <DialogContent>
          {itemToDelete ? (
            <Alert severity='warning' sx={{ mb: 2 }}>
              Bạn có chắc chắn muốn xóa mục này không? Hành động này không thể
              hoàn tác.
            </Alert>
          ) : (
            <Typography>
              Bạn có chắc chắn muốn chỉnh sửa mục &quot;{itemToEdit?.name.vi}
              &quot; không?
            </Typography>
          )}
        </DialogContent>
        <DialogActions>
          <Button onClick={handleCancelAction} color='inherit'>
            Hủy
          </Button>
          <Button
            onClick={itemToDelete ? handleConfirmDelete : handleConfirmEdit}
            color={itemToDelete ? "error" : "primary"}
            variant='contained'
            autoFocus
          >
            {itemToDelete ? "Xóa" : "Chỉnh sửa"}
          </Button>
        </DialogActions>
      </Dialog>
    </>
  );
}
