"use client";
import React, { use, useEffect, useState, } from "react";
import type { Student } from "@/types/student";
import { Box, Typography, Paper, Button } from "@mui/material";
import { People as PeopleIcon, Add as AddIcon } from "@mui/icons-material";
import Grid from "@mui/material/Grid2";
import Table from "@/components/Table";
import { StudentCols } from "@/constants";
import SearchBar from "@/components/SearchBar";
import Dialog from "@/components/Dialog";

const arr : Student[]= [ {
  id: "SV0231",
  name: "Nguyễn Văn A",
  dateOfBirth: "2000-01-01",
  gender: "Nam",
  email: "nguyenvana@example.com",
  address: "123 Đường ABC, Quận 1, TP.HCM",
  faculty: "Khoa Luật",
  course: 2021,
  program: "Cử nhân Luật",
  phone: "0123456789",
  status: "Đang học",
},
{
  id: "SV002",
  name: "Trần Thị B",
  dateOfBirth: "1999-02-15",
  gender: "Nữ",
  email: "tranthib@example.com",
  address: "456 Đường DEF, Quận 2, TP.HCM",
  faculty: "Khoa Tiếng Anh thương mại",
  course: 2020,
  program: "Cử nhân Tiếng Anh thương mại",
  phone: "0987654321",
  status: "Đã tốt nghiệp",
},
{
  id: "SV003",
  name: "Lê Văn C",
  dateOfBirth: "2001-03-20",
  gender: "Nam",
  email: "levanc@example.com",
  address: "789 Đường GHI, Quận 3, TP.HCM",
  faculty: "Khoa Tiếng Nhật",
  course: 2022,
  program: "Cử nhân Tiếng Nhật",
  phone: "0123987654",
  status: "Đang học",
},
{
  id: "SV004",
  name: "Phạm Thị D",
  dateOfBirth: "1998-04-10",
  gender: "Nữ",
  email: "phamthid@example.com",
  address: "101 Đường JKL, Quận 4, TP.HCM",
  faculty: "Khoa Tiếng Pháp",
  course: 2019,
  program: "Cử nhân Tiếng Pháp",
  phone: "0987123456",
  status: "Đã thôi học",
},
{
  id: "SV005",
  name: "Hoàng Văn E",
  dateOfBirth: "2002-05-25",
  gender: "Nam",
  email: "hoangvane@example.com",
  address: "202 Đường MNO, Quận 5, TP.HCM",
  faculty: "Khoa Luật",
  course: 2023,
  program: "Cử nhân Luật",
  phone: "0123456780",
  status: "Tạm dừng học",
},
{
  id: "SV001",
  name: "Nguyễn Văn A",
  dateOfBirth: "2000-01-01",
  gender: "Nam",
  email: "nguyenvana@example.com",
  address: "123 Đường ABC, Quận 1, TP.HCM",
  faculty: "Khoa Luật",
  course: 2021,
  program: "Cử nhân Luật",
  phone: "0123456789",
  status: "Đang học",
},
{
  id: "SV002",
  name: "Trần Thị B",
  dateOfBirth: "1999-02-15",
  gender: "Nữ",
  email: "tranthib@example.com",
  address: "456 Đường DEF, Quận 2, TP.HCM",
  faculty: "Khoa Tiếng Anh thương mại",
  course: 2020,
  program: "Cử nhân Tiếng Anh thương mại",
  phone: "0987654321",
  status: "Đã tốt nghiệp",
},
{
  id: "SV003",
  name: "Lê Văn C",
  dateOfBirth: "2001-03-20",
  gender: "Nam",
  email: "levanc@example.com",
  address: "789 Đường GHI, Quận 3, TP.HCM",
  faculty: "Khoa Tiếng Nhật",
  course: 2022,
  program: "Cử nhân Tiếng Nhật",
  phone: "0123987654",
  status: "Đang học",
},
{
  id: "SV004",
  name: "Phạm Thị D",
  dateOfBirth: "1998-04-10",
  gender: "Nữ",
  email: "phamthid@example.com",
  address: "101 Đường JKL, Quận 4, TP.HCM",
  faculty: "Khoa Tiếng Pháp",
  course: 2019,
  program: "Cử nhân Tiếng Pháp",
  phone: "0987123456",
  status: "Đã thôi học",
},
{
  id: "SV005",
  name: "Hoàng Văn E",
  dateOfBirth: "2002-05-25",
  gender: "Nam",
  email: "hoangvane@example.com",
  address: "202 Đường MNO, Quận 5, TP.HCM",
  faculty: "Khoa Luật",
  course: 2023,
  program: "Cử nhân Luật",
  phone: "0123456780",
  status: "Tạm dừng học",
},]
export default function Page() {
  const [total , setTotal] = useState(10);
  const [page, setPage] = useState(1);
  const [rowsPerPage, setRowsPerPage] = useState(5);
  useEffect(() => {
    getStudents(page, rowsPerPage);
  }
  , [page, rowsPerPage]);
  const [students, setStudents] = useState<Student[]>([]);
  const [curStudent, setCurStudent] = useState<Student | null>(null);
  const [isOpen, setIsOpen] = useState(false);
  const handleClickOpen = () => {
    setCurStudent(null);
    setIsOpen(true);
  };
  const addStudent = (student: Student) => {
    setStudents([...students, student]);
    setTotal(total + 1);
  };
  const deleteStudent = (id: string) => {
    setStudents(students.filter((student) => student.id !== id));
    setTotal(total - 1);
  };
  const updateStudent = (student: Student) => {
    setStudents(students.map((s) => (s.id === student.id ? student : s)));
  };
  const getStudents = async (page: number, rowsPerPage: number) => {
    console.log(page, rowsPerPage);
    await setTimeout(() => {}, 1000);
    setStudents(arr.slice((page - 1) * rowsPerPage, page * rowsPerPage));
  }
  const searchStudent = async (query: string) => {
    console.log(query);
    await setTimeout(() => {}, 1000);
    setStudents(arr.filter((student) => student.id.includes(query) || student.name.includes
    (query)));
  };
  return (
    <Box sx={{ p: 3, maxWidth: "100%" }}>
      <Box
        sx={{
          display: "flex",
          justifyContent: "space-between",
          alignItems: "center",
          mb: 3,
        }}
      >
        <Typography variant="h5" fontWeight="bold">
          Danh sách sinh viên
        </Typography>
        <Box sx={{ display: "flex", gap: 1 }}>
          <Button
            variant="contained"
            color="success"
            startIcon={<AddIcon />}
            sx={{ borderRadius: "20px" }}
            onClick={handleClickOpen}
          >
            Thêm sinh viên
          </Button>
        </Box>
      </Box>
      <Grid
        container
        direction="row"
        sx={{
          justifyContent: "space-between",
          alignItems: "center",
        }}
      >
        <Paper
          sx={{
            p: 1,
            display: "flex",
            alignItems: "center",
            border: "1px solid #f0f7ff",
          }}
        >
          <Box
            sx={{
              bgcolor: "#f0f7ff",
              borderRadius: "50%",
              width: 50,
              height: 50,
              display: "flex",
              alignItems: "center",
              justifyContent: "center",
              mr: 2,
            }}
          >
            <PeopleIcon sx={{ color: "#1976d2", fontSize: 30 }} />
          </Box>
          <Box>
            <Typography variant="body2" color="text.secondary">
              Tổng số sinh viên
            </Typography>
            <Typography variant="h5" fontWeight="bold">
              {total}
            </Typography>
          </Box>
        </Paper>
        <Grid size={{ xs: 4, md: 2 }}>
          <SearchBar search= { searchStudent}/>
        </Grid>
      </Grid>
      <Grid container spacing={3} sx={{ mb: 3 }}>
        <Grid size={{ xs: 12, sm: 6, md: 2 }}></Grid>
      </Grid>
      <Table
        data={students}
        fieldCols={StudentCols}
        updateStudent={setCurStudent}
        setIsOpen={setIsOpen}
        deleteStudent={deleteStudent}
        getStudents={getStudents}
        total={total}
        page={page}
        setPage={setPage}
        rowsPerPage={rowsPerPage}
        setRowsPerPage={setRowsPerPage}
      />
      <Dialog
        isOpen={isOpen}
        student={curStudent}
        setStudent={setCurStudent}
        setIsOpen={setIsOpen}
        addStudent={addStudent}
        updateStudent={updateStudent}
      />
    </Box>
  );
}
