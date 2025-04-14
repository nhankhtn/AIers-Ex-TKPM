import { Box, Typography } from "@mui/material"
import { TranscriptForm } from "@/app/transcripts/_component/transcript-form"

export default function TranscriptsPage(){
  return (
    <Box sx={{ display: "flex", flexDirection: "column", gap: 3 }}>
      <Typography variant="h4" component="h1" fontWeight="bold">
        In bảng điểm
      </Typography>
      <TranscriptForm />
    </Box>
  )
}
