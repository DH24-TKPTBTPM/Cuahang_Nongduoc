-- =============================================================
-- 05 - Dữ liệu demo cho buổi bảo vệ (chạy SAU 01 → 04, có thể chạy lại nhiều lần)
-- Việc   : QA-08 (QA2 phụ trách, BE1 hỗ trợ dữ liệu)
-- Nội dung: 1 nhà cung cấp, 1 khách lẻ, 1 đại lý, 3 sản phẩm, 1 phiếu nhập 3 lô hàng
--          (1 lô sắp hết hạn để demo báo cáo), 1 phiếu bán sỉ tháng trước còn nợ (demo công nợ).
-- Quy ước: mọi mã đều bắt đầu bằng 'DEMO-' → chạy lại sẽ xóa bản cũ rồi nạp lại;
--          KHÔNG đụng dữ liệu thật và bảng THAM_SO (mã phiếu tự tăng giữ nguyên).
-- Tác giả: BE1 – Nguyễn Tuấn Anh | Hỗ trợ: Claude (AI) | Kiểm tra: QA2 – <Họ tên>
-- Ngày   : 15/11/2026
-- =============================================================

USE [CuahangNongduoc];
GO
SET NOCOUNT ON;
SET XACT_ABORT ON;
BEGIN TRANSACTION;

-- 1. Xóa dữ liệu demo cũ (theo thứ tự khóa ngoại)
DELETE FROM dbo.CHI_TIET_PHIEU_BAN WHERE ID_PHIEU_BAN LIKE 'DEMO-%' OR ID_MA_SAN_PHAM LIKE 'DEMO-%';
DELETE FROM dbo.PHIEU_THANH_TOAN   WHERE ID LIKE 'DEMO-%' OR ID_KHACH_HANG LIKE 'DEMO-%';
DELETE FROM dbo.DU_NO_KH           WHERE ID_KHACH_HANG LIKE 'DEMO-%';
DELETE FROM dbo.PHIEU_BAN          WHERE ID LIKE 'DEMO-%' OR ID_KHACH_HANG LIKE 'DEMO-%';
DELETE FROM dbo.MA_SAN_PHAM        WHERE ID LIKE 'DEMO-%' OR ID_SAN_PHAM LIKE 'DEMO-%';
DELETE FROM dbo.PHIEU_NHAP         WHERE ID LIKE 'DEMO-%';
DELETE FROM dbo.SAN_PHAM           WHERE ID LIKE 'DEMO-%';
DELETE FROM dbo.KHACH_HANG         WHERE ID LIKE 'DEMO-%';
DELETE FROM dbo.NHA_CUNG_CAP       WHERE ID LIKE 'DEMO-%';

DECLARE @HomNay DATETIME = CAST(GETDATE() AS DATE);
DECLARE @DVT INT = (SELECT MIN(ID) FROM dbo.DON_VI_TINH);

-- 2. Danh mục
INSERT INTO dbo.NHA_CUNG_CAP (ID, HO_TEN, DIA_CHI, DIEN_THOAI) VALUES
 ('DEMO-NCC1', N'Cty Vật tư Nông nghiệp Miền Tây', N'Cần Thơ', '0292 3888 999');

INSERT INTO dbo.KHACH_HANG (ID, HO_TEN, DIA_CHI, DIEN_THOAI, LOAI_KH) VALUES
 ('DEMO-KL1', N'Nguyễn Văn An',              N'Long Xuyên, An Giang', '0909 111 222', 0),  -- khách lẻ
 ('DEMO-DL1', N'Đại lý Phân bón Hòa Bình',   N'Châu Đốc, An Giang',   '0909 333 444', 1);  -- đại lý (bán sỉ)

-- SO_LUONG / DON_GIA_NHAP khớp với các lô ở bước 3–4
INSERT INTO dbo.SAN_PHAM (ID, TEN_SAN_PHAM, SO_LUONG, DON_GIA_NHAP, GIA_BAN_SI, GIA_BAN_LE, ID_DON_VI_TINH) VALUES
 ('DEMO-SP1', N'Thuốc trừ sâu Regent 800WG', 50, 20000, 24000, 27000, @DVT),
 ('DEMO-SP2', N'Phân bón NPK 16-16-8',        90, 15000, 18000, 20000, @DVT),
 ('DEMO-SP3', N'Thuốc diệt cỏ Glyphosate',    30, 45000, 52000, 58000, @DVT);

