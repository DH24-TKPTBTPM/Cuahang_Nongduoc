using CuahangNongduoc.BusinessObject;
using CuahangNongduoc.Controller;
using CuahangNongduoc.DataLayer;
using CuahangNongduoc.Patterns.Factory; // Import Factory ở Backend
using CuahangNongduoc.Patterns.Singleton;
using CuahangNongduoc.Patterns.Strategy;
using System;
using System.Collections.Generic;

namespace CuahangNongduoc.Patterns.Facade
{
    /// <summary>
    /// Facade Pattern (Structural): Cung cấp một giao diện cấp cao duy nhất,
    /// che giấu sự phức tạp của 5-6 Controller (SanPham, MaSanPham, PhieuBan, ChiTiet, ThamSo)
    /// khi thực hiện nghiệp vụ bán hàng.
    /// </summary>
    public class BanHangFacade
    {
        private SanPhamController _ctrlSanPham;
        private KhachHangController _ctrlKhachHang;
        private MaSanPhamController _ctrlMaSanPham;
        private PhieuBanController _ctrlPhieuBan;
        private ChiTietPhieuBanController _ctrlChiTiet;

        private MaSanPhanFactory _maSanPhanFactory;

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
            if (strategy == null) return 0;
            return strategy.LayDonGia(sp, masp);
        }

        // ==================== [FE02] REFACTORING ====================
        // Lỗi (Code smell) : Inappropriate Intimacy / Violating Facade - UI gọi trực tiếp Factory khởi tạo mã
        // Kỹ thuật         : Move Method (Di chuyển logic gọi Factory từ Form sang Facade)
        // Trước            : Form trực tiếp tạo MaSanPhamFactory và xử lý sinh mã từng sản phẩm
        // Sau              : Di chuyển MaSanPhamFactory.TaoMaSanPhamTuDong() vào tập trung trong LuuHoaDon()
        // Tác giả          : FE1 - Dương Thị Kim Anh
        // Hỗ trợ           : FE2 - Nguyễn Huỳnh Ngọc Anh 
        // Kiểm tra         : QA1 – <Họ và tên> (QA điền khi test đạt)
        // Ngày chỉnh sửa   : 10/11/2026
        // ===========================================================
        public bool LuuHoaDon(string maPhieu, string maKhachHang, DateTime ngayBan,
                              decimal tongTien, decimal daTra, decimal conNo,
                              IList<MaSanPham> dsXoa, bool isThemMoi)
        {
            try
            {
                if (isThemMoi)
                {
                    // 1. Kiểm tra trùng mã phiếu (Sử dụng Controller nội bộ của Facade)
                    DataTable dtPhieu = _ctrlPhieuBan.LayPhieuBan(maPhieu);
                    if (dtPhieu != null && dtPhieu.Rows.Count > 0) 
                    {
                        throw new Exception("Mã Phiếu bán này đã tồn tại!");
                    }

                    // 2. Thêm dòng phiếu mới
                    DataRow row = _ctrlPhieuBan.NewRow();
                    row["ID"] = maPhieu;
                    row["ID_KHACH_HANG"] = maKhachHang;
                    row["NGAY_BAN"] = ngayBan.Date;
                    row["TONG_TIEN"] = tongTien;
                    row["DA_TRA"] = daTra;
                    row["CON_NO"] = conNo;
                    _ctrlPhieuBan.Add(row);

                    // 3. Tăng mã tự động
                    if (ThamSo.LaSoNguyen(maPhieu))
                    {
                        long so = Convert.ToInt64(maPhieu);
                        if (so >= ThamSo.LayMaPhieuBan())
                        {
                            ThamSo.GanMaPhieuBan(so + 1);
                        }
                    }
                }
                else
                {
                    // 4. Nếu là cập nhật: Hoàn kho cho các sản phẩm đã bị xóa khỏi phiếu
                    if (dsXoa != null && dsXoa.Count > 0)
                    {
                        foreach (MaSanPham masp in dsXoa)
                        {
                            MaSanPhanFactory.CapNhatSoLuong(masp.Id, masp.SoLuong);
                        }
                        dsXoa.Clear();
                    }
                }

                // 5. Lưu xuống DB
                _ctrlChiTiet.Save();
                _ctrlPhieuBan.Save();

                return true;
            }
            catch (Exception ex)
            {
                // Ném lỗi lên cho UI tự hiện MessageBox
                throw new Exception("Lỗi xử lý lưu hóa đơn tại BanHangFacade: " + ex.Message);
            }
        }

        // ==================== [FE-02] REFACTORING (MOVE METHOD) ====================
        // Chuyển logic hoàn kho khi xóa Phiếu bán từ Form về Facade
        // ===========================================================================
        public bool XoaPhieuBan(string maPhieu)
        {
            try
            {
                // Sử dụng _ctrlChiTiet nội bộ của Facade thay vì tạo mới
                IList<ChiTietPhieuBan> ds = _ctrlChiTiet.ChiTietPhieuBan(maPhieu);
                foreach (ChiTietPhieuBan ct in ds)
                {
                    MaSanPhanFactory.CapNhatSoLuong(ct.MaSanPham.Id, ct.SoLuong);
                }
                _ctrlPhieuBan.Save();
                
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi xử lý xóa hóa đơn tại BanHangFacade: " + ex.Message);
            }
        }
    }
}
