# Đăng nhập và phân quyền (C# WinForms / .NET Framework 4.8)

## Đã sửa
- Program.cs: mở frmDangNhap trước frmMain; không thể vào hệ thống khi chưa đăng nhập; bắt buộc đổi mật khẩu lần đầu nếu MUST_CHANGE_PWD=true.
- CuahangNongDuoc.csproj: đăng ký 18 file C#/resx thuộc Auth và 5 form đăng nhập/tài khoản, trước đây bị bỏ sót.
- frmMain.cs: menu Quản lý tài khoản, Đổi mật khẩu, Đăng xuất; kiểm tra quyền trước khi mở màn hình; vô hiệu hóa menu và nút toolbar không có quyền; hiển thị thông tin người dùng.
- frmTaiKhoanChiTiet.cs: không cho tự thay đổi quyền hoặc tự khóa chính mình qua biểu mẫu quản lý.

## Cơ sở dữ liệu - BẮT BUỘC kiểm tra trước khi chạy
1. Sao lưu file `bin/Debug/cuahang.dll` (thực chất là cơ sở dữ liệu Access, KHÔNG phải DLL .NET).
2. Kiểm tra bảng `NGUOI_DUNG` trong cơ sở dữ liệu. Nếu chưa có, thực hiện các lệnh trong `SQL_TAO_BANG_NGUOI_DUNG.txt` **từng lệnh một** bằng Microsoft Access (SQL View). Dùng bản sao có phần mở rộng .mdb để mở trong Access nếu cần rồi sao chép dữ liệu đã thay đổi trở lại, không đổi cấu trúc dữ liệu khác.
3. Nếu chưa có tài khoản admin, chạy INSERT trong file SQL đúng một lần. Tài khoản demo `admin`, mật khẩu tạm `ChangeMeNow!2026`, phải đổi ngay ở lần đăng nhập đầu tiên. Không sử dụng mật khẩu này khi triển khai thực tế.
4. Chương trình kết nối Jet OLEDB 4.0 (32-bit); bảo đảm Visual Studio đặt Platform Target là x86 và có Microsoft Access Database Engine tương thích.
5. Đường dẫn kết nối hiện tại là `Data Source=cuahang.dll` theo thư mục làm việc. Cần chắc chắn chạy từ thư mục chứa file này, hoặc điều chỉnh `DataService.m_ConnectString` phù hợp máy của bạn.

## Quyền
- admin: toàn quyền
- sales: bán lẻ/bán sỉ, khách hàng/đại lý, công nợ/thu, xem danh mục và một số báo cáo
- warehouse: quản lý danh mục vật tư, nhập hàng, xem tồn kho và báo cáo liên quan

## Thử nghiệm bắt buộc trên Windows
1. Chưa đăng nhập -> không mở frmMain; hủy login -> thoát.
2. Đăng nhập đúng/sai; tài khoản bị khóa; cơ sở dữ liệu không tồn tại.
3. Tài khoản phải đổi mật khẩu -> đóng form đổi mật khẩu mà chưa đổi phải thoát; đổi thành công mới mở frmMain.
4. Admin mở Quản lý tài khoản; nhân viên Sales và Warehouse bị chặn.
5. Sales không mở form Nhập hàng; Warehouse không mở form Bán hàng; thử cả menu và thanh công cụ.
6. Đăng xuất -> ứng dụng đóng, đăng nhập lại từ đầu sau khi khởi chạy lại.

## Giới hạn hiện tại
- Phân quyền mới được thực thi ở các điểm truy cập chính trên frmMain; chưa rà soát đầy đủ từng nút Thêm/Sửa/Xóa trong mọi form con hoặc truy cập trực tiếp từ code. Vì vậy chưa đủ để coi là bảo mật RBAC ở lớp dữ liệu.
- Chưa biên dịch/chạy GUI được trong môi trường Linux ở đây; cần Build và kiểm thử trên Windows + Visual Studio.
- Không tự ý thay đổi cơ sở dữ liệu hiện có. Script SQL chỉ là hướng dẫn, không được tự thực thi.
