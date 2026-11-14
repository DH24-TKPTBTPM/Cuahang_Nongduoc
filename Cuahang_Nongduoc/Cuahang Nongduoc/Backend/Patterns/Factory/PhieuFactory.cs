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

    // ==================== [FM-01] REFACTORING ====================
    // Kỹ thuật         : Factory Method Pattern (GoF)
    // Mục đích         : Dùng PhieuFactory thay vì new PhieuBan()/PhieuNhap()/PhieuChi() trong Form
    // Tác giả          : BE3 - Bùi Lê Tuấn Anh
    // Hỗ trợ           : FE2 - Nguyễn Huỳnh Ngọc Anh
    // Kiểm duyệt       : QA1 - Huỳnh Lư Anh Minh
    // Ngày chỉnh sửa   : 08/11/2026
    // ==============================================================
    public static class PhieuFactory
    {
        public static PhieuBan TaoPhieuBan(LoaiPhieu loai)
        {
            PhieuBan pb = new PhieuBan();
            pb.Id = ThamSoManager.Instance.LayMaPhieuBan().ToString();
            pb.NgayBan = DateTime.Now;
            pb.TongTien = 0;
            pb.DaTra = 0;
            return pb;
        }

        public static PhieuNhap TaoPhieuNhap()
        {
            PhieuNhap pn = new PhieuNhap();
            pn.Id = ThamSoManager.Instance.LayMaPhieuNhap().ToString();
            pn.NgayNhap = DateTime.Now;
            pn.TongTien = 0;
            pn.DaTra = 0;
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
