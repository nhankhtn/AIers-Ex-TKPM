# STUDENT MANAGEMENT

## Mô tả
1. **Thêm sinh viên mới**: Nhập thông tin của một sinh viên và lưu vào danh sách.
2. **Xóa sinh viên**: Xóa thông tin sinh viên dựa trên Mã số sinh viên (MSSV).
3. **Cập nhật thông tin sinh viên**: Cập nhật thông tin của sinh viên dựa trên MSSV.
4. **Tìm kiếm sinh viên**: Tìm kiếm sinh viên theo họ tên hoặc MSSV.


--- 

## Backend (ASP.NET Core Web API)

### Cài đặt Server
#### Tạo 1 Database

#### Cấu hình file `appsettings.json` với cấu hình database đã tạo

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost,1433;Database=StudentManagementDb;User Id=sa;Password=SqlServer@123;TrustServerCertificate=True;"
  }
}
```

#### Vào đúng thư mục chứa dự án StudentManagement
Chạy terminal và nhập lệnh:</br>
`dotnet ef database update -s ../StudentManagement.API -p ../StudentManagement.DAL`

để tạo table cho Database


#### Chạy Ctrl + F5 để Run Server
Server hoạt động ở </br>
`https://localhost:44324` với IIS Express </br>
`http://localhost:5231` với http </br>
`http://localhost:7143` với https


---
### Nuget Packages
- `Microsoft.EntityFrameworkCore.Design` - 9.0.2
- `AutoMapper` - 14.0.0
- `Microsoft.EntityFrameworkCore` - 9.0.2
- `Microsoft.EntityFrameworkCore.Design` - 9.0.2
- `Microsoft.EntityFrameworkCore.SqlServer` - 9.0.2
- `Microsoft.EntityFrameworkCore.Tools` - 9.0.2











