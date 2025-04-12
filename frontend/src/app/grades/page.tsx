import { Box, Typography } from "@mui/material"
import { GradeEntryForm } from "./_components/grade-entity-form"

export default function GradesPage() {
  return (
    <Box sx={{ display: "flex", flexDirection: "column", gap: 3 }}>
      <Typography variant="h4" component="h1" fontWeight="bold">
        Quản lý điểm số
      </Typography>
      <GradeEntryForm />
    </Box>
  )
}
