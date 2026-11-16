-- Chạy một lần trên file Access cuahang.dll trước khi phát hành phiên bản mới.
-- Cho phép NULL để giữ nguyên các hóa đơn lịch sử chưa lưu nhân viên lập.
ALTER TABLE PHIEU_BAN ADD COLUMN ID_NGUOI_DUNG LONG;

-- Không gán ngược người lập cho hóa đơn cũ vì không có dữ liệu đáng tin cậy.

