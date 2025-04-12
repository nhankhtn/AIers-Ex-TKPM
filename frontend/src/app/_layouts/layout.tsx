"use client"

import { useState } from "react"
import { Box, Drawer, AppBar, Toolbar, IconButton } from "@mui/material"
import MenuIcon from "@mui/icons-material/Menu"
import Sidebar from "./sidebar"
import RowStack from "@/components/row-stack"
import { drawerWidth } from "@/constants"
import { useDialog } from "@/hooks/use-dialog"


export default function Layout({ children }: { children: React.ReactNode }) {
  const { handleOpen, handleClose, open } = useDialog();


  return (
    <RowStack sx= {{ p: 3}}>
      <Box component="nav" sx={{ width: { sm: drawerWidth }, flexShrink: { sm: 0 }, }}>
      <AppBar
        sx={{
          width: { sm: `calc(100% - ${drawerWidth}px)` },
          ml: { sm: `${drawerWidth}px` },
          display: {  sm: "none", },
        }}
      >
        <Toolbar>
          <IconButton
            color="inherit"
            aria-label="open drawer"
            edge="start"
            onClick={handleOpen}
            sx={{ mr: 2, display: { sm: "none" } }}
          >
            <MenuIcon />
          </IconButton>
        </Toolbar>
      </AppBar>
        {/* Mobile drawer */}
        <Drawer
          variant="temporary"
          open={open}
          onClose={handleClose}
          ModalProps={{
            keepMounted: true, // Better open performance on mobile
          }}
          sx={{
            display: { xs: "block", sm: "none" },
            "& .MuiDrawer-paper": { boxSizing: "border-box", width: drawerWidth },
          }}
        >
          <Sidebar />
        </Drawer>

        {/* Desktop drawer */}
        <Drawer
          variant="permanent"
          sx={{
            display: { xs: "none", sm: "block" },
            "& .MuiDrawer-paper": { boxSizing: "border-box", width: drawerWidth },
          }}
          open
        >
          <Sidebar />
        </Drawer>
      </Box>

      <Box
        component="main"
        sx={{
          marginTop: { xs: 8, sm: 0 },
          overflow: "auto",
          flex:1,
          minHeight: "100vh",
        }}
      >
        {children}
      </Box>
    </RowStack>
  )
}

