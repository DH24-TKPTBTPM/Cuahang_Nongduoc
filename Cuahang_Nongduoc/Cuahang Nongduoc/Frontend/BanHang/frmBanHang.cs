using CuahangNongduoc.BusinessObject;
using CuahangNongduoc.Controller;
using CuahangNongduoc.Frontend.DungChung.Statess;
using CuahangNongduoc.Patterns.Facade;
using CuahangNongduoc.Patterns.Strategy;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace CuahangNongduoc
{
    /// [FE-01] VIẾT MỚI – Form Bán Hàng Hợp Nhất (Strategy Injection)
    /// Mục đích : Dựng form frmBanHang nhận IBanHangStrategy qua constructor để dùng chung cho Bán sỉ & Bán lẻ
    /// Thay thế : 2 form riêng biệt trùng lặp giao diện frmBanLe.cs và frmBanSi.cs
    /// Tác giả : FE1 – Dương Thị Kim Anh | Hỗ trợ: FE2 - Nguyễn Huỳnh Ngọc Anh | Kiểm tra: QA2 – Huỳnh Lư Anh Minh
    /// Ngày tạo : 06/11/2026
    public partial class frmBanHang : Form, IStateableForm
    {
        SanPhamController ctrlSanPham = new SanPhamController();
        KhachHangController ctrlKhachHang = new KhachHangController();
        MaSanPhamController ctrlMaSanPham = new MaSanPhamController();
        PhieuBanController ctrlPhieuBan = new PhieuBanController();
        ChiTietPhieuBanController ctrlChiTiet = new ChiTietPhieuBanController();
        IList<MaSanPham> deleted = new List<MaSanPham>();

        // [D09] VIẾT MỚI – Mẫu thiết kế: State (Behavioral, GoF)
        // Mục đích : Lưu trữ đối tượng trạng thái hiện tại (StateXem, StateThem, StateSua)
        private IFrmState _currentState;
    
        private BanHangFacade _facade = new BanHangFacade();

        // Biến lưu trữ Strategy
        private IBanHangStrategy _strategy;

        // 1. Constructor không tham số (Bắt buộc cho Visual Studio Designer)
        public frmBanHang()
        {
            InitializeComponent();
            // ==================== [FE-08a] REFACTORING ====================
            // Lỗi (Code smell) : Missing Event Handler – Đổi Số lượng không tự tính lại Thành tiền
            // Kỹ thuật         : Fix Bug
            // Trước            : Chỉ gắn sự kiện numDonGia.ValueChanged
            // Sau              : Gắn thêm numSoLuong.ValueChanged và dgvDanhsachSP.CellValueChanged / CellEndEdit
            // Tác giả          : FE1 – Dương Thị Kim Anh
            // Hỗ trợ           : —
            // Kiểm tra         : QA1 – Huỳnh Lư Anh Minh
            // Ngày chỉnh sửa   : 05/11/2026
            // ===========================================================
            numSoLuong.ValueChanged += new EventHandler(numDonGia_ValueChanged);
            dgvDanhsachSP.CellValueChanged += new DataGridViewCellEventHandler(dgvDanhsachSP_CellValueChanged);
            dgvDanhsachSP.CellEndEdit += new DataGridViewCellEventHandler(dgvDanhsachSP_CellEndEdit);
        }

        // 2. Constructor nhận Strategy từ bên ngoài tiêm vào
        public frmBanHang(IBanHangStrategy strategy) : this()
        {
            _strategy = strategy;
            SetState(new StateThem());
            if (_strategy != null)
            {
                this.Text = "Phiếu " + _strategy.TenChienLuoc;
            }
        }

        public frmBanHang(PhieuBanController ctrlPB, IBanHangStrategy strategy)
            : this(strategy)
        {
            this.ctrlPhieuBan = ctrlPB;
            SetState(new StateThem());
        }

        private void frmNhapHang_Load(object sender, EventArgs e)
        {
            ctrlSanPham.HienthiAutoComboBox(cmbSanPham);
            ctrlMaSanPham.HienThiDataGridViewComboBox(colMaSanPham);

            cmbSanPham.SelectedIndexChanged += new EventHandler(cmbSanPham_SelectedIndexChanged);

            // Dùng IsDaiLy từ Strategy để lọc danh sách khách hàng / đại lý
            ctrlKhachHang.HienthiAutoComboBox(cmbKhachHang, _strategy != null && _strategy.IsDaiLy);

            ctrlPhieuBan.HienthiPhieuBan(bindingNavigator, cmbKhachHang, txtMaPhieu, dtNgayLapPhieu, numTongTien, numDaTra, numConNo);

            bindingNavigator.BindingSource.CurrentChanged -= new EventHandler(BindingSource_CurrentChanged);
            bindingNavigator.BindingSource.CurrentChanged += new EventHandler(BindingSource_CurrentChanged);

            ctrlChiTiet.HienThiChiTiet(dgvDanhsachSP, txtMaPhieu.Text);

            // ==================== [D09-FE1] REFACTORING (STATE PATTERN) ====================
            // Lỗi (Code smell) : Conditional Complexity / Hardcoded State – Dùng cờ 'status' để phân nhánh UI
            // Kỹ thuật         : Polymorphism via State Pattern
            // Trước            : if (status == Controll.AddNew) ... else this.Allow(false);
            // Sau              : Kiểm tra (_currentState is StateThem) và giao việc Allow() cho State tự xử lý
            // Tác giả          : FE1 – Dương Thị Kim Anh
            // Hỗ trợ           : FE2 – Nguyễn Huỳnh Ngọc Anh
            // Kiểm tra         : QA
            // Ngày chỉnh sửa   : 10/11/2026
            // ==============================================================================
            if (_currentState is StateThem)
            {
                txtMaPhieu.Text = ThamSo.LayMaPhieuBan().ToString();
            }
        }


        #region IStateableForm Implementation
        /// [D09c] VIẾT MỚI – Mẫu thiết kế: State (Behavioral, GoF)
        /// Mục đích : Hàm cập nhật Trạng thái mới cho Form và thực thi Handle để phân bổ UI tương ứng
        /// Thay thế : Các hàm phân nhánh if (status == ...) đặt rải rác trên Form
        /// Tác giả : FE1 – Dương Thị Kim Anh | Hỗ trợ: FE2 – Nguyễn Huỳnh Ngọc Anh | Kiểm tra: QA1 - Huỳnh Lư Anh Minh
        /// Ngày tạo : 10/11/2026
        public void SetState(IFrmState state)
        {
            this._currentState = state;
            if (this._currentState != null) this._currentState.Handle(this);
        }

        public void AllowInput(bool enable)
        {
            txtMaPhieu.Enabled = enable;
            dtNgayLapPhieu.Enabled = enable;
            numTongTien.Enabled = enable;
            btnAdd.Enabled = enable;
            btnRemove.Enabled = enable;
            dgvDanhsachSP.Enabled = enable;
        }

        public void AllowEditMaPhieu(bool enable)
        {
            txtMaPhieu.Enabled = enable;
        }

        public void SetToolbarButtons(bool canAdd, bool canSave, bool canEdit, bool canDelete)
        {
            // Tên các tool này phụ thuộc vào giao diện thực tế của bạn, ví dụ:
            if (toolLuu != null) toolLuu.Enabled = canSave;
            if (toolChinhSua != null) toolChinhSua.Enabled = canEdit;
            if (toolXoa != null) toolXoa.Enabled = canDelete;
            // ... (bật/tắt các nút tương ứng trên UI)
        }
        #endregion

        

        void BindingSource_CurrentChanged(object sender, EventArgs e)
        {
            // ==================== [D09] REFACTORING (STATE PATTERN) ====================
            // Lỗi (Code smell) : Inappropriate Intimacy – Phụ thuộc vào biến trạng thái cũ
            // Kỹ thuật         : Type Check Refactoring via State
            // Trước            : if (status == Controll.Normal) ...
            // Sau              : if (_currentState is StateXem) ...
            // Tác giả          : FE1 – Dương Thị Kim Anh
            // Hỗ trợ           : FE2 - Nguyễn Huỳnh Ngọc Anh
            // Kiểm tra         : QA1 – Huỳnh Lư Anh Minh
            // Ngày chỉnh sửa   : 10/11/2026
            // ==============================================================================
            if (_currentState is StateXem)
            {
                ctrlChiTiet.HienThiChiTiet(dgvDanhsachSP, txtMaPhieu.Text);
            }
        }

        void cmbSanPham_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbSanPham.SelectedValue != null)
            {
                MaSanPhamController ctrlMSP = new MaSanPhamController();

                cmbMaSanPham.SelectedIndexChanged -= new EventHandler(cmbMaSanPham_SelectedIndexChanged);
                ctrlMSP.HienThiAutoComboBox(cmbSanPham.SelectedValue.ToString(), cmbMaSanPham);
                cmbMaSanPham.SelectedIndexChanged += new EventHandler(cmbMaSanPham_SelectedIndexChanged);
            }
        }

        void cmbMaSanPham_SelectedIndexChanged(object sender, EventArgs e)
        {
            MaSanPhamController ctrl = new MaSanPhamController();
            MaSanPham masp = ctrl.LayMaSanPham(cmbMaSanPham.SelectedValue.ToString());

            // Tính đơn giá dựa theo hàm LayDonGia của Strategy có sẵn
            if (_strategy != null && masp != null)
            {
                numDonGia.Value = _strategy.LayDonGia(masp.SanPham, masp);
            }

            txtGiaNhap.Text = masp.GiaNhap.ToString("#,###0");
            txtGiaBanSi.Text = masp.SanPham.GiaBanSi.ToString("#,###0");
            txtGiaBanLe.Text = masp.SanPham.GiaBanLe.ToString("#,###0");
            txtGiaBQGQ.Text = masp.SanPham.DonGiaNhap.ToString("#,###0");
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (cmbMaSanPham.SelectedValue == null)
            {
                MessageBox.Show("Vui lòng chọn Mã sản phẩm !", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (numSoLuong.Value <= 0)
            {
                MessageBox.Show("Vui lòng nhập Số lượng !", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (numDonGia.Value * numSoLuong.Value != numThanhTien.Value)
            {
                MessageBox.Show("Thành tiền sai!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                numTongTien.Value += numThanhTien.Value;
                DataRow row = ctrlChiTiet.NewRow();
                row["ID_MA_SAN_PHAM"] = cmbMaSanPham.SelectedValue;
                row["ID_PHIEU_BAN"] = txtMaPhieu.Text;
                row["DON_GIA"] = numDonGia.Value;
                row["SO_LUONG"] = numSoLuong.Value;
                row["THANH_TIEN"] = numThanhTien.Value;
                ctrlChiTiet.Add(row);
            }
        }

        private void numDonGia_ValueChanged(object sender, EventArgs e)
        {
            numThanhTien.Value = numDonGia.Value * numSoLuong.Value;
        }

        private void numTongTien_ValueChanged(object sender, EventArgs e)
        {
            numConNo.Value = numTongTien.Value - numDaTra.Value;
        }

        private void toolLuu_Click(object sender, EventArgs e)
        {
            bindingNavigatorPositionItem.Focus();
            this.Luu();
        }

        // ==================== [D09] REFACTORING (FACADE PATTERN) ====================
        // Lỗi (Code smell) : Direct Factory Access / Feature Envy – UI Form trực tiếp gọi MaSanPhanFactory
        // Kỹ thuật         : Hide Delegate / Move Method via Facade
        // Trước            : Form gọi hàm Luu() tự phân nhánh và loop gọi MaSanPhanFactory.CapNhatSoLuong(...)
        // Sau              : Nút Lưu CHỈ GỌI DUY NHẤT `_facade.LuuHoaDon(...)` và chuyển state sang StateXem()
        // Tác giả          : FE1 – Dương Thị Kim Anh
        // Hỗ trợ           : FE2 – Nguyễn Huỳnh Ngọc Anh
        // Kiểm tra         : QA1 – Huỳnh Lư Anh Minh
        // Ngày chỉnh sửa   : 10/11/2026
        // =================================================================================
        void Luu()
        {
            bool isThemMoi = (_currentState is StateThem);
            string maKhachHang = cmbKhachHang.SelectedValue != null ? cmbKhachHang.SelectedValue.ToString() : "";

            // GỌI DUY NHẤT FACADE ĐỂ LƯU VÀ HOÀN TỒN KHO
            bool thanhCong = _facade.LuuHoaDon(
                txtMaPhieu.Text.Trim(),
                maKhachHang,
                dtNgayLapPhieu.Value,
                numTongTien.Value,
                numDaTra.Value,
                numConNo.Value,
                ctrlChiTiet,
                ctrlPhieuBan,
                deleted,
                isThemMoi
            );

            if (thanhCong)
            {
                MessageBox.Show("Lưu phiếu bán thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                SetState(new StateXem());
            }
        }

        void CapNhat()
        {
            foreach (MaSanPham masp in deleted)
            {
                CuahangNongduoc.DataLayer.MaSanPhanFactory.CapNhatSoLuong(masp.Id, masp.SoLuong);
            }
            deleted.Clear();
            ctrlChiTiet.Save();
            ctrlPhieuBan.Update();
        }

        void ThemMoi()
        {
            DataRow row = ctrlPhieuBan.NewRow();
            row["ID"] = txtMaPhieu.Text;
            row["ID_KHACH_HANG"] = cmbKhachHang.SelectedValue;
            row["NGAY_BAN"] = dtNgayLapPhieu.Value.Date;
            row["TONG_TIEN"] = numTongTien.Value;
            row["DA_TRA"] = numDaTra.Value;
            row["CON_NO"] = numConNo.Value;
            ctrlPhieuBan.Add(row);

            PhieuBanController ctrl = new PhieuBanController();

            if (ctrl.LayPhieuBan(txtMaPhieu.Text) != null)
            {
                MessageBox.Show("Mã Phiếu bán này đã tồn tại !", "Phiếu Bán", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (ThamSo.LaSoNguyen(txtMaPhieu.Text))
            {
                long so = Convert.ToInt64(txtMaPhieu.Text);
                if (so >= ThamSo.LayMaPhieuBan())
                {
                    ThamSo.GanMaPhieuBan(so + 1);
                }
            }

            ctrlPhieuBan.Save();
            ctrlChiTiet.Save();
        }
        // ==================== [R18] REFACTORING ====================
        // Lỗi (Code smell) : Xóa dòng khi đang Thêm mới vẫn đưa vào danh sách hoàn kho (deleted)
        // Kỹ thuật         : Fix Bug / State Validation
        // Trước            : Xóa dòng ở trạng thái Thêm mới (AddNew) vẫn ghi nhận vào danh sách deleted và gọi hoàn kho, gây lỗi tồn kho sai lệch.
        // Sau              : Kiểm tra trạng thái (status != Controll.AddNew) trước khi add vào deleted và gọi deleted.Clear() khi nhấn Thêm mới.
        // Tác giả          : FE1 – Dương Thị Kim Anh
        // Hỗ trợ           : -
        // Kiểm tra         : QA
        // Ngày chỉnh sửa   : 05/11/2026
        // ===========================================================

        private void toolLuu_Them_Click(object sender, EventArgs e)
        {
            ctrlPhieuBan = new PhieuBanController();
            SetState(new StateThem());

            // [R18 - Fix Bug]: Làm sạch danh sách deleted khi bắt đầu tạo phiếu mới
            deleted.Clear();

            txtMaPhieu.Text = ThamSo.LayMaPhieuBan().ToString();
            numTongTien.Value = 0;
            ctrlChiTiet.HienThiChiTiet(dgvDanhsachSP, txtMaPhieu.Text);
            this.Allow(true);
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Bạn có chắc chắn xóa không?", "Phiếu Bán", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                BindingSource bs = ((BindingSource)dgvDanhsachSP.DataSource);
                DataRowView row = (DataRowView)bs.Current;
                numTongTien.Value -= Convert.ToInt64(row["THANH_TIEN"]);

                // [R18 - Fix Bug]: Chỉ ghi nhận vào deleted nếu KHÔNG phải đang ở trạng thái Thêm mới
                if (!(_currentState is StateThem))
                {
                    deleted.Add(new MaSanPham(Convert.ToString(row["ID_MA_SAN_PHAM"]), Convert.ToInt32(row["SO_LUONG"])));
                }

                bs.RemoveCurrent();
            }
        }

        private void dgvDanhsachSP_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
        {
            if (MessageBox.Show("Bạn có chắc chắn xóa không?", "Phiếu Bán", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            {
                e.Cancel = true;
            }
            else
            {
                BindingSource bs = ((BindingSource)dgvDanhsachSP.DataSource);
                DataRowView row = (DataRowView)bs.Current;
                numTongTien.Value -= Convert.ToInt64(row["THANH_TIEN"]);

                // [R18 - Fix Bug]: Chỉ ghi nhận vào deleted nếu KHÔNG phải đang ở trạng thái Thêm mới
                if (!(_currentState is StateThem))
                {
                    deleted.Add(new MaSanPham(Convert.ToString(row["ID_MA_SAN_PHAM"]), Convert.ToInt32(row["SO_LUONG"])));
                }
            }
        }

        private void toolLuuIn_Click(object sender, EventArgs e)
        {
            if (!(_currentState is StateXem))
            {
                MessageBox.Show("Vui lòng lưu lại Phiếu bán hiện tại!", "Phiếu Bán", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                String ma_phieu = txtMaPhieu.Text;
                PhieuBanController ctrlPB = new PhieuBanController();
                CuahangNongduoc.BusinessObject.PhieuBan ph = ctrlPB.LayPhieuBan(ma_phieu);
                frmInPhieuBan InPhieuBan = new frmInPhieuBan(ph);
                InPhieuBan.Show();
            }
        }

        private void dgvDanhsachSP_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            e.Cancel = true;
        }

        private void toolChinhSua_Click(object sender, EventArgs e)
        {
            SetState(new StateSua());
        }

        void Allow(bool val)
        {
            txtMaPhieu.Enabled = val;
            dtNgayLapPhieu.Enabled = val;
            numTongTien.Enabled = val;
            btnAdd.Enabled = val;
            btnRemove.Enabled = val;
            dgvDanhsachSP.Enabled = val;
        }

        private void toolThoat_Click(object sender, EventArgs e)
        {
            if (!(_currentState is StateXem))
            {
                if (MessageBox.Show("Bạn có muốn lưu lại Phiếu bán này không?", "Phiếu Bán", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    this.Luu();
                }
            }
            this.Close();
        }

        // ==================== [D09] REFACTORING (FACADE PATTERN) ====================
        // Lỗi (Code smell) : Direct Factory Access – Form tự vòng lặp gọi Factory hoàn tồn kho khi xóa phiếu
        // Kỹ thuật         : Hide Delegate / Move Method via Facade
        // Trước            : Tự gọi ChiTietPhieuBanController lấy danh sách và lặp MaSanPhanFactory.CapNhatSoLuong
        // Sau              : Ủy nhiệm hoàn toàn cho phương thức _facade.XoaPhieuBan(maPhieu, ctrlPhieuBan)
        // Tác giả          : FE1 – Dương Thị Kim Anh
        // Hỗ trợ           : FE2 – Nguyễn Huỳnh Ngọc Anh
        // Kiểm tra         : QA1 – Huỳnh Lư Anh Minh
        // Ngày chỉnh sửa   : 10/11/2026
        // =================================================================================
        private void toolXoa_Click(object sender, EventArgs e)
        {
            DataRowView view = (DataRowView)bindingNavigator.BindingSource.Current;
            if (view != null)
            {
                if (MessageBox.Show("Bạn có chắc chắn xóa không?", "Phiếu Bán", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    string maPhieu = view["ID"].ToString();
                    // GỌI FACADE XÓA VÀ HOÀN TỒN KHO ẨN DƯỚI TẦNG BACKEND
                    if (_facade.XoaPhieuBan(maPhieu, ctrlPhieuBan))
                    {
                        bindingNavigator.BindingSource.RemoveCurrent();
                    }
                }
            }
        }

        private void toolXemLai_Click(object sender, EventArgs e)
        {
            ctrlSanPham.HienthiAutoComboBox(cmbSanPham);
            ctrlMaSanPham.HienThiDataGridViewComboBox(colMaSanPham);
            ctrlKhachHang.HienthiAutoComboBox(cmbKhachHang, _strategy != null && _strategy.IsDaiLy);
        }

        private void btnThemDaiLy_Click(object sender, EventArgs e)
        {
            // Kết hợp kết quả từ task FE-06a: truyền cờ IsDaiLy trực tiếp vào form Khách hàng/Đại lý
            bool isDaiLy = _strategy != null ? _strategy.IsDaiLy : false;
            frmKhachHangDaiLy KhachHang = new frmKhachHangDaiLy(isDaiLy);
            KhachHang.ShowDialog();
            ctrlKhachHang.HienthiAutoComboBox(cmbKhachHang, isDaiLy);
        }

        private void btnThemSanPham_Click(object sender, EventArgs e)
        {
            frmSanPham SanPham = new frmSanPham();
            SanPham.ShowDialog();
            ctrlSanPham.HienthiAutoComboBox(cmbSanPham);
        }

        // ==================== [FE-08a] REFACTORING ====================
        // Lỗi (Code smell) : Missing Real-time Calculation – Đổi Số lượng trên lưới không tự tính lại Thành tiền
        // Kỹ thuật         : Fix Bug / Event-driven Update
        // Trước            : Sửa cột SO_LUONG trên dgvDanhsachSP không tự cập nhật THANH_TIEN và numTongTien
        // Sau              : Tự động tính lại rowView["THANH_TIEN"] = soLuong * donGia và cập nhật lại numTongTien.Value
        // Tác giả          : FE1 – Dương Thị Kim Anh
        // Hỗ trợ           : —
        // Kiểm tra         : QA1 
        // Ngày chỉnh sửa   : 05/11/2026
        // ===========================================================
        private void dgvDanhsachSP_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && dgvDanhsachSP.Columns[e.ColumnIndex].DataPropertyName == "SO_LUONG")
            {
                DataGridViewRow gridRow = dgvDanhsachSP.Rows[e.RowIndex];
                if (gridRow.DataBoundItem is DataRowView rowView)
                {
                    long soLuong = rowView["SO_LUONG"] != DBNull.Value ? Convert.ToInt64(rowView["SO_LUONG"]) : 0;
                    long donGia = rowView["DON_GIA"] != DBNull.Value ? Convert.ToInt64(rowView["DON_GIA"]) : 0;
                    rowView["THANH_TIEN"] = soLuong * donGia;

                    if (dgvDanhsachSP.DataSource is BindingSource bs && bs.DataSource is DataTable dt)
                    {
                        decimal tong = 0;
                        foreach (DataRow r in dt.Rows)
                        {
                            if (r.RowState != DataRowState.Deleted && r["THANH_TIEN"] != DBNull.Value)
                            {
                                tong += Convert.ToDecimal(r["THANH_TIEN"]);
                            }
                        }
                        numTongTien.Value = tong;
                    }
                }
            }
        }

        // Đẩy giá trị ô vừa sửa xuống DataTable ngay để CellValueChanged tính lại Thành tiền
        // (hàm bị mất khi giải quyết xung đột ở 013d74e, khôi phục từ 1789e88)
        private void dgvDanhsachSP_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            dgvDanhsachSP.CommitEdit(DataGridViewDataErrorContexts.Commit);
        }

    }
}
