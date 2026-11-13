using System;
using System.Collections.Generic;
using System.Data;
using CuahangNongduoc.BusinessObject;
using CuahangNongduoc.Controller;
using CuahangNongduoc.DataLayer;
using CuahangNongduoc.Patterns.Singleton;
using CuahangNongduoc.Patterns.Strategy;
using CuahangNongduoc.Patterns.Factory; // Import Factory ở Backend


namespace CuahangNongduoc.Patterns.Facade
{
    /// <summary>
    /// Facade Pattern (Structural): Cung cấp một giao diện cấp cao duy nhất,
    /// che giấu sự phức tạp của 5-6 Controller (SanPham, MaSanPham, PhieuBan, ChiTiet, ThamSo)
    /// khi thực hiện nghiệp vụ bán hàng.
    /// </summary>
    // ==================== [D11-BH] DESIGN PATTERN ====================
    // Mẫu thiết kế     : Facade (Structural, GoF - https://refactoring.guru/design-patterns/facade)
    // Mục đích         : Cung cấp giao diện cấp cao duy nhất cho Form bán hàng, gom Lưu phiếu bán + chi tiết + trừ/hoàn tồn kho + công nợ vào 1 UnitOfWork
    // Thay thế         : Stub LuuHoaDon() trả về true và các lời gọi rời rạc không transaction trên Form
    // Tác giả          : BE2
    // Hỗ trợ           : BE1
    // Kiểm tra         : QA2
    // Ngày tạo         : 12/11/2026
    // =================================================================
    public class BanHangFacade
    {
        private SanPhamController _ctrlSanPham;
        private KhachHangController _ctrlKhachHang;
        private MaSanPhamController _ctrlMaSanPham;
        private PhieuBanController _ctrlPhieuBan;
        private ChiTietPhieuBanController _ctrlChiTiet;

        public string LastErrorMessage { get; private set; }

        public BanHangFacade()
        {
            _ctrlSanPham = new SanPhamController();
            _ctrlKhachHang = new KhachHangController();
            _ctrlMaSanPham = new MaSanPhamController();
            _ctrlPhieuBan = new PhieuBanController();
            _ctrlChiTiet = new ChiTietPhieuBanController();

            _maSanPhanFactory = new MaSanPhanFactory();
        }

        public PhieuBan KhoiTaoPhieuMoi(IBanHangStrategy strategy)
        {
            PhieuBan pb = new PhieuBan();
            pb.Id = ThamSoManager.Instance.LayMaPhieuBan().ToString();
            pb.NgayBan = DateTime.Now;
            pb.TongTien = 0;
            pb.DaTra = 0;
            return pb;
        }

        public long LayDonGiaTheoChienLuoc(IBanHangStrategy strategy, SanPham sp, MaSanPham masp)
        {
            return strategy == null ? 0 : strategy.LayDonGia(sp, masp);
        }

