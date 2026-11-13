using System;
using System.Collections.Generic;
using System.Text;

namespace CuahangNongduoc.Backend.Patterns.State
{
    /// VIẾT MỚI – Mẫu thiết kế: State Pattern (Behavioral, GoF)
    /// Mục đích : Đóng gói và quản lý các trạng thái giao diện (Xem, Thêm mới, Chỉnh sửa) cho frmNhapHang thông qua XemState, ThemState, SuaState.
    /// Thay thế : Logic kiểm tra biến status rải rác và việc bật/tắt thủ công các ô nhập liệu trong frmNhapHang.
    /// Tác giả  : FE2 – Nguyễn Huỳnh Ngọc Anh | Hỗ trợ: <Không> | Kiểm tra: QA – <Họ_tên>
    /// Ngày tạo : 09/11/2026
    public interface IFrmNhapHangState
    {
        void Handle(frmNhapHang form);
    }
}
