import { SelectChangeEvent } from "@mui/material";
import { useCallback, useState } from "react";

interface UsePaginationProps {
  count: number;
  initialRowsPerPage?: number;
}

export interface UsePaginationResult {
  count: number;
  page: number;
  rowsPerPage: number;
  totalPages: number;
  onPageChange: (
    event: React.MouseEvent<HTMLButtonElement> | null,
    newPage: number
  ) => void;
  onRowsPerPageChange: (
    event:
      | React.ChangeEvent<HTMLInputElement | HTMLTextAreaElement>
      | number
      | SelectChangeEvent
  ) => void;
  setTotal: (newCount: number) => void;
}

function usePagination({
  count: initCount,
  initialRowsPerPage,
}: UsePaginationProps): UsePaginationResult {
  const [page, setPage] = useState(0);
  const [count, setCount] = useState(initCount);
  const [rowsPerPage, setRowsPerPage] = useState(initialRowsPerPage || 10);

  const totalPages = Math.ceil(count / rowsPerPage);

  const onPageChange = useCallback(
    (event: React.MouseEvent<HTMLButtonElement> | null, newPage: number) => {
      setPage(newPage);
    },
    []
  );

  const onRowsPerPageChange = useCallback(
    (
      event:
        | React.ChangeEvent<HTMLInputElement | HTMLTextAreaElement>
        | number
        | SelectChangeEvent
    ) => {
      setRowsPerPage(
        typeof event == "number" ? event : parseInt(event.target.value, 10)
      );
      setPage(0);
    },
    []
  );

  const setTotal = useCallback(
    (newCount: number) => {
      setCount(newCount);
    },
    [setCount]
  );

  return {
    count,
    page,
    rowsPerPage,
    totalPages,
    onPageChange,
    onRowsPerPageChange,
    setTotal,
  };
}

export default usePagination;
