import { Box, Typography, Button } from "@mui/material"
import AddCircleIcon from "@mui/icons-material/AddCircle"
import { ClassList } from "@/app/classes/_component/class-list"
import Link from "next/link"

export default function ClassesPage() {
  return (
    <Box sx={{ display: "flex", flexDirection: "column", gap: 3 }}>
      <Box sx={{ display: "flex", justifyContent: "space-between", alignItems: "center" }}>
        <Typography variant="h4" component="h1" fontWeight="bold">
          Quản lý lớp học
        </Typography>
        <Button variant="contained" startIcon={<AddCircleIcon />} component={Link} href="/classes/new">
          Mở lớp học mới
        </Button>
      </Box>

      <ClassList />
    </Box>
  )
}
