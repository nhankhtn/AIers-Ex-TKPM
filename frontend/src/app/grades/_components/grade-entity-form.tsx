"use client"

import { useState, useEffect } from "react"
import {
  Box,
  Card,
  CardHeader,
  CardContent,
  CardActions,
  Typography,
  FormControl,
  InputLabel,
  Select,
  MenuItem,
  TextField,
  Button,
  Table,
  TableBody,
  TableCell,
  TableContainer,
  TableHead,
  TableRow,
  Paper,
  Alert,
  AlertTitle,
  IconButton,
  Dialog,
  DialogTitle,
  DialogContent,
  DialogContentText,
  DialogActions,
  type SelectChangeEvent,
} from "@mui/material"
import SaveIcon from "@mui/icons-material/Save"
import InfoIcon from "@mui/icons-material/Info"
import CheckCircleIcon from "@mui/icons-material/CheckCircle"
import { mockClasses, mockStudents, mockCourses, mockRegistrations } from "@/lib/mock-data"

interface StudentGrade {
  studentId: string
  studentCode: string
  studentName: string
  midtermGrade: string
  finalGrade: string
  totalGrade: string
  letterGrade: string
  status: string
  notes: string
}

interface GradeInfo {
  isOpen: boolean
  title: string
  content: string
}

