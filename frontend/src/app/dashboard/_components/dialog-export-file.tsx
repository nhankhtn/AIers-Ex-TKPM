import React, { useCallback, useState } from "react";
import {
  Dialog,
  DialogTitle,
  DialogContent,
  DialogActions,
  Button,
  FormControl,
  InputLabel,
  Select,
  MenuItem,
  TextField,
  Stack,
  Typography,
  SelectChangeEvent,
  IconButton,
  Box,
} from "@mui/material";
import CloseIcon from "@mui/icons-material/Close";
import DownloadIcon from "@mui/icons-material/Download";
import { useTranslations } from "next-intl";

interface DialogExportFileProps {
  open: boolean;
  onClose: () => void;
  onExport: ({ format, rows }: { format: string; rows: number }) => void;
  totalRows?: number;
}

const DialogExportFile = ({
  open,
  onClose,
  onExport,
  totalRows = 100,
}: DialogExportFileProps) => {
  const t = useTranslations("dashboard.dialogs.export");
  const commonT = useTranslations("common");
  const [fileFormat, setFileFormat] = useState("excel");
  const [rowCount, setRowCount] = useState(totalRows);

  const handleRowCountChange = useCallback(
    (event: React.ChangeEvent<HTMLInputElement>) => {
      const value = parseInt(event.target.value);
      if (!isNaN(value) && value > 0) {
        setRowCount(Math.min(value, totalRows));
      } else {
        setRowCount(0);
      }
    },
    [totalRows]
  );

  return (
    <Dialog open={open} onClose={onClose} maxWidth="xs" fullWidth>
      <DialogTitle>
        <Stack
          direction="row"
          alignItems="center"
          justifyContent="space-between"
        >
          <Typography variant="h6">{t("title")}</Typography>
          <IconButton aria-label="close" onClick={onClose} size="small">
            <CloseIcon />
          </IconButton>
        </Stack>
      </DialogTitle>
      <DialogContent>
        <Stack spacing={3} sx={{ mt: 1 }}>
          <FormControl fullWidth>
            <InputLabel id="file-format-label">{t("fileFormat")}</InputLabel>
            <Select
              labelId="file-format-label"
              id="file-format"
              value={fileFormat}
              onChange={(event: SelectChangeEvent) => {
                setFileFormat(event.target.value);
              }}
              label={t("fileFormat")}
            >
              <MenuItem value="excel">{t("formats.excel")}</MenuItem>
              <MenuItem value="csv">{t("formats.csv")}</MenuItem>
              {/* <MenuItem value='pdf'>{t("formats.pdf")}</MenuItem> */}
            </Select>
          </FormControl>

          <FormControl fullWidth>
            <TextField
              label={t("rowCount")}
              type="number"
              value={rowCount}
              onChange={handleRowCountChange}
              inputProps={{
                min: 1,
                max: totalRows,
              }}
              helperText={t("maxRows", { count: totalRows })}
            />
          </FormControl>

          <Box sx={{ px: 1.5, bgcolor: "primary.lighter", borderRadius: 1 }}>
            <Typography variant="body2" color="primary.main">
              {t("filterNote")}
            </Typography>
          </Box>
        </Stack>
      </DialogContent>
      <DialogActions sx={{ px: 3, pb: 3 }}>
        <Button onClick={onClose} color="inherit" variant="outlined">
          {commonT("actions.cancel")}
        </Button>
        <Button
          onClick={() => {
            onExport({ format: fileFormat, rows: rowCount });
            onClose();
          }}
          variant="contained"
          startIcon={<DownloadIcon />}
          disabled={rowCount <= 0}
        >
          {t("exportButton")}
        </Button>
      </DialogActions>
    </Dialog>
  );
};

export default DialogExportFile;
