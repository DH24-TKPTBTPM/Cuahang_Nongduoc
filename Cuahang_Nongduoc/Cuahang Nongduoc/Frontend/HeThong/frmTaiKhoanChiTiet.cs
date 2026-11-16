using System;
using System.Windows.Forms;
using CuahangNongduoc.Auth;

namespace CuahangNongduoc
{
    /// <summary>
    /// Form thêm/sửa thông tin tài khoản.
    /// </summary>
    public partial class frmTaiKhoanChiTiet : Form
    {
        private readonly NguoiDungData _data;
        private readonly NguoiDung _taiKhoanHienTai; // null = thêm mới
        private readonly bool _laThemMoi;

        public frmTaiKhoanChiTiet(NguoiDung taiKhoan)
        {
            InitializeComponent();
            _data = new NguoiDungData();
            _taiKhoanHienTai = taiKhoan;
            _laThemMoi = (taiKhoan == null);
        }

        private void frmTaiKhoanChiTiet_Load(object sender, EventArgs e)
        {
            // Cài đặt combobox vai trò
            cboRole.Items.Clear();
            cboRole.Items.Add(new RoleItem("admin", "Quản trị viên"));
            cboRole.Items.Add(new RoleItem("sales", "Nhân viên bán hàng"));
            cboRole.Items.Add(new RoleItem("warehouse", "Nhân viên quản lý kho"));
            cboRole.DisplayMember = "TenHienThi";
            cboRole.ValueMember = "MaRole";

            if (_laThemMoi)
            {
                this.Text = "Thêm tài khoản mới";
                lblTitle.Text = "THÊM TÀI KHOẢN MỚI";
                cboRole.SelectedIndex = 1; // Mặc định: Sales
                chkMustChange.Checked = true;
                panelMatKhau.Visible = true;
                lblMatKhauNote.Visible = true;
            }
            else
            {
                this.Text = "Sửa thông tin tài khoản";
                lblTitle.Text = "SỬA THÔNG TIN TÀI KHOẢN";
                txtTenDangNhap.Text = _taiKhoanHienTai.TenDangNhap;
                txtHoTen.Text = _taiKhoanHienTai.HoTen;
                chkIsActive.Checked = _taiKhoanHienTai.IsActive;
                chkMustChange.Checked = _taiKhoanHienTai.MustChangePwd;

                // Chọn vai trò
                for (int i = 0; i < cboRole.Items.Count; i++)
                {
                    if (((RoleItem)cboRole.Items[i]).MaRole == (_taiKhoanHienTai.Role ?? "sales"))
                    {
                        cboRole.SelectedIndex = i;
                        break;
                    }
                }
                // Khi sửa, không hiện ô mật khẩu (dùng chức năng đặt lại mật khẩu riêng)
                panelMatKhau.Visible = false;
                lblMatKhauNote.Visible = false;
            }
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            if (!Validate_()) return;

            try
            {
                string tenDangNhap = txtTenDangNhap.Text.Trim();
                string hoTen = txtHoTen.Text.Trim();
                string role = ((RoleItem)cboRole.SelectedItem).MaRole;
                bool isActive = chkIsActive.Checked;
                bool mustChange = chkMustChange.Checked;

                if (!_laThemMoi && _taiKhoanHienTai.Id == UserSession.UserId &&
                    (!string.Equals(role, _taiKhoanHienTai.Role, StringComparison.OrdinalIgnoreCase) || !isActive))
                {
                    MessageBox.Show("Không thể tự thay đổi vai trò hoặc khóa tài khoản đang sử dụng.");
                    return;
                }

                if (!_laThemMoi && _taiKhoanHienTai.IsActive &&
                    string.Equals(_taiKhoanHienTai.Role, "admin", StringComparison.OrdinalIgnoreCase) &&
                    (!isActive || !string.Equals(role, "admin", StringComparison.OrdinalIgnoreCase)) &&
                    _data.DemAdminDangHoatDong() <= 1)
                {
                    MessageBox.Show("Không thể vô hiệu hóa hoặc hạ quyền Admin cuối cùng đang hoạt động.\nHệ thống cần ít nhất một Admin.",
                        "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (_laThemMoi)
                {
                    // Tạo hash mật khẩu ban đầu
                    string salt;
                    string hash = PasswordHasher.HashPassword(txtMatKhau.Text, out salt);

                    var nd = new NguoiDung
                    {
                        TenDangNhap = tenDangNhap,
                        HoTen = hoTen,
                        MatKhau = hash,
                        Salt = salt,
                        Role = role,
                        IsActive = isActive,
                        MustChangePwd = mustChange
                    };
                    _data.Them(nd);
                    MessageBox.Show("Tạo tài khoản thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    var nd = new NguoiDung
                    {
                        Id = _taiKhoanHienTai.Id,
                        TenDangNhap = tenDangNhap,
                        HoTen = hoTen,
                        Role = role,
                        IsActive = isActive,
                        MustChangePwd = mustChange
                    };
                    _data.CapNhatThongTin(nd);
                    MessageBox.Show("Cập nhật tài khoản thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (InvalidOperationException ex)
            {
                MessageBox.Show(ex.Message, "Lỗi dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool Validate_()
        {
            if (string.IsNullOrWhiteSpace(txtTenDangNhap.Text))
            {
                MessageBox.Show("Vui lòng nhập tên đăng nhập.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtTenDangNhap.Focus();
                return false;
            }

            // Kiểm tra tên đăng nhập không có ký tự đặc biệt
            string ten = txtTenDangNhap.Text.Trim();
            foreach (char c in ten)
            {
                if (!char.IsLetterOrDigit(c) && c != '_' && c != '.')
                {
                    MessageBox.Show("Tên đăng nhập chỉ được dùng chữ cái, số, dấu '_' và '.'.",
                        "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtTenDangNhap.Focus();
                    return false;
                }
            }

            if (cboRole.SelectedItem == null)
            {
                MessageBox.Show("Vui lòng chọn vai trò.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (_laThemMoi)
            {
                if (string.IsNullOrEmpty(txtMatKhau.Text))
                {
                    MessageBox.Show("Vui lòng nhập mật khẩu.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtMatKhau.Focus();
                    return false;
                }
                if (txtMatKhau.Text.Length < 6)
                {
                    MessageBox.Show("Mật khẩu phải có ít nhất 6 ký tự.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtMatKhau.Focus();
                    return false;
                }
                if (txtMatKhau.Text != txtXacNhanMK.Text)
                {
                    MessageBox.Show("Mật khẩu và xác nhận không khớp.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtXacNhanMK.Focus();
                    return false;
                }
            }

            return true;
        }

        private void btnHuy_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        // Class nhỏ để bind ComboBox
        private class RoleItem
        {
            public string MaRole { get; set; }
            public string TenHienThi { get; set; }
            public RoleItem(string ma, string ten) { MaRole = ma; TenHienThi = ten; }
            public override string ToString() { return TenHienThi; }
        }
    }
}
