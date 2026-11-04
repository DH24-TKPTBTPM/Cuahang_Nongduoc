-- =============================================================
-- 01 - Tạo CSDL
-- Nguồn  : cuahang.dll (Access) -> SQL Server. Sinh bởi Database/tools/gen_sql.py
-- Việc   : SQL-01
-- Tác giả: tuananh (Team Lead, làm thay BE1) | Hỗ trợ: Claude (AI) | Kiểm tra: QA2 – Dư Thị Trúc Hòa
-- Ngày   : 02/11/2026 | Ngày kiểm thử : 03/11/2026
-- =============================================================

USE master;
GO
IF DB_ID(N'CuahangNongduoc') IS NOT NULL
BEGIN
    ALTER DATABASE [CuahangNongduoc] SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    DROP DATABASE [CuahangNongduoc];
END
GO
-- Vietnamese_CI_AS: sắp xếp/so sánh tiếng Việt đúng, không phân biệt hoa thường
CREATE DATABASE [CuahangNongduoc] COLLATE Vietnamese_CI_AS;
GO
