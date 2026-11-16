using System;
using System.Windows.Forms;
using CuahangNongduoc.Auth;

namespace CuahangNongduoc
{
    /// <summary>
    /// Form đổi mật khẩu cho người dùng đã đăng nhập.
    /// Yêu cầu xác thực mật khẩu hiện tại trước khi đổi.
    /// </summary>
    public partial class frmDoiMatKhau : Form
    {
        private readonly NguoiDungData _data;

        public frmDoiMatKhau()
        {
            InitializeComponent();
            _data = new NguoiDungData();
        }

        private void frmDoiMatKhau_Load(object sender, EventArgs e)
        {
            // Kiểm tra quyền
            if (!AuthorizationService.CoQuyen(AuthorizationService.Permission.DoiMatKhau))
            {
                MessageBox.Show("Bạn không có quyền thực hiện chức năng này.", "Từ chối truy cập",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.Close();
                return;
            }

            txtMatKhauCu.Focus();
        }

        private void btnDoiMatKhau_Click(object sender, EventArgs e)
        {
            ThucHienDoiMatKhau();
        }

        private void btnHuy_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void chkHienThi_CheckedChanged(object sender, EventArgs e)
        {
            bool hien = chkHienThi.Checked;
            txtMatKhauCu.UseSystemPasswordChar = !hien;
            txtMatKhauMoi.UseSystemPasswordChar = !hien;
            txtXacNhan.UseSystemPasswordChar = !hien;
        }

        private void ThucHienDoiMatKhau()
        {
            lblThongBao.Text = "";
            lblThongBao.ForeColor = System.Drawing.Color.Red;

            // Kiểm tra đầu vào
            if (string.IsNullOrEmpty(txtMatKhauCu.Text))
            {
                HienThiLoi("Vui lòng nhập mật khẩu hiện tại.");
                txtMatKhauCu.Focus();
                return;
            }

            if (string.IsNullOrEmpty(txtMatKhauMoi.Text))
            {
                HienThiLoi("Vui lòng nhập mật khẩu mới.");
                txtMatKhauMoi.Focus();
                return;
            }

            if (txtMatKhauMoi.Text.Length < 6)
            {
                HienThiLoi("Mật khẩu mới phải có ít nhất 6 ký tự.");
                txtMatKhauMoi.Focus();
                return;
            }

            if (txtMatKhauMoi.Text != txtXacNhan.Text)
            {
                HienThiLoi("Mật khẩu mới và xác nhận không khớp.");
                txtXacNhan.Focus();
                return;
            }

            if (txtMatKhauMoi.Text == txtMatKhauCu.Text)
            {
                HienThiLoi("Mật khẩu mới phải khác mật khẩu hiện tại.");
                txtMatKhauMoi.Focus();
                return;
            }

            try
            {
                // Lấy thông tin người dùng hiện tại từ database để xác thực mật khẩu cũ
                NguoiDung nd = _data.LayTheoId(UserSession.UserId);
                if (nd == null)
                {
                    HienThiLoi("Không tìm thấy thông tin tài khoản.");
                    return;
                }

                // Xác thực mật khẩu hiện tại
                bool matKhauCuDung;
                if (PasswordHasher.IsPlainPassword(nd.MatKhau, nd.Salt))
                    matKhauCuDung = PasswordHasher.VerifyPlainPassword(txtMatKhauCu.Text, nd.MatKhau);
                else
                    matKhauCuDung = PasswordHasher.VerifyPassword(txtMatKhauCu.Text, nd.MatKhau, nd.Salt);

                if (!matKhauCuDung)
                {
                    HienThiLoi("Mật khẩu hiện tại không đúng.");
                    txtMatKhauCu.Clear();
                    txtMatKhauCu.Focus();
                    return;
                }

                // Tạo hash mật khẩu mới
                string salt;
                string hash = PasswordHasher.HashPassword(txtMatKhauMoi.Text, out salt);

                // Cập nhật vào database
                bool ok = _data.CapNhatMatKhau(UserSession.UserId, hash, salt);
                if (ok)
                {
                    lblThongBao.ForeColor = System.Drawing.Color.Green;
                    lblThongBao.Text = "✓ Đổi mật khẩu thành công!";
                    txtMatKhauCu.Clear();
                    txtMatKhauMoi.Clear();
                    txtXacNhan.Clear();

                    MessageBox.Show("Đổi mật khẩu thành công!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Close();
                }
                else
                {
                    HienThiLoi("Không thể cập nhật mật khẩu. Vui lòng thử lại.");
                }
            }
            catch (Exception ex)
            {
                HienThiLoi("Lỗi: " + ex.Message);
            }
        }

        private void HienThiLoi(string msg)
        {
            lblThongBao.Text = msg;
            lblThongBao.Visible = true;
        }
    }
}
