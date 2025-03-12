export interface Student {
  id: string;
  name: string;
  dateOfBirth: string | Date;
  gender: "Nam" | "Nữ" | "Khác";
  email: string;
  address: string;
  faculty:
    | "Khoa Luật"
    | "Khoa Tiếng Anh thương mại"
    | "Khoa Tiếng Nhật"
    | "Khoa Tiếng Pháp";
  course: number;
  program: string;
  phone: string;
  status: "Đang học" | "Đã tốt nghiệp" | "Đã thôi học" | "Tạm dừng học";
  [key: string]: any; // Add this line
}