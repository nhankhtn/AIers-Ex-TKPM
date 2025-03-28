# Bài tập 01:

## Yêu cầu: Viết một chương trình quản lý danh sách sinh viên với các chức năng sau:

1. **Thêm sinh viên mới**: Nhập thông tin của một sinh viên và lưu vào danh sách.
2. **Xóa sinh viên**: Xóa thông tin sinh viên dựa trên Mã số sinh viên (MSSV).
3. **Cập nhật thông tin sinh viên**: Cập nhật thông tin của sinh viên dựa trên MSSV.
4. **Tìm kiếm sinh viên**: Tìm kiếm sinh viên theo họ tên hoặc MSSV.

Thông tin cần quản lý bao gồm:

- Mã số sinh viên
- Họ tên
- Ngày tháng năm sinh
- Giới tính
- Khoa 
- Khóa 
- Chương trình 
- Địa chỉ
- Email
- Số điện thoại liên hệ
- Tình trạng sinh viên

Yêu cầu kỹ thuật:
- Chương trình có thể được viết bằng bất kỳ ngôn ngữ lập trình nào.
- Ứng dụng có thể là một ứng dụng console, desktop hoặc web.
- Cần thực hiện kiểm tra tính hợp lệ đối với định dạng email, số điện thoại, tên khoa, tình trạng sinh viên.

*Danh sách các khoa: Khoa Luật, Khoa Tiếng Anh thương mại, Khoa Tiếng Nhật, Khoa Tiếng Pháp*

*Các tình trạng sinh viên: Đang học, Đã tốt nghiệp, Đã thôi học, Tạm dừng học*

**Lưu ý: Chương trình cần đảm bảo tính đúng đắn và dễ kiểm thử.**

# Bài tập 02:

## Quản lý thêm một số thông tin SV như sau:

- Địa chỉ thường trú nếu có (thay thế cho Địa chỉ ở Bài tập 01) 
	[Số nhà, Tên đường], [Phường/Xã], [Quận/Huyện], [Tỉnh/Thành phố], [Quốc gia]
- Địa chỉ tạm trú (nếu có)
- Địa chỉ nhận thư 
- Giấy tờ chứng minh nhân thân của sinh viên. Một trong các loại giấy tờ sau đây
	- Chứng minh nhân dân (CMND): số CMND, ngày cấp, nơi cấp, ngày hết hạn 
	- Căn cước công dân (CCCD): số CCCD, ngày cấp, nơi cấp, ngày hết hạn, có gắn chip hay không
	- Hộ chiếu (passport): số hộ chiếu, ngày cấp, ngày hết hạn, nơi cấp, quốc gia cấp, ghi chú (nếu có) 
- Quốc tịch 

## Thêm chức năng:

- Cho phép đổi tên & thêm mới: khoa, tình trạng sinh viên, chương trình
- Thêm chức năng tìm kiếm:  tìm theo khoa, khoa + tên
- Hỗ trợ import/export dữ liệu: CSV, JSON, XML, Excel (chọn ít nhất 2)
- Thêm logging mechanism để troubleshooting production issue & audit purposes 

# Cách Commit Code

- **Mỗi commit phải gắn với một task/subtask cụ thể**.
- **Ví dụ:**
  ```bash
  git commit -m "Added faculty renaming feature"
  git commit -m "Implemented student search by faculty and name"
  git commit -m "Added CSV import/export functionality"
  git commit -m "Integrated logging for debugging and auditing"
  git commit -m "Fix search screen background color mismatch"
  ```
# Cập Nhật README.md & Tạo Tag v2.0

## **Bước 1: Cập Nhật README.md**

- Bổ sung hướng dẫn sử dụng **Version 2.0**.
- Thêm hình ảnh minh chứng các tính năng

## **Bước 2: Tạo Git Tag v2.0**

- Sau khi hoàn thành tất cả các tính năng:
  ```bash
  git tag v2.0
  git push origin v2.0
  ```

✅ **Đảm bảo tag được tạo và push lên GitHub trước thời hạn nộp bài.**

# Nộp File Lên Moodle  
Tạo file `GroupID_GroupName.txt` với nội dung sau:  

```
GitHub Repository: https://github.com/yourusername/GroupName-Ex-xxx.git

Những phần đã hoành thành, chưa hoàn thành:
- [Liệt kê nếu có]
```  
Sau đó, nộp file này lên Moodle.

# Bài tập 03

## Hoàn thành những phần còn thiếu sót của 2 bài tập trước (nếu có)

## The Broken Window Theory & The Boy Scout Rule

- Tìm hiểu báo cáo ngắn gọn 2-6 trang/slides (tổng cộng) về 2 chủ đề trên (include vào một trong những Github repo)
- Thực hiện code review and refactor to ensure the project follows consistent coding styles and standards
 
## Bổ sung Business Rules

01. MSSV phải là duy nhất  
   - Khi thêm hoặc cập nhật sinh viên, không được trùng MSSV với sinh viên khác.  

02. Email phải thuộc một tên miền nhất định và có thể cấu hình động (configurable) 
   - Ví dụ: Chỉ chấp nhận email có đuôi `@student.university.edu.vn`.  

03. Số điện thoại phải có định dạng hợp lệ theo quốc gia (configurable) 
   - Ví dụ: Việt Nam (`+84` hoặc `0[3|5|7|8|9]xxxxxxxx`).  		 

04. Tình trạng sinh viên chỉ có thể thay đổi theo một số quy tắc (configurable)
   - Ví dụ:  
     - `"Đang học"` → `"Bảo lưu"`, `"Tốt nghiệp"`, `"Đình chỉ"` (hợp lệ).  
     - `"Đã tốt nghiệp"` không thể quay lại `"Đang học"`.   
	 
## Nộp bài lên Github và Moodle tương tự bài tập 02 	 