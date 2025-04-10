import { ClassForm } from "@/app/classes/_component/class-form"

export default function NewClassPage() {
  return (
    <div className="flex flex-col gap-6">
      <h1 className="text-3xl font-bold">Mở lớp học mới</h1>
      <ClassForm />
    </div>
  )
}