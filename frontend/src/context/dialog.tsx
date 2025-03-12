"use client";

import { Student } from "@/types/student";
import {
  createContext,
  useState,
  ReactNode,
  Dispatch,
  SetStateAction,
} from "react";

interface DialogContextType {
  isOpen: boolean;
  setIsOpen: Dispatch<SetStateAction<boolean>>;
  curStudent: Student | null;
  setCurStudent: Dispatch<SetStateAction<Student | null>>;
}

export const DialogContext = createContext<DialogContextType>({
  isOpen: false,
  setIsOpen: () => {},
  curStudent: null,
  setCurStudent: () => {},
});

const Provider = ({ children }: { children: ReactNode }) => {
  const [curStudent, setCurStudent] = useState<Student | null>(null);
  const [isOpen, setIsOpen] = useState(false);

  return (
    <DialogContext.Provider
      value={{ isOpen, setIsOpen, curStudent, setCurStudent }}
    >
      {children}
    </DialogContext.Provider>
  );
};

export default Provider;
