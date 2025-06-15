# 🧑‍💻 Coding Standards

### Đặt tên

| Thành phần     | Quy ước       | Ví dụ                |
|----------------|----------------|-----------------------|
| Biến & hàm     | `camelCase`     | `studentId`, `getName()` |
| Class & File   | `PascalCase`    | `StudentService.cs`   |
| Interface      | `I` + Pascal    | `IStudentRepository`  |

### Tổ chức code

- Tách theo thư mục: `Controllers/Services/DTOs/...`
- Controller chỉ gọi service, **không chứa logic xử lý**

### Clean Code

- ❌ Không hardcode – dùng config hoặc constant
- ❌ Không để hàm dài quá 50 dòng
- ✅ Tên hàm thể hiện rõ ý định: `AddStudent`, `GetById`

### Xử lý lỗi

- Dùng `try-catch` ở tầng service
- Dùng `throw` kèm message rõ ràng
- API trả response chuẩn HTTP status + message

### Viết Test

- Ưu tiên test tầng service, business logic
- Dùng `xUnit`
- Không test trực tiếp database

</br>

# 🧠 Overview of Architecture

Ứng dụng chia làm 2 phần:

- **Frontend**: Next.js (TypeScript). 
- **Backend**: ASP.NET Core Web API (.NET 9).
- **Database**: SQL Server.

### Backend (ASP.NET Core Web API)
Triển khai **Clean Architecture**, </br>
API ──▶ Application (BLL) ──▶ Domain

- `Domain`: Entity + logic nghiệp vụ thuần.
- `BLL`: DTO, service, validation, interface.
- `DAL`: Triển khai repo, gọi API ngoài, email, file.
- `API`: Controller, middleware, DI, Swagger.


### Frontend

- Dùng **Next.js 14 App Router**
- i18n: next-intl
- Giao tiếp API qua REST (JSON)

### Database
- Dùng **SQL Server**
- ORM: **Entity Framework Core**

</br>

# 📁 Source code organization
### Backend

### Frontend

</br>

# 🚀 Getting Started with Your App Development
Tải Source Code: `git clone https://github.com/nhankhtn/AIers-Ex-TKPM.git`

### Setup Frontend
```sh
cd frontend                # Di chuyển vào thư mục frontend
touch .env (Linux) hoặc New-Item -Path . -Name ".env" -ItemType "File" (Windows)       # Tạo file .env
Trong file .env, định nghĩa biến 'NEXT_PUBLIC_HOST=http://localhost:5231' để gọi API với http
npm install                # Cài đặt các dependencies cần thiết (thay thế bằng npm install --force hoặc yarn install nếu gặp lỗi)  
npm run build              # Biên dịch mã nguồn frontend
npm start                  # Khởi chạy ứng dụng
```

#### Cấu hình file `appsettings.json` trong thư mục `StudentManagement.API` với cấu hình database đã tạo

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost,1433;Database=StudentManagementDb;User Id=sa;Password=SqlServer@123;TrustServerCertificate=True;"
  }
}
```
### Setup Backend
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

</br>

# 🧬 Database Schema

</br>

# ✏️ Updating an Existing Entity

</br>

# ♻️ Inversion of Control & Dependency Injection

</br>

# 🧪 Unit Testing

</br>




