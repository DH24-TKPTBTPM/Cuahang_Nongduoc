# DATABASE – Chuyển từ Access (cuahang.dll) sang SQL Server  (BE1 phụ trách, QA2 kiểm tra)

File gốc: `Cuahang_Nongduoc/Cuahang Nongduoc/bin/Debug/cuahang.dll` (thực chất là file Microsoft Access .mdb đổi đuôi).

Các file cần tạo trong thư mục này:
1. `01_CreateDatabase.sql` – tạo CSDL `CuahangNongduoc`.
2. `02_CreateTables.sql` – tạo bảng + khóa chính/khóa ngoại (chuyển kiểu: Yes/No → BIT, Currency → BIGINT/DECIMAL, Date/Time → DATETIME, Text → NVARCHAR).
3. `03_SeedData.sql` – dữ liệu (xuất từ Access bằng SSMA for Access hoặc Import/Export Wizard).
4. `04_SampleData_Demo.sql` – dữ liệu mẫu cho buổi bảo vệ (QA2).

Chuỗi kết nối đặt trong `App.config` (không hardcode trong code):
`Data Source=.\SQLEXPRESS;Initial Catalog=CuahangNongduoc;Integrated Security=True`
