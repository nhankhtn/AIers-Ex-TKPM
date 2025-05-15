"use client";

import { usePathname } from "next/navigation";
import Link from "next/link";
import {
  List,
  ListItem,
  ListItemButton,
  ListItemIcon,
  ListItemText,
  Divider,
  Toolbar,
  Typography,
} from "@mui/material";
import DashboardIcon from "@mui/icons-material/Dashboard";
import HomeIcon from "@mui/icons-material/Home";
import MenuBookIcon from "@mui/icons-material/MenuBook";
import CalendarMonthIcon from "@mui/icons-material/CalendarMonth";
import PlaylistAddCheckIcon from "@mui/icons-material/PlaylistAddCheck";
import PeopleIcon from "@mui/icons-material/People";
import SchoolIcon from "@mui/icons-material/School";
import SettingsIcon from "@mui/icons-material/Settings";
import { paths } from "@/paths";
import { useTranslations } from "next-intl";
import SettingsModal from "../_components/settings-modal";
import { useDialog } from "@/hooks/use-dialog";

const getNavItems = (t: (key: string) => string) => [
  {
    title: t("studentManagement"),
    href: paths.dashboard.index,
    icon: HomeIcon,
  },
  {
    title: t("courseManagement"),
    href: paths.courses.index,
    icon: MenuBookIcon,
  },
  {
    title: t("classManagement"),
    href: paths.classes.index,
    icon: CalendarMonthIcon,
  },
  {
    title: t("courseRegistration"),
    href: paths.registrations.index,
    icon: PlaylistAddCheckIcon,
  },
  {
    title: t("gradeManagement"),
    href: paths.grades.index,
    icon: PeopleIcon,
  },
  {
    title: t("transcripts"),
    href: paths.transcripts.index,
    icon: SchoolIcon,
  },
];

export default function Sidebar() {
  const pathname = usePathname();
  const t = useTranslations("sidebar");
  const navItems = getNavItems(t);
  const settingDialog= useDialog();
  return (
    <div>
      <Toolbar sx={{ display: "flex", alignItems: "center", px: [1] }}>
        <DashboardIcon sx={{ mr: 1 }} />
        <Typography variant="h6" noWrap component="div">
          {t("title")}
        </Typography>
      </Toolbar>
      <Divider />
      <List>
        {navItems.map((item) => (
          <ListItem key={item.href} disablePadding>
            <ListItemButton
              component={Link}
              href={item.href}
              selected={pathname.includes(item.href)}
            >
              <ListItemIcon>
                <item.icon />
              </ListItemIcon>
              <ListItemText primary={item.title} />
            </ListItemButton>
          </ListItem>
        ))}
        <ListItem disablePadding>
          <ListItemButton onClick={() => settingDialog.handleOpen()}>
            <ListItemIcon>
              <SettingsIcon />
            </ListItemIcon>
            <ListItemText primary={t("settings")} />
          </ListItemButton>
        </ListItem>
      </List>
      <SettingsModal
        open={settingDialog.open}
        onClose={() => settingDialog.handleClose()}
      />
    </div>
  );
}
