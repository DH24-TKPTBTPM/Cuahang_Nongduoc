# QUY ƯỚC CHÚ THÍCH CODE & QUY TRÌNH GIT

## 1. Khối chú thích bắt buộc cho MỖI chỗ sửa (theo yêu cầu giảng viên)

Yêu cầu: ghi rõ **lỗi gì, code trước → sau, tác giả, người hỗ trợ, người kiểm tra, ngày chỉnh sửa**.

### a) Sửa code cũ (Refactoring – code smell → clean code)
```csharp
// ==================== [R12] REFACTORING ====================
// Lỗi (Code smell) : Wrong Field Assignment – DaTra, ConNo đọc nhầm cột TONG_TIEN
// Kỹ thuật         : Fix Bug + Rename/Extract... (ghi đúng tên kỹ thuật trong catalog Fowler)
// Trước            : ph.DaTra = Convert.ToInt64(tbl.Rows[0]["TONG_TIEN"]);
//                    ph.ConNo = Convert.ToInt64(tbl.Rows[0]["TONG_TIEN"]);
// Sau              : đọc đúng cột DA_TRA, CON_NO (code bên dưới)
// Tác giả          : BE3 – <Họ tên>
// Hỗ trợ           : BE1 – <Họ tên>
// Kiểm tra         : QA1 – <Họ tên>   (QA điền khi test đạt)
// Ngày chỉnh sửa   : 05/11/2026
// ===========================================================
ph.DaTra = Convert.ToInt64(tbl.Rows[0]["DA_TRA"]);
ph.ConNo = Convert.ToInt64(tbl.Rows[0]["CON_NO"]);
```
- Đoạn "Trước" ngắn (≤ 5 dòng) thì chép vào chú thích; dài thì ghi "xem Legacy/BanGoc_TruocRefactor/<file> dòng x–y".
- Không để lại code cũ dạng comment hàng loạt – đã có Git và thư mục Legacy.

### b) Viết mới (Design Pattern GoF)
```csharp
/// <summary>
/// [D08] VIẾT MỚI – Mẫu thiết kế: Facade (Structural, GoF)
/// Mục đích : gom Lưu phiếu bán + chi tiết + trừ kho + công nợ vào 1 transaction
/// Thay thế : 4 lệnh rời rạc trong frmBanLe/frmBanSi.toolLuu_Click
/// Tác giả  : BE2 – <Họ tên> | Hỗ trợ: BE1 – <Họ tên> | Kiểm tra: QA2 – <Họ tên>
/// Ngày tạo : 10/11/2026
/// </summary>
public class BanHangFacade { ... }
```

## 2. Quy trình Git
- `main`: bản nộp / demo. `develop`: bản tích hợp. Không ai push thẳng lên `main` hay `develop`.
- Mỗi task 1 nhánh: `feature/<vai trò>-<mã>-<mô tả>`, ví dụ `feature/be3-R12-phieunhap-datra`, `feature/fe1-FE01-frmBanHang`.
- Commit: `fix(R12): ...`, `feat(D08): ...`, `refactor(R20): ...`, `test(QA): ...`.
- Pull Request vào `develop` theo template; cần 1 review của Team Lead + QA xác nhận "Tested".
- Trước khi tạo PR: Rebuild Solution 0 lỗi, chạy thử form liên quan.
- Đổi tên lớp/enum dùng chung (R20 MaSanPhanFactory, R21 Controll) chỉ làm ở **Tuần 4**, báo cả nhóm trước để tránh xung đột.
- Máy Windows: bật `git config core.autocrlf true` (repo lưu LF, máy làm việc CRLF).
