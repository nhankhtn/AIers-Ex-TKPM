export interface Student {
  id: string;
  name: string;
  dateOfBirth: string;
  gender: Gender;
  email: string;
  address: string;
  faculty: Faculty;
  course: number;
  program: string;
  phone: string;
  status: Status;
  [key: string]: any;
}
export enum Gender {
  Male,
  Female,
  Other,
}
export enum Faculty {
  Law,
  BusinessEnglish,
  Japanese,
  French,
}
export enum Status {
  Studying,
  Graduated,
  Droppedout,
  Paused,
}

export interface StudentFilter extends Partial<Student> {
  key: string;
}

export const mockData: Student[] = [
  {
    id: "SV0231",
    name: "Nguyễn Văn A",
    dateOfBirth: "2000-01-01",
    gender: Gender.Male,
    email: "nguyenvana@example.com",
    address: "123 Đường ABC, Quận 1, TP.HCM",
    faculty: Faculty.Law,
    course: 2021,
    program: "Cử nhân Luật",
    phone: "0123456789",
    status: Status.Studying,
  },
  {
    id: "SV002",
    name: "Trần Thị B",
    dateOfBirth: "1999-02-15",
    gender: Gender.Female,
    email: "tranthib@example.com",
    address: "456 Đường DEF, Quận 2, TP.HCM",
    faculty: Faculty.BusinessEnglish,
    course: 2020,
    program: "Cử nhân Tiếng Anh thương mại",
    phone: "0987654321",
    status: Status.Graduated,
  },
  {
    id: "SV003",
    name: "Lê Văn C",
    dateOfBirth: "2001-03-20",
    gender: Gender.Male,
    email: "levanc@example.com",
    address: "789 Đường GHI, Quận 3, TP.HCM",
    faculty: Faculty.Japanese,
    course: 2022,
    program: "Cử nhân Tiếng Nhật",
    phone: "0123987654",
    status: Status.Studying,
  },
  {
    id: "SV004",
    name: "Phạm Thị D",
    dateOfBirth: "1998-04-10",
    gender: Gender.Female,
    email: "phamthid@example.com",
    address: "101 Đường JKL, Quận 4, TP.HCM",
    faculty: Faculty.French,
    course: 2019,
    program: "Cử nhân Tiếng Pháp",
    phone: "0987123456",
    status: Status.Paused,
  },
  {
    id: "SV005",
    name: "Hoàng Văn E",
    dateOfBirth: "2002-05-25",
    gender: Gender.Male,
    email: "hoangvane@example.com",
    address: "202 Đường MNO, Quận 5, TP.HCM",
    faculty: Faculty.Law,
    course: 2023,
    program: "Cử nhân Luật",
    phone: "0123456780",
    status: Status.Droppedout,
  },
  {
    id: "SV001",
    name: "Nguyễn Văn A",
    dateOfBirth: "2000-01-01",
    gender: Gender.Male,
    email: "nguyenvana@example.com",
    address: "123 Đường ABC, Quận 1, TP.HCM",
    faculty: Faculty.Law,
    course: 2021,
    program: "Cử nhân Luật",
    phone: "0123456789",
    status: Status.Studying,
  },
  {
    id: "SV002",
    name: "Trần Thị B",
    dateOfBirth: "1999-02-15",
    gender: Gender.Female,
    email: "tranthib@example.com",
    address: "456 Đường DEF, Quận 2, TP.HCM",
    faculty: Faculty.BusinessEnglish,
    course: 2020,
    program: "Cử nhân Tiếng Anh thương mại",
    phone: "0987654321",
    status: Status.Graduated,
  },
  {
    id: "SV003",
    name: "Lê Văn C",
    dateOfBirth: "2001-03-20",
    gender: Gender.Male,
    email: "levanc@example.com",
    address: "789 Đường GHI, Quận 3, TP.HCM",
    faculty: Faculty.Japanese,
    course: 2022,
    program: "Cử nhân Tiếng Nhật",
    phone: "0123987654",
    status: Status.Studying,
  },
  {
    id: "SV004",
    name: "Phạm Thị D",
    dateOfBirth: "1998-04-10",
    gender: Gender.Female,
    email: "phamthid@example.com",
    address: "101 Đường JKL, Quận 4, TP.HCM",
    faculty: Faculty.French,
    course: 2019,
    program: "Cử nhân Tiếng Pháp",
    phone: "0987123456",
    status: Status.Paused,
  },
  {
    id: "SV005",
    name: "Hoàng Văn E",
    dateOfBirth: "2002-05-25",
    gender: Gender.Male,
    email: "hoangvane@example.com",
    address: "202 Đường MNO, Quận 5, TP.HCM",
    faculty: Faculty.Law,
    course: 2023,
    program: "Cử nhân Luật",
    phone: "0123456780",
    status: Status.Droppedout,
  },
];
