# FRONTEND – Tầng giao diện (WinForms)

**Phụ trách:** FE1, FE2, FE3. **Nhiệm vụ:** chỉ lo hiển thị, nhận thao tác người dùng, data binding và thông báo lỗi.
Mọi đọc/ghi dữ liệu phải gọi qua `Backend` (Controller / Service / Facade). **Không** viết SQL, không gọi thẳng `DataLayer` hay `DataService` trong Form.

| Thư mục | Nội dung | Người phụ trách |
|---|---|---|
| `HeThong/` | frmMain (menu điều hướng), frmThongtinCuahang, frmThongtinLienhe | FE3 |
| `DanhMuc/` | frmSanPham, frmDonViTinh, frmKhachHang, frmDaiLy, frmNhaCungCap, frmLyDoChi | FE2 |
| `BanHang/` | frmBanLe, frmBanSi (→ gộp thành frmBanHang + Strategy), danh sách & tìm phiếu bán | FE1 |
| `NhapHang/` | frmNhapHang, frmDanhsachPhieuNhap, frmTimPhieuNhap | FE2 |
| `ThuChi/` | frmPhieuChi, frmThanhToan, frmTimPhieuChi, frmTimPhieuThu | FE3 |
| `BaoCao/` | frmDoanhThu, frmDunoKhachhang, frmSoLuongBan, frmSoLuongTon, frmSanphamHethan | FE3 |
| `InAn/` | frmInPhieuBan, frmInPhieuNhap, frmInPhieuChi, frmInPhieuThanhToan, frmInDunoKhachHang (frmInPhieuNhapVatTu: form rỗng, không nằm trong .csproj → xóa) | FE3 |
| `Common/` *(tạo khi cần)* | Lớp dùng chung cho Form: FormState (State Pattern), GridErrorHelper, BaseReportForm | FE1 / FE3 |

Quy tắc:
- Form mới thêm bằng Visual Studio (Add → Windows Form) **ngay trong đúng thư mục module** để .csproj tự cập nhật đường dẫn.
- Giữ nguyên `namespace CuahangNongduoc` cho các Form (không đổi namespace theo thư mục) để tránh vỡ code Designer.
- Báo cáo `.rdlc` vẫn để ở `Report/` vì tên tài nguyên nhúng (`CuahangNongduoc.Report.xxx.rdlc`) phụ thuộc đường dẫn.
- Mỗi chỗ sửa phải có khối chú thích chuẩn (xem `Docs/QUY_UOC_CHU_THICH_VA_GIT.md`).
