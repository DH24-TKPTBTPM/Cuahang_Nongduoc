using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using CuahangNongduoc.BusinessObject;
using CuahangNongduoc.Controller;
using CuahangNongduoc.DataLayer;

namespace CuahangNongduoc
{
    public partial class frmBanSi : Form
    {
        private readonly SanPhamController ctrlSanPham = new SanPhamController();
        private readonly KhachHangController ctrlKhachHang = new KhachHangController();
        private readonly DaiLyController ctrlDaiLy = new DaiLyController();
        private readonly MaSanPhamController ctrlMaSanPham = new MaSanPhamController();
        private PhieuBanController ctrlPhieuBan = new PhieuBanController();
        private readonly BanLeRepository repository = new BanLeRepository();
        private readonly BindingSource dsChiTiet = new BindingSource();
        private readonly IDictionary<string, int> soLuongGocTheoLo = new Dictionary<string, int>();
        private Controll status = Controll.AddNew;
        private bool isBindingData;
        private bool daNapForm;
        private string maPhieuKhoiTao = null;
        private long daTraBanDau;
        private bool dangSuaThanhToanPhieuCu;
        private readonly IList<DichVuPhieuBan> dichVuDraft = new List<DichVuPhieuBan>();
        private ComboBox cmbLoaiGiamGia;
        private NumericUpDown numTyLeGiamGia;
        private NumericUpDown numKhoangCach;
        private NumericUpDown numPhiShipThuCong;
        private CheckBox chkGiaoHang;
        private TextBox txtDichVu;
        private NumericUpDown numTienDichVu;
        private NumericUpDown numTienBocXep;
        private NumericUpDown numTienDongGoi;
        private Button btnThemDichVu;
        private Button btnXoaDichVu;
        private Button btnThemBocXep;
        private Button btnThemDongGoi;
        private DataGridView dgvDichVu;
        private Label lblThanhToanChiTiet;
        private Label lblDichVuShipTomTat;
        private Label lblTienGiam;
        private Label lblTongThanhToanTomTat;
        private Label lblDaTraTomTat;
        private Label lblConNoTomTat;
        private Label lblKhoangCachKm;
        private Label lblLoaiGiamGia;
        private Label lblTyLeGiamGia;
        private Label lblPhiVanChuyen;
        private Label lblBocXep;
        private Label lblDongGoi;
        private Label lblDichVuKhac;
        private readonly InvoiceChangeSubject thayDoiHoaDon = new InvoiceChangeSubject();
        private readonly HoaDonBanFacade tinhHoaDonFacade = new HoaDonBanFacade();
        private HoaDonBanMediator dieuPhoiTinhHoaDon;

        public frmBanSi()
        {
            InitializeComponent();
            panel1.Resize += panel1_Resize;
            CanGiuaKhuVucNhap();
        }

        private void panel1_Resize(object sender, EventArgs e)
        {
            CanGiuaKhuVucNhap();
        }

        private void CanGiuaKhuVucNhap()
        {
            if (panel1 == null || groupBox2 == null || grpPhieuBanLe == null) return;

            int tongChieuRong = grpPhieuBanLe.Width + groupBox2.Width;
            int leTrai = Math.Max(0, (panel1.ClientSize.Width - tongChieuRong) / 2);
            grpPhieuBanLe.Location = new System.Drawing.Point(leTrai, 4);
            groupBox2.Location = new System.Drawing.Point(leTrai + grpPhieuBanLe.Width, 4);
        }

        public frmBanSi(string maPhieu) : this()
        {
            maPhieuKhoiTao = maPhieu;
            status = Controll.Normal;
        }

        public frmBanSi(PhieuBanController ctrlPB) : this()
        {
            ctrlPhieuBan = ctrlPB;
            status = Controll.Normal;
        }

