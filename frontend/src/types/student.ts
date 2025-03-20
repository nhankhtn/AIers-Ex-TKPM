import * as Yup from "yup";

export interface Student {
  id: string;
  name: string;
  dateOfBirth: string;
  gender: Gender;
  email: string;
  temporaryAddress?: string;
  permanentAddress: string;
  mailingAddress?: string;
  faculty: Faculty["id"];
  course: number;
  program: Program["id"];
  phone: string;
  status: Status["id"];
  identity: {
    type: number;
    documentNumber: string;
    issueDate: Date;
    issuePlace: string;
    expiryDate: Date;
    country: string;
    isChip: boolean;
    notes: string;
  };
}
export enum Gender {
  Male,
  Female,
  Other,
}

export interface Faculty {
  id: string;
  name: string;
}
export interface Program {
  id: string;
  name: string;
}

export interface Status {
  id: string;
  name: string;
}

export interface StudentFilter extends Partial<Student> {
  key: string;
  status_name: string;
  faculty_name: string;
}

export const mappingFiledStudent: Record<string, string> = {
  id: "Mã sinh viên",
  name: "Họ và tên",
  dateOfBirth: "Ngày sinh",
  gender: "Giới tính",
  email: "Email",
  address: "Địa chỉ",
  faculty: "Khoa",
  course: "Khóa",
  program: "Chương trình",
  phone: "Số điện thoại",
  status: "Trạng thái",
};

export const validationStudent = Yup.object().shape({
  name: Yup.string().required("Vui lòng nhập họ và tên"),
  dateOfBirth: Yup.string().required("Vui lòng nhập ngày tháng năm sinh"),
  email: Yup.string()
    .email("Email không hợp lệ")
    .required("Vui lòng nhập email"),

  // Permanent address validation
  permanentProvince: Yup.string().required("Vui lòng chọn tỉnh/thành phố"),
  permanentDistrict: Yup.string().required("Vui lòng chọn quận/huyện"),
  permanentWard: Yup.string().required("Vui lòng chọn phường/xã"),
  permanentDetail: Yup.string().required("Vui lòng nhập địa chỉ chi tiết"),

  // Temporary address validation (conditional)
  temporaryProvince: Yup.string().when("useTemporaryAddress", {
    is: true,
    then: (schema) => schema.required("Vui lòng chọn tỉnh/thành phố"),
  }),
  temporaryDistrict: Yup.string().when("useTemporaryAddress", {
    is: true,
    then: (schema) => schema.required("Vui lòng chọn quận/huyện"),
  }),
  temporaryWard: Yup.string().when("useTemporaryAddress", {
    is: true,
    then: (schema) => schema.required("Vui lòng chọn phường/xã"),
  }),
  temporaryDetail: Yup.string().when("useTemporaryAddress", {
    is: true,
    then: (schema) => schema.required("Vui lòng nhập địa chỉ chi tiết"),
  }),

  // Mailing address validation (conditional)
  mailingProvince: Yup.string().when("useMailingAddress", {
    is: true,
    then: (schema) => schema.required("Vui lòng chọn tỉnh/thành phố"),
  }),
  mailingDistrict: Yup.string().when("useMailingAddress", {
    is: true,
    then: (schema) => schema.required("Vui lòng chọn quận/huyện"),
  }),
  mailingWard: Yup.string().when("useMailingAddress", {
    is: true,
    then: (schema) => schema.required("Vui lòng chọn phường/xã"),
  }),
  mailingDetail: Yup.string().when("useMailingAddress", {
    is: true,
    then: (schema) => schema.required("Vui lòng nhập địa chỉ chi tiết"),
  }),

  // Academic info validation
  faculty: Yup.string().required("Vui lòng chọn khoa"),
  course: Yup.number()
    .required("Vui lòng nhập khóa học")
    .positive("Khóa học phải là số dương"),
  program: Yup.string().required("Vui lòng chọn chương trình"),
  phone: Yup.string().required("Vui lòng nhập số điện thoại"),
  status: Yup.string().required("Vui lòng chọn tình trạng sinh viên"),

  // Identity validation
  documentNumber: Yup.string().required("Vui lòng nhập số giấy tờ"),
  issueDate: Yup.string().required("Vui lòng nhập ngày cấp"),
  issuePlace: Yup.string().required("Vui lòng nhập nơi cấp"),
  expiryDate: Yup.string().required("Vui lòng nhập ngày hết hạn"),
});

