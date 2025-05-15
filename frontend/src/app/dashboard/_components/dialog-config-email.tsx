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
import { useMainContext } from "@/context/main/main-context";
import { useTranslations } from "next-intl";

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
  const t = useTranslations("dashboard.dialogs.email");
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
      setError(t("invalidDomains", { domains: invalidDomains.join(", ") }));
      return;
    }
    updateSettingsApi.call(
      emails
        .split(",")
        .map((e) => `@${e.trim()}`)
        .join(",")
    );
    onClose();
  }, [onClose, emails, updateSettingsApi, t]);

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
          <Typography variant="h6">{t("title")}</Typography>
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
          <Typography variant="body1">{t("description")}</Typography>
          <TextField
            fullWidth
            variant="outlined"
            size="small"
            value={emails}
            onChange={(e) => setEmails(e.target.value)}
            placeholder={t("placeholder")}
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
          <Button onClick={onClose} variant="contained" color="secondary">
            {t("cancel")}
          </Button>
          <Button onClick={handleSave} variant="contained" color="primary">
            {t("save")}
          </Button>
        </RowStack>
      </DialogActions>
    </Dialog>
  );
};

export default DialogConfigEmail;
