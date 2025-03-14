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
  Typography,
} from "@mui/material";
import { Faculty, Gender, Status, Student } from "../../../types/student";

interface DialogProps {
  student: Student | null;
  isOpen: boolean;
  onClose: () => void;
  addStudent: (student: Student) => Promise<string | undefined>;
  updateStudent: (student: Student) => void;
}

function Dialog({
  student,
  isOpen,
  onClose,
  addStudent,
  updateStudent,
}: DialogProps) {
  const [err, setErr] = useState<string>("");
  const [formData, setFormData] = useState<Student>({
    id: "",
    name: "",
    dateOfBirth: "",
    gender: Gender.Man,
    email: "",
    address: "",
    faculty: Faculty.Law,
    course: 0,
    program: "",
    phone: "",
    status: Status.Studying,
  });

  useEffect(() => {
    if (student) setFormData(student);
    else {
      setFormData({
        id: "",
        name: "",
        dateOfBirth: "",
        gender: Gender.Man,
        email: "",
        address: "",
        faculty: Faculty.Law,
        course: 0,
        program: "",
        phone: "",
        status: Status.Studying,
      });
    }
  }, [student]);

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

  const handleSubmit = async () => {
    //Cần thực hiện kiểm tra tính hợp lệ đối với định dạng email, số điện thoại
    if (
      !formData.name ||
      !formData.dateOfBirth ||
      !formData.email ||
      !formData.address ||
      !formData.phone ||
      !formData.program ||
      !formData.course
    ) {
      console.log(formData);
      setErr("Vui lòng nhập đầy đủ thông tin");
      return;
    }
    //  @"^[a-zA-Z0-9.!#$%&'*+/=?^_`{|}~-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$"
    else if (
      !/^[a-zA-Z0-9.!#$%&'*+/=?^_`{|}~-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$/.test(
        formData.email
      )
    ) {
      // check if email
      setErr("Email không hợp lệ");
      return;
    } else if (!/((09|03|07|08|05)+([0-9]{8})\b)/g.test(formData.phone)) {
      setErr("Số điện thoại không hợp lệ");
      return;
    } else if (/^20\d{2}$/.test(formData.course.toString()) == false) {
      setErr("Khóa học không hợp lệ");
      return;
    }
    if (student) {
      if (student.email == formData.email) {
        setErr("Email đã tồn tại");
        return;
      }
      updateStudent(formData);
    } else {
      const res: string | undefined = await addStudent(formData);
      if (res) {
        setErr(res);
        return;
      }
    }
    setErr("");
    onClose();
  };

  return (
    <MuiDialog open={isOpen} onClose={onClose}>
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
          value={
            typeof formData.dateOfBirth === "string"
              ? formData.dateOfBirth.split("T")[0]
              : ""
          }
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
            value={formData.gender as unknown as string}
            onChange={handleChangeSelect}
          >
            <MenuItem value={Gender.Man}>Nam</MenuItem>
            <MenuItem value={Gender.Woman}>Nữ</MenuItem>
            <MenuItem value={Gender.Other}>Khác</MenuItem>
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
          onChange={handleChange}
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
            value={formData.faculty as unknown as string}
            onChange={handleChangeSelect}
          >
            <MenuItem value={Faculty.Law}>Khoa Luật</MenuItem>
            <MenuItem value={Faculty.BusinessEnglish}>
              Khoa Tiếng Anh thương mại
            </MenuItem>
            <MenuItem value={Faculty.Japanese}>Khoa Tiếng Nhật</MenuItem>
            <MenuItem value={Faculty.French}>Khoa Tiếng Pháp</MenuItem>
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
            value={formData.status as unknown as string}
          >
            <MenuItem value={Status.Studying}>Đang học</MenuItem>
            <MenuItem value={Status.Graduated}>Đã tốt nghiệp</MenuItem>
            <MenuItem value={Status.Droppedout}>Đã thôi học</MenuItem>
            <MenuItem value={Status.Paused}>Tạm dừng học</MenuItem>
          </Select>
        </FormControl>
        <Typography
          color="error"
          sx={{ marginLeft: "10px", marginTop: "10px" }}
        >
          {err}
        </Typography>
      </DialogContent>
      <DialogActions>
        <Button onClick={onClose} color="secondary" variant="contained">
          Hủy
        </Button>
        <Button onClick={handleSubmit} color="primary" variant="contained">
          {student ? "Cập nhật" : "Thêm"}
        </Button>
      </DialogActions>
    </MuiDialog>
  );
}

export default Dialog;
