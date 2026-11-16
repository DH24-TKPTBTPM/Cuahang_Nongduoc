
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Microsoft.Win32;
using CuahangNongduoc.Auth;

namespace CuahangNongduoc
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
        }
        private void mnuTaiKhoan_Click(object sender, EventArgs e)
        {
            /* Proxy Pattern: chặn thao tác mở form nếu tài khoản không có quyền. */
            var authorizationProxy = new AuthorizationProxy();
            if (!authorizationProxy.Require(
                AuthorizationService.Permission.QuanLyTaiKhoan,
                "Quản lý tài khoản"))
                return;

            using (var formQuanLyTaiKhoan = new frmQuanLyTaiKhoan())
            {
                formQuanLyTaiKhoan.ShowDialog(this);
            }
        }

        private void ApDungPhanQuyen()
        {
            var p = AuthorizationService.Permission.QuanLyTaiKhoan;
            mnuTaiKhoan.Visible = AuthorizationService.CoQuyen(p);
            itemNguoiDung.Visible = AuthorizationService.CoQuyen(p);
            toolNguoiDung.Visible = AuthorizationService.CoQuyen(p);
            mnuDoiMK.Visible = AuthorizationService.CoQuyen(AuthorizationService.Permission.DoiMatKhau);
            mnuDangNhap.Visible = !UserSession.IsLoggedIn;
            mnuDangXua.Visible = UserSession.IsLoggedIn;
            mnuDonViTinh.Visible = AuthorizationService.CoQuyen(AuthorizationService.Permission.XemDonViTinh);
            mnuSanPham.Visible = AuthorizationService.CoQuyen(AuthorizationService.Permission.XemSanPham);
            mnuKhachHang.Visible = AuthorizationService.CoQuyen(AuthorizationService.Permission.XemKhachHang);
            mnuDaiLy.Visible = AuthorizationService.CoQuyen(AuthorizationService.Permission.XemDaiLy);
            mnuNhapHang.Visible = AuthorizationService.CoQuyen(AuthorizationService.Permission.NhapHang);
            mnuBanHangKH.Visible = AuthorizationService.CoQuyen(AuthorizationService.Permission.BanHangLe);
            mnuBanHangDL.Visible = AuthorizationService.CoQuyen(AuthorizationService.Permission.BanHangSi);
            mnuThanhtoan.Visible = AuthorizationService.CoQuyen(AuthorizationService.Permission.PhieuThu);
            mnuTonghopDuno.Visible = AuthorizationService.CoQuyen(AuthorizationService.Permission.CongNo);
            mnuBaocaoSoluongton.Visible = AuthorizationService.CoQuyen(AuthorizationService.Permission.BaoCaoTonKho);
            mnuSoLuongBan.Visible = AuthorizationService.CoQuyen(AuthorizationService.Permission.BaoCaoSoLuongBan);
            mnuBaoCaoGiamGia.Visible = AuthorizationService.CoQuyen(AuthorizationService.Permission.BaoCaoDoanhThu);
            mnuSanphamHethan.Visible = AuthorizationService.CoQuyen(AuthorizationService.Permission.BaoCaoHetHan);
            mnuTuychinhThongtin.Visible = AuthorizationService.CoQuyen(AuthorizationService.Permission.CauHinhHeThong);
            mnuNhaCungCap.Visible = AuthorizationService.CoQuyen(AuthorizationService.Permission.XemNhaCungCap);
            mnuLyDoChi.Visible = AuthorizationService.CoQuyen(AuthorizationService.Permission.XemLyDoChi);
            mnuPhieuChi.Visible = AuthorizationService.CoQuyen(AuthorizationService.Permission.PhieuChi);
            toolSanPham.Visible = AuthorizationService.CoQuyen(AuthorizationService.Permission.XemSanPham);
            toolKhachHang.Visible = AuthorizationService.CoQuyen(AuthorizationService.Permission.XemKhachHang);
            toolDaiLy.Visible = AuthorizationService.CoQuyen(AuthorizationService.Permission.XemDaiLy);
            toolNhapHang.Visible = AuthorizationService.CoQuyen(AuthorizationService.Permission.NhapHang);
            toolBanSi.Visible = AuthorizationService.CoQuyen(AuthorizationService.Permission.BanHangSi);
            toolBanLe.Visible = AuthorizationService.CoQuyen(AuthorizationService.Permission.BanHangLe);
            toolTonKho.Visible = AuthorizationService.CoQuyen(AuthorizationService.Permission.BaoCaoTonKho);
            toolNhaCungCap.Visible = AuthorizationService.CoQuyen(AuthorizationService.Permission.XemNhaCungCap);
            toolPhieuChi.Visible = AuthorizationService.CoQuyen(AuthorizationService.Permission.PhieuChi);
            toolThanhtoan.Visible = AuthorizationService.CoQuyen(AuthorizationService.Permission.PhieuThu);
            mnuBanHang.Visible = mnuBanHangKH.Visible || mnuBanHangDL.Visible;
        }

        frmDonViTinh DonViTinh = null;

        private void mnuDonViTinh_Click(object sender, EventArgs e)
        {
            if (!AuthorizationService.KiemTraVaThongBao(AuthorizationService.Permission.XemDonViTinh)) return;
            if (DonViTinh == null || DonViTinh.IsDisposed)
            {
                DonViTinh = new frmDonViTinh();
                DonViTinh.MdiParent = this;
                DonViTinh.Show();

            }
            else
                DonViTinh.Activate();
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            //RegistryKey regKey = Registry.CurrentUser.OpenSubKey("Software\\CoolSoft\\CuahangNongduoc");

            //if (regKey == null)
            //{
            //    DataService.m_ConnectString = "";
            //}
            //else
            //{
            //    try
            //    {
            //        DataService.m_ConnectString = (String)regKey.GetValue("ConnectString");
            //    }
            //    catch
            //    {
            //    }
            //    finally
            //    {
            //        regKey.Close();
            //    }
            //}

            //if (DataService.OpenConnection() == false)
            //{
            //    MessageBox.Show("Không thể kết nối dữ liệu!", "Cua hang Nong duoc", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //    this.Close();
            //}

            if (!UserSession.IsLoggedIn) { Close(); return; }
            if (!DataService.OpenConnection())
            {
                MessageBox.Show("Không thể kết nối cơ sở dữ liệu.", "Lỗi kết nối", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Close(); return;
            }
            Text += " — " + UserSession.HoTen + " (" + UserSession.RoleDisplayName + ")";
            ApDungPhanQuyen();

        }
        frmSanPham SanPham = null;
        private void mnuSanPham_Click(object sender, EventArgs e)
        {
            if (!AuthorizationService.KiemTraVaThongBao(AuthorizationService.Permission.XemSanPham)) return;
            if (SanPham == null || SanPham.IsDisposed)
            {
                SanPham = new frmSanPham();
                SanPham.MdiParent = this;
                SanPham.Show();
            }
            else
                SanPham.Activate();
        }
        frmKhachHang KhachHang = null;
        private void mnuKhachHang_Click(object sender, EventArgs e)
        {
            if (!AuthorizationService.KiemTraVaThongBao(AuthorizationService.Permission.XemKhachHang)) return;
            if (KhachHang == null || KhachHang.IsDisposed)
            {
                KhachHang = new frmKhachHang();
                KhachHang.MdiParent = this;
                KhachHang.Show();
            }
            else
                KhachHang.Activate();
        }
        frmDaiLy DaiLy = null;
        private void mnuDaiLy_Click(object sender, EventArgs e)
        {
            if (!AuthorizationService.KiemTraVaThongBao(AuthorizationService.Permission.XemDaiLy)) return;
            if (DaiLy == null || DaiLy.IsDisposed)
            {
                DaiLy = new frmDaiLy();
                DaiLy.MdiParent = this;
                DaiLy.Show();
            }
            else
                DaiLy.Activate();

        }
        frmDanhsachPhieuNhap NhapHang = null;
        private void mnuNhapHang_Click(object sender, EventArgs e)
        {
            if (!AuthorizationService.KiemTraVaThongBao(AuthorizationService.Permission.NhapHang)) return;
            if (NhapHang == null || NhapHang.IsDisposed)
            {
                NhapHang = new frmDanhsachPhieuNhap();
                NhapHang.MdiParent = this;
                NhapHang.Show();
            }
            else
                NhapHang.Activate();
        }
        frmDanhsachPhieuBanLe BanLe = null;
        private void mnuBanHangKH_Click(object sender, EventArgs e)
        {
            if (!AuthorizationService.KiemTraVaThongBao(AuthorizationService.Permission.BanHangLe)) return;
            if (BanLe == null || BanLe.IsDisposed)
            {
                BanLe = new frmDanhsachPhieuBanLe();
                BanLe.MdiParent = this;
                BanLe.Show();
            }
            else
                BanLe.Activate();
        }
        frmDanhsachPhieuBanSi BanSi = null;
        private void mnuBanHangDL_Click(object sender, EventArgs e)
        {
            if (!AuthorizationService.KiemTraVaThongBao(AuthorizationService.Permission.BanHangSi)) return;
            if (BanSi == null || BanSi.IsDisposed)
            {
                BanSi = new frmDanhsachPhieuBanSi();
                BanSi.MdiParent = this;
                BanSi.Show();
            }
            else
                BanSi.Activate();
        }

        private void mnuThanhCongCu_Click(object sender, EventArgs e)
        {
            mnuThanhCongCu.Checked = !mnuThanhCongCu.Checked;
            toolStrip.Visible = mnuThanhCongCu.Checked;
        }

        private void mnuThanhChucNang_Click(object sender, EventArgs e)
        {
            mnuThanhChucNang.Checked = !mnuThanhChucNang.Checked;
            taskPane.Visible = mnuThanhChucNang.Checked;
        }
        frmThanhToan ThanhToan = null;
        private void mnuThanhtoan_Click(object sender, EventArgs e)
        {
            if (!AuthorizationService.KiemTraVaThongBao(AuthorizationService.Permission.PhieuThu)) return;
            if (ThanhToan == null || ThanhToan.IsDisposed)
            {
                ThanhToan = new frmThanhToan();
                ThanhToan.MdiParent = this;
                ThanhToan.Show();
            }
            else
                ThanhToan.Activate();
        }
        frmDunoKhachhang DunoKhachhang = null;
        private void mnuTonghopDuno_Click(object sender, EventArgs e)
        {
            if (!AuthorizationService.KiemTraVaThongBao(AuthorizationService.Permission.CongNo)) return;
            if (DunoKhachhang == null || DunoKhachhang.IsDisposed)
            {
                DunoKhachhang = new frmDunoKhachhang();
                DunoKhachhang.MdiParent = this;
                DunoKhachhang.Show();
            }
            else
                DunoKhachhang.Activate();
        }
        frmDoanhThu DoanhThu = null;
        private frmBaoCaoGiamGia BaoCaoGiamGia = null;
        private void mnuBaoCaoGiamGia_Click(object sender, EventArgs e)
        {
            if (!AuthorizationService.KiemTraVaThongBao(AuthorizationService.Permission.BaoCaoDoanhThu)) return;
            if (BaoCaoGiamGia == null || BaoCaoGiamGia.IsDisposed)
            {
                BaoCaoGiamGia = new frmBaoCaoGiamGia(); BaoCaoGiamGia.MdiParent = this; BaoCaoGiamGia.Show();
            }
            else BaoCaoGiamGia.Activate();
        }
        private void mnuBaocaoDoanhThu_Click(object sender, EventArgs e)
        {
            if (!AuthorizationService.KiemTraVaThongBao(AuthorizationService.Permission.BaoCaoDoanhThu)) return;
            if (DoanhThu == null || DoanhThu.IsDisposed)
            {
                DoanhThu = new frmDoanhThu();
                DoanhThu.MdiParent = this;
                DoanhThu.Show();
            }
            else
                DoanhThu.Activate();

        }

        frmSoLuongTon SoLuongTon = null;
        private void mnuBaocaoSoluongton_Click(object sender, EventArgs e)
        {
            if (!AuthorizationService.KiemTraVaThongBao(AuthorizationService.Permission.BaoCaoTonKho)) return;

            if (SoLuongTon == null || SoLuongTon.IsDisposed)
            {
                SoLuongTon = new frmSoLuongTon();
                SoLuongTon.MdiParent = this;
                SoLuongTon.Show();
            }
            else
                SoLuongTon.Activate();

        }
        frmSoLuongBan SoLuongBan = null;
        private void mnuSoLuongBan_Click(object sender, EventArgs e)
        {
            if (!AuthorizationService.KiemTraVaThongBao(AuthorizationService.Permission.BaoCaoSoLuongBan)) return;
            if (SoLuongBan == null || SoLuongBan.IsDisposed)
            {
                SoLuongBan = new frmSoLuongBan();
                SoLuongBan.MdiParent = this;
                SoLuongBan.Show();
            }
            else
                SoLuongBan.Activate();
        }
        frmSanphamHethan SanphamHethan = null;
        private void mnuSanphamHethan_Click(object sender, EventArgs e)
        {
            if (!AuthorizationService.KiemTraVaThongBao(AuthorizationService.Permission.BaoCaoHetHan)) return;
            if (SanphamHethan == null || SanphamHethan.IsDisposed)
            {
                SanphamHethan = new frmSanphamHethan();
                SanphamHethan.MdiParent = this;
                SanphamHethan.Show();
            }
            else
                SanphamHethan.Activate();
        }

        private void mnuThoat_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        frmThongtinCuahang ThongtinCuahang = null;
        private void mnuTuychinhThongtin_Click(object sender, EventArgs e)
        {
            if (!AuthorizationService.KiemTraVaThongBao(AuthorizationService.Permission.CauHinhHeThong)) return;

            if (ThongtinCuahang == null || ThongtinCuahang.IsDisposed)
            {
                ThongtinCuahang = new frmThongtinCuahang();
                ThongtinCuahang.MdiParent = this;
                ThongtinCuahang.Show();
            }
            else
                ThongtinCuahang.Activate();
        }
        frmThongtinLienhe ThongtinLienhe = null;
        private void mnuTrogiupLienhe_Click(object sender, EventArgs e)
        {
            if (ThongtinLienhe == null || ThongtinLienhe.IsDisposed)
            {
                ThongtinLienhe = new frmThongtinLienhe();
                ThongtinLienhe.MdiParent = this;
                ThongtinLienhe.Show();
            }
            else
                ThongtinLienhe.Activate();
        }

        frmNhaCungCap NhaCungCap = null;
        private void mnuNhaCungCap_Click(object sender, EventArgs e)
        {
            if (!AuthorizationService.KiemTraVaThongBao(AuthorizationService.Permission.XemNhaCungCap)) return;
            if (NhaCungCap == null || NhaCungCap.IsDisposed)
            {
                NhaCungCap = new frmNhaCungCap();
                NhaCungCap.MdiParent = this;
                NhaCungCap.Show();
            }
            else
                NhaCungCap.Activate();
        }
        frmLyDoChi LyDoChi = null;
        private void mnuLyDoChi_Click(object sender, EventArgs e)
        {
            if (!AuthorizationService.KiemTraVaThongBao(AuthorizationService.Permission.XemLyDoChi)) return;
            if (LyDoChi == null || LyDoChi.IsDisposed)
            {
                LyDoChi = new frmLyDoChi();
                LyDoChi.MdiParent = this;
                LyDoChi.Show();
            }
            else
                LyDoChi.Activate();
        }

        frmPhieuChi PhieuChi = null;

        private void mnuDangXua_Click(object sender, EventArgs e)
        {
            DialogResult ketQua = MessageBox.Show(
                "Bạn có chắc muốn đăng xuất không?",
                "Xác nhận đăng xuất",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (ketQua != DialogResult.Yes)
                return;

            new AuthenticationService().DangXuat();
            Close();
        }

        private void mnuDoiMK_Click(object sender, EventArgs e)
        {
            if (!AuthorizationService.KiemTraVaThongBao(
                AuthorizationService.Permission.DoiMatKhau,
                "Đổi mật khẩu"))
                return;

            using (var formDoiMatKhau = new frmDoiMatKhau())
            {
                formDoiMatKhau.ShowDialog(this);
            }
        }

        private void mnuDangNhap_Click(object sender, EventArgs e)
        {
            if (UserSession.IsLoggedIn)
            {
                MessageBox.Show(
                    "Bạn đang đăng nhập với tài khoản: " + UserSession.TenDangNhap,
                    "Thông tin đăng nhập",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                return;
            }

            using (var formDangNhap = new frmDangNhap())
            {
                if (formDangNhap.ShowDialog(this) == DialogResult.OK && UserSession.IsLoggedIn)
                {
                    Text = "Cửa hàng nông dược — " + UserSession.HoTen +
                        " (" + UserSession.RoleDisplayName + ")";
                    ApDungPhanQuyen();
                }
            }
        }

        private void mnuPhieuChi_Click(object sender, EventArgs e)
        {
            if (!AuthorizationService.KiemTraVaThongBao(AuthorizationService.Permission.PhieuChi)) return;
            if (PhieuChi == null || PhieuChi.IsDisposed)
            {
                PhieuChi = new frmPhieuChi();
                PhieuChi.MdiParent = this;
                PhieuChi.Show();
            }
            else
                PhieuChi.Activate();
        }

        private frmHuongDanSuDung HuongDan = null;
        private void mnuTrogiupHuongdan_Click(object sender, EventArgs e)
        {
            if (HuongDan == null || HuongDan.IsDisposed)
            {
                HuongDan = new frmHuongDanSuDung();
                HuongDan.Show();
            }
            else
            {
                HuongDan.Activate();
            }
        }

        private void itemDoiMatKhau_Click(object sender, EventArgs e)
        {
            if (!AuthorizationService.KiemTraVaThongBao(
               AuthorizationService.Permission.DoiMatKhau,
               "Đổi mật khẩu"))
                return;

            using (var formDoiMatKhau = new frmDoiMatKhau())
            {
                formDoiMatKhau.ShowDialog(this);
            }
        }

        private void itemDangXuat_Click(object sender, EventArgs e)
        {
            DialogResult ketQua = MessageBox.Show(
                "Bạn có chắc muốn đăng xuất không?",
                "Xác nhận đăng xuất",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (ketQua != DialogResult.Yes)
                return;

            new AuthenticationService().DangXuat();
            Close();
        }

        private void itemNguoiDung_Click(object sender, EventArgs e)
        {
            if (!AuthorizationService.KiemTraVaThongBao(
               AuthorizationService.Permission.QuanLyTaiKhoan,
               "Quản lý tài khoản"))
                return;

            using (var formQuanLyTaiKhoan = new frmQuanLyTaiKhoan())
            {
                formQuanLyTaiKhoan.ShowDialog(this);
            }
        }

        private void toolNguoiDung_Click(object sender, EventArgs e)
        {
            if (!AuthorizationService.KiemTraVaThongBao(
              AuthorizationService.Permission.QuanLyTaiKhoan,
              "Quản lý tài khoản"))
                return;

            using (var formQuanLyTaiKhoan = new frmQuanLyTaiKhoan())
            {
                formQuanLyTaiKhoan.ShowDialog(this);
            }
        }
    }

}
