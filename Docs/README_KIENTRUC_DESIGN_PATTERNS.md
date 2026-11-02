# KIẾN TRÚC & CẤU TRÚC THƯ MỤC – CỬA HÀNG NÔNG DƯỢC

Mô hình: **WinForms C# 3 lớp**, tách 2 khối **Frontend (giao diện)** và **Backend (nghiệp vụ + dữ liệu)**, CSDL chuyển từ **Access (cuahang.dll) → SQL Server**.
Nhóm: Team Lead + 3 FE + 3 BE + 2 QA. Báo cáo: 12/11/2026 (đóng băng code 20h 10/11). Kế hoạch chi tiết: `KeHoach_PhanCong_SuaDoAn.xlsx` (thư mục gốc, không commit).

```text
Cuahang_Nongduoc/                         (repo)
├── Docs/                                 Tài liệu kiến trúc, quy ước chú thích & Git
├── Database/                             Script SQL Server (BE1)
├── Tests/                                Test plan, test case, ảnh minh chứng trước/sau (QA1, QA2)
├── Legacy/BanGoc_TruocRefactor/          Mã gốc để đối chiếu "Trước → Sau" (không sửa)
└── Cuahang_Nongduoc/Cuahang Nongduoc/
    ├── CuahangNongDuoc.sln, CuahangNongduoc.csproj, Program.cs
    ├── Properties/, Resources/ (icon), Report/ (*.rdlc – giữ nguyên vị trí vì tên tài nguyên nhúng)
    ├── Frontend/                         [FE1, FE2, FE3]
    │   ├── HeThong/   frmMain, frmThongtinCuahang, frmThongtinLienhe              (FE3)
    │   ├── DanhMuc/   frmSanPham, frmDonViTinh, frmKhachHang, frmDaiLy, frmNhaCungCap, frmLyDoChi (FE2)
    │   ├── BanHang/   frmBanLe, frmBanSi → frmBanHang, DS & tìm phiếu bán       (FE1)
    │   ├── NhapHang/  frmNhapHang, frmDanhsachPhieuNhap, frmTimPhieuNhap          (FE2)
    │   ├── ThuChi/    frmPhieuChi, frmThanhToan, frmTimPhieuChi, frmTimPhieuThu   (FE3)
    │   ├── BaoCao/    frmDoanhThu, frmDunoKhachhang, frmSoLuongBan, frmSoLuongTon, frmSanphamHethan (FE3)
    │   └── InAn/      frmInPhieu*, frmInDunoKhachHang                             (FE3)
    └── Backend/                          [BE1, BE2, BE3]
        ├── Core/            DataService, ThamSo, Settings                         (BE1)
        ├── Utilities/       Num2Str, AppLogger                                   (BE1)
        ├── BusinessObject/  Entity (+ Validation/)                               (BE2/BE3)
        ├── DataLayer/       DAO *Factory.cs (+ Mappers/)                          (BE2/BE3)
        ├── Controller/      Controller nghiệp vụ                                  (BE2/BE3)
        ├── Services/        Facade dịch vụ: SaleService, PurchaseService...       (BE2/BE3)
        └── Patterns/        Singleton, Factory, Facade, Strategy, TemplateMethod
```

## Frontend làm gì?
Hiển thị dữ liệu, nhận thao tác, kiểm tra nhập liệu cơ bản trên ô (rỗng, đúng kiểu số/ngày), data binding, bật/tắt nút theo trạng thái (State Pattern), hiện thông báo lỗi do Backend ném lên, gọi báo cáo in ấn.
**Không**: viết SQL, mở kết nối, tự trừ kho/tính công nợ, gọi thẳng `DataLayer`/`DataService`.

## Backend làm gì?
Kết nối SQL Server (Singleton), thực thi truy vấn có tham số, ánh xạ dữ liệu → Entity, nghiệp vụ (giá lẻ/sỉ, tồn kho, giá nhập bình quân, công nợ, sinh mã phiếu), transaction khi lưu nhiều bảng, ghi log, ném lỗi có kiểm soát.
**Không**: tham chiếu `System.Windows.Forms` (ComboBox, DataGridView, MessageBox).

## Mẫu thiết kế áp dụng (GoF – 23 mẫu)
| Mẫu | Loại | Ở đâu |
|---|---|---|
| Singleton | Creational | DatabaseManager, AppLogger, ThamSoManager |
| Factory Method | Creational | PhieuFactory (tạo PhieuBan/PhieuNhap/PhieuChi) |
| Builder | Creational | ReportBuilder (in ấn) |
| Facade | Structural | BanHangFacade / SaleService, PurchaseService |
| Strategy | Behavioral | BanLeStrategy / BanSiStrategy; bộ Validator |
| Template Method | Behavioral | QuyTrinhXuLyPhieuBase, BaseFactory, BaseReportForm |
| Observer | Behavioral | Cập nhật tồn kho khi lưu phiếu |
| State | Behavioral | Trạng thái form Xem / Thêm / Sửa |

Repository, Unit of Work, Specification, Mapper là mẫu **kiến trúc (Fowler – PoEAA)**, không thuộc 23 mẫu GoF – ghi rõ như vậy trong báo cáo.

Chi tiết từng file: xem `Frontend/README.md`, `Backend/README.md`.
