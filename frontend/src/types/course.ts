import { Faculty } from "./student";

export interface Course {
  id: string;
  courseId: string;
  courseName: {
    vi: string;
    en: string;
  };
  credits: number;
  facultyId: Faculty["id"];
  facultyName?: {
    vi: string;
    en: string;
  };
  description: {
    vi: string;
    en: string;
  };
  requiredCourseId?: string;
  requiredCourseName?: {
    vi: string;
    en: string;
  };
  deletedAt?: string;
  createdAt?: string;
}

export interface CourseGrade {
  code: string;
  name: {
    vi: string;
    en: string;
  };
  credits: number;
  score: number;
  grade: string;
}

export type CourseFormData = {
  code: string;
  nameVi: string;
  nameEn: string;
  credits: number | string;
  faculty: string;
  descriptionVi: string;
  descriptionEn: string;
  prerequisites: string;
};

export interface TranscriptData {
  courses: CourseGrade[];
  gpa: number;
  totalCredits: number;
  completedCredits: number;
}
