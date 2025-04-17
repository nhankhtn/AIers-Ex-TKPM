"use client";

import StudentProvider from "@/context/student-student-context";
import Content from "./_sections/content";

export default function Page() {
  return (
    <StudentProvider>
      <Content />
    </StudentProvider>
  );
}
