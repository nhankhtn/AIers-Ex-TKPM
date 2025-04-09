export interface Class {
  id: number
  code: string
  courseId: number
  courseCode: string
  courseName: string
  instructor: string
  year: string
  semester: number
  schedule: string
  room: string
  maxStudents: number
  enrolledStudents: number
}

export type ClassFormData = {
  code: string
  courseId: number | string
  year: string
  semester: string
  instructor: string
  maxStudents: number | string
  schedule: string
  room: string
}