using System;
using System.Collections.Generic;

namespace CuahangNongduoc.BusinessObject
{
    public class DichVuThongKeItem
    {
        public string TenDichVu { get; set; }
        public int SoLan { get; set; }
        public long TongTien { get; set; }
    }

    public class ThongKeChiPhiTonKho
    {
        public DateTime TuNgay { get; set; }
        public DateTime DenNgay { get; set; }
        public int SoHoaDon { get; set; }
        public long TongTienHang { get; set; }
        public long TongPhiVanChuyen { get; set; }
        public long TongPhiDichVu { get; set; }
        public long TongTienGiamGia { get; set; }
        public long TongDoanhThu { get; set; }

        // Alias properties for compatibility
        public long TongTienVanChuyen
        {
            get { return TongPhiVanChuyen; }
            set { TongPhiVanChuyen = value; }
        }

        public long TongTienDichVu
        {
            get { return TongPhiDichVu; }
            set { TongPhiDichVu = value; }
        }

        public IList<DichVuThongKeItem> ChiTietDichVu { get; set; }
        public IList<SoLuongTon> DanhSachTonKho { get; set; }

        public ThongKeChiPhiTonKho()
        {
            ChiTietDichVu = new List<DichVuThongKeItem>();
            DanhSachTonKho = new List<SoLuongTon>();
        }
    }
}
