using System;
using System.Collections.Generic;
using System.Windows.Forms;
using CuahangNongduoc.Auth;
using CuahangNongduoc.Controller;

namespace CuahangNongduoc
{
    public partial class frmBaoCaoGiamGia : Form
    {
        private readonly BaoCaoGiamGiaController controller = new BaoCaoGiamGiaController();
        private IList<CuahangNongduoc.BusinessObject.ChiTietBaoCaoGiamGia> chiTietBaoCao;
        private IList<CuahangNongduoc.BusinessObject.BaoCaoGiamGiaNhanVien> tongHopBaoCao;
        private const string MaTatCaNhanVien = "ALL";
        private const string MaNhanVienChuaXacDinh = "NONE";
        private bool dangNapDanhSachNhanVien;

        private sealed class LuaChonNhanVien
        {
            public string Ma { get; set; }
            public string Ten { get; set; }
        }

        public frmBaoCaoGiamGia()
        {
            InitializeComponent();
            dgvNhanVien.AutoGenerateColumns = false;
            dgvHoaDon.AutoGenerateColumns = false;

            Load += (s, e) =>
            {
                if (!AuthorizationService.KiemTraVaThongBao(AuthorizationService.Permission.BaoCaoDoanhThu)) { Close(); return; }
                dtTuNgay.Value = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
                dtDenNgay.Value = DateTime.Today;
                TaiBaoCao();
                dtTuNgay.ValueChanged += (sender, args) => TaiBaoCao();
                dtDenNgay.ValueChanged += (sender, args) => TaiBaoCao();
            };
            cmbNhanVien.SelectedIndexChanged += CmbNhanVien_SelectedIndexChanged;
        }

        private void BtnXem_Click(object sender, EventArgs e) { TaiBaoCao(); }

        private void TaiBaoCao()
        {
            try
            {
                string maDaChon = cmbNhanVien.SelectedValue == null
                    ? MaTatCaNhanVien : Convert.ToString(cmbNhanVien.SelectedValue);
                tongHopBaoCao = controller.LayTongHop(dtTuNgay.Value.Date, dtDenNgay.Value.Date);
                chiTietBaoCao = controller.LayChiTiet(dtTuNgay.Value.Date, dtDenNgay.Value.Date);
                if (cmbNhanVien.DataSource == null) NapDanhSachNhanVien(maDaChon);
                HienThiBaoCaoTheoNhanVien();
            }
            catch (Exception ex) { MessageBox.Show("Không tải được báo cáo. Hãy kiểm tra migration và kết nối cơ sở dữ liệu.\n" + ex.Message, "Báo cáo giảm giá", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }

        private void NapDanhSachNhanVien(string maDaChon)
        {
            var data = new List<LuaChonNhanVien>
            {
                new LuaChonNhanVien { Ma = MaTatCaNhanVien, Ten = "Tất cả nhân viên" }
            };
            foreach (var nv in new NguoiDungData().LayTatCa())
                data.Add(new LuaChonNhanVien { Ma = "USER:" + nv.Id, Ten = String.IsNullOrWhiteSpace(nv.HoTen) ? nv.TenDangNhap : nv.HoTen });
            dangNapDanhSachNhanVien = true;
            try
            {
                cmbNhanVien.DataSource = data;
                cmbNhanVien.DisplayMember = "Ten";
                cmbNhanVien.ValueMember = "Ma";
                cmbNhanVien.SelectedValue = maDaChon;
                if (cmbNhanVien.SelectedIndex < 0) cmbNhanVien.SelectedIndex = 0;
            }
            finally { dangNapDanhSachNhanVien = false; }
        }

        private void CmbNhanVien_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!dangNapDanhSachNhanVien && tongHopBaoCao != null) HienThiBaoCaoTheoNhanVien();
        }

        private void HienThiBaoCaoTheoNhanVien()
        {
            string maNhanVien = cmbNhanVien.SelectedValue == null
                ? MaTatCaNhanVien : Convert.ToString(cmbNhanVien.SelectedValue);
            dgvNhanVien.DataSource = maNhanVien != MaTatCaNhanVien
                ? LocTongHopTheoNhanVien(maNhanVien) : tongHopBaoCao;
            dgvHoaDon.DataSource = LocChiTietTheoNhanVien(maNhanVien);
            HienThiTongHopNhanVien(true);
            DinhDangCot(dgvNhanVien); DinhDangCot(dgvHoaDon);
        }

        private IList<CuahangNongduoc.BusinessObject.BaoCaoGiamGiaNhanVien> LocTongHopTheoNhanVien(string maNhanVien)
        {
            var result = new List<CuahangNongduoc.BusinessObject.BaoCaoGiamGiaNhanVien>();
            if (tongHopBaoCao == null) return result;
            foreach (var nv in tongHopBaoCao)
                if (TaoMaNhanVien(nv.IdNguoiDung) == maNhanVien) result.Add(nv);
            return result;
        }

        private IList<CuahangNongduoc.BusinessObject.ChiTietBaoCaoGiamGia> LocChiTietTheoNhanVien(string maNhanVien)
        {
            var result = new List<CuahangNongduoc.BusinessObject.ChiTietBaoCaoGiamGia>();
            if (chiTietBaoCao == null) return result;
            foreach (var hd in chiTietBaoCao)
                if (maNhanVien == MaTatCaNhanVien || TaoMaNhanVien(hd.IdNguoiDung) == maNhanVien) result.Add(hd);
            return result;
        }

        private static string TaoMaNhanVien(int? idNguoiDung)
        {
            return idNguoiDung.HasValue ? "USER:" + idNguoiDung.Value : MaNhanVienChuaXacDinh;
        }

        private void HienThiTongHopNhanVien(bool hien)
        {
            dgvNhanVien.Visible = hien;
            layout.RowStyles[1].SizeType = SizeType.Absolute;
            layout.RowStyles[1].Height = hien ? 175F : 0F;
        }

        private static void DinhDangCot(DataGridView grid)
        {
            foreach (DataGridViewColumn col in grid.Columns)
            {
                string property = col.DataPropertyName;
                if (property == "IdNguoiDung") col.Visible = false;
                if (property == "TenDangNhap") { col.Visible = false; col.HeaderText = "Tài khoản"; }
                if (property == "HoTen") col.HeaderText = "Nhân viên lập hóa đơn";
                if (property == "SoHoaDon") col.HeaderText = "Số hóa đơn";
                if (property == "TongTienHang") col.HeaderText = "Tổng tiền hàng";
                if (property == "TongTienGiam") col.HeaderText = "Tổng tiền giảm";
                if (property == "TongThanhToan") col.HeaderText = "Tổng thanh toán";
                if (property == "IdPhieuBan") col.HeaderText = "Mã hóa đơn";
                if (property == "NgayBan") { col.HeaderText = "Ngày bán"; col.DefaultCellStyle.Format = "dd/MM/yyyy"; }
                if (property == "LoaiGiamGia") col.HeaderText = "Loại giảm";
                if (property == "TyLeGiamGia") col.HeaderText = "Tỷ lệ giảm (%)";
                if (property == "TienHang") col.HeaderText = "Tiền hàng";
                if (property == "TienGiamGia") col.HeaderText = "Tiền giảm";
                if (property == "TongTien") col.HeaderText = "Tổng thanh toán";
                if (property == "TongTienHang" || property == "TongTienGiam" || property == "TongThanhToan" || property == "TienHang" || property == "TienGiamGia" || property == "TongTien") col.DefaultCellStyle.Format = "N0";
            }
        }

 
    }

}
