import { Faculty } from "./student"

export interface Course {
  id: string
  courseId: string
  courseName: string
  credits: number
  facultyId: Faculty["id"]
  facultyName?: string
  description: string
  requiredCourseId?: string
  requiredCourseName?: string
  deletedAt?: string
  createdAt?: string
}

export interface CourseGrade {
  code: string
  name: string
  credits: number
  score: number
  grade: string
}

export type CourseFormData = {
  code: string
  name: string
  credits: number | string
  faculty: string
  description: string
  prerequisites: string
}

export interface TranscriptData {
  courses: CourseGrade[]
  gpa: number
  totalCredits: number
  completedCredits: number
}