using System;
using CuahangNongduoc.BusinessObject;

namespace CuahangNongduoc.Patterns.Strategy
{
    // ==================== [STR-01] REFACTORING ====================
    // Lỗi (Code smell) : Trộn lẫn logic tính giá bán sỉ vào chung form giao diện.
    // Kỹ thuật         : Concrete Strategy (Strategy Pattern)
    // Trước            : Form phải viết điều kiện riêng để lấy GiaBanSi phức tạp.
    // Sau              : Đóng gói toàn bộ logic lấy giá bán sỉ và xác định là đại lý vào class riêng biệt BanSiStrategy.
    // Tác giả          : BE2
    // Hỗ trợ           : FE1
    // Kiểm tra         : QA1
    // Ngày chỉnh sửa   : 12/11/2026
    // ==============================================================
    /// <summary>
    /// Concrete Strategy: Chiến lược Bán Sỉ.
    /// Lấy giá bán sỉ (GiaBanSi) và lọc khách hàng đại lý (IsDaiLy = true).
    /// </summary>
    public class BanSiStrategy : IBanHangStrategy
    {
        public string TenChienLuoc
        {
            get { return "Bán sỉ"; }
        }

        public bool IsDaiLy
        {
            get { return true; }
        }

        public long LayDonGia(SanPham sp, MaSanPham masp)
        {
            if (sp != null)
            {
                return sp.GiaBanSi;
            }
            if (masp != null && masp.SanPham != null)
            {
                return masp.SanPham.GiaBanSi;
            }
            return 0;
        }
    }
}