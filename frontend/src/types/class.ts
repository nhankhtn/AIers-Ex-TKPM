export interface Class {
  id: string;
  classId: string;
  courseId: string;
  courseName: {
    vi: string;
    en: string;
  };
  teacherName: string;
  academicYear: number;
  semester: number;
  startTime: number;
  endTime: number;
  room: string;
  maxStudents: number;
  deadline: string;
  dayOfWeek: number;
}