-- 3. Phiếu nhập 10 ngày trước: 3 lô hàng (lô SP3 còn 20 ngày là hết hạn)
INSERT INTO dbo.PHIEU_NHAP (ID, NGAY_NHAP, TONG_TIEN, DA_TRA, CON_NO, ID_NHA_CUNG_CAP) VALUES
 ('DEMO-PN1', DATEADD(DAY, -10, @HomNay), 3850000, 3000000, 850000, 'DEMO-NCC1');

INSERT INTO dbo.MA_SAN_PHAM (ID, ID_SAN_PHAM, ID_PHIEU_NHAP, DON_GIA_NHAP, SO_LUONG, NGAY_NHAP, NGAY_SAN_XUAT, NGAY_HET_HAN) VALUES
 ('DEMO-SP1-L1', 'DEMO-SP1', 'DEMO-PN1', 20000,  50, DATEADD(DAY, -10, @HomNay), DATEADD(MONTH, -2, @HomNay), DATEADD(YEAR, 1, @HomNay)),
 ('DEMO-SP2-L1', 'DEMO-SP2', 'DEMO-PN1', 15000, 100, DATEADD(DAY, -10, @HomNay), DATEADD(MONTH, -1, @HomNay), DATEADD(YEAR, 2, @HomNay)),
 ('DEMO-SP3-L1', 'DEMO-SP3', 'DEMO-PN1', 45000,  30, DATEADD(DAY, -10, @HomNay), DATEADD(YEAR, -2, @HomNay),  DATEADD(DAY, 20, @HomNay));

-- 4. Phiếu bán sỉ tháng trước cho đại lý: 10 bao NPK, trả 100.000 → còn nợ 80.000
DECLARE @ThangTruoc DATETIME = DATEADD(MONTH, -1, @HomNay);
INSERT INTO dbo.PHIEU_BAN (ID, ID_KHACH_HANG, NGAY_BAN, TONG_TIEN, DA_TRA, CON_NO) VALUES
 ('DEMO-PB1', 'DEMO-DL1', @ThangTruoc, 180000, 100000, 80000);
INSERT INTO dbo.CHI_TIET_PHIEU_BAN (ID_PHIEU_BAN, ID_MA_SAN_PHAM, SO_LUONG, DON_GIA, THANH_TIEN) VALUES
 ('DEMO-PB1', 'DEMO-SP2-L1', 10, 18000, 180000);
UPDATE dbo.MA_SAN_PHAM SET SO_LUONG = SO_LUONG - 10 WHERE ID = 'DEMO-SP2-L1';   -- lô còn 90 = SAN_PHAM.SO_LUONG

COMMIT TRANSACTION;
GO

-- 5. Kiểm tra nhanh (phải ra đúng các số ghi bên phải)
SELECT N'Sản phẩm demo'       AS Muc, COUNT(*) AS GiaTri, 3       AS MongDoi FROM dbo.SAN_PHAM WHERE ID LIKE 'DEMO-%'
UNION ALL SELECT N'Lô hàng demo',            COUNT(*),               3       FROM dbo.MA_SAN_PHAM WHERE ID LIKE 'DEMO-%'
UNION ALL SELECT N'Tồn kho DEMO-SP2 (lô)',   SUM(SO_LUONG),          90      FROM dbo.MA_SAN_PHAM WHERE ID_SAN_PHAM = 'DEMO-SP2'
UNION ALL SELECT N'Nợ đại lý DEMO-DL1',      SUM(CON_NO),            80000   FROM dbo.PHIEU_BAN WHERE ID_KHACH_HANG = 'DEMO-DL1'
UNION ALL SELECT N'Lô sắp hết hạn (<30 ngày)', COUNT(*),             1       FROM dbo.MA_SAN_PHAM WHERE ID LIKE 'DEMO-%' AND NGAY_HET_HAN < DATEADD(DAY, 30, GETDATE());
GO
