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
            pb.ConNo = 0;
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
        public bool LuuHoaDon(PhieuBan phieuBan, IList<ChiTietPhieuBan> danhSachChiTiet, IList<MaSanPham> dsXoa)
        {
            // Facade tự chịu trách nhiệm điều phối: Lưu phiếu bán, lưu chi tiết, trừ/hoàn tồn kho
            try
            {
                // [Hide Delegate]: Duyệt danh sách sản phẩm bị xóa và gọi MaSanPhanFactory hoàn tồn kho tại Facade
                if (dsXoa != null && dsXoa.Count > 0)
                {
                    foreach (MaSanPham masp in dsXoa)
                    {
                        MaSanPhanFactory.CapNhatSoLuong(masp.Id, masp.SoLuong);
                    }
                }

                // Thực hiện lưu dữ liệu qua các Controller
                if (phieuBan != null)
                {
                    _ctrlPhieuBan.Save();
                    _ctrlChiTiet.Save();
                }

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi xử lý lưu hóa đơn tại BanHangFacade: " + ex.Message);
            }
        }
    }
}
