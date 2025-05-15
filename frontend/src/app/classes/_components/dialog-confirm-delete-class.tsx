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
import { Class } from "@/types/class";
import { useTranslations } from "next-intl";

interface DialogConfirmDeleteClassProps {
  open: boolean;
  onClose: () => void;
  onConfirm: () => void;
  data: Class;
}

const DialogConfirmDeleteClass = ({
  open,
  onClose,
  onConfirm,
  data,
}: DialogConfirmDeleteClassProps) => {
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
        <RowStack justifyContent={"space-between"}>
          <Typography variant={"h6"}>
            {t("classes.dialogs.delete.title")}
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
        <Stack gap={"10px"}>
          <Typography variant={"body1"}>
            {t("classes.dialogs.delete.confirmMessage")}{" "}
            <Typography
              variant={"subtitle1"}
              fontWeight={600}
              component={"span"}
            >
              &quot;{data.id}&quot;?{" "}
            </Typography>
          </Typography>
        </Stack>
      </DialogContent>
      <DialogActions>
        <RowStack justifyContent={"flex-end"} gap={1}>
          <Button onClick={onClose} variant="contained" color="secondary">
            {t("classes.dialogs.delete.cancel")}
          </Button>
          <Button
            onClick={() => {
              onConfirm();
              onClose();
            }}
            variant="contained"
            color="error"
          >
            {t("classes.dialogs.delete.delete")}
          </Button>
        </RowStack>
      </DialogActions>
    </Dialog>
  );
};

export default DialogConfirmDeleteClass;
