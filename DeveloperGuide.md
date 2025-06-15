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

</br>

# 🧬 Database Schema

</br>

# ✏️ Updating an Existing Entity

</br>

# ♻️ Inversion of Control & Dependency Injection

</br>

# 🧪 Unit Testing

</br>




