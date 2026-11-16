using System;

namespace CuahangNongduoc.BusinessObject
{
    public class PhanBoLoXuat
    {
        public string MaLo;
        public string TenSanPham;
        public int SoLuong;
        public long GiaBan;
        public long GiaVonDonVi;
        public long GiaVonTong;
        public DateTime NgayHetHan;

        public long ThanhTien
        {
            get { return GiaBan * SoLuong; }
        }

        public long ThanhTienVon
        {
            get { return GiaVonTong; }
        }
    }
}
