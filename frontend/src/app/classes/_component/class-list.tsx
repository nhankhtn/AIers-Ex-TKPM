"use client"

import { useState, type MouseEvent } from "react"
import Link from "next/link"
import {
  Box,
  Paper,
  Table,
  TableBody,
  TableCell,
  TableContainer,
  TableHead,
  TableRow,
  TextField,
  InputAdornment,
  IconButton,
  Menu,
  MenuItem,
  ListItemIcon,
  Chip,
  FormControl,
  InputLabel,
  Select,
  Typography,
  type SelectChangeEvent,
} from "@mui/material"
import SearchIcon from "@mui/icons-material/Search"
import EditIcon from "@mui/icons-material/Edit"
import MoreVertIcon from "@mui/icons-material/MoreVert"
import CalendarMonthIcon from "@mui/icons-material/CalendarMonth"
import PeopleIcon from "@mui/icons-material/People"
import { mockClasses } from "@/lib/mock-data"
import type { Class } from "@/types/class"
import type { JSX } from "react/jsx-runtime"

export function ClassList(): JSX.Element {
  const [searchTerm, setSearchTerm] = useState<string>("")
  const [semesterFilter, setSemesterFilter] = useState<string>("all")
  const [yearFilter, setYearFilter] = useState<string>("all")
  const [menuAnchorEl, setMenuAnchorEl] = useState<HTMLElement | null>(null)
  const [menuClass, setMenuClass] = useState<Class | null>(null)

  // Filter classes based on search term and filters
  const filteredClasses = mockClasses.filter((classItem) => {
    const matchesSearch =
      classItem.code.toLowerCase().includes(searchTerm.toLowerCase()) ||
      classItem.courseName.toLowerCase().includes(searchTerm.toLowerCase()) ||
      classItem.instructor.toLowerCase().includes(searchTerm.toLowerCase())

    const matchesSemester = semesterFilter === "all" || classItem.semester === Number.parseInt(semesterFilter)

    const matchesYear = yearFilter === "all" || classItem.year === yearFilter

    return matchesSearch && matchesSemester && matchesYear
  })

  const handleMenuOpen = (event: MouseEvent<HTMLButtonElement>, classItem: Class): void => {
    setMenuAnchorEl(event.currentTarget)
    setMenuClass(classItem)
  }

  const handleMenuClose = (): void => {
    setMenuAnchorEl(null)
    setMenuClass(null)
  }

  const handleSemesterFilterChange = (event: SelectChangeEvent): void => {
    setSemesterFilter(event.target.value)
  }

  const handleYearFilterChange = (event: SelectChangeEvent): void => {
    setYearFilter(event.target.value)
  }

  return (
    <Box sx={{ display: "flex", flexDirection: "column", gap: 2 }}>
      <Box sx={{ display: "flex", flexDirection: { xs: "column", md: "row" }, gap: 2 }}>
        <TextField
          placeholder="Tìm kiếm lớp học..."
          variant="outlined"
          fullWidth
          value={searchTerm}
          onChange={(e) => setSearchTerm(e.target.value)}
          InputProps={{
            startAdornment: (
              <InputAdornment position="start">
                <SearchIcon />
              </InputAdornment>
            ),
          }}
          sx={{ flexGrow: 1 }}
        />

        <FormControl sx={{ minWidth: 180 }}>
          <InputLabel id="year-filter-label">Năm học</InputLabel>
          <Select
            labelId="year-filter-label"
            id="year-filter"
            value={yearFilter}
            label="Năm học"
            onChange={handleYearFilterChange}
          >
            <MenuItem value="all">Tất cả năm học</MenuItem>
            <MenuItem value="2023-2024">2023-2024</MenuItem>
            <MenuItem value="2022-2023">2022-2023</MenuItem>
            <MenuItem value="2021-2022">2021-2022</MenuItem>
          </Select>
        </FormControl>

        <FormControl sx={{ minWidth: 180 }}>
          <InputLabel id="semester-filter-label">Học kỳ</InputLabel>
          <Select
            labelId="semester-filter-label"
            id="semester-filter"
            value={semesterFilter}
            label="Học kỳ"
            onChange={handleSemesterFilterChange}
          >
            <MenuItem value="all">Tất cả học kỳ</MenuItem>
            <MenuItem value="1">Học kỳ 1</MenuItem>
            <MenuItem value="2">Học kỳ 2</MenuItem>
            <MenuItem value="3">Học kỳ hè</MenuItem>
          </Select>
        </FormControl>
      </Box>

      <TableContainer component={Paper}>
        <Table sx={{ minWidth: 650 }} aria-label="class table">
          <TableHead>
            <TableRow>
              <TableCell>Mã lớp</TableCell>
              <TableCell>Khóa học</TableCell>
              <TableCell sx={{ display: { xs: "none", md: "table-cell" } }}>Giảng viên</TableCell>
              <TableCell sx={{ display: { xs: "none", md: "table-cell" } }}>Năm học</TableCell>
              <TableCell sx={{ display: { xs: "none", md: "table-cell" } }}>Học kỳ</TableCell>
              <TableCell>Sĩ số</TableCell>
              <TableCell align="right">Thao tác</TableCell>
            </TableRow>
          </TableHead>
          <TableBody>
            {filteredClasses.length === 0 ? (
              <TableRow>
                <TableCell colSpan={7} align="center" sx={{ py: 3 }}>
                  Không tìm thấy lớp học nào.
                </TableCell>
              </TableRow>
            ) : (
              filteredClasses.map((classItem) => (
                <TableRow key={classItem.id}>
                  <TableCell component="th" scope="row" sx={{ fontWeight: "medium" }}>
                    {classItem.code}
                  </TableCell>
                  <TableCell>
                    <Box>
                      <Typography variant="body2" fontWeight="medium">
                        {classItem.courseCode}
                      </Typography>
                      <Typography variant="body2" color="text.secondary">
                        {classItem.courseName}
                      </Typography>
                    </Box>
                  </TableCell>
                  <TableCell sx={{ display: { xs: "none", md: "table-cell" } }}>{classItem.instructor}</TableCell>
                  <TableCell sx={{ display: { xs: "none", md: "table-cell" } }}>{classItem.year}</TableCell>
                  <TableCell sx={{ display: { xs: "none", md: "table-cell" } }}>
                    {classItem.semester === 1 ? "Học kỳ 1" : classItem.semester === 2 ? "Học kỳ 2" : "Học kỳ hè"}
                  </TableCell>
                  <TableCell>
                    <Chip
                      label={`${classItem.enrolledStudents}/${classItem.maxStudents}`}
                      color="info"
                      size="small"
                      variant="outlined"
                    />
                  </TableCell>
                  <TableCell align="right">
                    <IconButton aria-label="more" onClick={(e) => handleMenuOpen(e, classItem)} size="small">
                      <MoreVertIcon fontSize="small" />
                    </IconButton>
                  </TableCell>
                </TableRow>
              ))
            )}
          </TableBody>
        </Table>
      </TableContainer>

      {/* Menu for row actions */}
      <Menu anchorEl={menuAnchorEl} open={Boolean(menuAnchorEl)} onClose={handleMenuClose}>
        <MenuItem component={Link} href={`/classes/${menuClass?.id}`} onClick={handleMenuClose}>
          <ListItemIcon>
            <EditIcon fontSize="small" />
          </ListItemIcon>
          Chỉnh sửa
        </MenuItem>
        <MenuItem component={Link} href={`/classes/${menuClass?.id}/schedule`} onClick={handleMenuClose}>
          <ListItemIcon>
            <CalendarMonthIcon fontSize="small" />
          </ListItemIcon>
          Lịch học
        </MenuItem>
        <MenuItem component={Link} href={`/classes/${menuClass?.id}/students`} onClick={handleMenuClose}>
          <ListItemIcon>
            <PeopleIcon fontSize="small" />
          </ListItemIcon>
          Danh sách sinh viên
        </MenuItem>
      </Menu>
    </Box>
  )
}
