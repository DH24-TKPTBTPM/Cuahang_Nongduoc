using System;

namespace CuahangNongduoc.BusinessObject
{
    public class BaoCaoGiamGiaNhanVien
    {
        public int? IdNguoiDung { get; set; }
        public string TenDangNhap { get; set; }
        public string HoTen { get; set; }
        public int SoHoaDon { get; set; }
        public long TongTienHang { get; set; }
        public long TongTienGiam { get; set; }
        public long TongThanhToan { get; set; }
    }

    public class ChiTietBaoCaoGiamGia
    {
        public string IdPhieuBan { get; set; }
        public DateTime NgayBan { get; set; }
        public int? IdNguoiDung { get; set; }
        public string TenDangNhap { get; set; }
        public string HoTen { get; set; }
        public string LoaiGiamGia { get; set; }
        public decimal TyLeGiamGia { get; set; }
        public long TienHang { get; set; }
        public long TienGiamGia { get; set; }
        public long TongTien { get; set; }
    }

}
