using System;
using CuahangNongduoc.BusinessObject;

namespace CuahangNongduoc.Patterns.Strategy
{
    // ==================== [STR-01] REFACTORING ====================
    // Lỗi (Code smell) : Trộn lẫn logic tính giá bán lẻ vào chung form giao diện gây rối rắm.
    // Kỹ thuật         : Concrete Strategy (Strategy Pattern)
    // Trước            : Form phải tự kiểm tra điều kiện để lấy GiaBanLe thủ công bằng nhiều dòng if-else.
    // Sau              : Đóng gói toàn bộ logic lấy giá bán lẻ và xác định không phải đại lý vào class riêng biệt BanLeStrategy.
    // Tác giả          : BE2
    // Hỗ trợ           : FE1
    // Kiểm tra         : QA1
    // Ngày chỉnh sửa   : 12/11/2026
    // ==============================================================
    /// <summary>
    /// Concrete Strategy: Chiến lược Bán Lẻ.
    /// Lấy giá bán lẻ (GiaBanLe) và lọc khách hàng cá nhân (IsDaiLy = false).
    /// </summary>
    public class BanLeStrategy : IBanHangStrategy
    {
        public string TenChienLuoc
        {
            get { return "Bán lẻ"; }
        }

        public bool IsDaiLy
        {
            get { return false; }
        }

        public long LayDonGia(SanPham sp, MaSanPham masp)
        {
            if (sp != null)
            {
                return sp.GiaBanLe;
            }
            if (masp != null && masp.SanPham != null)
            {
                return masp.SanPham.GiaBanLe;
            }
            return 0;
        }
    }
}