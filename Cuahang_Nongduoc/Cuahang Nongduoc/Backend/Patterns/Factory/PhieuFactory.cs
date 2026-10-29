using System;
using CuahangNongduoc.BusinessObject;
using CuahangNongduoc.Patterns.Singleton;

namespace CuahangNongduoc.Patterns.Factory
{
    public enum LoaiPhieu
    {
        BanLe,
        BanSi,
        NhapHang,
        ChiTien,
        ThanhToan
    }

    /// <summary>
    /// Factory Method Pattern (Creational): Đóng gói cơ chế khởi tạo các đối tượng chứng từ
    /// (Phiếu bán, Phiếu nhập, Phiếu chi, Phiếu thanh toán).
    /// Chuẩn hóa định nghĩa Factory chuẩn GoF, phân biệt hoàn toàn với các lớp DAO/DataLayer.
    /// </summary>
    public static class PhieuFactory
    {
        public static PhieuBan TaoPhieuBan(LoaiPhieu loai)
        {
            PhieuBan pb = new PhieuBan();
            pb.Id = ThamSoManager.Instance.LayMaPhieuBan().ToString();
            pb.NgayBan = DateTime.Now;
            pb.TongTien = 0;
            pb.DaTra = 0;
            pb.ConNo = 0;
            return pb;
        }

        public static PhieuNhap TaoPhieuNhap()
        {
            PhieuNhap pn = new PhieuNhap();
            pn.Id = ThamSoManager.Instance.LayMaPhieuNhap().ToString();
            pn.NgayNhap = DateTime.Now;
            pn.TongTien = 0;
            pn.DaTra = 0;
            pn.ConNo = 0;
            return pn;
        }

        public static PhieuChi TaoPhieuChi()
        {
            PhieuChi pc = new PhieuChi();
            pc.NgayChi = DateTime.Now;
            pc.TongTien = 0;
            return pc;
        }

        public static PhieuThanhToan TaoPhieuThanhToan()
        {
            PhieuThanhToan ptt = new PhieuThanhToan();
            ptt.NgayThanhToan = DateTime.Now;
            ptt.TongTien = 0;
            return ptt;
        }
    }
}
