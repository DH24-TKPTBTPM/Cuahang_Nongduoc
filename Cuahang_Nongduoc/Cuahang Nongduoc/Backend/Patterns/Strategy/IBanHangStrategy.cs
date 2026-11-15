using System;
using CuahangNongduoc.BusinessObject;

namespace CuahangNongduoc.Patterns.Strategy
{
    // ==================== [STR-01] REFACTORING ====================
    // Lỗi (Code smell) : Thiếu giao diện chuẩn (Interface) để đóng gói các thuật toán xử lý riêng cho từng hình thức bán hàng.
    // Kỹ thuật         : Strategy Pattern (Behavioral, GoF)
    // Trước            : Các xử lý tính giá và lọc loại khách hàng bị gom chung, gây trùng lặp và khó mở rộng.
    // Sau              : Tạo Interface IBanHangStrategy chuẩn hóa các phương thức lấy đơn giá và kiểm tra đại lý, giúp các class chiến lược tuân thủ chung một khuôn mẫu.
    // Tác giả          : BE2
    // Hỗ trợ           : FE1
    // Kiểm tra         : QA1
    // Ngày chỉnh sửa   : 12/11/2026
    // ==============================================================
    /// <summary>
    /// Strategy Pattern (Behavioral): Giao diện chiến lược bán hàng,
    /// đóng gói thuật toán xác định đơn giá và lọc đối tượng khách hàng (Bán lẻ / Bán sỉ).
    /// </summary>
    public interface IBanHangStrategy
    {
        /// <summary>
        /// Tên hiển thị của chiến lược (ví dụ: "Bán lẻ", "Bán sỉ").
        /// </summary>
        string TenChienLuoc { get; }

        /// <summary>
        /// True nếu là khách hàng đại lý (Bán sỉ), False nếu là khách lẻ (Bán lẻ).
        /// </summary>
        bool IsDaiLy { get; }

        /// <summary>
        /// Trả về đơn giá theo chiến lược tương ứng.
        /// </summary>
        long LayDonGia(SanPham sp, MaSanPham masp);
    }
}