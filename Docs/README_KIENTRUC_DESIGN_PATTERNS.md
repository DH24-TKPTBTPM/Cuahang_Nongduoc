# KIẾN TRÚC VÀ CẤU TRÚC THƯ MỤC HỆ THỐNG - DESIGN PATTERNS (FE & BE)

Dự án: **Phần mềm Quản lý Cửa hàng Nông dược**  
Mô hình tổ chức: **Phân chia 2 Module độc lập: Frontend (FE) & Backend (BE)**  
Tham chiếu chuẩn: **Refactoring Guru (GoF Design Patterns)**  
Trạng thái biên dịch: **MSBuild Rebuild Succeeded (0 Errors)**

---

## 1. SƠ ĐỒ CẤU TRÚC THƯ MỤC ĐÃ TÁI CẤU TRÚC (FE / BE)

```text
d:\Code\Cuahang_Nongduoc\
├── BaoCao_KeHoach_Refactoring_DesignPatterns.docx    (Tài liệu kế hoạch gốc)
├── Bang_Theo_Doi_Loi_Va_Ke_Hoach_Refactoring.xlsx    (File Excel theo dõi 11 bugs & phân công 8 người)
├── Docs\                                            (Tài liệu kiến trúc & hướng dẫn)
│   ├── README_KIENTRUC_DESIGN_PATTERNS.md
│   └── BANG_THEO_DOI_LOI_VA_KE_HOACH_REFACTORING.md
│
└── Cuahang_Nongduoc\
    └── Cuahang Nongduoc\
        ├── CuahangNongDuoc.sln                      (Visual Studio Solution)
        ├── CuahangNongduoc.csproj                   (Project File C# đã cập nhật đường dẫn FE/BE)
        ├── Program.cs                               (Điểm khởi chạy ứng dụng)
        │
        ├── Frontend\                                [MODULE GIAO DIỆN - CHO 2 BẠN FE]
        │   ├── frmMain.cs                           (Màn hình chính & Menu điều hướng)
        │   ├── frmBanHang.cs                        (Form Bán Hàng hợp nhất Strategy MỚI)
        │   ├── frmBanLe.cs, frmBanSi.cs             (Form cũ)
        │   ├── frmDanhsachPhieuBanLe.cs, frmDanhsachPhieuBanSi.cs, frmDanhsachPhieuNhap.cs...
        │   ├── frmSanPham.cs, frmKhachHang.cs, frmDaiLy.cs, frmNhaCungCap.cs, frmDonViTinh.cs...
        │   ├── frmNhapHang.cs, frmPhieuChi.cs, frmThanhToan.cs, frmLyDoChi.cs...
        │   ├── frmDoanhThu.cs, frmDunoKhachhang.cs, frmInPhieuBan.cs, frmInPhieuNhap.cs...
        │   └── (Toàn bộ các file .Designer.cs và .resx tương ứng đi kèm)
        │
        └── Backend\                                 [MODULE NGHIỆP VỤ & DỮ LIỆU - CHO 2 BẠN BE]
            │
            ├── Patterns\                            (CÁC MẪU THIẾT KẾ GOF)
            │   ├── Singleton\                       [Creational Pattern]
            │   │   ├── DatabaseManager.cs           (Quản lý OleDbConnection thread-safe duy nhất)
            │   │   └── ThamSoManager.cs             (Quản lý cấu hình & sinh mã chứng từ)
            │   │
            │   ├── Factory\                         [Creational Pattern]
            │   │   └── PhieuFactory.cs              (Factory Method tạo các loại chứng từ)
            │   │
            │   ├── Facade\                          [Structural Pattern]
            │   │   └── BanHangFacade.cs             (Mặt bọc điều phối Controller & Transaction)
            │   │
            │   ├── Strategy\                        [Behavioral Pattern - Trọng tâm]
            │   │   ├── IBanHangStrategy.cs          (Interface chiến lược bán hàng)
            │   │   ├── BanLeStrategy.cs             (Chiến lược bán lẻ: GiaBanLe, khách lẻ)
            │   │   └── BanSiStrategy.cs             (Chiến lược bán sỉ: GiaBanSi, đại lý)
            │   │
            │   └── TemplateMethod\                  [Behavioral Pattern]
            │       ├── QuyTrinhXuLyPhieuBase.cs     (Khung sườn 5 bước xử lý chứng từ)
            │       ├── QuyTrinhBanHang.cs           (Lớp kế thừa xử lý Phiếu Bán)
            │       └── QuyTrinhNhapHang.cs          (Lớp kế thừa xử lý Phiếu Nhập)
            │
            ├── Core\                                (CÁC DỊCH VỤ DỮ LIỆU CỐT LÕI)
            │   ├── DataService.cs                   (Tầng dịch vụ dữ liệu - kết nối Access)
            │   ├── ThamSo.cs                        (Tham số hệ thống)
            │   ├── Num2Str.cs                       (Đọc số thành chữ cho hóa đơn)
            │   └── Settings.cs                      (Cấu hình hệ thống)
            │
            ├── BusinessObject\                      (TẦNG THỰC THỂ / DOMAIN MODELS)
            │   ├── SanPham.cs, KhachHang.cs, PhieuBan.cs, MaSanPham.cs...
            │
            ├── Controller\                          (TẦNG ĐIỀU KHIỂN NGHIỆP VỤ)
            │   ├── SanPhamController.cs, KhachHangController.cs, PhieuBanController.cs...
            │
            └── DataLayer\                           (TẦNG TRUY VẤN DỮ LIỆU - DAO)
                ├── SanPhamFactory.cs (Đóng vai trò SanPhamDAO)
                ├── KhachHangFactory.cs (Đóng vai trò KhachHangDAO)...
```

