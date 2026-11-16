using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using CuahangNongduoc.Auth;

namespace CuahangNongduoc
{
    public partial class frmQuanLyTaiKhoan : Form
    {
        private readonly NguoiDungData _data;
        private List<NguoiDung> _danhSach;
        private NguoiDung _taiKhoanDangChon = null;
        private int _sortColumnIndex = -1;
        private bool _sortAscending = true;

        public frmQuanLyTaiKhoan()
        {
            InitializeComponent();
            _data = new NguoiDungData();
        }

        private void frmQuanLyTaiKhoan_Load(object sender, EventArgs e)
        {
            if (!AuthorizationService.CoQuyen(AuthorizationService.Permission.QuanLyTaiKhoan))
            {
                MessageBox.Show("Chỉ Quản trị viên mới có quyền truy cập chức năng này.",
                    "Từ chối truy cập", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.Close();
                return;
            }

            CauHinhGrid();
            TaiDanhSach();
        }

        private void CauHinhGrid()
        {
            dgvTaiKhoan.AutoGenerateColumns = false;
            dgvTaiKhoan.Columns.Clear();
            dgvTaiKhoan.Columns.Add(new DataGridViewTextBoxColumn { Name = "colId", HeaderText = "ID", DataPropertyName = "Id", Width = 55, ReadOnly = true, SortMode = DataGridViewColumnSortMode.Programmatic });
            dgvTaiKhoan.Columns.Add(new DataGridViewTextBoxColumn { Name = "colTen", HeaderText = "Tên đăng nhập", DataPropertyName = "TenDangNhap", Width = 160, ReadOnly = true, SortMode = DataGridViewColumnSortMode.Programmatic });
            dgvTaiKhoan.Columns.Add(new DataGridViewTextBoxColumn { Name = "colHoTen", HeaderText = "Họ tên", DataPropertyName = "HoTen", Width = 220, ReadOnly = true, SortMode = DataGridViewColumnSortMode.Programmatic });
            dgvTaiKhoan.Columns.Add(new DataGridViewTextBoxColumn { Name = "colRole", HeaderText = "Vai trò", DataPropertyName = "RoleDisplayName", Width = 170, ReadOnly = true, SortMode = DataGridViewColumnSortMode.Programmatic });
            dgvTaiKhoan.Columns.Add(new DataGridViewCheckBoxColumn { Name = "colActive", HeaderText = "Hoạt động", DataPropertyName = "IsActive", Width = 95, ReadOnly = true, SortMode = DataGridViewColumnSortMode.Programmatic });
            dgvTaiKhoan.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvTaiKhoan.MultiSelect = false;
            dgvTaiKhoan.ReadOnly = true;
            dgvTaiKhoan.AllowUserToAddRows = false;
            dgvTaiKhoan.RowHeadersVisible = false;
            dgvTaiKhoan.BackgroundColor = Color.White;
            dgvTaiKhoan.SelectionChanged += new EventHandler(dgvTaiKhoan_SelectionChanged);
        }

        private void TaiDanhSach()
        {
            try
            {
                _danhSach = _data.LayTatCa();
                HienThiDanhSach(_danhSach);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải danh sách người dùng: " + ex.Message,
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void HienThiDanhSach(List<NguoiDung> list)
        {
            dgvTaiKhoan.DataSource = null;
            dgvTaiKhoan.DataSource = list;
            lblSoLuong.Text = "Tổng: " + (list != null ? list.Count : 0) + " tài khoản";
            CapNhatTrangThaiNut();
        }

        private void CapNhatTrangThaiNut()
        {
            bool coChon = _taiKhoanDangChon != null;
            btnSua.Enabled = coChon;
            btnKhoaMoKhoa.Enabled = coChon;
            btnDatLaiMatKhau.Enabled = coChon;
            btnXoaNguoiDung.Enabled = coChon;

            if (coChon)
            {
                btnKhoaMoKhoa.Text = _taiKhoanDangChon.IsActive ? "Khóa tài khoản" : "Mở khóa";
            }
            else
            {
                btnKhoaMoKhoa.Text = "Khóa / Mở khóa";
            }
        }

        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            string tuKhoa = txtTimKiem.Text.Trim();
            if (string.IsNullOrEmpty(tuKhoa))
            {
                HienThiDanhSach(_danhSach);
                return;
            }

            string kw = tuKhoa.ToLower();
            var ketQua = (_danhSach ?? _data.LayTatCa()).FindAll(x =>
                (x.TenDangNhap != null && x.TenDangNhap.ToLower().Contains(kw)) ||
                (x.HoTen != null && x.HoTen.ToLower().Contains(kw)));
            HienThiDanhSach(ketQua);
        }

        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            txtTimKiem.Clear();
            TaiDanhSach();
        }

        private void txtTimKiem_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnTimKiem_Click(sender, e);
                e.Handled = true;
                e.SuppressKeyPress = true;
            }
        }

