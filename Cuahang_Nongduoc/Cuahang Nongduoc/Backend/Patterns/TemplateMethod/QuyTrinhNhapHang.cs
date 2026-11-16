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
using CuahangNongduoc.Controller;
using CuahangNongduoc.DataLayer;

namespace CuahangNongduoc.Patterns.TemplateMethod
{
    public class QuyTrinhNhapHang : QuyTrinhXuLyPhieuBase
    {
        private PhieuNhap _phieuNhap;
        private IList<ChiTietPhieuNhap> _dsChiTiet;

        public QuyTrinhNhapHang(PhieuNhap phieuNhap, IList<ChiTietPhieuNhap> dsChiTiet)
        {
            _phieuNhap = phieuNhap;
            _dsChiTiet = dsChiTiet;
        }

        protected override bool KiemTraHopLe()
        {
            if (_phieuNhap == null || string.IsNullOrEmpty(_phieuNhap.Id))
            {
                LastErrorMessage = "Mã phiếu nhập không được để trống.";
                return false;
            }
            return true;
        }

        protected override bool CapNhatKho()
        {
            if (_dsChiTiet != null)
            {
                SanPhamController ctrlSP = new SanPhamController();
                foreach (ChiTietPhieuNhap ct in _dsChiTiet)
                {
                    if (ct != null && ct.MaSanPham != null && !string.IsNullOrEmpty(ct.MaSanPham.Id))
                    {
                        MaSanPhanFactory.CapNhatSoLuong(ct.MaSanPham.Id, ct.SoLuong);
                        if (ct.MaSanPham.SanPham != null)
                            ctrlSP.CapNhatGiaNhap(ct.MaSanPham.SanPham.Id, ct.DonGia, ct.SoLuong);
                    }
                }
            }
            return true;
        }

        protected override bool GhiDuLieu()
        {
            PhieuNhapFactory pnFactory = new PhieuNhapFactory();
            DataTable tblPN = pnFactory.LayPhieuNhap(_phieuNhap.Id);
            DataRow rPN = (tblPN.Rows.Count == 0) ? pnFactory.NewRow() : tblPN.Rows[0];
            rPN["ID"] = _phieuNhap.Id;
            if (_phieuNhap.NhaCungCap != null) rPN["ID_NHA_CUNG_CAP"] = _phieuNhap.NhaCungCap.Id;
            rPN["NGAY_NHAP"] = _phieuNhap.NgayNhap.Date;
            rPN["TONG_TIEN"] = _phieuNhap.TongTien;
            rPN["DA_TRA"] = _phieuNhap.DaTra;
            rPN["CON_NO"] = _phieuNhap.ConNo;
            if (tblPN.Rows.Count == 0) pnFactory.Add(rPN);

            if (!pnFactory.Save())
            {
                LastErrorMessage = "Không thể lưu phiếu nhập vào cơ sở dữ liệu.";
                return false;
            }

            if (_dsChiTiet != null)
            {
                ChiTietPhieuNhapFactory ctFactory = new ChiTietPhieuNhapFactory();
                DataTable tblCT = ctFactory.LayChiTietPhieuNhap(_phieuNhap.Id);
                foreach (ChiTietPhieuNhap ct in _dsChiTiet)
                {
                    DataRow rCT = ctFactory.NewRow();
                    rCT["ID_PHIEU_NHAP"] = _phieuNhap.Id;
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
    }
}
