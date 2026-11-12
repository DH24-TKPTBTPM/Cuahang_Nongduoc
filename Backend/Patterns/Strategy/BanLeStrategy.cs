using System;
using CuahangNongduoc.BusinessObject;

namespace CuahangNongduoc.Patterns.Strategy
{
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
