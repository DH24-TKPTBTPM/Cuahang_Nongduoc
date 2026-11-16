using System;
using System.Windows.Forms;
using CuahangNongduoc.Auth;

namespace CuahangNongduoc
{
    public partial class frmTaiKhoanChiTiet : Form
    {
        private class RoleOption
        {
            public string Key { get; set; }
            public string Value { get; set; }
            public override string ToString() { return Value; }
        }

        private readonly NguoiDung _nguoiDung;
        private readonly bool _laCheDoThem;
        private readonly NguoiDungData _data;

        public frmTaiKhoanChiTiet()
        {
            InitializeComponent();
            _laCheDoThem = true;
            _nguoiDung = new NguoiDung();
            _data = new NguoiDungData();
        }

        public frmTaiKhoanChiTiet(NguoiDung nguoiDung)
        {
            InitializeComponent();
            _laCheDoThem = false;
            _nguoiDung = nguoiDung;
            _data = new NguoiDungData();
        }

        private void frmTaiKhoanChiTiet_Load(object sender, EventArgs e)
        {
            cboRole.Items.Clear();
            cboRole.Items.Add(new RoleOption { Key = "admin", Value = "Quản trị viên" });
            cboRole.Items.Add(new RoleOption { Key = "manager", Value = "Quản lý" });
            cboRole.Items.Add(new RoleOption { Key = "sales", Value = "Nhân viên bán hàng" });
            cboRole.SelectedIndex = 2; // sales

            if (_laCheDoThem)
            {
                lblTitle.Text = "THÊM TÀI KHOẢN MỚI";
                this.Text = "Thêm tài khoản mới";
                panelMatKhau.Visible = true;
                txtTenDangNhap.Enabled = true;
                this.Height = 550;
            }
            else
            {
                lblTitle.Text = "SỬA THÔNG TIN TÀI KHOẢN";
                this.Text = "Sửa thông tin tài khoản";
                panelMatKhau.Visible = false;
                txtTenDangNhap.Text = _nguoiDung.TenDangNhap;
                txtTenDangNhap.Enabled = false;
                txtHoTen.Text = _nguoiDung.HoTen;

                for (int i = 0; i < cboRole.Items.Count; i++)
                {
                    RoleOption item = cboRole.Items[i] as RoleOption;
                    if (item != null && item.Key == _nguoiDung.Role)
                    {
                        cboRole.SelectedIndex = i;
                        break;
                    }
                }

                chkIsActive.Checked = _nguoiDung.IsActive;
                chkMustChange.Checked = _nguoiDung.MustChangePwd;

                btnLuu.Top = panelMatKhau.Top + 10;
                btnHuy.Top = panelMatKhau.Top + 10;
                this.Height = 390;
            }
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            string tenDangNhap = txtTenDangNhap.Text.Trim();
            string hoTen = txtHoTen.Text.Trim();
            RoleOption selectedRole = cboRole.SelectedItem as RoleOption;
            string role = selectedRole != null ? selectedRole.Key : "sales";
            bool isActive = chkIsActive.Checked;
            bool mustChange = chkMustChange.Checked;

            if (string.IsNullOrEmpty(tenDangNhap))
            {
                MessageBox.Show("Vui lòng nhập tên đăng nhập.", "Cảnh báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtTenDangNhap.Focus();
                return;
            }

            if (string.IsNullOrEmpty(hoTen))
            {
                MessageBox.Show("Vui lòng nhập họ và tên.", "Cảnh báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtHoTen.Focus();
                return;
            }

            if (_laCheDoThem)
            {
                string matKhau = txtMatKhau.Text;
                string xacNhan = txtXacNhanMK.Text;

                if (string.IsNullOrEmpty(matKhau))
                {
                    MessageBox.Show("Vui lòng nhập mật khẩu khởi tạo.", "Cảnh báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtMatKhau.Focus();
                    return;
                }

                if (matKhau.Length < 6)
                {
                    MessageBox.Show("Mật khẩu phải có ít nhất 6 ký tự.", "Cảnh báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtMatKhau.Focus();
                    return;
                }

                if (matKhau != xacNhan)
                {
                    MessageBox.Show("Xác nhận mật khẩu không trùng khớp.", "Cảnh báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtXacNhanMK.Focus();
                    return;
                }

                if (_data.LayTheoTenDangNhap(tenDangNhap) != null)
                {
                    MessageBox.Show("Tên đăng nhập [" + tenDangNhap + "] đã tồn tại trong hệ thống.",
                        "Trùng tên đăng nhập", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtTenDangNhap.Focus();
                    return;
                }

                try
                {
                    string salt;
                    string hash = PasswordHasher.HashPassword(matKhau, out salt);

                    var ndMoi = new NguoiDung
                    {
                        TenDangNhap = tenDangNhap,
                        MatKhau = hash,
                        Salt = salt,
                        HoTen = hoTen,
                        Role = role,
                        IsActive = isActive,
                        MustChangePwd = mustChange
                    };

                    _data.Them(ndMoi);
                    MessageBox.Show("Thêm tài khoản [" + tenDangNhap + "] thành công!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi thêm tài khoản: " + ex.Message, "Lỗi",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                try
                {
                    _nguoiDung.HoTen = hoTen;
                    _nguoiDung.Role = role;
                    _nguoiDung.IsActive = isActive;
                    _nguoiDung.MustChangePwd = mustChange;

                    _data.CapNhatThongTin(_nguoiDung);
                    MessageBox.Show("Cập nhật thông tin tài khoản thành công!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi cập nhật: " + ex.Message, "Lỗi",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnHuy_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
