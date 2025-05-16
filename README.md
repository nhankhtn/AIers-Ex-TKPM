# STUDENT MANAGEMENT

## CẤU TRÚC THƯ MỤC

### Backend:
![alt text](Images/image.png)
- `StudentManagement.API`: Project API cung cấp các endpoints để tương tác với hệ thống.
    - **`Controllers/`**: Chứa các controller định nghĩa các API endpoint.
    - **`appsettings.json`**: Tệp cấu hình chính của ứng dụng, lưu trữ thông tin về database, logging, các API bên thứ 3,...
    - **`Utils`**: Chứa class ApiResponse để chuẩn hóa phản hồi từ API
    - **`Program.cs`**: Entry point của ứng dụng API.
- `StudentManagement.BLL`: Project Business Logic Layer (BLL) chứa các logic nghiệp vụ.
    - **`DTOs/`**: Chứa các Data Transfer Object (DTO) để trao đổi dữ liệu giữa các tầng.
    - **`Services/`**: Chứa các service xử lý logic nghiệp vụ.
    - **`MappingProfile.cs`**: Cấu hình AutoMapper để ánh xạ dữ liệu giữa Enity và DTO.

- `StudentManagement.DAL`: Project Data Access Layer (DAL) để truy xuất và quản lý dữ liệu.
    - **`Data/`**: Chứa các ApplicationDbcontext, AuditInterceptor, utils và repositories để làm việc với database.
    - **`Migrations/`**: Chứa các tệp migration để cập nhật database schema.
- `StudentManagement.Domain`: Project chứa các định nghĩa thực thể và cấu trúc dữ liệu chung.
    - **`Enums/`**: Chứa các enum dùng chung trong hệ thống.
    - **`Models/`**: Chứa các model định nghĩa thực thể dữ liệu.
    - **`Utils/`**: Chứa các hàm tiện ích chung.
    - **`Attributes`**: Chứa class UniqueConstrainAttribute dùng để đánh dấu một thuộc tínhtrong model cần đảm bảo tính duy nhất.
- `StudentManagement.Tests`: Project chứa các unit tests cho ứng dụng.
    - **`Units/`**: Chứa các unit test của ứng dụng.
### Frontend:
![alt text](Images/image-1.png)
- **`public`**: chứa các image, icon,… của trang web
- **`api`**: định nghĩa các API sẽ dùng trong trang web
- **`app`**: định nghĩ giao diện chính cho tưng trang trong trang web
- **`components`**: chứa các thành phần tái sử dụng trong cả project
- **`constants`**: định nghĩa các hằng số
- **`context`**: định nghĩa cắc context để chia sẻ dữ liệu
- **`hooks`**: định nghĩa các custom hook trong project
- **`theme`**: định nghĩa các màu, text,.. trong project
- **`types`**: định nghĩa các type dùng chung trong project
- **`utils`**: các tiện ích tái sử dụng trong project
- **`.env`**: file môi trường (chứa API keys, biến môi trường)
- **`.gitignore`**: file bỏ qua khi đẩy lên Git
- **`eslint.config.mjs`**:cấu hình ESLint (kiểm tra lỗi code)
- **`next-env.d.ts`**: hỗ trợ TypeScript cho Next.js
- **`next.config.ts`**: cấu hình Next.js (ví dụ: rewrites, redirects)
- **`package.json`**: danh sách package, scripts
- **`package-lock.json`**: khóa phiên bản package (đảm bảo cài đúng)

## Các chức năng chính
### Màn hình xem, lọc, tìm kiếm sinh viên
![alt text](Images/dashboard.png)
### Các phím chức năng như thêm khoa, thêm chương trình, thêm trạng thái, thêm sinh viên mới, nhập/xuất file
![alt text](Images/task.png)
#### Giao diện thêm khoa
![alt text](Images/faculty.png)
#### Giao diện thêm chương trình
![alt text](Images/program.png)
#### Giao diện thêm trạng thái
![alt text](Images/status.png)
#### Giao diện import danh sách sinh viên
![alt text](Images/import.png)
#### Giao diện export danh sách sinh viên
![alt text](Images/export.png)
### Giao diện thêm/chỉnh sửa sinh viên 
![alt text](Images/add_1.png)
![alt text](Images/add_2.png)

## Hình ảnh minh chứng các chức năng của úng dụng
### Thêm logging mechanism để troubleshooting production issue & audit purposes
- Kế thừa từ SaveChangesInterceptor để can thiệp vào quá trình SaveChanges của Entity Framework, từ đó ghi lại lịch sử thay đổi dữ liệu (audit logging) khi thao tác với database, nếu có lỗi sẽ thông báo vào error message. Logs sẽ được ghi vào database.
![alt text](Images/image-2.png)
## Minh chứng các chức năng tag3.0 
### Thêm validate cho số điện thoại và email
![alt text](Images/add_student_validate_phone_tag3.0.jpg)
![alt text](Images/dialog_config_allowed_domain_email_tag3.0.jpg)
### Thêm trường thứ tự cho status để ràng buộc khi cập nhật status
![alt text](Images/dialog_settings_status_tag3.0.jpg)

