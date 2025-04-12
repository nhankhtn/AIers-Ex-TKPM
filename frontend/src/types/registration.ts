export interface Registration {
  id: number
  studentId: number
  classId: number
  registrationDate: string
  status: "active" | "cancelled"
}