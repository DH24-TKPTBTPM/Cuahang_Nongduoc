using System;
using System.Windows.Forms;
using CuahangNongduoc.Auth;

namespace CuahangNongduoc
{
    public partial class frmDatLaiMatKhau : Form
    {
        private readonly NguoiDung _nguoiDung;
        private readonly NguoiDungData _data;

        public frmDatLaiMatKhau(NguoiDung nguoiDung)
        {
            InitializeComponent();
            _nguoiDung = nguoiDung;
            _data = new NguoiDungData();
        }

        private void frmDatLaiMatKhau_Load(object sender, EventArgs e)
        {
            lblTaiKhoan.Text = "Tài khoản: " + _nguoiDung.TenDangNhap + " (" + _nguoiDung.HoTen + ")";
        }

        private void chkHienThi_CheckedChanged(object sender, EventArgs e)
        {
            txtMatKhauMoi.UseSystemPasswordChar = !chkHienThi.Checked;
            txtXacNhan.UseSystemPasswordChar = !chkHienThi.Checked;
            txtMatKhauMoi.PasswordChar = chkHienThi.Checked ? '\0' : '●';
            txtXacNhan.PasswordChar = chkHienThi.Checked ? '\0' : '●';
        }

        private void btnDatLai_Click(object sender, EventArgs e)
        {
            string matKhauMoi = txtMatKhauMoi.Text;
            string xacNhan = txtXacNhan.Text;

            if (string.IsNullOrEmpty(matKhauMoi))
            {
                MessageBox.Show("Vui lòng nhập mật khẩu mới.", "Cảnh báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtMatKhauMoi.Focus();
                return;
            }

            if (matKhauMoi.Length < 6)
            {
                MessageBox.Show("Mật khẩu mới phải có ít nhất 6 ký tự.", "Cảnh báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtMatKhauMoi.Focus();
                return;
            }

            if (matKhauMoi != xacNhan)
            {
                MessageBox.Show("Xác nhận mật khẩu không trùng khớp.", "Cảnh báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtXacNhan.Focus();
                return;
            }

            try
            {
                string salt;
                string hash = PasswordHasher.HashPassword(matKhauMoi, out salt);
                _data.DatLaiMatKhau(_nguoiDung.Id, hash, salt);

                MessageBox.Show(
                    "Đặt lại mật khẩu cho tài khoản [" + _nguoiDung.TenDangNhap + "] thành công!\n" +
                    "Người dùng sẽ được yêu cầu đổi mật khẩu ở lần đăng nhập tiếp theo.",
                    "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi đặt lại mật khẩu: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnHuy_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