        // ==================== [D11-BH] REFACTORING ====================
        // Lỗi (Code smell) : Stub Implementation (FE BUG-02) – LuuHoaDon() chỉ là khung stub return true, chưa điều phối các bảng trong transaction
        // Kỹ thuật         : Facade Pattern (GoF - https://refactoring.guru/design-patterns/facade)
        // Trước            : public bool LuuHoaDon(...) { return true; } // Chưa lưu phiếu, chi tiết, tồn kho và công nợ
        // Sau              : Điều phối đồng bộ Phiếu bán + Chi tiết + Tồn kho (ChiTietPhieuBanFactory.Save) + Công nợ (DU_NO_KH) trong 1 UnitOfWork (SqlTransaction)
        // Tác giả          : BE2
        // Hỗ trợ           : BE1
        // Kiểm tra         : QA2
        // Ngày chỉnh sửa   : 12/11/2026
        // ==============================================================
  // ==================== [FE02] REFACTORING ====================
        // Lỗi (Code smell) : Inappropriate Intimacy / Violating Facade - UI gọi trực tiếp Factory khởi tạo mã
        // Kỹ thuật         : Move Method (Di chuyển logic gọi Factory từ Form sang Facade)
        // Trước            : Form trực tiếp tạo MaSanPhamFactory và xử lý sinh mã từng sản phẩm
        // Sau              : Di chuyển MaSanPhamFactory.TaoMaSanPhamTuDong() vào tập trung trong LuuHoaDon()
        // Tác giả          : FE1 - Dương Thị Kim Anh
        // Hỗ trợ           : FE2 - Nguyễn Huỳnh Ngọc Anh 
        // Kiểm tra         : QA1 – <Họ và tên> (QA điền khi test đạt)
        // Ngày chỉnh sửa   : 10/11/2026
        public bool LuuHoaDon(PhieuBan phieuBan, IList<ChiTietPhieuBan> danhSachChiTiet, IList<MaSanPham> dsXoa)
        {
            LastErrorMessage = null;
            if (!HopLe(phieuBan, danhSachChiTiet, dsXoa)) return false;

            try
            {
                using (IUnitOfWork uow = new UnitOfWork())
                {
                    bool laThemMoi;
                    string khCu;
                    DateTime ngayCu;

                    // 1. Lưu phiếu bán + cập nhật mã phiếu tự động
                    if (!LuuPhieuBan(phieuBan, out laThemMoi, out khCu, out ngayCu))
                    {
                        uow.Rollback();
                        return false;
                    }

                    // 2. Lưu chi tiết + đồng bộ trừ/hoàn tồn kho qua ChiTietPhieuBanFactory
                    LuuChiTietVaTonKho(phieuBan.Id, laThemMoi, danhSachChiTiet, dsXoa);

                    // 3. Cập nhật dư nợ khách hàng (và kỳ cũ nếu sửa đổi KH/ngày)
                    DateTime ngayMoi = ChuanHoaNgay(phieuBan.NgayBan);
                    CapNhatCongNo(phieuBan.KhachHang.Id, ngayMoi.Month, ngayMoi.Year);
                    if (!laThemMoi && !string.IsNullOrEmpty(khCu) &&
                        (khCu != phieuBan.KhachHang.Id || ngayCu.Month != ngayMoi.Month || ngayCu.Year != ngayMoi.Year))
                    {
                        CapNhatCongNo(khCu, ngayCu.Month, ngayCu.Year);
                    }

                    uow.Commit();
                }
                return true;
            }
            catch (Exception ex)
            {
                LastErrorMessage = "Lỗi lưu hóa đơn: " + ex.Message;
                AppLogger.Instance.Error("BanHangFacade.LuuHoaDon thất bại (đã Rollback)", ex);
                return false;
            }
        }

        // ==================== [FE-02] REFACTORING (MOVE METHOD) ====================
        // Chuyển logic hoàn kho khi xóa Phiếu bán từ Form về Facade
        // ===========================================================================
        public bool XoaPhieuBan(string maPhieu)
        {
            LastErrorMessage = null;
            try
            {
                using (IUnitOfWork uow = new UnitOfWork())
                {
                    // 1. Lấy thông tin phiếu bán cũ
                    PhieuBanFactory pbFactory = new PhieuBanFactory();
                    DataTable tblPhieu = pbFactory.LayPhieuBan(maPhieu);

                    if (tblPhieu.Rows.Count == 0)
                    {
                        LastErrorMessage = "Phiếu bán không tồn tại.";
                        return false;
                    }

                    string idKH = Convert.ToString(tblPhieu.Rows[0]["ID_KHACH_HANG"]);
                    DateTime ngayBan = ChuanHoaNgay(Convert.ToDateTime(tblPhieu.Rows[0]["NGAY_BAN"]));

                    // 2. Xóa chi tiết phiếu bán (ChiTietPhieuBanFactory tự trigger hoàn tồn kho)
                    ChiTietPhieuBanFactory ctFactory = new ChiTietPhieuBanFactory();
                    DataTable tblCT = ctFactory.LayChiTietPhieuBan(maPhieu);
                    foreach (DataRow row in tblCT.Rows)
                    {
                        row.Delete();
                    }
                    if (tblCT.GetChanges() != null && !ctFactory.Save())
                        throw new DataAccessException("Lỗi khi xóa chi tiết phiếu và hoàn tồn kho.", null);

                    // 3. Xóa phiếu bán gốc
                    tblPhieu.Rows[0].Delete();
                    if (tblPhieu.GetChanges() != null && !pbFactory.Save())
                        throw new DataAccessException("Lỗi khi xóa dòng phiếu bán.", null);

                    // 4. Tính toán lại Công nợ khách hàng sau khi xóa phiếu
                    CapNhatCongNo(idKH, ngayBan.Month, ngayBan.Year);

                    // 5. Commit giao dịch an toàn
                    uow.Commit();
                    return true;
                }
            }
            catch (Exception ex)
            {
                LastErrorMessage = "Lỗi khi xóa hóa đơn: " + ex.Message;
                AppLogger.Instance.Error("BanHangFacade.XoaPhieuBan thất bại (đã Rollback)", ex);
                return false;
            }
        }

