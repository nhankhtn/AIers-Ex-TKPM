import { Gender, mappingFiledStudent, mappingGender, Student } from "@/types/student";
import { exportToCSV, exportToExcel, exportToPDF, importFromCSV, importFromExcel } from "./export-helper";
import { isJSONString } from "./string-helper";
import { objectToAddress } from "@/app/dashboard/_sections/table-config";


export async function importFiles(file: File) {
  let studentsImported = null;

  if (file.type.includes("csv")) {
    const data = await importFromCSV(file);
    let identity = {};
    studentsImported = data.map((item) => {
      const mappedStudent: Record<string, any> = {};

      Object.entries(mappingFiledStudent).forEach(([key, value]) => {
        if (
          key === "temporaryAddress" ||
          key === "mailingAddress" ||
          key === "permanentAddress"
        ) {
          mappedStudent[key] = parseAddress(item[value]);
          return;
        }
        if (key === "identity") {
          mappedStudent[key] = identity;
          return;
        }
        if (
          key === "type" ||
          key === "documentNumber" ||
          key === "issueDate" ||
          key === "issuePlace" ||
          key === "expiryDate" ||
          key === "country" ||
          key === "isChip" ||
          key === "notes"
        ) {
          identity = {
            ...identity,
            [key]: item[value],
          };
          return;
        }
        mappedStudent[key] = item[value];
      });

      return mappedStudent;
    });
  } else if (
    file.type ===
      "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" ||
    file.type === "application/vnd.ms-excel"
  ) {
    const data = await importFromExcel(file);

    studentsImported = data.map((item) => {
      const mappedStudent: Record<string, any> = {};
      Object.entries(mappingFiledStudent).forEach(([key, value]) => {
        mappedStudent[key] = item[value];
      });
      return mappedStudent;
    });
  }
  if (!studentsImported || studentsImported.length === 0) {
    return [];
  }
  const studentArr = studentsImported.map((student) => {
    const mappedStudent: Student = {
      id: student.id || "",
      name: student.name || "",
      dateOfBirth: student.dateOfBirth || "",
      gender: student.gender,
      email: student.email || "",
      temporaryAddress: student.temporaryAddress || undefined,
      permanentAddress: student.permanentAddress || "",
      mailingAddress: student.mailingAddress || undefined,
      faculty: student.faculty || "",
      course: Number(student.course) || 0,
      program: student.program || "",
      phone: student.phone || "",
      status: student.status || "",
      identity: {
        type: student.type || 0,
        documentNumber: student.documentNumber || "",
        issueDate: student.issueDate ,
        issuePlace: student.issuePlace || "",
        expiryDate: student.expiryDate,
        countryIssue: student.countryIssue || "",
        isChip: Boolean(student.isChip),
        notes: student.notes || "",
      },
      nationality: student.nationality || "",
    };
    return mappedStudent;
  });
  return studentArr;
}

export function exportFiles(format: string, students: Student[], rows: number) {
  const data = students.slice(0, rows).map((student) => {
    const mappedStudent: Record<string, any> = {};
    Object.entries(mappingFiledStudent).forEach(([key, value]) => {
      const typedKey = key as keyof Student;
      if (typeof student[typedKey] === "object") {
        Object.entries(student[typedKey]).forEach(([subKey, subValue]) => {
          if (
            subValue === "" ||
            subValue === undefined ||
            subValue === null
          )
            return;
          // if (subKey === "issueDate" || subKey === "expiryDate") {
          //   if (typeof subValue === "string" || typeof subValue === "number" || subValue instanceof Date) {
          //     mappedStudent[mappingFiledStudent[subKey]] = new Date(subValue).toLocaleDateString(
          //       "vi-VN"
          //     );
          //   }
          //   return;
          // }
          mappedStudent[mappingFiledStudent[subKey]] = subValue;
        });
      }
      if (isJSONString(student[typedKey] as string)) {
        mappedStudent[value] = objectToAddress(
          JSON.parse(student[typedKey] as string)
        );
      } else if (typedKey === "gender") {
        mappedStudent[value] = mappingGender[student[typedKey] as Gender];
      } else {
        mappedStudent[value] = student[typedKey];
      }
    });
    return mappedStudent;
  });
  switch (format) {
    case "csv": {
      exportToCSV(data, "students");
      break;
    }
    case "excel": {
      exportToExcel(data, "students");
      break;
    }
    case "pdf": {
      exportToPDF(data, "students");
    }
    default:
      break;
  }
}

function parseAddress(address: string) {
  const parts = address.split(",").map((part) => part.trim());
  const len = parts.length;
  return {
    detail: parts.slice(0, len - 4).join(", "),
    ward: parts[len - 4] || "",
    district: parts[len - 3] || "",
    provinces: parts[len - 2] || "",
    contry: parts[len - 1] || "",
  };
}