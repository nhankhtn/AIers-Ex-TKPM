import React, { useState, useRef, useCallback, useEffect } from "react";
import {
  Dialog,
  DialogTitle,
  DialogContent,
  DialogActions,
  Button,
  Typography,
  Stack,
  Box,
  IconButton,
  Paper,
  Chip,
} from "@mui/material";
import CloseIcon from "@mui/icons-material/Close";
import CloudUploadIcon from "@mui/icons-material/CloudUpload";
import InsertDriveFileIcon from "@mui/icons-material/InsertDriveFile";
import DescriptionIcon from "@mui/icons-material/Description";
import TableChartIcon from "@mui/icons-material/TableChart";
import DeleteIcon from "@mui/icons-material/Delete";
import RowStack from "@/components/row-stack";
import { downloadUrl } from "@/utils/url-handler";
import { useTranslations } from "next-intl";

interface DialogImportFileProps {
  open: boolean;
  onClose: () => void;
  onUpload: (file: File) => void;
  allowedFileTypes?: string[];
  maxFileSize?: number; // in MB
  title?: string;
}

const DialogImportFile = ({
  open,
  onClose,
  onUpload,
  allowedFileTypes = [".csv", ".xlsx", ".xls"],
  maxFileSize = 10, // 10MB default
  title,
}: DialogImportFileProps) => {
  const t = useTranslations("dashboard.dialogs.import");
  const commonT = useTranslations("common");
  const [file, setFile] = useState<File | null>(null);
  const [isDragging, setIsDragging] = useState(false);
  const [error, setError] = useState<string | null>(null);
  const fileInputRef = useRef<HTMLInputElement>(null);

  useEffect(() => {
    if (!open) {
      setFile(null);
      setError(null);
    }
  }, [open]);

  const validateAndSetFile = useCallback(
    (selectedFile: File | null) => {
      if (!selectedFile) {
        setFile(null);
        setError(null);
        return;
      }

      // Check file type
      const fileExtension =
        "." + selectedFile.name.split(".").pop()?.toLowerCase();
      if (!allowedFileTypes.includes(fileExtension)) {
        setError(
          t("errors.unsupportedType", {
            types: allowedFileTypes.join(", "),
          })
        );
        return;
      }

      // Check file size
      if (selectedFile.size > maxFileSize * 1024 * 1024) {
        setError(t("errors.maxSize", { size: maxFileSize }));
        return;
      }

      setFile(selectedFile);
      setError(null);
    },
    [allowedFileTypes, maxFileSize, t]
  );

  const handleDragOver = useCallback(
    (event: React.DragEvent<HTMLDivElement>) => {
      event.preventDefault();
      setIsDragging(true);
    },
    []
  );

  const handleDragLeave = useCallback(
    (event: React.DragEvent<HTMLDivElement>) => {
      event.preventDefault();
      setIsDragging(false);
    },
    []
  );

  const handleDrop = useCallback(
    (event: React.DragEvent<HTMLDivElement>) => {
      event.preventDefault();
      setIsDragging(false);

      const droppedFile = event.dataTransfer.files[0];
      validateAndSetFile(droppedFile);
    },
    [validateAndSetFile]
  );

  const handleUpload = useCallback(() => {
    if (file) {
      onUpload(file);
      onClose();
    }
  }, [file, onUpload, onClose]);

  const handleRemoveFile = useCallback(() => {
    setFile(null);
    if (fileInputRef.current) {
      fileInputRef.current.value = "";
    }
  }, []);

  const getFileIcon = useCallback(() => {
    if (!file)
      return <CloudUploadIcon sx={{ fontSize: 48, color: "primary.main" }} />;

    const fileType = file.name.split(".").pop()?.toLowerCase();

    if (fileType === "csv") {
      return <TableChartIcon sx={{ fontSize: 40, color: "#4caf50" }} />;
    } else if (fileType === "xlsx" || fileType === "xls") {
      return <DescriptionIcon sx={{ fontSize: 40, color: "#2E7D32" }} />;
    } else {
      return (
        <InsertDriveFileIcon sx={{ fontSize: 40, color: "primary.main" }} />
      );
    }
  }, [file]);

  const formatFileSize = useCallback((bytes: number) => {
    if (bytes < 1024) return bytes + " bytes";
    else if (bytes < 1024 * 1024) return (bytes / 1024).toFixed(1) + " KB";
    else return (bytes / (1024 * 1024)).toFixed(1) + " MB";
  }, []);

  return (
    <Dialog open={open} onClose={onClose} maxWidth="sm" fullWidth>
      <DialogTitle>
        <RowStack justifyContent="space-between">
          <Typography variant="h6">{title || t("title")}</Typography>
          <IconButton aria-label="close" onClick={onClose} size="small">
            <CloseIcon />
          </IconButton>
        </RowStack>
      </DialogTitle>
      <DialogContent>
        <Stack spacing={3} sx={{ mt: 1 }}>
          <Paper
            variant="outlined"
            sx={{
              p: 3,
              borderStyle: isDragging ? "dashed" : "solid",
              borderColor: isDragging ? "primary.main" : "divider",
              borderWidth: isDragging ? 2 : 1,
              borderRadius: 1,
              bgcolor: isDragging ? "primary.lighter" : "background.paper",
              cursor: "pointer",
              transition: "all 0.2s ease",
            }}
            onDragOver={handleDragOver}
            onDragLeave={handleDragLeave}
            onDrop={handleDrop}
            onClick={() => fileInputRef.current?.click()}
          >
            <input
              type="file"
              ref={fileInputRef}
              onChange={(event: React.ChangeEvent<HTMLInputElement>) => {
                validateAndSetFile(event.target.files?.[0] || null);
              }}
              accept={allowedFileTypes.join(",")}
              style={{ display: "none" }}
            />

            <Stack
              alignItems="center"
              justifyContent="center"
              spacing={2}
              sx={{ minHeight: 150 }}
            >
              {file ? (
                <Stack alignItems="center" spacing={2} width="100%">
                  {getFileIcon()}
                  <Typography variant="subtitle1" noWrap textAlign="center">
                    {file.name}
                  </Typography>
                  <RowStack spacing={1}>
                    <Chip
                      label={formatFileSize(file.size)}
                      size="small"
                      variant="outlined"
                    />
                    <IconButton
                      size="small"
                      color="error"
                      onClick={(e) => {
                        e.stopPropagation();
                        handleRemoveFile();
                      }}
                    >
                      <DeleteIcon fontSize="small" />
                    </IconButton>
                  </RowStack>
                </Stack>
              ) : (
                <>
                  {getFileIcon()}
                  <Typography variant="subtitle1" textAlign="center">
                    {t("dropzone.title")}
                  </Typography>
                  <Typography
                    variant="body2"
                    color="text.secondary"
                    textAlign="center"
                  >
                    {t("dropzone.subtitle", {
                      types: allowedFileTypes.join(", "),
                      size: maxFileSize,
                    })}
                  </Typography>
                </>
              )}
            </Stack>
          </Paper>

          {error && (
            <Typography color="error" variant="body2">
              {error}
            </Typography>
          )}

          <Stack gap={1}>
            <Box sx={{ bgcolor: "primary.lighter", borderRadius: 1 }}>
              <Typography variant="body2" color="primary.main">
                {t("processingNote")}
              </Typography>
            </Box>
            <Typography variant="body2">
              {t("downloadTemplate.prefix")}{" "}
              <Typography
                component={"span"}
                variant="body2"
                color="primary"
                sx={{ cursor: "pointer", textDecoration: "underline" }}
                onClick={() =>
                  downloadUrl(
                    "/docs/student-template.xlsx",
                    t("downloadTemplate.filename")
                  )
                }
              >
                {t("downloadTemplate.link")}
              </Typography>
            </Typography>
          </Stack>
        </Stack>
      </DialogContent>
      <DialogActions sx={{ px: 3, pb: 3 }}>
        <Button onClick={onClose} color="inherit" variant="outlined">
          {commonT("actions.cancel")}
        </Button>
        <Button
          onClick={handleUpload}
          variant="contained"
          startIcon={<CloudUploadIcon />}
          disabled={!file || !!error}
        >
          {t("uploadButton")}
        </Button>
      </DialogActions>
    </Dialog>
  );
};

export default DialogImportFile;
