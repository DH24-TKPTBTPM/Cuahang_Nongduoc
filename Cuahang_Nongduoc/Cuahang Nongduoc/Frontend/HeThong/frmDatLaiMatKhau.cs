using System;
using System.Windows.Forms;
using CuahangNongduoc.Auth;

namespace CuahangNongduoc
{
    /// <summary>
    /// Form đặt lại mật khẩu cho tài khoản khác (Admin thực hiện).
    /// </summary>
    public partial class frmDatLaiMatKhau : Form
    {
        private readonly NguoiDungData _data;
        private readonly int _userId;
        private readonly string _tenDangNhap;

        public frmDatLaiMatKhau(int userId, string tenDangNhap)
        {
            InitializeComponent();
            _data = new NguoiDungData();
            _userId = userId;
            _tenDangNhap = tenDangNhap;
        }

        private void frmDatLaiMatKhau_Load(object sender, EventArgs e)
        {
            lblTaiKhoan.Text = string.Format("Tài khoản: {0}", _tenDangNhap);
        }

        private void btnDatLai_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtMatKhauMoi.Text))
            {
                MessageBox.Show("Vui lòng nhập mật khẩu mới.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtMatKhauMoi.Focus();
                return;
            }
            if (txtMatKhauMoi.Text.Length < 6)
            {
                MessageBox.Show("Mật khẩu mới phải có ít nhất 6 ký tự.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtMatKhauMoi.Focus();
                return;
            }
            if (txtMatKhauMoi.Text != txtXacNhan.Text)
            {
                MessageBox.Show("Mật khẩu và xác nhận không khớp.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtXacNhan.Focus();
                return;
            }

            try
            {
                string salt;
                string hash = PasswordHasher.HashPassword(txtMatKhauMoi.Text, out salt);
                bool ok = _data.DatLaiMatKhau(_userId, hash, salt);
                if (ok)
                {
                    MessageBox.Show(
                        string.Format("Đã đặt lại mật khẩu cho tài khoản '{0}'.\nNgười dùng sẽ phải đổi mật khẩu khi đăng nhập lần tiếp theo.", _tenDangNhap),
                        "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Không thể cập nhật mật khẩu. Vui lòng thử lại.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnHuy_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void chkHienThi_CheckedChanged(object sender, EventArgs e)
        {
            bool hien = chkHienThi.Checked;
            txtMatKhauMoi.UseSystemPasswordChar = !hien;
            txtXacNhan.UseSystemPasswordChar = !hien;
        }
    }
}