        public void ChuyenDenPhieu(string maPhieu)
        {
            if (string.IsNullOrEmpty(maPhieu)) return;
            if (status == Controll.AddNew || status == Controll.Edit)
            {
                if (status == Controll.AddNew && ((DataTable)dsChiTiet.DataSource).Rows.Count == 0 && dichVuDraft.Count == 0)
                {
                    status = Controll.Normal;
                    dangSuaThanhToanPhieuCu = false;
                    Allow(false);
                    bindingNavigator.BindingSource?.CancelEdit();
                }
                else
                {
                    DialogResult result = MessageBox.Show("Lưu thay đổi hiện tại trước khi chuyển phiếu? Chọn No để bỏ thay đổi.", "Bán sỉ",
                        MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                    if (result == DialogResult.Cancel) return;
                    if (result == DialogResult.Yes)
                    {
                        if (!Luu()) return;
                        dangSuaThanhToanPhieuCu = false;
                        status = Controll.Normal;
                        soLuongGocTheoLo.Clear();
                        Allow(false);
                        RicarregarPhieu();
                    }
                    if (result == DialogResult.No)
                    {
                        dangSuaThanhToanPhieuCu = false;
                        status = Controll.Normal;
                        if (bindingNavigator.BindingSource != null && bindingNavigator.BindingSource.Current != null)
                            DoDuLieuTuLoiLenForm();
                    }
                }
            }
            if (bindingNavigator.BindingSource != null)
            {
                int pos = bindingNavigator.BindingSource.Find("ID", maPhieu);
                if (pos < 0)
                {
                    RicarregarPhieu();
                    pos = bindingNavigator.BindingSource.Find("ID", maPhieu);
                }
                if (pos >= 0)
                {
                    bindingNavigator.BindingSource.Position = pos;
                    DoDuLieuTuLoiLenForm();
                }
            }
        }

        private void frmBanSi_Load(object sender, EventArgs e)
        {
            if (daNapForm)
            {
                RicarregarPhieu();
                return;
            }

            isBindingData = true;
            ctrlSanPham.HienthiAutoComboBox(cmbSanPham);
            cmbSanPham.AutoCompleteMode = AutoCompleteMode.None;
            cmbSanPham.AutoCompleteSource = AutoCompleteSource.None;
            ctrlDaiLy.HienthiAutoComboBox(cmbDaiLy);
            cmbSanPham.SelectedIndexChanged += cmbSanPham_SelectedIndexChanged;
            cmbMaSanPham.SelectedIndexChanged += cmbMaSanPham_SelectedIndexChanged;
            cmbCachChonLo.SelectedIndexChanged += cmbCachChonLo_SelectedIndexChanged;
            cmbCachChonLo.SelectedIndex = 0;
            cmbCachTinhGiaVon.SelectedIndex = 0;
            dsChiTiet.DataSource = TaoBangChiTiet();
            dgvDanhsachSP.DataSource = dsChiTiet;
            TaoDieuKhienChiPhi();
            numSoLuong.Minimum = 0;
            numDaTra.Minimum = 0;
            numTongTien.ValueChanged += numTongTien_ValueChanged;
            numDaTra.ValueChanged += numTongTien_ValueChanged;
            dieuPhoiTinhHoaDon = new HoaDonBanMediator(TaoYeuCauTinhHoaDon, CapNhatKetQuaTinhHoaDon);
            thayDoiHoaDon.Subscribe(dieuPhoiTinhHoaDon);
            ctrlPhieuBan.HienthiPhieuBan(bindingNavigator, cmbDaiLy, txtMaPhieu,
                dtNgayLapPhieu, numTongTien, numDaTra, numConNo);
            dtNgayLapPhieu.MaxDate = DateTime.Now.Date.AddDays(1).AddSeconds(-1);
            GanSuKienCurrentChanged();
            dsChiTiet.ListChanged -= DsChiTiet_ListChanged;
            dsChiTiet.ListChanged += DsChiTiet_ListChanged;
            isBindingData = false;
            daNapForm = true;

            if (status == Controll.AddNew)
            {
                txtMaPhieu.Text = ThamSo.LayMaPhieuBan().ToString();
                isBindingData = true;
                dtNgayLapPhieu.Value = DateTime.Now;
                numTongTien.Value = 0;
                numDaTra.Value = 0;
                numConNo.Value = 0;
                isBindingData = false;
                ResetChiTiet();
                Allow(true);
            }
            else
            {
                if (!string.IsNullOrEmpty(maPhieuKhoiTao) && bindingNavigator.BindingSource != null)
                {
                    int pos = bindingNavigator.BindingSource.Find("ID", maPhieuKhoiTao);
                    if (pos >= 0)
                        bindingNavigator.BindingSource.Position = pos;
                }
                Allow(false);
                DoDuLieuTuLoiLenForm();
            }
        }

        private void RicarregarPhieu()
        {
            if (status == Controll.AddNew || status == Controll.Edit)
            {
                MessageBox.Show("Hãy lưu hoặc bỏ thay đổi trước khi tải lại phiếu.", "Bán sỉ",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            isBindingData = true;
            ctrlPhieuBan = new PhieuBanController();
            ctrlPhieuBan.HienthiPhieuBan(bindingNavigator, cmbDaiLy, txtMaPhieu,
                dtNgayLapPhieu, numTongTien, numDaTra, numConNo);
            GanSuKienCurrentChanged();
            isBindingData = false;
            Allow(false);
            DoDuLieuTuLoiLenForm();
        }

        private DataTable TaoBangChiTiet()
        {
            DataTable table = new DataTable();
            table.Columns.Add("ID_MA_SAN_PHAM", typeof(string));
            table.Columns.Add("TEN_SAN_PHAM", typeof(string));
            table.Columns.Add("NGAY_HET_HAN", typeof(DateTime));
            table.Columns.Add("SO_LUONG", typeof(int));
            table.Columns.Add("DON_GIA", typeof(long));
            table.Columns.Add("THANH_TIEN", typeof(long));
            table.Columns.Add("GIA_VON", typeof(long));
            return table;
        }

        private void TaoDieuKhienChiPhi()
        {
            btnThemBocXep.Click += delegate { ThemDichVuCoTen(new DichVuBocXepCreator(), numTienBocXep); };
            btnThemDongGoi.Click += delegate { ThemDichVuCoTen(new DichVuDongGoiCreator(), numTienDongGoi); };
            chkGiaoHang.CheckedChanged += ChiPhi_ValueChanged; numKhoangCach.ValueChanged += ChiPhi_ValueChanged;
            numPhiShipThuCong.ValueChanged += ChiPhi_ValueChanged;
            cmbLoaiGiamGia.SelectedIndexChanged += ChiPhi_ValueChanged;
            numTyLeGiamGia.ValueChanged += ChiPhi_ValueChanged; btnThemDichVu.Click += btnThemDichVu_Click; btnXoaDichVu.Click += btnXoaDichVu_Click;
            CapNhatTrangThaiKhoangCach();
        }

        private void ChiPhi_ValueChanged(object sender, EventArgs e)
        {
            if (isBindingData) return;
            if (numPhiShipThuCong != null) numPhiShipThuCong.Visible = chkGiaoHang.Checked;
            CapNhatTrangThaiTyLeGiamGia();
            CapNhatTongHoaDon();
        }

        private void CapNhatTrangThaiTyLeGiamGia()
        {
            if (cmbLoaiGiamGia.SelectedIndex == 0 && numTyLeGiamGia.Value != 0)
            {
                bool dangGanDuLieu = isBindingData;
                isBindingData = true;
                numTyLeGiamGia.Value = 0;
                isBindingData = dangGanDuLieu;
            }
            numTyLeGiamGia.Enabled = status != Controll.Normal && !dangSuaThanhToanPhieuCu && cmbLoaiGiamGia.SelectedIndex > 0;
        }

        private void btnThemDichVu_Click(object sender, EventArgs e)
        {
            if (status == Controll.Normal || dangSuaThanhToanPhieuCu) return;
            string ten = txtDichVu.Text.Trim();
            if (ten.Length == 0 || numTienDichVu.Value <= 0) { BaoLoi("Nhập tên dịch vụ và số tiền lớn hơn 0."); return; }
            dichVuDraft.Add(new DichVuPhieuBanFactory().TaoDichVu(ten, Decimal.ToInt64(numTienDichVu.Value)));
            HienThiDichVu(); txtDichVu.Clear(); numTienDichVu.Value = 0; CapNhatTongHoaDon();
        }

        private void ThemDichVuCoTen(DichVuPhieuBanCreator creator, NumericUpDown controlTien)
        {
            if (status == Controll.Normal || dangSuaThanhToanPhieuCu) return;
            try
            {
                dichVuDraft.Add(creator.TaoDichVu(Decimal.ToInt64(controlTien.Value)));
                HienThiDichVu();
                controlTien.Value = 0;
                CapNhatTongHoaDon();
            }
            catch (Exception ex) { BaoLoi(ex.Message); }
        }

        private void btnXoaDichVu_Click(object sender, EventArgs e)
        {
            if (status == Controll.Normal || dangSuaThanhToanPhieuCu || dgvDichVu.CurrentRow == null) return;
            int index = dgvDichVu.CurrentRow.Index;
            if (index >= 0 && index < dichVuDraft.Count) dichVuDraft.RemoveAt(index);
            HienThiDichVu(); CapNhatTongHoaDon();
        }

        private void HienThiDichVu()
        {
            if (dgvDichVu == null) return;
            dgvDichVu.Rows.Clear();
            foreach (DichVuPhieuBan dong in dichVuDraft) dgvDichVu.Rows.Add(dong.TenDichVu, dong.SoTien.ToString("#,##0"));
        }

        private void NapChiPhi(PhieuBan phieu)
        {
            isBindingData = true; dichVuDraft.Clear();
            if (phieu != null && phieu.DichVu != null)
                foreach (DichVuPhieuBan dichVu in phieu.DichVu) dichVuDraft.Add(new DichVuPhieuBan { Id = dichVu.Id, IdPhieuBan = dichVu.IdPhieuBan, TenDichVu = dichVu.TenDichVu, SoTien = dichVu.SoTien });
            if (phieu != null)
            {
                chkGiaoHang.Checked = phieu.GiaoHang;
                numKhoangCach.Value = Math.Min(numKhoangCach.Maximum, Math.Max(numKhoangCach.Minimum, phieu.KhoangCachKm));
                cmbLoaiGiamGia.SelectedIndex = phieu.LoaiGiamGia == "Giảm giá" ? 1 : phieu.LoaiGiamGia == "Chiết khấu" ? 2 : 0;
                numTyLeGiamGia.Value = Math.Min(numTyLeGiamGia.Maximum, Math.Max(numTyLeGiamGia.Minimum, phieu.TyLeGiamGia));
                numPhiShipThuCong.Value = Math.Min(numPhiShipThuCong.Maximum, phieu.PhiVanChuyen);
            }
            HienThiDichVu(); isBindingData = false;
            numPhiShipThuCong.Visible = phieu != null && phieu.GiaoHang;
            HienThiTongChiPhi(phieu);
        }

        private void HienThiTongChiPhi(PhieuBan phieu)
        {
            if (phieu == null || lblThanhToanChiTiet == null) return;
            lblTienGiam.Text = (String.IsNullOrEmpty(phieu.LoaiGiamGia) ? "Giảm giá" : phieu.LoaiGiamGia) +
                (phieu.TyLeGiamGia > 0 ? " " + phieu.TyLeGiamGia.ToString("0.##") + "%" : "") +
                ": -" + phieu.TienGiamGia.ToString("#,##0") + " đ";
            lblThanhToanChiTiet.Text = "Tiền hàng: " + phieu.TienHang.ToString("#,##0") + " đ";
            lblDichVuShipTomTat.Text = "Dịch vụ: " + phieu.TongDichVu.ToString("#,##0") + " đ   |   Ship: " + phieu.PhiVanChuyen.ToString("#,##0") + " đ";
            CapNhatTomTatThanhToan(phieu.TongTien, phieu.DaTra);
        }

        private ThongTinTinhHoaDon TaoYeuCauTinhHoaDon()
        {
            long tienHang = 0;
            foreach (DataRow row in ((DataTable)dsChiTiet.DataSource).Rows)
                tienHang = checked(tienHang + Convert.ToInt64(row["THANH_TIEN"]));
            long tongDichVu = 0;
            foreach (DichVuPhieuBan dong in dichVuDraft) tongDichVu = checked(tongDichVu + dong.SoTien);
            string loaiGiam = cmbLoaiGiamGia.SelectedIndex == 1 ? "Giảm giá" : cmbLoaiGiamGia.SelectedIndex == 2 ? "Chiết khấu" : null;
            ThongTinTinhHoaDon request = new HoaDonBanBuilder().ChoLoai(LoaiHoaDonBan.BanSi).TienHang(tienHang).DichVu(tongDichVu)
                .GiaoHang(chkGiaoHang.Checked, numKhoangCach.Value).GiamGia(loaiGiam, numTyLeGiamGia.Value).DaTra(0).Build();
            return request;
        }

        private ThongTinChiPhiPhieuBan TinhChiPhi(long tienHang)
        {
            ThongTinTinhHoaDon request = TaoYeuCauTinhHoaDon();
            request.TienHang = tienHang;
            return TaoChiPhiTuKetQua(tinhHoaDonFacade.Tinh(request));
        }

        private ThongTinChiPhiPhieuBan TaoChiPhiTuKetQua(KetQuaTinhHoaDon result)
        {
            ThongTinChiPhiPhieuBan chiPhi = new ThongTinChiPhiPhieuBan();
            chiPhi.TienHang = result.TienHang; chiPhi.TongDichVu = result.TongDichVu; chiPhi.GiaoHang = result.GiaoHang;
            chiPhi.KhoangCachKm = result.KhoangCachKm; chiPhi.PhiVanChuyen = result.PhiVanChuyen; chiPhi.LoaiGiamGia = result.LoaiGiamGia;
            chiPhi.TyLeGiamGia = result.TyLeGiamGia; chiPhi.TienGiamGia = result.TienGiamGia; chiPhi.TongThanhToan = result.TongThanhToan;
            chiPhi.DichVu = new List<DichVuPhieuBan>(dichVuDraft);
            bool dangGanDuLieu = isBindingData;
            isBindingData = true;
            numPhiShipThuCong.Value = Math.Min(numPhiShipThuCong.Maximum, result.PhiVanChuyen);
            isBindingData = dangGanDuLieu;
            lblTienGiam.Text = (String.IsNullOrEmpty(result.LoaiGiamGia) ? "Giảm giá" : result.LoaiGiamGia) +
                (result.TyLeGiamGia > 0 ? " " + result.TyLeGiamGia.ToString("0.##") + "%" : "") +
                ": -" + result.TienGiamGia.ToString("#,##0") + " đ";
            lblThanhToanChiTiet.Text = "Tiền hàng: " + result.TienHang.ToString("#,##0") + " đ";
            lblDichVuShipTomTat.Text = "Dịch vụ: " + result.TongDichVu.ToString("#,##0") + " đ   |   Ship: " + result.PhiVanChuyen.ToString("#,##0") + " đ";
            CapNhatTomTatThanhToan(result.TongThanhToan, Decimal.ToInt64(numDaTra.Value));
            return chiPhi;
        }

        private void CapNhatTomTatThanhToan(decimal tongThanhToan, decimal daTra)
        {
            if (lblTongThanhToanTomTat == null) return;
            lblTongThanhToanTomTat.Text = "Tổng thanh toán: " + tongThanhToan.ToString("#,##0") + " đ";
            lblDaTraTomTat.Text = "Đã trả: " + daTra.ToString("#,##0") + " đ";
            lblConNoTomTat.Text = "Còn nợ: " + Math.Max(0, tongThanhToan - daTra).ToString("#,##0") + " đ";
        }

        private void CapNhatKetQuaTinhHoaDon(KetQuaTinhHoaDon result)
        {
            ThongTinChiPhiPhieuBan chiPhi = TaoChiPhiTuKetQua(result);
            isBindingData = true;
            try { numTongTien.Value = chiPhi.TongThanhToan; numConNo.Value = Math.Max(0, numTongTien.Value - numDaTra.Value); }
            finally { isBindingData = false; }
        }

        private void CapNhatTongHoaDon()
        {
            if (isBindingData || (status == Controll.Normal && !dangSuaThanhToanPhieuCu) || numTongTien == null) return;
            try { thayDoiHoaDon.Notify(); }
            catch { isBindingData = false; lblTienGiam.Text = "Kiểm tra thông tin giảm giá/giao hàng"; }
        }

        private void numConNo_ValueChanged(object sender, EventArgs e)
        {
            if (isBindingData || numTongTien == null) return;
            isBindingData = true;
            try { numConNo.Value = Math.Max(0, numTongTien.Value - numDaTra.Value); }
            finally { isBindingData = false; }
        }

        private string LayMaPhieuHienTai()
        {
            if (bindingNavigator.BindingSource != null && bindingNavigator.BindingSource.Current != null)
            {
                DataRowView row = bindingNavigator.BindingSource.Current as DataRowView;
                if (row != null && row.Row.Table.Columns.Contains("ID") && row["ID"] != DBNull.Value)
                    return Convert.ToString(row["ID"]);
            }
            return txtMaPhieu.Text;
        }

        // Refactoring: Extract Method DoDuLieuTuLoiLenForm kết hợp cờ Control Flag isBindingData
        private void DoDuLieuTuLoiLenForm()
        {
            string maPhieu = LayMaPhieuHienTai();
            if (string.IsNullOrEmpty(maPhieu)) return;
            isBindingData = true;
            try
            {
                if (txtMaPhieu.Text != maPhieu)
                    txtMaPhieu.Text = maPhieu;

                PhieuBan phieu = ctrlPhieuBan.LayPhieuBan(maPhieu);
                NapChiPhi(phieu);
                DataTable chiTiet = repository.LayChiTiet(maPhieu);
                DataTable draft = (DataTable)dsChiTiet.DataSource;
                draft.Clear();
                foreach (DataRow row in chiTiet.Rows)
                {
                    DataRow newRow = draft.NewRow();
                    newRow["ID_MA_SAN_PHAM"] = row["ID_MA_SAN_PHAM"];
                    newRow["TEN_SAN_PHAM"] = row["TEN_SAN_PHAM"];
                    newRow["NGAY_HET_HAN"] = row["NGAY_HET_HAN"];
                    newRow["SO_LUONG"] = row["SO_LUONG"];
                    newRow["DON_GIA"] = row["DON_GIA"];
                    newRow["THANH_TIEN"] = row["THANH_TIEN"];
                    newRow["GIA_VON"] = row["GIA_VON"];
                    draft.Rows.Add(newRow);
                }
                if (phieu != null)
                {
                    daTraBanDau = phieu.DaTra;
                    numConNo.Value = phieu.ConNo;
                }
            }
            finally
            {
                isBindingData = false;
            }
        }

        private void GanSuKienCurrentChanged()
        {
            if (bindingNavigator.BindingSource != null)
            {
                bindingNavigator.BindingSource.CurrentChanged -= BindingSource_CurrentChanged;
                bindingNavigator.BindingSource.CurrentChanged += BindingSource_CurrentChanged;
            }
        }

        private void DsChiTiet_ListChanged(object sender, System.ComponentModel.ListChangedEventArgs e)
        {
            if (!isBindingData) CapNhatTongHoaDon();
        }

        private void BindingSource_CurrentChanged(object sender, EventArgs e)
        {
            if (status == Controll.Normal && bindingNavigator.BindingSource != null &&
                bindingNavigator.BindingSource.Current != null)
                DoDuLieuTuLoiLenForm();
        }

        private void toolXemLai_Click(object sender, EventArgs e)
        {
            RicarregarPhieu();
        }

        private void cmbSanPham_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbSanPham.SelectedValue == null || cmbSanPham.SelectedValue is DataRowView)
            {
                ResetThongTinGia();
                return;
            }
            bool baoGomHetHan = cmbCachChonLo.SelectedIndex == 1;
            ctrlMaSanPham.HienThiAutoComboBox(Convert.ToString(cmbSanPham.SelectedValue), cmbMaSanPham, baoGomHetHan);
            cmbMaSanPham.AutoCompleteMode = AutoCompleteMode.None;
            cmbMaSanPham.AutoCompleteSource = AutoCompleteSource.None;
            HienThiGiaSanPham();
        }

        private void cmbCachChonLo_SelectedIndexChanged(object sender, EventArgs e)
        {
            cmbMaSanPham.Enabled = (status == Controll.AddNew || status == Controll.Edit) && cmbCachChonLo.SelectedIndex == 1;
            if (cmbSanPham.SelectedValue != null && !(cmbSanPham.SelectedValue is DataRowView))
                cmbSanPham_SelectedIndexChanged(cmbSanPham, EventArgs.Empty);
        }

        private void cmbMaSanPham_SelectedIndexChanged(object sender, EventArgs e)
        {
            HienThiGiaSanPham();
        }

        // Refactoring: Extract Method ResetThongTinGia
        private void ResetThongTinGia()
        {
            numDonGia.Value = 0;
            numThanhTien.Value = 0;
            txtGiaNhap.Text = "0";
            txtGiaBanSi.Text = "0";
            txtGiaBanLe.Text = "0";
            txtGiaBQGQ.Text = "0";
        }

        private void HienThiGiaSanPham()
        {
            if (cmbMaSanPham.SelectedValue == null || cmbMaSanPham.SelectedValue is DataRowView)
            {
                ResetThongTinGia();
                return;
            }
            MaSanPham lo = ctrlMaSanPham.LayMaSanPham(Convert.ToString(cmbMaSanPham.SelectedValue));
            if (lo == null || lo.SanPham == null)
            {
                ResetThongTinGia();
                return;
            }
            numDonGia.Value = Math.Min(numDonGia.Maximum, lo.SanPham.GiaBanSi);
            txtGiaNhap.Text = lo.GiaNhap.ToString("#,###0");
            txtGiaBanSi.Text = lo.SanPham.GiaBanSi.ToString("#,###0");
            txtGiaBanLe.Text = lo.SanPham.GiaBanLe.ToString("#,###0");
            txtGiaBQGQ.Text = lo.SanPham.DonGiaNhap.ToString("#,###0");
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            // Guard Clauses: Kiểm tra điều kiện và thoát sớm
            if (status == Controll.Normal || dangSuaThanhToanPhieuCu)
            {
                MessageBox.Show("Chọn Thêm mới trước khi thay đổi chi tiết.");
                return;
            }
            if (cmbSanPham.SelectedValue == null || cmbSanPham.SelectedValue is DataRowView)
            {
                BaoLoi("Vui lòng chọn sản phẩm."); return;
            }
            if (cmbCachChonLo.SelectedIndex == 1 && (cmbMaSanPham.SelectedValue == null || cmbMaSanPham.SelectedValue is DataRowView))
            {
                BaoLoi("Vui lòng chọn lô xuất."); return;
            }
            if (numSoLuong.Value <= 0)
            {
                BaoLoi("Số lượng phải lớn hơn hoặc bằng 1."); return;
            }
            if (numDonGia.Value < 0)
            {
                BaoLoi("Đơn giá không được âm."); return;
            }
            try
            {
                CachChonLo cachChon = cmbCachChonLo.SelectedIndex == 0 ? CachChonLo.HetHanTruoc : CachChonLo.ChiDinh;
                CachTinhGiaVon cachTinh = cmbCachTinhGiaVon.SelectedIndex == 0 ? CachTinhGiaVon.BinhQuanGiaQuyen : CachTinhGiaVon.NhapTruocXuatTruoc;
                string maLo = cachChon == CachChonLo.ChiDinh ? Convert.ToString(cmbMaSanPham.SelectedValue) : null;
                IDictionary<string, int> soLuongDaPhanBo = LaySoLuongDraftTheoLo();
                IList<PhanBoLoXuat> phanBo = repository.LayPhanBo(Convert.ToString(cmbSanPham.SelectedValue),
                    Decimal.ToInt32(numSoLuong.Value), maLo, cachChon, cachTinh, soLuongDaPhanBo);
                long giaBan = Decimal.ToInt64(numDonGia.Value);
                long tongMoi = decimal.ToInt64(numTongTien.Value);
                foreach (PhanBoLoXuat dong in phanBo)
                {
                    dong.GiaBan = giaBan;
                    tongMoi = checked(tongMoi + dong.ThanhTien);
                }
                if (tongMoi > decimal.ToInt64(numTongTien.Maximum))
                {
                    BaoLoi("Tổng tiền vượt quá giới hạn của phiếu.");
                    return;
                }
                foreach (PhanBoLoXuat dong in phanBo) ThemDongChiTiet(dong);
                TinhLaiTongTien();
                numSoLuong.Value = 1;
                numThanhTien.Value = numDonGia.Value;
            }
            catch (Exception ex) { BaoLoi(ex.Message); }
        }

        private IDictionary<string, int> LaySoLuongDraftTheoLo()
        {
            IDictionary<string, int> soLuongTheoLo = new Dictionary<string, int>();
            DataTable table = (DataTable)dsChiTiet.DataSource;
            foreach (DataRow row in table.Rows)
            {
                string maLo = Convert.ToString(row["ID_MA_SAN_PHAM"]);
                int soLuong = Convert.ToInt32(row["SO_LUONG"]);
                int daPhanBo;
                if (soLuongTheoLo.TryGetValue(maLo, out daPhanBo))
                    soLuongTheoLo[maLo] = checked(daPhanBo + soLuong);
                else
                    soLuongTheoLo.Add(maLo, soLuong);
            }
            if (status == Controll.Edit && soLuongGocTheoLo.Count > 0)
            {
                IDictionary<string, int> soLuongChenhLech = new Dictionary<string, int>(soLuongTheoLo);
                foreach (KeyValuePair<string, int> cap in soLuongGocTheoLo)
                {
                    int daCo;
                    if (soLuongChenhLech.TryGetValue(cap.Key, out daCo))
                        soLuongChenhLech[cap.Key] = checked(daCo - cap.Value);
                    else
                        soLuongChenhLech.Add(cap.Key, -cap.Value);
                }
                return soLuongChenhLech;
            }
            return soLuongTheoLo;
        }

        private void CapNhatSoLuongGoc()
        {
            soLuongGocTheoLo.Clear();
            if (status == Controll.Edit && !String.IsNullOrEmpty(txtMaPhieu.Text))
            {
                DataTable chiTietCu = repository.LayChiTiet(txtMaPhieu.Text);
                foreach (DataRow row in chiTietCu.Rows)
                {
                    string maLo = Convert.ToString(row["ID_MA_SAN_PHAM"]);
                    int soLuong = Convert.ToInt32(row["SO_LUONG"]);
                    int daCo;
                    if (soLuongGocTheoLo.TryGetValue(maLo, out daCo))
                        soLuongGocTheoLo[maLo] = checked(daCo + soLuong);
                    else
                        soLuongGocTheoLo.Add(maLo, soLuong);
                }
            }
        }

        private void ThemDongChiTiet(PhanBoLoXuat dong)
        {
            DataTable table = (DataTable)dsChiTiet.DataSource;
            DataRow row = table.NewRow();
            row["ID_MA_SAN_PHAM"] = dong.MaLo;
            row["TEN_SAN_PHAM"] = dong.TenSanPham;
            row["NGAY_HET_HAN"] = dong.NgayHetHan;
            row["SO_LUONG"] = dong.SoLuong;
            row["DON_GIA"] = dong.GiaBan;
            row["THANH_TIEN"] = dong.ThanhTien;
            row["GIA_VON"] = dong.GiaVonTong;
            table.Rows.Add(row);
        }

        private void numDonGia_ValueChanged(object sender, EventArgs e)
        {
            if (numSoLuong.Value < 0) numSoLuong.Value = 0;
            decimal total = numDonGia.Value * numSoLuong.Value;
            numThanhTien.Value = Math.Min(numThanhTien.Maximum, total);
        }

        private void numTongTien_ValueChanged(object sender, EventArgs e)
        {
            if (isBindingData) return;
            if (numDaTra.Value < 0) numDaTra.Value = 0;
            if (!dangSuaThanhToanPhieuCu && numDaTra.Value > numTongTien.Value) numDaTra.Value = numTongTien.Value;
            isBindingData = true;
            numConNo.Value = Math.Max(0, numTongTien.Value - numDaTra.Value);
            isBindingData = false;
            CapNhatDongThanhToanTomTat();
        }

        private void CapNhatDongThanhToanTomTat()
        {
            CapNhatTomTatThanhToan(numTongTien.Value, numDaTra.Value);
        }

        // Refactoring: Extract Method TinhLaiTongTien
        private void TinhLaiTongTien()
        {
            long tienHang = 0;
            foreach (DataRow row in ((DataTable)dsChiTiet.DataSource).Rows)
                tienHang = checked(tienHang + Convert.ToInt64(row["THANH_TIEN"]));
            if (tienHang > decimal.ToInt64(numTongTien.Maximum))
            {
                BaoLoi("Tổng tiền vượt quá giới hạn của phiếu.");
                return;
            }
            CapNhatTongHoaDon();
        }

        private void toolLuu_Click(object sender, EventArgs e)
        {
            if (Luu())
            {
                dangSuaThanhToanPhieuCu = false;
                status = Controll.Normal;
                soLuongGocTheoLo.Clear();
                Allow(false);
                ctrlPhieuBan = new PhieuBanController();
                ctrlPhieuBan.HienthiPhieuBan(bindingNavigator, cmbDaiLy, txtMaPhieu,
                    dtNgayLapPhieu, numTongTien, numDaTra, numConNo);
                GanSuKienCurrentChanged();
                DoDuLieuTuLoiLenForm();
            }
        }

        private bool Luu()
        {
            if (dangSuaThanhToanPhieuCu)
            {
                try
                {
                    if (numDaTra.Value < 0)
                    {
                        BaoLoi("Số tiền đã trả không được âm.");
                        return false;
                    }
                    if (numDaTra.Value > numTongTien.Value)
                    {
                        BaoLoi("Số tiền đã trả không được lớn hơn tổng tiền.");
                        return false;
                    }
                    long daTraMoi = Decimal.ToInt64(numDaTra.Value);
                    if (daTraMoi < daTraBanDau) { BaoLoi("Số tiền đã trả không được thấp hơn số đã ghi nhận trước đó."); return false; }
                    repository.CapNhatDaTra(new ThanhToanPhieuBan { MaPhieu = LayMaPhieuHienTai(), DaTraBanDau = daTraBanDau, DaTraMoi = daTraMoi });
                    daTraBanDau = daTraMoi;
                    return true;
                }
                catch (Exception ex) { BaoLoi(ex.Message); return false; }
            }
            // Guard Clauses: Chống lưu Hóa đơn rỗng và kiểm tra hợp lệ
            if (cmbDaiLy.SelectedValue == null || cmbDaiLy.SelectedValue is DataRowView)
            {
                BaoLoi("Vui lòng chọn khách hàng."); return false;
            }
            DataTable table = (DataTable)dsChiTiet.DataSource;
            if (table == null || table.Rows.Count == 0)
            {
                BaoLoi("Hóa đơn phải có ít nhất một dòng sản phẩm trước khi lưu.");
                return false;
            }
            if (numTongTien.Value <= 0)
            {
                BaoLoi("Tổng tiền của hóa đơn phải lớn hơn 0 trước khi lưu.");
                return false;
            }
            try
            {
                IList<PhanBoLoXuat> chiTiet = new List<PhanBoLoXuat>();
                foreach (DataRow row in table.Rows)
                {
                    PhanBoLoXuat dong = new PhanBoLoXuat();
                    dong.MaLo = Convert.ToString(row["ID_MA_SAN_PHAM"]);
                    dong.SoLuong = Convert.ToInt32(row["SO_LUONG"]);
                    dong.GiaBan = Convert.ToInt64(row["DON_GIA"]);
                    dong.GiaVonTong = Convert.ToInt64(row["GIA_VON"]);
                    dong.GiaVonDonVi = dong.SoLuong == 0 ? 0 : dong.GiaVonTong / dong.SoLuong;
                    chiTiet.Add(dong);
                }
                bool laPhieuMoi = status == Controll.AddNew;
                long tienHang = 0;
                foreach (PhanBoLoXuat dong in chiTiet)
                    tienHang = checked(tienHang + dong.ThanhTien);
                ThongTinChiPhiPhieuBan chiPhi = TinhChiPhi(tienHang);
                if (chiPhi.TongThanhToan > decimal.ToInt64(numTongTien.Maximum))
                {
                    BaoLoi("Tổng tiền vượt quá giới hạn của phiếu.");
                    return false;
                }
                if (numDaTra.Value < 0)
                {
                    BaoLoi("Số tiền đã trả không được âm.");
                    return false;
                }
                long daTra = Decimal.ToInt64(numDaTra.Value);
                if (daTra > chiPhi.TongThanhToan)
                {
                    BaoLoi("Số tiền đã trả không được lớn hơn tổng tiền.");
                    return false;
                }
                repository.LuuPhieu(txtMaPhieu.Text, Convert.ToString(cmbDaiLy.SelectedValue),
                    dtNgayLapPhieu.Value.Date, chiPhi, daTra, chiTiet, laPhieuMoi, LoaiHoaDonBan.BanSi);
                if (laPhieuMoi && ThamSo.LaSoNguyen(txtMaPhieu.Text))
                {
                    long soPhieu = Convert.ToInt64(txtMaPhieu.Text);
                    if (soPhieu >= ThamSo.LayMaPhieuBan()) ThamSo.GanMaPhieuBan(soPhieu + 1);
                }
                return true;
            }
            catch (Exception ex) { BaoLoi(ex.Message); return false; }
        }

        private void toolLuu_Them_Click(object sender, EventArgs e)
        {
            if (status == Controll.Normal && !dangSuaThanhToanPhieuCu)
            {
                TaoPhieuMoi();
                return;
            }
            if (status == Controll.AddNew || status == Controll.Edit)
            {
                DialogResult ketQua = MessageBox.Show("Lưu phiếu hiện tại và tạo phiếu mới? Chọn No để bỏ thay đổi.", "Bán sỉ",
                    MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                if (ketQua == DialogResult.Cancel) return;
                if (ketQua == DialogResult.Yes)
                {
                    if (!Luu()) return;
                    if (dangSuaThanhToanPhieuCu)
                    {
                        dangSuaThanhToanPhieuCu = false;
                        status = Controll.Normal;
                        soLuongGocTheoLo.Clear();
                        Allow(false);
                        ctrlPhieuBan = new PhieuBanController();
                        ctrlPhieuBan.HienthiPhieuBan(bindingNavigator, cmbDaiLy, txtMaPhieu,
                            dtNgayLapPhieu, numTongTien, numDaTra, numConNo);
                        GanSuKienCurrentChanged();
                        DoDuLieuTuLoiLenForm();
                        return;
                    }
                    status = Controll.Normal;
                }
                else
                {
                    if (status == Controll.Edit)
                    {
                        dangSuaThanhToanPhieuCu = false;
                        status = Controll.Normal;
                        Allow(false);
                        ctrlPhieuBan = new PhieuBanController();
                        ctrlPhieuBan.HienthiPhieuBan(bindingNavigator, cmbDaiLy, txtMaPhieu,
                            dtNgayLapPhieu, numTongTien, numDaTra, numConNo);
                        GanSuKienCurrentChanged();
                        DoDuLieuTuLoiLenForm();
                    }
                    else
                    {
                        if (((DataTable)dsChiTiet.DataSource).Rows.Count == 0 && dichVuDraft.Count == 0)
                        {
                            TaoPhieuMoi();
                            return;
                        }
                        ResetChiTiet();
                        dichVuDraft.Clear();
                        isBindingData = true;
                        chkGiaoHang.Checked = false;
                        numKhoangCach.Value = 0;
                        cmbLoaiGiamGia.SelectedIndex = 0;
                        numTyLeGiamGia.Value = 0;
                        HienThiDichVu();
                        numDaTra.Value = 0;
                        isBindingData = false;
                        status = Controll.Normal;
                        CapNhatTongHoaDon();
                    }
                }
            }
            TaoPhieuMoi();
        }

        private void TaoPhieuMoi()
        {
            ctrlPhieuBan = new PhieuBanController();
            dangSuaThanhToanPhieuCu = false;
            daTraBanDau = 0;
            status = Controll.AddNew;
            soLuongGocTheoLo.Clear();
            Allow(true);
            isBindingData = true;
            txtMaPhieu.Text = ThamSo.LayMaPhieuBan().ToString();
            dtNgayLapPhieu.MaxDate = DateTime.Now.Date.AddDays(1).AddSeconds(-1);
            dtNgayLapPhieu.Value = DateTime.Now;
            numTongTien.Value = 0;
            numDaTra.Value = 0;
            numConNo.Value = 0;
            isBindingData = false;
            ResetChiTiet();
            dichVuDraft.Clear();
            chkGiaoHang.Checked = false;
            numKhoangCach.Value = 0;
            cmbLoaiGiamGia.SelectedIndex = 0;
            numTyLeGiamGia.Value = 0;
            HienThiDichVu();
            CapNhatTongHoaDon();
        }

        private void ResetChiTiet()
        {
            if (status == Controll.AddNew) soLuongGocTheoLo.Clear();
            ((DataTable)dsChiTiet.DataSource).Clear();
            TinhLaiTongTien();
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            if (status == Controll.Normal || dangSuaThanhToanPhieuCu || dsChiTiet.Current == null) return;
            if (MessageBox.Show("Xóa dòng chi tiết đang chọn?", "Bán sỉ", MessageBoxButtons.YesNo,
                MessageBoxIcon.Question) != DialogResult.Yes) return;
            dsChiTiet.RemoveCurrent();
            TinhLaiTongTien();
        }

        private void toolLuuIn_Click(object sender, EventArgs e)
        {
            if (status != Controll.Normal)
            {
                MessageBox.Show("Vui lòng lưu lại Phiếu bán hiện tại trước khi in."); return;
            }
            string maPhieu = LayMaPhieuHienTai();
            if (string.IsNullOrEmpty(maPhieu)) { BaoLoi("Không xác định được mã phiếu."); return; }
            PhieuBan phieu = ctrlPhieuBan.LayPhieuBan(maPhieu);
            if (phieu == null) { BaoLoi("Không tìm thấy phiếu bán."); return; }
            frmInPhieuBan formIn = new frmInPhieuBan(phieu);
            formIn.Show();
        }

        private void dgvDanhsachSP_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            e.Cancel = true;
        }

        private void toolChinhSua_Click(object sender, EventArgs e)
        {
            if (status != Controll.Normal) return;
            status = Controll.Edit;
            dangSuaThanhToanPhieuCu = true;
            daTraBanDau = Decimal.ToInt64(numDaTra.Value);
            Allow(true);
        }

        private void Allow(bool value)
        {
            bool suaGiaoDien = value && (status == Controll.AddNew || status == Controll.Edit && !dangSuaThanhToanPhieuCu);
            txtMaPhieu.Enabled = false;
            bindingNavigator.Enabled = true;
            bindingNavigatorMoveFirstItem.Enabled = !dangSuaThanhToanPhieuCu;
            bindingNavigatorMovePreviousItem.Enabled = !dangSuaThanhToanPhieuCu;
            bindingNavigatorMoveNextItem.Enabled = !dangSuaThanhToanPhieuCu;
            bindingNavigatorMoveLastItem.Enabled = !dangSuaThanhToanPhieuCu;
            bindingNavigatorPositionItem.Enabled = !dangSuaThanhToanPhieuCu;
            toolThem.Enabled = !dangSuaThanhToanPhieuCu;
            toolChinhSua.Enabled = !dangSuaThanhToanPhieuCu;
            toolXoa.Enabled = !dangSuaThanhToanPhieuCu;
            toolLuu.Enabled = value || dangSuaThanhToanPhieuCu;
            cmbDaiLy.Enabled = value && status == Controll.AddNew;
            dtNgayLapPhieu.Enabled = false;
            numDaTra.Enabled = value;
            numTongTien.Enabled = false;
            numConNo.Enabled = false;
            cmbSanPham.Enabled = suaGiaoDien;
            cmbMaSanPham.Enabled = (suaGiaoDien) && cmbCachChonLo.SelectedIndex == 1;
            cmbCachChonLo.Enabled = suaGiaoDien;
            cmbCachTinhGiaVon.Enabled = suaGiaoDien;
            numDonGia.Enabled = false;
            numSoLuong.Enabled = suaGiaoDien;
            btnAdd.Enabled = suaGiaoDien;
            btnRemove.Enabled = suaGiaoDien;
            dgvDanhsachSP.Enabled = suaGiaoDien;
            chkGiaoHang.Enabled = suaGiaoDien;
            CapNhatTrangThaiKhoangCach();
            numPhiShipThuCong.Enabled = suaGiaoDien;
            numTienBocXep.Enabled = suaGiaoDien;
            numTienDongGoi.Enabled = suaGiaoDien;
            btnThemBocXep.Enabled = suaGiaoDien;
            btnThemDongGoi.Enabled = suaGiaoDien;
            cmbLoaiGiamGia.Enabled = suaGiaoDien;
            numTyLeGiamGia.Enabled = suaGiaoDien;
            CapNhatTrangThaiTyLeGiamGia();
            txtDichVu.Enabled = suaGiaoDien;
            numTienDichVu.Enabled = suaGiaoDien;
            btnThemDichVu.Enabled = suaGiaoDien;
            btnXoaDichVu.Enabled = suaGiaoDien;
            dgvDichVu.Enabled = suaGiaoDien;
        }

        public bool XacNhanDongFormBanSi()
        {
            if (status == Controll.AddNew || status == Controll.Edit)
            {
                DialogResult result = MessageBox.Show("Lưu thay đổi trước khi thoát? Chọn No để bỏ thay đổi.",
                    "Bán sỉ", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                if (result == DialogResult.Cancel) return false;
                if (result == DialogResult.Yes && !Luu()) return false;
                if (result == DialogResult.No && dangSuaThanhToanPhieuCu)
                {
                    dangSuaThanhToanPhieuCu = false;
                    status = Controll.Normal;
                    Allow(false);
                    ctrlPhieuBan = new PhieuBanController();
                    ctrlPhieuBan.HienthiPhieuBan(bindingNavigator, cmbDaiLy, txtMaPhieu,
                        dtNgayLapPhieu, numTongTien, numDaTra, numConNo);
                    GanSuKienCurrentChanged();
                    DoDuLieuTuLoiLenForm();
                }
                else if (result == DialogResult.No && status == Controll.Edit)
                {
                    RicarregarPhieu();
                }
            }
            Close();
            return true;
        }

        private void toolThoat_Click(object sender, EventArgs e)
        {
            XacNhanDongFormBanSi();
        }

        private void toolXoa_Click(object sender, EventArgs e)
        {
            if (status != Controll.Normal) return;
            if (bindingNavigator.BindingSource == null || bindingNavigator.BindingSource.Current == null) return;
            if (MessageBox.Show("Xóa phiếu bán này và hoàn tồn các lô?", "Bán sỉ",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;
            try
            {
                repository.XoaPhieu(txtMaPhieu.Text);
                bindingNavigator.BindingSource.RemoveCurrent();
                status = Controll.Normal;
            }
            catch (Exception ex) { BaoLoi(ex.Message); }
        }

        private void btnThemDaiLy_Click(object sender, EventArgs e)
        {
            frmDaiLy form = new frmDaiLy();
            form.ShowDialog();
            ctrlDaiLy.HienthiAutoComboBox(cmbDaiLy);
        }

        private void btnThemSanPham_Click(object sender, EventArgs e)
        {
            frmSanPham form = new frmSanPham();
            form.ShowDialog();
            ctrlSanPham.HienthiAutoComboBox(cmbSanPham);
            cmbSanPham.AutoCompleteMode = AutoCompleteMode.None;
            cmbSanPham.AutoCompleteSource = AutoCompleteSource.None;
        }

        private void BaoLoi(string message)
        {
            MessageBox.Show(message, "Bán sỉ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void dgvDanhsachSP_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
        {
            e.Cancel = true;
        }

        private void chkGiaoHang_CheckedChanged(object sender, EventArgs e)
        {
            CapNhatTrangThaiKhoangCach();
        }

        private void CapNhatTrangThaiKhoangCach()
        {
            numKhoangCach.Enabled = chkGiaoHang.Checked && chkGiaoHang.Enabled;
        }
    }
}