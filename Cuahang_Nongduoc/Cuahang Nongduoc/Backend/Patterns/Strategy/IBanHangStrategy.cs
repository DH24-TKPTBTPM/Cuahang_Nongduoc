using System;
using CuahangNongduoc.BusinessObject;

namespace CuahangNongduoc.Patterns.Strategy
{
    /// <summary>
    /// Strategy Pattern (Behavioral): Giao diện chiến lược bán hàng,
    /// đóng gói thuật toán xác định đơn giá và lọc đối tượng khách hàng (Bán lẻ / Bán sỉ / Khuyến mãi).
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
        /// <param name="sp">Đối tượng SanPham</param>
        /// <param name="masp">Đối tượng MaSanPham</param>
        /// <returns>Đơn giá áp dụng</returns>
        long LayDonGia(SanPham sp, MaSanPham masp);
    }
}
