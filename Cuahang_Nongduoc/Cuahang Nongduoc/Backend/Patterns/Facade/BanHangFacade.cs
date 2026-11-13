using CuahangNongduoc.BusinessObject;
using CuahangNongduoc.Controller;
using CuahangNongduoc.DataLayer; // Chứa MaSanPhanFactory
using CuahangNongduoc.Patterns.Singleton;
using CuahangNongduoc.Patterns.Strategy;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Data;

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

        // ==================== [FE-02] REFACTORING (HIDE DELEGATE) ====================
        // Gói gọn toàn bộ nghiệp vụ lưu phiếu bán:
        // - Tạo/cập nhật dòng Phiếu bán
        // - Hoàn tồn kho các sản phẩm trong danh sách đã xóa (bằng MaSanPhanFactory)
        // - Thực thi lưu Chi tiết và Phiếu bán xuống Database
        // =============================================================================
        public bool LuuHoaDon(string maPhieu, string maKhachHang, DateTime ngayBan,
                             decimal tongTien, decimal daTra, decimal conNo,
                             ChiTietPhieuBanController ctrlChiTiet,
                             PhieuBanController ctrlPhieuBan,
                             IList<MaSanPham> dsXoa,
                             bool isThemMoi)
        {
            try
            {
                if (isThemMoi)
                {
                    // 1. Kiểm tra trùng mã phiếu
                    if (ctrlPhieuBan.LayPhieuBan(maPhieu) != null)
                    {
                        MessageBox.Show("Mã Phiếu bán này đã tồn tại !", "Phiếu Bán", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }

                    // 2. Thêm dòng phiếu mới
                    DataRow row = ctrlPhieuBan.NewRow();
                    row["ID"] = maPhieu;
                    row["ID_KHACH_HANG"] = maKhachHang;
                    row["NGAY_BAN"] = ngayBan.Date;
                    row["TONG_TIEN"] = tongTien;
                    row["DA_TRA"] = daTra;
                    row["CON_NO"] = conNo;
                    ctrlPhieuBan.Add(row);

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
                            // Move Method & Hide Delegate: Ẩn gọi MaSanPhanFactory khỏi Form
                            MaSanPhanFactory.CapNhatSoLuong(masp.Id, masp.SoLuong);
                        }
                        dsXoa.Clear();
                    }
                    ctrlPhieuBan.Update();
                }

                // 5. Lưu xuống DB
                ctrlChiTiet.Save();
                ctrlPhieuBan.Save();

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi lưu phiếu bán: " + ex.Message, "Lỗi Facade", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        // ==================== [FE-02] REFACTORING (MOVE METHOD) ====================
        // Chuyển logic hoàn kho khi xóa Phiếu bán từ Form về Facade
        // ===========================================================================
        public bool XoaPhieuBan(string maPhieu, PhieuBanController ctrlPhieuBan)
        {
            try
            {
                ChiTietPhieuBanController ctrl = new ChiTietPhieuBanController();
                IList<ChiTietPhieuBan> ds = ctrl.ChiTietPhieuBan(maPhieu);
                foreach (ChiTietPhieuBan ct in ds)
                {
                    // Ẩn MaSanPhanFactory khỏi Form
                    MaSanPhanFactory.CapNhatSoLuong(ct.MaSanPham.Id, ct.SoLuong);
                }
                ctrlPhieuBan.Save();
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi xóa phiếu bán: " + ex.Message, "Lỗi Facade", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }
    }
}