---

## 2. PHÂN ĐỊNH TRÁCH NHIỆM CHO 2 FE VÀ 2 BE

- **Frontend Developers (FE 1 & FE 2):**
  - Làm việc 100% trong thư mục: [**`Cuahang Nongduoc/Frontend/`**](file:///d:/Code/Cuahang_Nongduoc/Cuahang_Nongduoc/Cuahang%20Nongduoc/Frontend)
  - `FE 1`: Chịu trách nhiệm form bán hàng [`frmBanHang.cs`](file:///d:/Code/Cuahang_Nongduoc/Cuahang_Nongduoc/Cuahang%20Nongduoc/Frontend), gộp logic từ `frmBanLe` và `frmBanSi`.
  - `FE 2`: Chịu trách nhiệm menu [`frmMain.cs`](file:///d:/Code/Cuahang_Nongduoc/Cuahang_Nongduoc/Cuahang%20Nongduoc/Frontend/frmMain.cs), danh sách phiếu và các form báo cáo/in ấn.
  - *Nguyên tắc:* Không tự viết câu lệnh SQL hay logic Transaction trong Form; mọi tương tác dữ liệu đều gọi qua các lớp trong `Backend/`.

- **Backend Developers (BE 1 & BE 2):**
  - Làm việc 100% trong thư mục: [**`Cuahang Nongduoc/Backend/`**](file:///d:/Code/Cuahang_Nongduoc/Cuahang_Nongduoc/Cuahang%20Nongduoc/Backend)
  - `BE 1`: Chịu trách nhiệm [`Backend/Core/DataService.cs`](file:///d:/Code/Cuahang_Nongduoc/Cuahang_Nongduoc/Cuahang%20Nongduoc/Backend/Core/DataService.cs), [`Backend/Patterns/Singleton/`](file:///d:/Code/Cuahang_Nongduoc/Cuahang_Nongduoc/Cuahang%20Nongduoc/Backend/Patterns/Singleton) và hoàn thiện Transaction trong [`Backend/Patterns/Facade/BanHangFacade.cs`](file:///d:/Code/Cuahang_Nongduoc/Cuahang_Nongduoc/Cuahang%20Nongduoc/Backend/Patterns/Facade/BanHangFacade.cs).
  - `BE 2`: Chịu trách nhiệm [`Backend/Patterns/Strategy/`](file:///d:/Code/Cuahang_Nongduoc/Cuahang_Nongduoc/Cuahang%20Nongduoc/Backend/Patterns/Strategy), [`Backend/Patterns/Factory/`](file:///d:/Code/Cuahang_Nongduoc/Cuahang_Nongduoc/Cuahang%20Nongduoc/Backend/Patterns/Factory), [`Backend/Patterns/TemplateMethod/`](file:///d:/Code/Cuahang_Nongduoc/Cuahang_Nongduoc/Cuahang%20Nongduoc/Backend/Patterns/TemplateMethod) và tách WinForms controls khỏi [`Backend/Controller/`](file:///d:/Code/Cuahang_Nongduoc/Cuahang_Nongduoc/Cuahang%20Nongduoc/Backend/Controller).
  - *Nguyên tắc:* Không đụng vào file Form `.Designer.cs` hay `.resx`; chỉ tập trung xử lý Business Logic, Design Patterns và Data Access.