// export const mockData: Student[] = [
//   {
//     id: "SV0231",
//     name: "Nguyễn Văn A",
//     dateOfBirth: "2000-01-01",
//     gender: Gender.Male,
//     email: "nguyenvana@example.com",
//     permanent_address: "123 Đường ABC, Quận 1, TP.HCM",
//     faculty: "Luật",
//     course: 2021,
//     program: "Cử nhân Luật",
//     phone: "0123456789",
//     status: "Đang học",
//   },
//   {
//     id: "SV002",
//     name: "Trần Thị B",
//     dateOfBirth: "1999-02-15",
//     gender: Gender.Female,
//     email: "tranthib@example.com",
//     permanent_address: "456 Đường DEF, Quận 2, TP.HCM",
//     faculty: {
//       id: "K002",
//       name: "Tiếng Anh thương mại",
//     },
//     course: 2020,
//     program: {
//       id: "P002",
//       name: "Cử nhân Tiếng Anh thương mại",
//     },
//     phone: "0987654321",
//     status: {
//       id: "graduated",
//       name: "Đã tốt nghiệp",
//     },
//   },
//   {
//     id: "SV003",
//     name: "Lê Văn C",
//     dateOfBirth: "2001-03-20",
//     gender: Gender.Male,
//     email: "levanc@example.com",
//     permanent_address: "789 Đường GHI, Quận 3, TP.HCM",
//     faculty: {
//       id: "K003",
//       name: "Tiếng Nhật",
//     },
//     course: 2022,
//     program: {
//       id: "P003",
//       name: "Cử nhân Tiếng Nhật",
//     },
//     phone: "0123987654",
//     status: {
//       id: "studying",
//       name: "Đang học",
//     },
//   },
//   {
//     id: "SV004",
//     name: "Phạm Thị D",
//     dateOfBirth: "1998-04-10",
//     gender: Gender.Female,
//     email: "phamthid@example.com",
//     permanent_address: "101 Đường JKL, Quận 4, TP.HCM",
//     faculty: {
//       id: "K004",
//       name: "Tiếng Pháp",
//     },
//     course: 2019,
//     program: {
//       id: "P004",
//       name: "Cử nhân Tiếng Pháp",
//     },
//     phone: "0987123456",
//     status: {
//       id: "paused",
//       name: "Tạm dừng",
//     },
//   },
//   {
//     id: "SV005",
//     name: "Hoàng Văn E",
//     dateOfBirth: "2002-05-25",
//     gender: Gender.Male,
//     email: "hoangvane@example.com",
//     permanent_address: "202 Đường MNO, Quận 5, TP.HCM",
//     faculty: {
//       id: "K001",
//       name: "Luật",
//     },
//     course: 2023,
//     program: {
//       id: "P001",
//       name: "Cử nhân Luật",
//     },
//     phone: "0123456780",
//     status: {
//       id: "droppedout",
//       name: "Đã thôi học",
//     },
//   },
//   {
//     id: "SV001",
//     name: "Nguyễn Văn A",
//     dateOfBirth: "2000-01-01",
//     gender: Gender.Male,
//     email: "nguyenvana@example.com",
//     permanent_address: "123 Đường ABC, Quận 1, TP.HCM",
//     faculty: {
//       id: "K001",
//       name: "Luật",
//     },
//     course: 2021,
//     program: {
//       id: "P001",
//       name: "Cử nhân Luật",
//     },
//     phone: "0123456789",
//     status: {
//       id: "studying",
//       name: "Đang học",
//     },
//   },
//   {
//     id: "SV002",
//     name: "Trần Thị B",
//     dateOfBirth: "1999-02-15",
//     gender: Gender.Female,
//     email: "tranthib@example.com",
//     permanent_address: "456 Đường DEF, Quận 2, TP.HCM",
//     faculty: {
//       id: "K002",
//       name: "Tiếng Anh thương mại",
//     },
//     course: 2020,
//     program: {
//       id: "P002",
//       name: "Cử nhân Tiếng Anh thương mại",
//     },
//     phone: "0987654321",
//     status: {
//       id: "graduated",
//       name: "Đã tốt nghiệp",
//     },
//   },
//   {
//     id: "SV003",
//     name: "Lê Văn C",
//     dateOfBirth: "2001-03-20",
//     gender: Gender.Male,
//     email: "levanc@example.com",
//     permanent_address: "789 Đường GHI, Quận 3, TP.HCM",
//     faculty: {
//       id: "K003",
//       name: "Tiếng Nhật",
//     },

//     course: 2022,
//     program: {
//       id: "P003",
//       name: "Cử nhân Tiếng Nhật",
//     },
//     phone: "0123987654",
//     status: {
//       id: "studying",
//       name: "Đang học",
//     },
//   },
//   {
//     id: "SV004",
//     name: "Phạm Thị D",
//     dateOfBirth: "1998-04-10",
//     gender: Gender.Female,
//     email: "phamthid@example.com",
//     permanent_address: "101 Đường JKL, Quận 4, TP.HCM",
//     faculty: {
//       id: "K004",
//       name: "Tiếng Pháp",
//     },
//     course: 2019,
//     program: {
//       id: "P004",
//       name: "Cử nhân Tiếng Pháp",
//     },
//     phone: "0987123456",
//     status: {
//       id: "paused",
//       name: "Tạm dừng",
//     },
//   },
//   {
//     id: "SV005",
//     name: "Hoàng Văn E",
//     dateOfBirth: "2002-05-25",
//     gender: Gender.Male,
//     email: "hoangvane@example.com",
//     permanent_address: "202 Đường MNO, Quận 5, TP.HCM",
//     faculty: {
//       id: "K001",
//       name: "Luật",
//     },
//     course: 2023,
//     program: {
//       id: "P001",
//       name: "Cử nhân Luật",
//     },
//     phone: "0123456780",
//     status: {
//       id: "droppedout",
//       name: "Đã thôi học",
//     },
//   },
// ];