        private bool HopLe(PhieuBan pb, IList<ChiTietPhieuBan> dsCT, IList<MaSanPham> dsXoa)
        {
            if (pb == null || string.IsNullOrEmpty(pb.Id) || pb.KhachHang == null || string.IsNullOrEmpty(pb.KhachHang.Id))
            {
                LastErrorMessage = "Thiếu thông tin phiếu bán hoặc khách hàng.";
                return false;
            }
            if ((dsCT == null || dsCT.Count == 0) && (dsXoa == null || dsXoa.Count == 0))
            {
                LastErrorMessage = "Phiếu bán phải có ít nhất một chi tiết sản phẩm.";
                return false;
            }
            if (dsCT != null)
            {
                foreach (ChiTietPhieuBan ct in dsCT)
                {
                    if (ct == null || ct.MaSanPham == null || string.IsNullOrEmpty(ct.MaSanPham.Id) || ct.SoLuong <= 0 || ct.DonGia < 0)
                    {
                        LastErrorMessage = "Chi tiết sản phẩm không hợp lệ.";
                        return false;
                    }
                }
            }
            return true;
        }

        private bool LuuPhieuBan(PhieuBan pb, out bool laThemMoi, out string khCu, out DateTime ngayCu)
        {
            PhieuBanFactory pbFactory = new PhieuBanFactory();
            DataTable tbl = pbFactory.LayPhieuBan(pb.Id);
            laThemMoi = (tbl.Rows.Count == 0);
            DateTime ngayBan = ChuanHoaNgay(pb.NgayBan);
            khCu = laThemMoi || tbl.Rows[0]["ID_KHACH_HANG"] == DBNull.Value ? null : Convert.ToString(tbl.Rows[0]["ID_KHACH_HANG"]);
            ngayCu = laThemMoi || tbl.Rows[0]["NGAY_BAN"] == DBNull.Value ? ngayBan : ChuanHoaNgay(Convert.ToDateTime(tbl.Rows[0]["NGAY_BAN"]));

            if (laThemMoi && PhieuBanFactory.LaySoPhieu() >= 50)
            {
                LastErrorMessage = "Đây là bản dùng thử! Chỉ lưu được 50 phiếu bán!";
                return false;
            }

            DataRow row = laThemMoi ? pbFactory.NewRow() : tbl.Rows[0];
            if (laThemMoi) row["ID"] = pb.Id;
            row["ID_KHACH_HANG"] = pb.KhachHang.Id;
            row["NGAY_BAN"] = ngayBan;
            row["TONG_TIEN"] = pb.TongTien;
            row["DA_TRA"] = pb.DaTra;
            row["CON_NO"] = pb.ConNo;
            if (laThemMoi) pbFactory.Add(row);

            if (tbl.GetChanges() != null && !pbFactory.Save())
                throw new DataAccessException("Không thể lưu phiếu bán.", null);

            long soPhieu;
            if (laThemMoi && long.TryParse(pb.Id, out soPhieu) && soPhieu >= ThamSoManager.Instance.LayMaPhieuBan())
                ThamSoManager.Instance.CapNhatMaPhieuBan(soPhieu + 1);

            return true;
        }