export function GradeEntryForm() {
  const [classId, setClassId] = useState<string>("")
  const [studentGrades, setStudentGrades] = useState<StudentGrade[]>([])
  const [saveSuccess, setSaveSuccess] = useState<boolean | null>(null)
  const [gradeInfo, setGradeInfo] = useState<GradeInfo>({
    isOpen: false,
    title: "",
    content: "",
  })

  const handleClassChange = (event: SelectChangeEvent<string>): void => {
    setClassId(event.target.value)
    setSaveSuccess(null)
  }

  // Generate student grades when class is selected
  useEffect(() => {
    if (!classId) {
      setStudentGrades([])
      return
    }

    // Find all registrations for this class
    const classRegistrations = mockRegistrations.filter(
      (reg) => reg.classId === classId && reg.status === "active",
    )

    // Create grade entries for each student
    const grades = classRegistrations
      .map((reg) => {
        const student = mockStudents.find((s) => s.id === reg.studentId)
        if (!student) return null

        // In a real app, we would fetch existing grades from the database
        // For this demo, we'll generate random grades for some students and empty for others
        const hasGrades = Math.random() > 0.5
        const midterm = hasGrades ? (Math.random() * 10).toFixed(1) : ""
        const final = hasGrades ? (Math.random() * 10).toFixed(1) : ""

        // Calculate total and letter grade if both midterm and final exist
        let total = ""
        let letter = ""
        let status = ""

        if (midterm && final) {
          const midtermNum = Number.parseFloat(midterm)
          const finalNum = Number.parseFloat(final)
          const totalNum = midtermNum * 0.4 + finalNum * 0.6
          total = totalNum.toFixed(1)

          if (totalNum >= 8.5) letter = "A"
          else if (totalNum >= 7.0) letter = "B"
          else if (totalNum >= 5.5) letter = "C"
          else if (totalNum >= 4.0) letter = "D"
          else letter = "F"

          status = totalNum >= 4.0 ? "Đạt" : "Không đạt"
        }

        return {
          studentId: student.id,
          studentCode: student.code,
          studentName: student.name,
          midtermGrade: midterm,
          finalGrade: final,
          totalGrade: total,
          letterGrade: letter,
          status: status,
          notes: "",
        }
      })
      .filter((grade): grade is StudentGrade => grade !== null)

    setStudentGrades(grades)
  }, [classId])

  const handleGradeChange = (studentId: string, field: keyof StudentGrade, value: string): void => {
    setStudentGrades((prevGrades) => {
      return prevGrades.map((grade) => {
        if (grade.studentId !== studentId) return grade

        const updatedGrade = { ...grade, [field]: value }

        // Recalculate total and letter grade if midterm or final changed
        if (field === "midtermGrade" || field === "finalGrade") {
          const midterm = field === "midtermGrade" ? value : grade.midtermGrade
          const final = field === "finalGrade" ? value : grade.finalGrade

          if (midterm && final) {
            const midtermNum = Number.parseFloat(midterm)
            const finalNum = Number.parseFloat(final)

            if (!isNaN(midtermNum) && !isNaN(finalNum)) {
              const totalNum = midtermNum * 0.4 + finalNum * 0.6
              updatedGrade.totalGrade = totalNum.toFixed(1)

              if (totalNum >= 8.5) updatedGrade.letterGrade = "A"
              else if (totalNum >= 7.0) updatedGrade.letterGrade = "B"
              else if (totalNum >= 5.5) updatedGrade.letterGrade = "C"
              else if (totalNum >= 4.0) updatedGrade.letterGrade = "D"
              else updatedGrade.letterGrade = "F"

              updatedGrade.status = totalNum >= 4.0 ? "Đạt" : "Không đạt"
            } else {
              updatedGrade.totalGrade = ""
              updatedGrade.letterGrade = ""
              updatedGrade.status = ""
            }
          } else {
            updatedGrade.totalGrade = ""
            updatedGrade.letterGrade = ""
            updatedGrade.status = ""
          }
        }

        return updatedGrade
      })
    })

    // Reset save status when changes are made
    setSaveSuccess(null)
  }

  const handleSaveGrades = (): void => {
    // In a real app, this would call an API to save the grades
    console.log("Saving grades:", studentGrades)

    // Simulate successful save
    setSaveSuccess(true)
  }

  const handleShowGradeInfo = (title: string, content: string): void => {
    setGradeInfo({
      isOpen: true,
      title,
      content,
    })
  }

  const handleCloseGradeInfo = (): void => {
    setGradeInfo({
      ...gradeInfo,
      isOpen: false,
    })
  }

  const selectedClass = classId ? mockClasses.find((c) => c.id === classId) : null
  const selectedCourse = selectedClass ? mockCourses.find((c) => c.id === selectedClass.courseId.toString()) : null

  return (
    <Box sx={{ display: "flex", flexDirection: "column", gap: 3 }}>
      <Card>
        <CardHeader
          title="Nhập điểm cho sinh viên"
          subheader="Chọn lớp học và nhập điểm giữa kỳ, cuối kỳ cho sinh viên."
        />
        <CardContent sx={{ pt: 0 }}>
          <Box sx={{ display: "flex", flexDirection: "column", gap: 3 }}>
            <FormControl fullWidth required>
              <InputLabel id="class-select-label">Lớp học</InputLabel>
              <Select
                labelId="class-select-label"
                id="class-select"
                value={classId}
                label="Lớp học"
                onChange={handleClassChange}
              >
                {mockClasses.map((classItem) => {
                  const course = mockCourses.find((c) => c.id === classItem.courseId.toString())
                  return (
                    <MenuItem key={classItem.id} value={classItem.id}>
                      {classItem.code}: {course?.courseName} ({classItem.instructor})
                    </MenuItem>
                  )
                })}
              </Select>
            </FormControl>

            {selectedClass && selectedCourse && (
              <Paper variant="outlined" sx={{ p: 2, bgcolor: "background.default" }}>
                <Typography variant="subtitle1" fontWeight="medium" gutterBottom>
                  Thông tin lớp học
                </Typography>
                <Box sx={{ display: "grid", gridTemplateColumns: "1fr 1fr", gap: 1 }}>
                  <Box>
                    <Typography variant="body2" color="text.secondary" component="span">
                      Mã lớp:
                    </Typography>{" "}
                    <Typography variant="body2" component="span">
                      {selectedClass.code}
                    </Typography>
                  </Box>
                  <Box>
                    <Typography variant="body2" color="text.secondary" component="span">
                      Khóa học:
                    </Typography>{" "}
                    <Typography variant="body2" component="span">
                      {selectedCourse.courseId}: {selectedCourse.courseName}
                    </Typography>
                  </Box>
                  <Box>
                    <Typography variant="body2" color="text.secondary" component="span">
                      Giảng viên:
                    </Typography>{" "}
                    <Typography variant="body2" component="span">
                      {selectedClass.instructor}
                    </Typography>
                  </Box>
                  <Box>
                    <Typography variant="body2" color="text.secondary" component="span">
                      Học kỳ:
                    </Typography>{" "}
                    <Typography variant="body2" component="span">
                      {selectedClass.semester === 1
                        ? "Học kỳ 1"
                        : selectedClass.semester === 2
                          ? "Học kỳ 2"
                          : "Học kỳ hè"}
                      , {selectedClass.year}
                    </Typography>
                  </Box>
                </Box>
              </Paper>
            )}

            {studentGrades.length > 0 && (
              <Box>
                <Box sx={{ display: "flex", justifyContent: "space-between", alignItems: "center", mb: 1 }}>
                  <Typography variant="subtitle1" fontWeight="medium">
                    Danh sách sinh viên và điểm số
                  </Typography>
                  <Box sx={{ display: "flex", alignItems: "center", gap: 1 }}>
                    <Typography variant="body2" color="text.secondary">
                      Điểm tổng = 40% điểm giữa kỳ + 60% điểm cuối kỳ
                    </Typography>
                    <IconButton
                      size="small"
                      color="primary"
                      onClick={() =>
                        handleShowGradeInfo(
                          "Thang điểm chữ",
                          "A: 8.5-10\nB: 7.0-8.4\nC: 5.5-6.9\nD: 4.0-5.4\nF: 0-3.9\n\nĐiểm đạt: từ D trở lên",
                        )
                      }
                    >
                      <InfoIcon fontSize="small" />
                    </IconButton>
                  </Box>
                </Box>
                <TableContainer component={Paper} variant="outlined">
                  <Table size="small">
                    <TableHead>
                      <TableRow>
                        <TableCell>STT</TableCell>
                        <TableCell>Mã SV</TableCell>
                        <TableCell>Họ tên</TableCell>
                        <TableCell align="center">Điểm giữa kỳ</TableCell>
                        <TableCell align="center">Điểm cuối kỳ</TableCell>
                        <TableCell align="center">Điểm tổng</TableCell>
                        <TableCell align="center">Điểm chữ</TableCell>
                        <TableCell align="center">Kết quả</TableCell>
                        <TableCell>Ghi chú</TableCell>
                      </TableRow>
                    </TableHead>
                    <TableBody>
                      {studentGrades.map((grade, index) => (
                        <TableRow key={grade.studentId}>
                          <TableCell>{index + 1}</TableCell>
                          <TableCell>{grade.studentCode}</TableCell>
                          <TableCell>{grade.studentName}</TableCell>
                          <TableCell align="center">
                            <TextField
                              variant="outlined"
                              size="small"
                              value={grade.midtermGrade}
                              onChange={(e) => handleGradeChange(grade.studentId, "midtermGrade", e.target.value)}
                              inputProps={{
                                style: { textAlign: "center" },
                                min: 0,
                                max: 10,
                                step: 0.1,
                              }}
                              sx={{ width: 80 }}
                            />
                          </TableCell>
                          <TableCell align="center">
                            <TextField
                              variant="outlined"
                              size="small"
                              value={grade.finalGrade}
                              onChange={(e) => handleGradeChange(grade.studentId, "finalGrade", e.target.value)}
                              inputProps={{
                                style: { textAlign: "center" },
                                min: 0,
                                max: 10,
                                step: 0.1,
                              }}
                              sx={{ width: 80 }}
                            />
                          </TableCell>
                          <TableCell align="center">
                            <Typography fontWeight="medium">{grade.totalGrade}</Typography>
                          </TableCell>
                          <TableCell align="center">
                            <Typography
                              fontWeight="medium"
                              color={
                                grade.letterGrade === "F"
                                  ? "error.main"
                                  : grade.letterGrade === "A"
                                    ? "success.main"
                                    : "text.primary"
                              }
                            >
                              {grade.letterGrade}
                            </Typography>
                          </TableCell>
                          <TableCell align="center">
                            <Typography
                              fontWeight="medium"
                              color={grade.status === "Đạt" ? "success.main" : "error.main"}
                            >
                              {grade.status}
                            </Typography>
                          </TableCell>
                          <TableCell>
                            <TextField
                              variant="outlined"
                              size="small"
                              value={grade.notes}
                              onChange={(e) => handleGradeChange(grade.studentId, "notes", e.target.value)}
                              placeholder="Ghi chú"
                              sx={{ width: 120 }}
                            />
                          </TableCell>
                        </TableRow>
                      ))}
                    </TableBody>
                  </Table>
                </TableContainer>
              </Box>
            )}

            {saveSuccess && (
              <Alert severity="success" icon={<CheckCircleIcon />}>
                <AlertTitle>Thành công</AlertTitle>
                Đã lưu điểm cho sinh viên thành công!
              </Alert>
            )}
          </Box>
        </CardContent>
        <CardActions sx={{ px: 3, pb: 3 }}>
          <Button
            variant="contained"
            startIcon={<SaveIcon />}
            onClick={handleSaveGrades}
            disabled={!selectedClass || studentGrades.length === 0}
          >
            Lưu điểm
          </Button>
        </CardActions>
      </Card>

      <Dialog open={gradeInfo.isOpen} onClose={handleCloseGradeInfo}>
        <DialogTitle>{gradeInfo.title}</DialogTitle>
        <DialogContent>
          <DialogContentText sx={{ whiteSpace: "pre-line" }}>{gradeInfo.content}</DialogContentText>
        </DialogContent>
        <DialogActions>
          <Button onClick={handleCloseGradeInfo}>Đóng</Button>
        </DialogActions>
      </Dialog>
    </Box>
  )
}
