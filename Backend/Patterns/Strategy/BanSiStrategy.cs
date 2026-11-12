using System;
using CuahangNongduoc.BusinessObject;

namespace CuahangNongduoc.Patterns.Strategy
{
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
