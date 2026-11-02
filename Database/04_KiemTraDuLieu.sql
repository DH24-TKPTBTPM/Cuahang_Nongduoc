-- =============================================================
-- 04 - Đối chiếu số dòng Access <-> SQL Server
-- Nguồn  : cuahang.dll (Access) -> SQL Server. Sinh bởi Database/tools/gen_sql.py
-- Việc   : SQL-02 (QA2 chạy để kiểm tra)
-- Tác giả: tuananh (Team Lead, làm thay BE1) | Hỗ trợ: Claude (AI) | Kiểm tra: QA2 – <Họ tên>
-- Ngày   : 02/11/2026
-- =============================================================

USE [CuahangNongduoc];
GO
SELECT t.Bang, t.SoDongAccess, t.SoDongSqlServer,
       CASE WHEN t.SoDongAccess = t.SoDongSqlServer THEN N'Khớp' ELSE N'LỆCH' END AS KetQua
FROM (
    SELECT N'THAM_SO' AS Bang, 1 AS SoDongAccess, (SELECT COUNT(*) FROM dbo.THAM_SO) AS SoDongSqlServer
    UNION ALL
    SELECT N'DON_VI_TINH' AS Bang, 3 AS SoDongAccess, (SELECT COUNT(*) FROM dbo.DON_VI_TINH) AS SoDongSqlServer
    UNION ALL
    SELECT N'LY_DO_CHI' AS Bang, 3 AS SoDongAccess, (SELECT COUNT(*) FROM dbo.LY_DO_CHI) AS SoDongSqlServer
    UNION ALL
    SELECT N'KHACH_HANG' AS Bang, 8 AS SoDongAccess, (SELECT COUNT(*) FROM dbo.KHACH_HANG) AS SoDongSqlServer
    UNION ALL
    SELECT N'DAI_LY' AS Bang, 0 AS SoDongAccess, (SELECT COUNT(*) FROM dbo.DAI_LY) AS SoDongSqlServer
    UNION ALL
    SELECT N'NHA_CUNG_CAP' AS Bang, 3 AS SoDongAccess, (SELECT COUNT(*) FROM dbo.NHA_CUNG_CAP) AS SoDongSqlServer
    UNION ALL
    SELECT N'SAN_PHAM' AS Bang, 4 AS SoDongAccess, (SELECT COUNT(*) FROM dbo.SAN_PHAM) AS SoDongSqlServer
    UNION ALL
    SELECT N'PHIEU_NHAP' AS Bang, 12 AS SoDongAccess, (SELECT COUNT(*) FROM dbo.PHIEU_NHAP) AS SoDongSqlServer
    UNION ALL
    SELECT N'MA_SAN_PHAM' AS Bang, 14 AS SoDongAccess, (SELECT COUNT(*) FROM dbo.MA_SAN_PHAM) AS SoDongSqlServer
    UNION ALL
    SELECT N'PHIEU_BAN' AS Bang, 5 AS SoDongAccess, (SELECT COUNT(*) FROM dbo.PHIEU_BAN) AS SoDongSqlServer
    UNION ALL
    SELECT N'CHI_TIET_PHIEU_BAN' AS Bang, 8 AS SoDongAccess, (SELECT COUNT(*) FROM dbo.CHI_TIET_PHIEU_BAN) AS SoDongSqlServer
    UNION ALL
    SELECT N'PHIEU_CHI' AS Bang, 2 AS SoDongAccess, (SELECT COUNT(*) FROM dbo.PHIEU_CHI) AS SoDongSqlServer
    UNION ALL
    SELECT N'PHIEU_THANH_TOAN' AS Bang, 2 AS SoDongAccess, (SELECT COUNT(*) FROM dbo.PHIEU_THANH_TOAN) AS SoDongSqlServer
    UNION ALL
    SELECT N'DU_NO_KH' AS Bang, 3 AS SoDongAccess, (SELECT COUNT(*) FROM dbo.DU_NO_KH) AS SoDongSqlServer
) t
ORDER BY t.Bang;
GO

-- Đối chiếu tổng tiền (phải bằng số trong Access)
SELECT N'PHIEU_BAN.TONG_TIEN' AS Cot, 846000 AS TongAccess, (SELECT ISNULL(SUM(CAST([TONG_TIEN] AS BIGINT)),0) FROM dbo.PHIEU_BAN) AS TongSqlServer
UNION ALL
SELECT N'PHIEU_NHAP.TONG_TIEN' AS Cot, 4697500 AS TongAccess, (SELECT ISNULL(SUM(CAST([TONG_TIEN] AS BIGINT)),0) FROM dbo.PHIEU_NHAP) AS TongSqlServer
UNION ALL
SELECT N'PHIEU_CHI.TONG_TIEN' AS Cot, 12000000 AS TongAccess, (SELECT ISNULL(SUM(CAST([TONG_TIEN] AS BIGINT)),0) FROM dbo.PHIEU_CHI) AS TongSqlServer
UNION ALL
SELECT N'PHIEU_THANH_TOAN.TONG_TIEN' AS Cot, 70000 AS TongAccess, (SELECT ISNULL(SUM(CAST([TONG_TIEN] AS BIGINT)),0) FROM dbo.PHIEU_THANH_TOAN) AS TongSqlServer
UNION ALL
SELECT N'CHI_TIET_PHIEU_BAN.THANH_TIEN' AS Cot, 846000 AS TongAccess, (SELECT ISNULL(SUM(CAST([THANH_TIEN] AS BIGINT)),0) FROM dbo.CHI_TIET_PHIEU_BAN) AS TongSqlServer
UNION ALL
SELECT N'SAN_PHAM.SO_LUONG' AS Cot, 270 AS TongAccess, (SELECT ISNULL(SUM(CAST([SO_LUONG] AS BIGINT)),0) FROM dbo.SAN_PHAM) AS TongSqlServer
UNION ALL
SELECT N'MA_SAN_PHAM.SO_LUONG' AS Cot, 261 AS TongAccess, (SELECT ISNULL(SUM(CAST([SO_LUONG] AS BIGINT)),0) FROM dbo.MA_SAN_PHAM) AS TongSqlServer;
GO
