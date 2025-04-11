import {
  Button,
  Dialog,
  DialogActions,
  DialogContent,
  DialogTitle,
  IconButton,
  Stack,
  Typography,
} from "@mui/material";
import CloseIcon from "@mui/icons-material/Close";
import RowStack from "@/components/row-stack";
import { Course } from "@/types/course";

interface DialogConfirmDeleteCourseProps {
  open: boolean;
  onClose: () => void;
  onConfirm: () => void;
  data: Course;
}

const DialogConfirmDeleteCourse = ({
  open,
  onClose,
  onConfirm,
  data,
}: DialogConfirmDeleteCourseProps) => {
  return (
    <Dialog
      open={open}
      onClose={onClose}
      sx={{
        "& .MuiDialog-paper": {
          width: {
            lg: 491,
          },
        },
      }}
    >
      <DialogTitle sx={{ pb: 1 }}>
        <RowStack justifyContent={"space-between"}>
          <Typography variant={"h6"}>Xoá khóa học</Typography>
          <IconButton
            onClick={onClose}
            disableRipple
            sx={{
              width: "40px",
              height: "40px",
            }}
          >
            <CloseIcon />
          </IconButton>
        </RowStack>
      </DialogTitle>
      <DialogContent sx={{ px: 3, py: 0.5 }}>
        <Stack gap={"10px"}>
          <Typography variant={"body1"}>
            Bạn có chắc chắn muốn xoá khóa học{" "}
            <Typography
              variant={"subtitle1"}
              fontWeight={600}
              component={"span"}
            >
              &quot;{data.courseName}&quot;?{" "}
            </Typography>
          </Typography>
        </Stack>
      </DialogContent>
      <DialogActions>
        <RowStack justifyContent={"flex-end"} gap={1}>
          <Button onClick={onClose} variant="contained" color="secondary">
            Huỷ
          </Button>
          <Button
            onClick={() => {
              onConfirm();
              onClose();
            }}
            variant="contained"
            color="error"
          >
            Xoá
          </Button>
        </RowStack>
      </DialogActions>
    </Dialog>
  );
};

export default DialogConfirmDeleteCourse;
