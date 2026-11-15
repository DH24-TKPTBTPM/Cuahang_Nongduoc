# TESTS – Kiểm thử (QA1, QA2)

- `TestPlan.xlsx` – kế hoạch kiểm thử (phạm vi, môi trường, tiêu chí đạt).
- `TestCases_*.xlsx` – ca kiểm thử theo module (BanHang, NhapHang, DanhMuc, ThuChi, BaoCao, CSDL).
- `Evidence/` – ảnh chụp **trước khi sửa** và **sau khi sửa** cho từng mã lỗi (đặt tên `R12_truoc.png`, `R12_sau.png`).
Mỗi lỗi chỉ được chuyển "Đã đóng" khi QA chạy lại ca kiểm thử và điền tên vào mục **Kiểm tra** trong chú thích code.
(File .xlsx bị .gitignore – lưu trên Drive nhóm, chỉ commit ảnh minh chứng nếu cần.)

## Kiểm thử tự động tầng CSDL (QA-06, BE1 hỗ trợ)
`KiemThuCSDL/ChayKiemThu.bat` – Rebuild Solution (Debug) rồi double-click; không cần Visual Studio (dùng csc .NET 3.5 có sẵn trong Windows).
Kiểm tra: UnitOfWork commit/rollback (D08), **mất kết nối giữa lúc lưu** (KILL phiên SQL) → không ghi dở và tự kết nối lại, nhiều form mở cùng lúc (R04), lưu/xóa hóa đơn qua Facade có trừ/hoàn kho (D11-BH). Tự dọn dữ liệu thử. Kết quả mong đợi: `14 dat, 0 truot`.

## Đối chiếu dữ liệu Access ↔ SQL Server (QA-04)
Chạy `Database/04_KiemTraDuLieu.sql` (số dòng 14 bảng + tổng tiền + tổng tồn kho) – tất cả phải **Khớp** (khi chưa nạp dữ liệu demo).

## Dữ liệu và kịch bản demo (QA-08)
`Database/05_SampleData_Demo.sql` + `KichBan_Demo.md` (bản nháp 10 bước, QA2 chốt).

## Lưu ý báo cáo / in ấn
Máy chạy báo cáo cần **Microsoft Report Viewer 2010 Redistributable** (cung cấp `Microsoft.ReportViewer.ProcessingObjectModel` 10.0). Thiếu thì mở báo cáo sẽ báo *Could not load file or assembly 'Microsoft.ReportViewer.ProcessingObjectModel'*.
