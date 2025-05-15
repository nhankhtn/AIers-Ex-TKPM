"use client";

import { useState, type SyntheticEvent } from "react";
import { Box, Typography, Tab, Tabs } from "@mui/material";
import type React from "react";
import { Registration } from "@/types/registration";
import { RegistrationForm } from "./registration-form";
import RegistrationList from "./registration-list";
import UnregisterList from "./unregister-list";
import { useTranslations } from "next-intl";

export default function RegistrationsContent() {
  const [value, setValue] = useState<number>(0);
  const t = useTranslations("registrations");

  return (
    <Box sx={{ display: "flex", flexDirection: "column", gap: 3 }}>
      <Typography variant="h4" component="h1" fontWeight="bold">
        {t("title")}
      </Typography>

      <Box sx={{ width: "100%" }}>
        <Box sx={{ borderBottom: 1, borderColor: "divider" }}>
          <Tabs
            value={value}
            onChange={(_event: SyntheticEvent, newValue: number): void => {
              setValue(newValue);
            }}
            aria-label="registration tabs"
          >
            <Tab
              label={t("tabs.newRegistration")}
              id="tab-0"
              aria-controls="tabpanel-0"
            />
            <Tab
              label={t("tabs.registrationHistory")}
              id="tab-1"
              aria-controls="tabpanel-1"
            />
            <Tab
              label={t("tabs.unregistrationHistory")}
              id="tab-2"
              aria-controls="tabpanel-2"
            />
          </Tabs>
        </Box>
        <TabPanel value={value} index={0}>
          <RegistrationForm />
        </TabPanel>
        <TabPanel value={value} index={1}>
          <RegistrationList />
        </TabPanel>
        <TabPanel value={value} index={2}>
          <UnregisterList />
        </TabPanel>
      </Box>
    </Box>
  );
}

// Custom TabPanel component to replace @mui/lab's TabPanel
interface TabPanelProps {
  children?: React.ReactNode;
  index: number;
  value: number;
}

function TabPanel(props: TabPanelProps) {
  const { children, value, index, ...other } = props;

  return (
    <div
      role="tabpanel"
      hidden={value !== index}
      id={`tabpanel-${index}`}
      aria-labelledby={`tab-${index}`}
      {...other}
      style={{ paddingTop: "16px" }}
    >
      {value === index && <Box>{children}</Box>}
    </div>
  );
}
