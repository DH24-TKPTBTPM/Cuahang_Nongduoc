# Kế hoạch: Thống kê hóa đơn giảm giá và khuyến mãi theo nhân viên

## Mục tiêu

Xây dựng báo cáo các hóa đơn bán lẻ/bán sỉ có áp dụng giảm giá hoặc chiết khấu trong khoảng thời gian người dùng chọn, nhóm theo tài khoản nhân viên lập hóa đơn. Báo cáo cần thể hiện số hóa đơn, tiền hàng, tỷ lệ và số tiền giảm, tổng thanh toán; hỗ trợ xem chi tiết hóa đơn.

## Khảo sát hiện trạng và giả định

- Hóa đơn bán dùng chung bảng `PHIEU_BAN`; thông tin giảm giá đã có các trường `LOAI_GIAM_GIA`, `TY_LE_GIAM_GIA`, `TIEN_GIAM_GIA`.
- `UserSession` có `UserId`, `TenDangNhap`, `HoTen`, nhưng luồng ghi hóa đơn hiện tại trong `BanLeRepository` chưa lưu ID người lập vào `PHIEU_BAN`.
- Vì vậy phải bổ sung dấu vết người lập khi tạo hóa đơn. Hóa đơn cũ chưa có người lập sẽ được báo cáo vào nhóm **Chưa xác định** (không suy đoán người lập theo tài khoản hiện đăng nhập).
- “Khuyến mãi” hiện được biểu diễn bằng loại `Giảm giá` hoặc `Chiết khấu`; phạm vi báo cáo bao gồm hóa đơn có `TIEN_GIAM_GIA > 0`. Nếu nghiệp vụ muốn tính thêm chương trình khuyến mãi theo sản phẩm, cần có yêu cầu/mô hình riêng.
- Khoảng ngày được hiểu bao gồm cả ngày bắt đầu và ngày kết thúc.

## Phân chia công việc

| Người | Phạm vi | Đầu việc và bàn giao |
|---|---|---|
| **Người 1 – dữ liệu và nghiệp vụ** | Lưu nhân viên lập hóa đơn, truy vấn báo cáo | 1. Xác định trường liên kết tài khoản trong `PHIEU_BAN` (ưu tiên `ID_NGUOI_DUNG`) và tạo migration tương thích dữ liệu hiện có. 2. Khi lưu hóa đơn mới, ghi `UserSession.UserId`; xác định cách xử lý cập nhật hóa đơn cũ nếu có luồng cập nhật. 3. Bổ sung lớp truy vấn báo cáo trong DataLayer/Controller, lọc theo ngày bao gồm ngày cuối, chỉ lấy hóa đơn có tiền giảm dương, trả về thông tin hóa đơn và nhân viên. 4. Trả dữ liệu nhóm/tổng hợp theo nhân viên và chi tiết từng hóa đơn; dùng truy vấn tham số hóa. 5. Bàn giao script migration, API truy vấn và mô tả trường dữ liệu cho Người 2. |
| **Người 2 – giao diện và báo cáo** | Màn hình lọc, bảng kết quả, xuất/in | 1. Tạo màn hình thống kê hoặc mở rộng khu vực báo cáo hiện có. 2. Bổ sung chọn từ ngày/đến ngày, nút xem và kiểm tra khoảng ngày hợp lệ. 3. Hiển thị nhóm nhân viên: số hóa đơn, tổng tiền hàng, tổng tiền giảm, tổng thanh toán; cho phép xem danh sách hóa đơn thuộc từng nhân viên. 4. Hiển thị loại giảm (`Giảm giá`/`Chiết khấu`), tỷ lệ, số tiền giảm và ngày/số hóa đơn; hỗ trợ in/xuất theo khả năng báo cáo hiện có. 5. Tích hợp API từ Người 1, xử lý trạng thái rỗng/lỗi và định dạng tiền/ngày theo văn hóa Việt Nam. |

## Điểm tích hợp

- Thống nhất tên cột khóa nhân viên và kiểu dữ liệu trước khi bắt đầu ghép code.
- Người 1 bàn giao phương thức truy vấn cùng cấu trúc kết quả; Người 2 không tự viết SQL báo cáo.
- Cùng dùng chung quy ước ngày: `NGAY_BAN >= từ ngày` và `NGAY_BAN < ngày kế tiếp sau đến ngày` để bao trọn ngày cuối.
- Nếu một người làm báo cáo, có thể triển khai tuần tự theo thứ tự: migration/lưu nhân viên → truy vấn → giao diện.

## Tiêu chí hoàn thành

- Hóa đơn mới ghi đúng tài khoản đang đăng nhập; tài khoản được tra ra bằng ID, không phụ thuộc tên hiển thị có thể đổi.
- Lọc được chính xác hóa đơn giảm giá/chiết khấu trong khoảng ngày, kể cả toàn bộ ngày kết thúc.
- Tổng số hóa đơn và các tổng tiền theo nhân viên khớp với các dòng chi tiết; hóa đơn cũ thiếu ID được gộp rõ ràng vào “Chưa xác định”.
- Có thể xem chi tiết và in/xuất báo cáo; khoảng ngày sai và kết quả rỗng được thông báo phù hợp.

## Trình tự phối hợp

1. Chốt tên cột lưu người lập và quy tắc “khuyến mãi” nêu trên.
2. Người 1 hoàn thành migration và API dữ liệu mẫu; Người 2 dựng giao diện bằng cấu trúc kết quả đã thống nhất.
3. Ghép chức năng, rà soát kết quả với các hóa đơn có giảm và không giảm, nhiều nhân viên, hai mốc ngày, và dữ liệu lịch sử thiếu người lập.