        private void btnThemMoi_Click(object sender, EventArgs e)
        {
            using (var form = new frmTaiKhoanChiTiet())
            {
                if (form.ShowDialog(this) == DialogResult.OK)
                {
                    TaiDanhSach();
                }
            }
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            if (_taiKhoanDangChon == null) return;

            using (var form = new frmTaiKhoanChiTiet(_taiKhoanDangChon))
            {
                if (form.ShowDialog(this) == DialogResult.OK)
                {
                    TaiDanhSach();
                }
            }
        }

        private void btnKhoaMoKhoa_Click(object sender, EventArgs e)
        {
            if (_taiKhoanDangChon == null) return;

            if (_taiKhoanDangChon.Id == UserSession.UserId)
            {
                MessageBox.Show("Bạn không thể tự khóa tài khoản của chính mình.",
                    "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            bool trangThaiMoi = !_taiKhoanDangChon.IsActive;
            string hanhDong = trangThaiMoi ? "mở khóa" : "khóa";

            if (MessageBox.Show("Bạn có chắc muốn " + hanhDong + " tài khoản [" + _taiKhoanDangChon.TenDangNhap + "]?",
                "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                return;

            try
            {
                _data.DatTrangThai(_taiKhoanDangChon.Id, trangThaiMoi);
                TaiDanhSach();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi cập nhật trạng thái: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnDatLaiMatKhau_Click(object sender, EventArgs e)
        {
            if (_taiKhoanDangChon == null) return;

            using (var form = new frmDatLaiMatKhau(_taiKhoanDangChon))
            {
                form.ShowDialog(this);
            }
        }

        private void btnXoaNguoiDung_Click(object sender, EventArgs e)
        {
            if (_taiKhoanDangChon == null) return;

            if (_taiKhoanDangChon.Id == UserSession.UserId)
            {
                MessageBox.Show("Bạn không thể tự xóa tài khoản của chính mình đang đăng nhập.",
                    "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (MessageBox.Show("Bạn có chắc chắn muốn xóa vĩnh viễn tài khoản [" + _taiKhoanDangChon.TenDangNhap + "]?\nHành động này không thể hoàn tác!",
                "Cảnh báo xóa tài khoản", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes)
                return;

            try
            {
                _data.XoaNguoiDung(_taiKhoanDangChon.Id);
                TaiDanhSach();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi xóa tài khoản: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnDong_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void dgvTaiKhoan_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0 || e.RowIndex >= dgvTaiKhoan.Rows.Count) return;

            var nd = dgvTaiKhoan.Rows[e.RowIndex].DataBoundItem as NguoiDung;
            if (nd != null && !nd.IsActive)
            {
                e.CellStyle.ForeColor = Color.FromArgb(148, 163, 184);
                e.CellStyle.Font = new Font(dgvTaiKhoan.Font, FontStyle.Italic);
            }
        }

        private void dgvTaiKhoan_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (_danhSach == null || _danhSach.Count == 0) return;

            if (_sortColumnIndex == e.ColumnIndex)
                _sortAscending = !_sortAscending;
            else
            {
                _sortColumnIndex = e.ColumnIndex;
                _sortAscending = true;
            }

            string propName = dgvTaiKhoan.Columns[e.ColumnIndex].DataPropertyName;
            if (!string.IsNullOrEmpty(propName))
            {
                _danhSach.Sort((a, b) =>
                {
                    var prop = typeof(NguoiDung).GetProperty(propName);
                    if (prop == null) return 0;
                    var valA = prop.GetValue(a, null) as IComparable;
                    var valB = prop.GetValue(b, null) as IComparable;
                    if (valA == null && valB == null) return 0;
                    if (valA == null) return _sortAscending ? -1 : 1;
                    if (valB == null) return _sortAscending ? 1 : -1;
                    return _sortAscending ? valA.CompareTo(valB) : valB.CompareTo(valA);
                });
                HienThiDanhSach(_danhSach);
            }
        }

        private void dgvTaiKhoan_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvTaiKhoan.CurrentRow != null && dgvTaiKhoan.CurrentRow.Index >= 0)
            {
                _taiKhoanDangChon = dgvTaiKhoan.CurrentRow.DataBoundItem as NguoiDung;
            }
            else
            {
                _taiKhoanDangChon = null;
            }
            CapNhatTrangThaiNut();
        }

        private void dgvTaiKhoan_DoubleClick(object sender, EventArgs e)
        {
            if (_taiKhoanDangChon != null)
            {
                btnSua_Click(sender, e);
            }
        }
    }
}
