"use client";

import { useState } from "react";
import {
  Box,
  Button,
  Card,
  CardContent,
  CardHeader,
  CardActions,
  Typography,
  FormControl,
  InputLabel,
  Select,
  MenuItem,
  Paper,
  type SelectChangeEvent,
} from "@mui/material";
import PrintIcon from "@mui/icons-material/Print";
import { mockStudents } from "@/lib/mock-data";
import { TranscriptPreview } from "@/app/transcripts/_component/transcript-preview";
import { Gender } from "@/types/student";

export function TranscriptForm() {
  const [studentId, setStudentId] = useState<string>("");
  const [showPreview, setShowPreview] = useState<boolean>(false);

  const handleStudentChange = (event: SelectChangeEvent<string>): void => {
    setStudentId(event.target.value);
    setShowPreview(false);
  };

  const handleGenerateTranscript = (): void => {
    setShowPreview(true);
  };

  const handlePrint = (): void => {
    window.print();
  };

  const selectedStudent = studentId
    ? mockStudents.find((student) => student.id.toString() === studentId)
    : null;

  // Create a complete student object with all required properties
  const completeStudent = selectedStudent
    ? {
        id: selectedStudent.id.toString(),
        name: selectedStudent.name,
        code: selectedStudent.code,
        department: selectedStudent.department,
        batch: selectedStudent.batch,
        dateOfBirth: "",
        gender: Gender.Male,
        email: "",
        permanentAddress: "",
        temporaryAddress: "",
        mailingAddress: "",
        faculty: "CNTT",
        course: 2020,
        program: "KTPM",
        phone: "",
        status: "Active",
        identity: {
          type: "CCCD",
          documentNumber: "",
          issueDate: new Date(),
          issuePlace: "",
          expiryDate: new Date(),
          countryIssue: "Việt Nam",
          isChip: false,
          notes: "",
        },
        nationality: "Việt Nam",
      }
    : null;

  return (
    <Box sx={{ display: "flex", flexDirection: "column", gap: 3 }}>
      <Card>
        <CardHeader
          title="In bảng điểm chính thức"
          subheader="Chọn sinh viên để in bảng điểm chính thức."
        />
        <CardContent sx={{ pt: 0 }}>
          <Box sx={{ display: "flex", flexDirection: "column", gap: 2 }}>
            <FormControl fullWidth required>
              <InputLabel id="student-select-label">Sinh viên</InputLabel>
              <Select
                labelId="student-select-label"
                id="student-select"
                value={studentId}
                label="Sinh viên"
                onChange={handleStudentChange}
              >
                {mockStudents.map((student) => (
                  <MenuItem key={student.id} value={student.id.toString()}>
                    {student.code}: {student.name}
                  </MenuItem>
                ))}
              </Select>
            </FormControl>

            {selectedStudent && (
              <Paper
                variant="outlined"
                sx={{ p: 2, bgcolor: "background.default" }}
              >
                <Typography
                  variant="subtitle1"
                  fontWeight="medium"
                  gutterBottom
                >
                  Thông tin sinh viên
                </Typography>
                <Box
                  sx={{
                    display: "grid",
                    gridTemplateColumns: "1fr 1fr",
                    gap: 1,
                  }}
                >
                  <Box>
                    <Typography
                      variant="body2"
                      color="text.secondary"
                      component="span"
                    >
                      Mã sinh viên:
                    </Typography>{" "}
                    <Typography variant="body2" component="span">
                      {selectedStudent.code}
                    </Typography>
                  </Box>
                  <Box>
                    <Typography
                      variant="body2"
                      color="text.secondary"
                      component="span"
                    >
                      Họ tên:
                    </Typography>{" "}
                    <Typography variant="body2" component="span">
                      {selectedStudent.name}
                    </Typography>
                  </Box>
                  <Box>
                    <Typography
                      variant="body2"
                      color="text.secondary"
                      component="span"
                    >
                      Khoa:
                    </Typography>{" "}
                    <Typography variant="body2" component="span">
                      {selectedStudent.department}
                    </Typography>
                  </Box>
                  <Box>
                    <Typography
                      variant="body2"
                      color="text.secondary"
                      component="span"
                    >
                      Khóa:
                    </Typography>{" "}
                    <Typography variant="body2" component="span">
                      {selectedStudent.batch}
                    </Typography>
                  </Box>
                </Box>
              </Paper>
            )}
          </Box>
        </CardContent>
        <CardActions sx={{ px: 3, pb: 3 }}>
          <Button
            variant="contained"
            onClick={handleGenerateTranscript}
            disabled={!selectedStudent}
          >
            Tạo bảng điểm
          </Button>
        </CardActions>
      </Card>

      {showPreview && selectedStudent && (
        <Box sx={{ display: "flex", flexDirection: "column", gap: 2 }}>
          <Box
            sx={{
              display: "flex",
              justifyContent: "space-between",
              alignItems: "center",
            }}
          >
            <Typography variant="h5" fontWeight="medium">
              Bảng điểm chính thức
            </Typography>
            <Button
              variant="contained"
              startIcon={<PrintIcon />}
              onClick={handlePrint}
              className="print:hidden"
            >
              In bảng điểm
            </Button>
          </Box>
          <TranscriptPreview student={completeStudent!} />
        </Box>
      )}
    </Box>
  );
}
