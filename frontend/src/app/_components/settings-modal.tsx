"use client";

import { useDialog } from "@/hooks/use-dialog";
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
} from "@mui/material";
import { useTranslations } from "next-intl";
import { useState } from "react";

interface SettingsModalProps {
  open: boolean;
  onClose: () => void;
}

export default function SettingsModal({ open, onClose }: SettingsModalProps) {
  const t = useTranslations("settings");
  const [language, setLanguage] = useState(
    typeof window !== "undefined"
      ? localStorage.getItem("language") || "vi"
      : "vi"
  );

  const handleSave = () => {
    localStorage.setItem("language", language);
    window.location.reload(); // Reload to apply new language
    onClose();
  };

  return (
    <Dialog open={open} onClose={onClose}>
      <DialogTitle>{t("title")}</DialogTitle>
      <DialogContent sx={{ p: 3 }}> 
        <FormControl fullWidth sx={{ mt: 2 }}>
          <InputLabel id="language-select-label">{t("language")}</InputLabel>
          <Select
            sx={{ width: "250px" }}
            labelId="language-select-label"
            value={language}
            label={t("language")}
            onChange={(e) => setLanguage(e.target.value)}
          >
            <MenuItem value="vi">Tiếng Việt</MenuItem>
            <MenuItem value="en">English</MenuItem>
          </Select>
        </FormControl>
      </DialogContent>
      <DialogActions>
        <Button onClick={onClose}>{t("cancel")}</Button>
        <Button onClick={handleSave} variant="contained">
          {t("save")}
        </Button>
      </DialogActions>
    </Dialog>
  );
}
