# BACKEND – Tầng nghiệp vụ & dữ liệu

**Phụ trách:** BE1, BE2, BE3. **Nhiệm vụ:** kết nối SQL Server, truy vấn dữ liệu, xử lý nghiệp vụ (tính tiền, tồn kho, công nợ, sinh mã phiếu), transaction, ghi log, cài đặt Design Pattern.
Backend **không** được tham chiếu `System.Windows.Forms` (không nhận ComboBox/DataGridView, không gọi MessageBox). Chỉ nhận/trả Model, DataTable hoặc kết quả (bool / SaveResult / Exception).

```
Backend/
├── Core/            Hạ tầng: DataService (kết nối & thực thi SQL), ThamSo, Settings         – BE1
├── Utilities/       Tiện ích: Num2Str (đọc số thành chữ), AppLogger (D02 – tạo mới)          – BE1
├── BusinessObject/  Thực thể (Model): SanPham, KhachHang, PhieuBan, PhieuNhap...            – BE2/BE3 theo module
│   └── Validation/  (D06 – tạo mới) IValidator, PhieuBanValidator...                        – BE3
├── DataLayer/       DAO truy vấn bảng (*Factory.cs – thực chất là DAO, không phải GoF Factory)
│   └── Mappers/     (D05 – tạo mới) DataRow → Entity                                         – BE2
├── Controller/      Điều khiển nghiệp vụ từng thực thể                                       – BE2/BE3 theo module
├── Services/        (D11 – tạo mới) SaleService, PurchaseService, CustomerDebtService (Facade)
└── Patterns/        Mẫu GoF dùng chung: Singleton, Factory, Facade, Strategy, TemplateMethod
```

Chia theo module để 3 BE không sửa trùng file:
- **BE1 – Hạ tầng & SQL Server:** Core/, Utilities/, Patterns/Singleton/, UnitOfWork/Transaction, script `Database/`.
- **BE2 – Bán hàng & Kho:** SanPham, MaSanPham, PhieuBan, ChiTietPhieuBan, DonViTinh; Patterns/Strategy, Patterns/Facade, Mappers, Observer tồn kho.
- **BE3 – Nhập hàng, Thu chi, Công nợ, Đối tác:** PhieuNhap, ChiTietPhieuNhap, PhieuChi, LyDoChi, PhieuThanhToan, DuNoKhachHang, KhachHang, NhaCungCap, CuaHang; Patterns/Factory, Patterns/TemplateMethod, Validation.

Lưu ý: `BusinessObject/ChiTietPhieuNhap.cs` và `Controller/ChiTietPhieuNhapController.cs` hiện **chưa được đưa vào .csproj** (không được biên dịch) → BE3 phải Include lại trước khi sửa R13, R25.
