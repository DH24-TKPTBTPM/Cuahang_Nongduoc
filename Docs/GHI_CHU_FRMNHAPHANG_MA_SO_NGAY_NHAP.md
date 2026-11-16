# GHI CHÚ THAY ĐỔI – frmNhapHang (bảng "phong thần")

Yêu cầu: **Mã số tự sinh** (tên sản phẩm + số, vd `Nuti001`), **Số lô do người dùng nhập**, **khóa Ngày nhập**.
Ràng buộc: không đụng database, không đụng `DataService.cs`, không sửa thứ không liên quan.

## File đã sửa (chỉ 1 file)
| File | Thay đổi |
|---|---|
| `frmNhapHang.cs` | Xem bên dưới |

Không sửa: `frmNhapHang.Designer.cs`, `DataService.cs`, mọi Controller/DataLayer/BusinessObject, database, form khác. Không thêm câu SQL mới (chỉ dùng hàm có sẵn `MaSanPhamController.LayMaSanPham`).

## Hành vi mới
1. **Mã số tự sinh, ô Mã số chỉ đọc.**
   - Chọn sản phẩm → ô Mã số tự hiện mã: **4 chữ cái đầu của tên sản phẩm (bỏ dấu, viết hoa chữ đầu) + số thứ tự 3 chữ số**. Ví dụ "Nutifood" → `Nuti001`, "Đạm cá" → `Damc001`, "Phân NPK" → `Phan001`. Tên không có chữ cái → tiền tố `SP`.
   - Số thứ tự là số nhỏ nhất kế tiếp chưa dùng, kiểm tra cả các lô đang nhập trong lưới và các mã đã lưu trong database, nên không trùng.
   - Thêm xong 1 lô: ô Mã số tự nhảy sang mã kế tiếp của cùng sản phẩm. Đổi sản phẩm: mã đổi theo.
   - Chưa chọn sản phẩm mà bấm Thêm: báo "chọn sản phẩm để hệ thống tự sinh Mã số".
   - Nếu mã vừa sinh bị người khác dùng mất trước khi bấm Thêm: tự cấp lại mã mới và báo cho người dùng.
   - Vào chế độ Chỉnh sửa phiếu cũ: thêm lô mới cũng dùng mã tự sinh. Xem phiếu đã lưu: ô Mã số để trống.
2. **Số lô**: vẫn do người dùng nhập tay như cũ (bắt buộc), chữ gợi ý "Số in trên bao bì".
3. **Ngày nhập bị khóa** (luôn bị làm xám, không sửa được): phiếu mới tự lấy ngày hiện tại, phiếu đã lưu giữ nguyên ngày cũ. Trước đó chỉ khóa khi chỉnh sửa.
4. Cập nhật lại các dòng hướng dẫn/tooltip/thông báo về Mã số cho đúng cách làm mới.

## Tùy chỉnh nhanh (nếu cần)
- Muốn tiền tố dài/ngắn hơn: sửa hằng `DoDaiTienToMaSo` (đang là 4).
- Muốn cho người dùng gõ đè Mã số: đổi `txtMaSo.ReadOnly = true;` trong constructor.

## Chưa kiểm chứng
Không có Visual Studio ở môi trường làm việc nên **chưa build/chạy thử**. Đã kiểm tra: ngoặc cân bằng, định dạng file (UTF-8 BOM, CRLF) giữ nguyên, thuật toán tiền tố và tìm số kế tiếp đã thử riêng bằng Python.

## Cần thử khi chạy
- [ ] Mở Nhập hàng, chọn sản phẩm → ô Mã số hiện dạng `Xxxx001`, không gõ tay được.
- [ ] Nhập số lô, giá, số lượng, bấm Thêm → dòng vào lưới, Mã số nhảy `Xxxx002`.
- [ ] Chọn sản phẩm khác → mã đổi tiền tố, số bắt đầu 001.
- [ ] Lưu phiếu rồi lập phiếu mới cùng sản phẩm → mã tiếp theo không trùng mã đã lưu.
- [ ] Ô Ngày nhập bị xám, không đổi được ở phiếu mới, khi chỉnh sửa và khi xem.
