export interface Class {
  id: number
  courseId: string
  teacherName: string
  academicYear: number
  semester: number
  startTime: number
  endTime: number
  room: string
  maxStudents: number
  deadline: string
}

export type ClassFormData = {
  id: number
  courseId:  string
  academicYear: string
  semester: string
  teacherName: string
  maxStudents: number | string
  startTime: number
  endTime: number
  room: string
  deadline: string
}