// ==================== [TM-01] REFACTORING ====================
// Kỹ thuật         : Template Method Pattern (GoF)
// Tác giả          : BE3 - Bùi Lê Tuấn Anh
// Hỗ trợ           : BE2 - Trần Tuấn Anh
// Kiểm duyệt       : QA1 - Huỳnh Lư Anh Minh
// Ngày chỉnh sửa   : 08/11/2026
// ==============================================================
using System;
using System.Collections.Generic;
using System.Data;
using CuahangNongduoc.BusinessObject;
using CuahangNongduoc.DataLayer;

namespace CuahangNongduoc.Patterns.TemplateMethod
{
    public class QuyTrinhBanHang : QuyTrinhXuLyPhieuBase
    {
        private PhieuBan _phieuBan;
        private IList<ChiTietPhieuBan> _dsChiTiet;
        private IList<MaSanPham> _dsXoa;

        public QuyTrinhBanHang(PhieuBan phieuBan, IList<ChiTietPhieuBan> dsChiTiet, IList<MaSanPham> dsXoa)
        {
            _phieuBan = phieuBan;
            _dsChiTiet = dsChiTiet;
            _dsXoa = dsXoa;
        }

        public QuyTrinhBanHang(PhieuBan phieuBan, IList<ChiTietPhieuBan> dsChiTiet)
            : this(phieuBan, dsChiTiet, null)
        {
        }

        protected override bool KiemTraHopLe()
        {
            if (_phieuBan == null || string.IsNullOrEmpty(_phieuBan.Id))
            {
                LastErrorMessage = "Mã phiếu bán không được để trống.";
                return false;
            }
            return true;
        }

        protected override bool CapNhatKho()
        {
            if (_dsXoa != null)
            {
                foreach (MaSanPham sp in _dsXoa)
                {
                    if (sp != null && !string.IsNullOrEmpty(sp.Id))
                        MaSanPhanFactory.CapNhatSoLuong(sp.Id, sp.SoLuong);
                }
            }

            if (_dsChiTiet != null)
            {
                foreach (ChiTietPhieuBan ct in _dsChiTiet)
                {
                    if (ct != null && ct.MaSanPham != null && !string.IsNullOrEmpty(ct.MaSanPham.Id))
                        MaSanPhanFactory.CapNhatSoLuong(ct.MaSanPham.Id, -ct.SoLuong);
                }
            }
            return true;
        }

        protected override bool GhiDuLieu()
        {
            PhieuBanFactory pbFactory = new PhieuBanFactory();
            DataTable tblPB = pbFactory.LayPhieuBan(_phieuBan.Id);
            DataRow rPB = (tblPB.Rows.Count == 0) ? pbFactory.NewRow() : tblPB.Rows[0];
            rPB["ID"] = _phieuBan.Id;
            if (_phieuBan.KhachHang != null) rPB["ID_KHACH_HANG"] = _phieuBan.KhachHang.Id;
            rPB["NGAY_BAN"] = _phieuBan.NgayBan.Date;
            rPB["TONG_TIEN"] = _phieuBan.TongTien;
            rPB["DA_TRA"] = _phieuBan.DaTra;
            rPB["CON_NO"] = _phieuBan.ConNo;
            if (tblPB.Rows.Count == 0) pbFactory.Add(rPB);

            if (!pbFactory.Save())
            {
                LastErrorMessage = "Không thể lưu phiếu bán vào cơ sở dữ liệu.";
                return false;
            }

            if (_dsChiTiet != null)
            {
                ChiTietPhieuBanFactory ctFactory = new ChiTietPhieuBanFactory();
                DataTable tblCT = ctFactory.LayChiTietPhieuBan(_phieuBan.Id);
                foreach (ChiTietPhieuBan ct in _dsChiTiet)
                {
                    DataRow rCT = ctFactory.NewRow();
                    rCT["ID_PHIEU_BAN"] = _phieuBan.Id;
                    rCT["ID_MA_SAN_PHAM"] = ct.MaSanPham.Id;
                    rCT["DON_GIA"] = ct.DonGia;
                    rCT["SO_LUONG"] = ct.SoLuong;
                    rCT["THANH_TIEN"] = ct.ThanhTien;
                    ctFactory.Add(rCT);
                }
                ctFactory.Save();
            }
            return true;
        }

        protected override void CapNhatCongNo()
        {
            if (_phieuBan == null || _phieuBan.KhachHang == null) return;
            try
            {
                DateTime ngay = _phieuBan.NgayBan.Date;
                DuNoKhachHangFactory dnFactory = new DuNoKhachHangFactory();
                DataTable tbl = dnFactory.LayDuNoKhachHang(_phieuBan.KhachHang.Id, ngay.Month, ngay.Year);
                if (tbl.Rows.Count > 0)
                {
                    tbl.Rows[0]["PHAT_SINH"] = PhieuBanFactory.LayConNo(_phieuBan.KhachHang.Id, ngay.Month, ngay.Year);
                    dnFactory.Save();
                }
            }
            catch
            {
            }
        }
    }
}
