using System;

namespace CuahangNongduoc.BusinessObject
{
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
