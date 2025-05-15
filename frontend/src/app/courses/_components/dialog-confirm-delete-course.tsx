import {
  Button,
  Dialog,
  DialogActions,
  DialogContent,
  DialogTitle,
  IconButton,
  Stack,
  Typography,
} from "@mui/material";
import CloseIcon from "@mui/icons-material/Close";
import RowStack from "@/components/row-stack";
import { useTranslations } from "next-intl";

interface DialogConfirmDeleteCourseProps {
  open: boolean;
  onClose: () => void;
  onConfirm: () => void;
  courseName: string;
}

export default function DialogConfirmDeleteCourse({
  open,
  onClose,
  onConfirm,
  courseName,
}: DialogConfirmDeleteCourseProps) {
  const t = useTranslations();

  return (
    <Dialog
      open={open}
      onClose={onClose}
      sx={{
        "& .MuiDialog-paper": {
          width: {
            lg: 491,
          },
        },
      }}
    >
      <DialogTitle sx={{ pb: 1 }}>
        <RowStack justifyContent="space-between">
          <Typography variant="h6">
            {t("courses.dialogs.delete.title")}
          </Typography>
          <IconButton
            onClick={onClose}
            disableRipple
            sx={{
              width: "40px",
              height: "40px",
            }}
          >
            <CloseIcon />
          </IconButton>
        </RowStack>
      </DialogTitle>
      <DialogContent sx={{ px: 3, py: 0.5 }}>
        <Stack gap="10px">
          <Typography variant="body1">
            {t("courses.dialogs.delete.confirmMessage")}{" "}
            <Typography variant="subtitle1" fontWeight={600} component="span">
              &quot;{courseName}&quot;?{" "}
            </Typography>
          </Typography>
        </Stack>
      </DialogContent>
      <DialogActions>
        <RowStack justifyContent="flex-end" gap={1}>
          <Button onClick={onClose} variant="contained" color="secondary">
            {t("common.actions.cancel")}
          </Button>
          <Button
            onClick={() => {
              onConfirm();
              onClose();
            }}
            variant="contained"
            color="error"
          >
            {t("common.actions.delete")}
          </Button>
        </RowStack>
      </DialogActions>
    </Dialog>
  );
}
