# DATABASE – Access (cuahang.dll) → SQL Server  (BE1 phụ trách, QA2 kiểm tra)

| File | Nội dung | Việc |
|---|---|---|
| `01_CreateDatabase.sql` | Tạo CSDL `CuahangNongduoc` (collation Vietnamese_CI_AS). **Xóa CSDL cũ nếu đã có.** | SQL-01 |
| `02_CreateTables.sql` | 14 bảng, khóa chính, 5 khóa ngoại gốc (CASCADE) + 5 khóa ngoại bổ sung, chỉ mục | SQL-01 |
| `03_SeedData.sql` | 68 dòng dữ liệu từ Access, chạy trong 1 transaction | SQL-02 |
| `04_KiemTraDuLieu.sql` | Đối chiếu số dòng + tổng tiền với Access (tất cả phải “Khớp”) | SQL-02 / QA2 |
| `CaiDatCSDL.bat` | Chạy lần lượt 01 → 04 bằng sqlcmd | |
| `tools/` | `XuatDuLieuAccess.bat` + `export_access.ps1` (xuất Access ra JSON), `gen_sql.py` (sinh lại 01–04) | |

## Cài đặt trên máy mới
1. Cài **SQL Server Express** (bản Basic) và **SSMS** (có sẵn sqlcmd).
2. Double-click `CaiDatCSDL.bat` (hoặc mở từng file trong SSMS theo thứ tự 01 → 04, bấm Execute).
3. Nếu tên instance không phải `.\SQLEXPRESS`: sửa dòng `set SERVER=` trong `.bat` và chuỗi `CuahangSqlServer` trong `App.config`.

## Chuyển kiểu dữ liệu
| Access | SQL Server | Ghi chú |
|---|---|---|
| Text(n) | NVARCHAR(n) | giữ tiếng Việt có dấu |
| Long Integer | INT | tiền đang lưu INT như bản gốc (đủ tới 2,1 tỷ/phiếu) |
| Integer | SMALLINT | THANG, NAM |
| Yes/No | BIT NOT NULL DEFAULT 0 | LOAI_KH |
| Date/Time | DATETIME | |
| AutoNumber | INT IDENTITY(1,1) | DON_VI_TINH.ID, LY_DO_CHI.ID |

Khóa ngoại bổ sung (Access thiếu): PHIEU_BAN→KHACH_HANG, CHI_TIET_PHIEU_BAN→MA_SAN_PHAM, PHIEU_NHAP→NHA_CUNG_CAP, SAN_PHAM→DON_VI_TINH, PHIEU_CHI→LY_DO_CHI (NO ACTION: không cho xóa bản ghi đang được dùng). Dữ liệu hiện có đã kiểm tra: 0 dòng mồ côi.
THAM_SO giữ nguyên không có khóa chính (bảng 1 dòng, code cũ đang dùng như vậy).

Muốn tạo dữ liệu demo riêng: thêm `05_SampleData_Demo.sql` (QA2).
