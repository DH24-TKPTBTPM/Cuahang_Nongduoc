# KỊCH BẢN DEMO 10 BƯỚC – buổi bảo vệ (QA-08)

> Bản nháp do BE1 chuẩn bị theo dữ liệu `Database/05_SampleData_Demo.sql`; **QA2 chỉnh và chốt**.
> Chuẩn bị: chạy `Database/CaiDatCSDL.bat` (01 → 04) rồi chạy `05_SampleData_Demo.sql`; Rebuild Solution; mở chương trình.

| # | Thao tác | Dữ liệu demo | Kết quả mong đợi | Điểm cần nói (refactor / pattern) |
|---|---|---|---|---|
| 1 | Mở chương trình | – | Vào màn hình chính, không báo lỗi kết nối | CSDL SQL Server, chuỗi kết nối trong App.config (R03), DatabaseManager Singleton (D01) |
| 2 | Danh mục → Sản phẩm, gõ `DEMO` vào ô tìm | 3 sản phẩm `DEMO-SP1..3` | Lưới chỉ còn 3 dòng DEMO | Tìm kiếm có tham số (R08), DataService tách khỏi DataTable (R04) |
| 3 | Nhập hàng: NCC `Cty Vật tư Nông nghiệp Miền Tây`, thêm 20 `DEMO-SP1` giá 22.000 → Lưu | tồn SP1 = 50 | Tồn SP1 = 70, giá nhập bình quân ≈ 20.571 | Cập nhật giá & tồn kho (R28-BH, R22), State form nhập (FE2) |
| 4 | Bán lẻ cho `Nguyễn Văn An`: 2 `DEMO-SP1` (giá lẻ 27.000) → Lưu | – | Lưu thành công, tồn SP1 giảm 2 | Strategy giá lẻ (FE-01), Facade + UnitOfWork 1 transaction (D11-BH, D08) |
| 5 | Bán sỉ cho `Đại lý Phân bón Hòa Bình`: 5 `DEMO-SP2` → Lưu | giá sỉ 18.000 | Đơn giá tự lấy giá sỉ | Strategy giá sỉ (BanSiStrategy) |
| 6 | Danh sách phiếu bán sỉ → mở lại phiếu vừa lưu → In | – | Cột hiển thị **Đại lý**, phiếu in có tên cửa hàng, số tiền bằng chữ | Builder + Template Method in ấn (D10) |
| 7 | Xóa phiếu bán ở bước 4 | – | Tồn SP1 hoàn lại như trước bước 4 | Xóa + hoàn kho + tính lại công nợ trong 1 transaction |
| 8 | Thu chi → Thanh toán: đại lý trả 80.000 | nợ cũ `DEMO-PB1` = 80.000 | Lưu phiếu thanh toán | Gọi qua Controller (FE-TC), tham số đối tượng (R26) |
| 9 | Báo cáo → Dư nợ khách hàng → Tổng hợp tháng này | – | Đại lý DEMO: đầu kỳ 80.000, đã trả 80.000 | Tổng hợp dư nợ qua Controller (FE-CN, R24) |
| 10 | Báo cáo → Sản phẩm hết hạn | lô `DEMO-SP3-L1` còn 20 ngày | Lô Glyphosate xuất hiện trong báo cáo | Báo cáo RDLC trên SQL Server |

**Kiểm tra rollback (nếu hội đồng hỏi):** chạy `Tests/KiemThuCSDL/ChayKiemThu.bat` – mục 2 mô phỏng mất kết nối giữa lúc lưu, dữ liệu không bị ghi dở.

⚠️ Báo cáo/in ấn cần **ReportViewer 2010 runtime** trên máy demo (xem ghi chú trong Tests/README.md).
