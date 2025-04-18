export interface Registration {
  id: number
  studentId: number
  classId: number
  registrationDate: string
  status: "active" | "cancelled"
}

export interface UnregisterClass {
  id: number
  classId: number
  courseName: number
  studentId: number
  studentName: string
  semester: number
  academicYear: number
  time: string
}

