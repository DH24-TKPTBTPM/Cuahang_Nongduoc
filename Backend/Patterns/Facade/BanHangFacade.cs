using System;
using System.Collections.Generic;
using CuahangNongduoc.BusinessObject;
using CuahangNongduoc.Controller;
using CuahangNongduoc.Patterns.Strategy;
using CuahangNongduoc.Patterns.Singleton;

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

        public BanHangFacade()
        {
            _ctrlSanPham = new SanPhamController();
            _ctrlKhachHang = new KhachHangController();
            _ctrlMaSanPham = new MaSanPhamController();
            _ctrlPhieuBan = new PhieuBanController();
            _ctrlChiTiet = new ChiTietPhieuBanController();
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

        public bool LuuHoaDon(PhieuBan phieuBan, IList<ChiTietPhieuBan> danhSachChiTiet, IList<MaSanPham> dsXoa)
        {
            // Facade tự chịu trách nhiệm điều phối: Lưu phiếu bán, lưu chi tiết, trừ tồn kho, cập nhật nợ
            // Chi tiết cài đặt sẽ hoàn thiện ở Giai đoạn 3
            return true;
        }
    }
}
