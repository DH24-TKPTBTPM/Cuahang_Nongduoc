using System;
using System.Collections.Generic;

namespace CuahangNongduoc.BusinessObject
{
    public class ThongTinChiPhiPhieuBan
    {
        public long TienHang { get; set; }
        public long TongDichVu { get; set; }
        public bool GiaoHang { get; set; }
        public decimal KhoangCachKm { get; set; }
        public long PhiVanChuyen { get; set; }
        public string LoaiGiamGia { get; set; }
        public decimal TyLeGiamGia { get; set; }
        public long TienGiamGia { get; set; }
        public long TongThanhToan { get; set; }
        public IList<DichVuPhieuBan> DichVu { get; set; }

        public ThongTinChiPhiPhieuBan()
        {
            DichVu = new List<DichVuPhieuBan>();
        }
    }
}
