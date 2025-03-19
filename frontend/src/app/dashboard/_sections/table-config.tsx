import { CustomTableConfig } from "@/components/custom-table";
import { Student } from "@/types/student";
import { Typography } from "@mui/material";

export const getTableConfig = (): CustomTableConfig<
  Student["id"],
  Student
>[] => [
  {
    key: "mssv",
    headerLabel: "Mã số sinh viên",
    type: "string",
    headerCellProps: {
      sx: {
        position: "sticky",
        backgroundColor: "white",
      },
    },
    renderCell: (data) => <Typography variant='body2'>{data.id}</Typography>,
  },
  {
    key: "name",
    headerLabel: "Họ và tên",
    type: "string",
    headerCellProps: {
      sx: {
        position: "sticky",
        backgroundColor: "white",
      },
    },
    renderCell: (data) => <Typography variant='body2'>{data.name}</Typography>,
  },
  {
    key: "dob",
    headerLabel: "Ngày sinh",
    type: "string",
    renderCell: (data) => (
      <Typography variant='body2'>{data.dateOfBirth}</Typography>
    ),
  },
  {
    key: "gender",
    headerLabel: "Giới tính",
    type: "string",
    renderCell: (data) => (
      <Typography variant='body2'>{data.gender}</Typography>
    ),
  },
  {
    key: "email",
    headerLabel: "Email",
    type: "string",
    renderCell: (data) => <Typography variant='body2'>{data.email}</Typography>,
  },
  {
    key: "phone",
    headerLabel: "Số điện thoại",
    type: "string",
    renderCell: (data) => <Typography variant='body2'>{data.phone}</Typography>,
  },
  {
    key: "permanent_address",
    headerLabel: "Địa chị thường trú",
    type: "string",
    renderCell: (data) => (
      <Typography variant='body2'>{data.permanent_address}</Typography>
    ),
  },
  {
    key: "temporary_address",
    headerLabel: "Địa chị tạm trú",
    type: "string",
    renderCell: (data) => (
      <Typography variant='body2'>
        {data.temporary_address || "Trống"}
      </Typography>
    ),
  },
  {
    key: "faculty",
    headerLabel: "Khoa",
    type: "string",
    renderCell: (data) => (
      <Typography variant='body2'>{data.faculty}</Typography>
    ),
  },
  {
    key: "course",
    headerLabel: "Khoá",
    type: "string",
    renderCell: (data) => (
      <Typography variant='body2'>{data.course}</Typography>
    ),
  },
  {
    key: "program",
    headerLabel: "Chương trình",
    type: "string",
    renderCell: (data) => (
      <Typography variant='body2'>{data.program}</Typography>
    ),
  },
  {
    key: "status",
    headerLabel: "Trạng thái",
    type: "string",
    renderCell: (data) => (
      <Typography variant='body2'>{data.status}</Typography>
    ),
  },
];
