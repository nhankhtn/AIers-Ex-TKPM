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
import { paths } from "@/paths";
const navItems = [
  {
    title: "Quản lý sinh viên",
    href: paths.dashboard.index,
    icon: HomeIcon,
  },
  {
    title: "Quản lý khóa học",
    href: paths.courses.index,
    icon: MenuBookIcon,
  },
  {
    title: "Quản lý lớp học",
    href: paths.classes.index,
    icon: CalendarMonthIcon,
  },
  {
    title: "Đăng ký khóa học",
    href: paths.registrations.index,
    icon: PlaylistAddCheckIcon,
  },
  {
    title: "Quản lý điểm số",
    href: paths.grades.index,
    icon: PeopleIcon,
  },
  {
    title: "Bảng điểm",
    href: paths.transcripts.index,
    icon: SchoolIcon,
  },
];

export default function Sidebar() {
  const pathname = usePathname();
  return (
    <div>
      <Toolbar sx={{ display: "flex", alignItems: "center", px: [1] }}>
        <DashboardIcon sx={{ mr: 1 }} />
        <Typography variant='h6' noWrap component='div'>
          Quản lý đào tạo
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
      </List>
    </div>
  );
}
