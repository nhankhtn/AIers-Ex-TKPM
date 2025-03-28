import {
  Button,
  Dialog,
  DialogActions,
  DialogContent,
  DialogTitle,
  IconButton,
  Stack,
  Typography,
  TextField,
  FormHelperText,
} from "@mui/material";
import CloseIcon from "@mui/icons-material/Close";
import RowStack from "@/components/row-stack";
import { useCallback, useState } from "react";
import { useMainContext } from "@/context";

interface DialogConfigEmailProps {
  open: boolean;
  onClose: () => void;
  allowedEmail: string[];
}

const DialogConfigEmail = ({
  open,
  onClose,
  allowedEmail,
}: DialogConfigEmailProps) => {
  const [emails, setEmails] = useState(
    allowedEmail.map((e) => e.split("@")[1]).join(", ")
  );
  const [error, setError] = useState<string | null>(null);
  const { updateSettingsApi } = useMainContext();

  const handleSave = useCallback(() => {
    const domainRegex = /^[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$/;
    const invalidDomains = emails
      .split(",")
      .map((e) => e.trim())
      .filter((e) => !domainRegex.test(e));
    if (invalidDomains.length) {
      setError(`Danh sách domain không hợp lệ: ${invalidDomains.join(", ")}`);
      return;
    }
    updateSettingsApi.call(
      emails
        .split(",")
        .map((e) => `@${e.trim()}`)
        .join(",")
    );
    onClose();
  }, [onClose, emails, updateSettingsApi]);

  return (
    <Dialog
      open={open}
      onClose={onClose}
      sx={{
        "& .MuiDialog-paper": {
          width: {
            lg: 500,
          },
        },
      }}
    >
      <DialogTitle sx={{ pb: 1 }}>
        <RowStack justifyContent={"space-between"}>
          <Typography variant='h6'>Cấu hình Domain Email</Typography>
          <IconButton
            onClick={onClose}
            disableRipple
            sx={{ width: "40px", height: "40px" }}
          >
            <CloseIcon />
          </IconButton>
        </RowStack>
      </DialogTitle>
      <DialogContent sx={{ px: 3, py: 0.5 }}>
        <Stack gap={"10px"}>
          <Typography variant='body1'>
            Nhập danh sách domain email được phép (phân tách bằng dấu phẩy):
          </Typography>
          <TextField
            fullWidth
            variant='outlined'
            size='small'
            value={emails}
            onChange={(e) => setEmails(e.target.value)}
            placeholder='example.com, mydomain.com'
          />
          {error && (
            <FormHelperText
              sx={{
                color: "red",
              }}
            >
              {error}
            </FormHelperText>
          )}
        </Stack>
      </DialogContent>
      <DialogActions>
        <RowStack justifyContent={"flex-end"} gap={1}>
          <Button onClick={onClose} variant='contained' color='secondary'>
            Huỷ
          </Button>
          <Button onClick={handleSave} variant='contained' color='primary'>
            Lưu
          </Button>
        </RowStack>
      </DialogActions>
    </Dialog>
  );
};

export default DialogConfigEmail;