## Minh chứng các chức năng tag v5.0
### 2.1.Quản Lý Khóa Học

#### Xem các khóa học
![Xem khóa học](Images/v5.0/courses.png)

#### Thêm khóa học mới
![Thêm khóa học](Images/v5.0/add_course.png)

#### Chỉnh sửa khóa học
![Chỉnh sửa khóa học](Images/v5.0/edit_course.png)

#### Xóa khóa học
![Xóa khóa học](Images/v5.0/delete_course.png)

#### Xem danh sách lớp học đã mở
![Xem lớp học](Images/v5.0/classes.png)

#### Mở lớp cho một khóa học
![Mở lớp học](Images/v5.0/add_class.png)

#### Chỉnh sửa lớp của khóa học
![Chỉnh sửa lớp](Images/v5.0/edit_class.png)

#### Xóa lớp của khóa học
![Xóa lớp học](Images/v5.0/delete_class.png)



### 2.2.Đăng Ký Khóa Học Cho Sinh Viên (Thủ Công Bởi Giáo Vụ)

#### Đăng ký các khóa học cho sinh viên
![Đăng ký khóa học](Images/v5.0/register_courses.png)

#### Xem, hủy các môn đã đăng ký
![Quản lý đăng ký](Images/v5.0/register_classes.png)

#### Xem lịch sử hủy đăng ký
![Lịch sử hủy](Images/v5.0/unregister_classes.png)

#### Nhập điểm số cho sinh viên
![Nhập điểm](Images/v5.0/edit_scores.png)
#### In bảng điểm cho sinh viên
##### Ảnh 1
![In bảng điểm (ảnh 1)](Images/v5.0/print_scores_1.png)
##### Ảnh 2
![In bảng điểm (ảnh 2)](Images/v5.0/print_scores_2.png)
##### Ảnh 3
![In bảng điểm (ảnh 3)](Images/v5.0/print_scores_3.png)


## Minh chứng các chức năng tag v6.0
#### Đa ngôn ngữ

#### Modal chỉnh sửa ngôn ngữ (bấm vào Settings bên SideBar)
![Modal chọn ngôn ngữ](Images/v6.0/modal_setting.png)
##### Trang khóa học (Tiếng Anh)
![Trang Khóa học (en)](Images/v6.0/course_page_en.png)
##### Trang khóa học (Tiếng Việt)
![Trang Khóa học (vi)](Images/v6.0/course_page_vi.png)
##### Trang thêm khóa học (Tiếng Việt)
###### Các trường như Tên khóa học, Mô tả sẽ có 2 loại Tiếng Anh và Tiếng Việt
![Thêm khóa học](Images/v6.0/add_course_vi.png)

#### Tạo UnitTest
##### Kết quả chạy các UnitTest
![UnitTest](Images/v6.0/unit_test.png)



## Hướng dẫn cài đặt & chạy chương trình
### Backend:

#### Cấu hình file `appsettings.json` trong thư mục `StudentManagement.API` với cấu hình database đã tạo

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost,1433;Database=StudentManagementDb;User Id=sa;Password=SqlServer@123;TrustServerCertificate=True;"
  }
}
```

#### Chạy các migrations
- Vào thư mục `Backend`
```sh
cd Backend
```
- Mở terminal và nhập lệnh sau để tạo table cho Database
:
```sh
dotnet ef database update -s ./StudentManagement.API -p ./StudentManagement.DAL
```
#### Chạy script `data.sql` trong thư mục `Backend/scripts`

#### Chạy Ctrl + F5 để Run Server
Server hoạt động ở </br>
`https://localhost:44324` với IIS Express </br>
`http://localhost:5231` với http </br>
`http://localhost:7143` với https

### Frontend:
Để cài đặt và khởi chạy giao diện frontend của dự án, hãy thực hiện lần lượt các lệnh sau:

```sh
cd frontend                # Di chuyển vào thư mục frontend
touch .env (Linux) hoặc New-Item -Path . -Name ".env" -ItemType "File" (Windows)       # Tạo file .env
Trong file .env, định nghĩa biến 'NEXT_PUBLIC_HOST=http://localhost:5231' để gọi API với http
npm install                # Cài đặt các dependencies cần thiết (thay thế bằng npm install --force hoặc yarn install nếu gặp lỗi)  
npm run build              # Biên dịch mã nguồn frontend
npm start                  # Khởi chạy ứng dụng
```