        private void LuuChiTietVaTonKho(string idPB, bool laThemMoi, IList<ChiTietPhieuBan> dsCT, IList<MaSanPham> dsXoa)
        {
            ChiTietPhieuBanFactory ctFactory = new ChiTietPhieuBanFactory();
            DataTable tblCT = ctFactory.LayChiTietPhieuBan(idPB);

            if (!laThemMoi && dsXoa != null)
            {
                foreach (MaSanPham masp in dsXoa)
                {
                    DataRow rowXoa = (masp == null || string.IsNullOrEmpty(masp.Id)) ? null : TimDongChiTiet(tblCT, masp.Id);
                    if (rowXoa != null) rowXoa.Delete();
                }
            }

            if (dsCT != null)
            {
                HashSet<string> daXuLy = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
                foreach (ChiTietPhieuBan ct in dsCT)
                {
                    string idMaSP = ct.MaSanPham.Id;
                    DataRow row = TimDongChiTiet(tblCT, idMaSP);
                    bool laMoi = (row == null);
                    if (laMoi)
                    {
                        row = ctFactory.NewRow();
                        row["ID_PHIEU_BAN"] = idPB;
                        row["ID_MA_SAN_PHAM"] = idMaSP;
                    }
                    bool gopTrung = !laMoi && daXuLy.Contains(idMaSP);
                    row["DON_GIA"] = ct.DonGia;
                    row["SO_LUONG"] = gopTrung ? checked((int)(Convert.ToInt64(row["SO_LUONG"]) + ct.SoLuong)) : ct.SoLuong;
                    row["THANH_TIEN"] = gopTrung ? Convert.ToInt64(row["THANH_TIEN"]) + ct.ThanhTien : ct.ThanhTien;
                    if (laMoi) ctFactory.Add(row);
                    daXuLy.Add(idMaSP);
                }
            }

            if (tblCT.GetChanges() != null && !ctFactory.Save())
                throw new DataAccessException("Không thể lưu chi tiết phiếu bán và cập nhật tồn kho.", null);
        }

        private static DataRow TimDongChiTiet(DataTable tblCT, string idMaSP)
        {
            foreach (DataRow r in tblCT.Rows)
            {
                if (r.RowState != DataRowState.Deleted && string.Equals(Convert.ToString(r["ID_MA_SAN_PHAM"]), idMaSP, StringComparison.OrdinalIgnoreCase))
                    return r;
            }
            return null;
        }

        private void CapNhatCongNo(string idKH, int thang, int nam)
        {
            int thangTruoc = 0, namTruoc = 0;
            ThamSo.PreMonth(ref thangTruoc, ref namTruoc, thang, nam);
            long dauKy = DuNoKhachHangFactory.LayDuNo(idKH, thangTruoc, namTruoc);
            long phatSinh = PhieuBanFactory.LayConNo(idKH, thang, nam);
            long daTra = PhieuThanhToanFactory.LayTongTien(idKH, thang, nam);
            long cuoiKy = dauKy + phatSinh - daTra;

            DuNoKhachHangFactory dnFactory = new DuNoKhachHangFactory();
            DataTable tblDN = dnFactory.LayDuNoKhachHang(idKH, thang, nam);
            bool laMoi = (tblDN.Rows.Count == 0);
            if (laMoi && dauKy == 0 && phatSinh == 0 && daTra == 0 && cuoiKy == 0) return;

            DataRow row = laMoi ? dnFactory.NewRow() : tblDN.Rows[0];
            if (laMoi) { row["ID_KHACH_HANG"] = idKH; row["THANG"] = thang; row["NAM"] = nam; }
            row["DAU_KY"] = dauKy;
            row["PHAT_SINH"] = phatSinh;
            row["DA_TRA"] = daTra;
            row["CUOI_KY"] = cuoiKy;
            if (laMoi) dnFactory.Add(row);

            if (tblDN.GetChanges() != null && !dnFactory.Save())
                throw new DataAccessException("Không thể cập nhật dư nợ khách hàng.", null);
        }

        private static DateTime ChuanHoaNgay(DateTime ngay)
        {
            return (ngay < new DateTime(1753, 1, 1)) ? DateTime.Today : ngay.Date;
        }
    }
}
