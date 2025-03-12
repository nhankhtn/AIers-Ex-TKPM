"use client";
import React, { useState, useEffect } from "react";
import {
  Button,
  TextField,
  DialogActions,
  Dialog as MuiDialog,
  DialogContent,
  DialogTitle,
  Select,
  MenuItem,
  SelectChangeEvent,
  FormControl,
  InputLabel,
} from "@mui/material";
import { Student } from "../types/student";

interface DialogProps {
  student: Student | null;
  isOpen: boolean;
  setIsOpen: (isOpen: boolean) => void;
  setStudent: (student: Student | null) => void;
  addStudent: (student: Student) => void;
  updateStudent: (student: Student) => void;
}

function Dialog({ student, isOpen,setIsOpen, setStudent, addStudent, updateStudent }: DialogProps) {
  const [formData, setFormData] = useState<Student>({
    id: "",
    name: "",
    dateOfBirth: "",
    gender: "Nam",
    email: "",
    address: "",
    faculty: "Khoa Luật",
    course: 0,
    program: "",
    phone: "",
    status: "Đang học",
  });

  useEffect(() => {
    if (student) setFormData(student);
    else {
      setFormData({
        id: "",
        name: "",
        dateOfBirth: "",
        gender: "Nam",
        email: "",
        address: "",
        faculty: "Khoa Luật",
        course: 0,
        program: "",
        phone: "",
        status: "Đang học",
      });
    }
  }, [student]);

  const handleClose = () => {
    setIsOpen(false);
    setStudent(null);
  };

  const handleChange = (
    e: React.ChangeEvent<HTMLInputElement | { name?: string; value: unknown }>
  ) => {
    const { name, value } = e.target;
    setFormData((prevData) => ({
      ...prevData,
      [name as string]: value,
    }));
  };
  const handleChangeSelect = (event: SelectChangeEvent<string>) => {
    const { name, value } = event.target;
    setFormData((prevData) => ({
      ...prevData,
      [name as string]: value,
    }));
  };

  const handleSubmit = () => {
    if (student) {
      updateStudent(formData);
    } else {
      addStudent(formData);
    }
    handleClose();
  };

  return (
    <MuiDialog open={isOpen} onClose={handleClose}>
      <DialogTitle>
        {student ? "Cập nhật sinh viên" : "Thêm sinh viên"}
      </DialogTitle>
      <DialogContent>
        <TextField
          autoFocus
          margin="dense"
          id="name"
          name="name"
          label="Họ và tên"
          type="text"
          fullWidth
          variant="outlined"
          value={formData.name}
          onChange={handleChange}
        />
        <TextField
          margin="dense"
          id="dateOfBirth"
          name="dateOfBirth"
          label="Ngày tháng năm sinh"
          type="date"
          fullWidth
          variant="outlined"
          value={formData.dateOfBirth}
          onChange={handleChange}
        />
        <FormControl fullWidth>
          <InputLabel>Giới tính</InputLabel>
          <Select
            margin="dense"
            id="gender"
            name="gender"
            label="Giới tính"
            fullWidth
            variant="outlined"
            value={formData.gender}
            onChange={handleChangeSelect}
          >
            <MenuItem value="Nam">Nam</MenuItem>
            <MenuItem value="Nữ">Nữ</MenuItem>
            <MenuItem value="Khác">Khác</MenuItem>
          </Select>
        </FormControl>

        <TextField
          margin="dense"
          id="email"
          name="email"
          label="Email"
          type="email"
          fullWidth
          variant="outlined"
          value={formData.email}
          onChange={handleChange}
        />
        <TextField
          margin="dense"
          id="address"
          name="address"
          label="Địa chỉ"
          type="text"
          fullWidth
          variant="outlined"
          value={formData.address}
        />
        <FormControl fullWidth>
          <InputLabel>Khoa</InputLabel>
          <Select
            margin="dense"
            id="faculty"
            name="faculty"
            label="Khoa"
            fullWidth
            variant="outlined"
            value={formData.faculty}
            onChange={handleChangeSelect}
          >
            <MenuItem value="Khoa Luật">Khoa Luật</MenuItem>
            <MenuItem value="Khoa Tiếng Anh thương mại">
              Khoa Tiếng Anh thương mại
            </MenuItem>
            <MenuItem value="Khoa Tiếng Nhật">Khoa Tiếng Nhật</MenuItem>
            <MenuItem value="Khoa Tiếng Pháp">Khoa Tiếng Pháp</MenuItem>
          </Select>
        </FormControl>

        <TextField
          margin="dense"
          id="course"
          name="course"
          label="Khóa"
          type="number"
          fullWidth
          variant="outlined"
          value={formData.course}
          onChange={handleChange}
        />
        <TextField
          margin="dense"
          id="program"
          name="program"
          label="Chương trình"
          type="text"
          fullWidth
          variant="outlined"
          value={formData.program}
          onChange={handleChange}
        />
        <TextField
          margin="dense"
          id="phone"
          name="phone"
          label="Số điện thoại liên hệ"
          type="text"
          fullWidth
          variant="outlined"
          value={formData.phone}
          onChange={handleChange}
        />

        <FormControl fullWidth>
          <InputLabel>Tình trạng sinh viên</InputLabel>
          <Select
            margin="dense"
            id="status"
            name="status"
            label="Tình trạng sinh viên"
            fullWidth
            variant="outlined"
            onChange={handleChangeSelect}
            value={formData.status}
          >
            <MenuItem value="Đang học">Đang học</MenuItem>
            <MenuItem value="Đã tốt nghiệp">Đã tốt nghiệp</MenuItem>
            <MenuItem value="Đã thôi học">Đã thôi học</MenuItem>
            <MenuItem value="Tạm dừng học">Tạm dừng học</MenuItem>
          </Select>
        </FormControl>
      </DialogContent>
      <DialogActions>
        <Button onClick={handleClose} color="primary">
          Hủy
        </Button>
        <Button onClick={handleSubmit} color="primary">
          {student ? "Cập nhật" : "Thêm"}
        </Button>
      </DialogActions>
    </MuiDialog>
  );
}

export default Dialog;
