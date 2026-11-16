using System;
using System.Drawing;
using System.Windows.Forms;
using CuahangNongduoc.Auth;

namespace CuahangNongduoc
{
    public partial class frmDangNhap : Form
    {
        private readonly IAuthenticationAdapter _authService;

        public bool DangNhapThanhCong { get; private set; }
        public Auth.NguoiDung NguoiDungDangNhap { get; private set; }

        public frmDangNhap()
        {
            InitializeComponent();
            _authService = new AuthenticationAdapter();
            DangNhapThanhCong = false;
        }

        private void frmDangNhap_Load(object sender, EventArgs e)
        {
            CenterCard();
            if (string.IsNullOrEmpty(txtTenDangNhap.Text))
            {
                txtTenDangNhap.Text = "vodichbatphuong";
            }
            txtMatKhau.Focus();
        }

        private void pnlBackground_Resize(object sender, EventArgs e)
        {
            CenterCard();
        }

        private void CenterCard()
        {
            if (pnlBackground == null || cardContainer == null) return;
            int left = (pnlBackground.ClientSize.Width - cardContainer.Width) / 2;
            int top = (pnlBackground.ClientSize.Height - cardContainer.Height) / 2;
            cardContainer.Location = new Point(Math.Max(10, left), Math.Max(10, top));
        }

        private void chkHienMatKhau_CheckedChanged(object sender, EventArgs e)
        {
            txtMatKhau.UseSystemPasswordChar = !chkHienMatKhau.Checked;
            txtMatKhau.PasswordChar = chkHienMatKhau.Checked ? '\0' : '●';
        }

        private void btnDangNhap_Click(object sender, EventArgs e)
        {
            ThucHienDangNhap();
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            DangNhapThanhCong = false;
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void ThucHienDangNhap()
        {
            string username = txtTenDangNhap.Text.Trim();
            string password = txtMatKhau.Text;

            if (string.IsNullOrEmpty(username))
            {
                HienThiThongBao("Vui lòng nhập tên đăng nhập.", true);
                txtTenDangNhap.Focus();
                return;
            }

            if (string.IsNullOrEmpty(password))
            {
                HienThiThongBao("Vui lòng nhập mật khẩu.", true);
                txtMatKhau.Focus();
                return;
            }

            btnDangNhap.Enabled = false;
            btnDangNhap.Text = "Đang xác thực...";
            Cursor = Cursors.WaitCursor;

            try
            {
                frmLoading.ShowLoading(this, "ĐANG XÁC THỰC TÀI KHOẢN...");
                var waitAuth = DateTime.Now.AddMilliseconds(300);
                while (DateTime.Now < waitAuth)
                {
                    Application.DoEvents();
                    System.Threading.Thread.Sleep(20);
                }

                Auth.NguoiDung nguoiDung;
                LoginResult result = _authService.DangNhap(username, password, out nguoiDung);

                switch (result)
                {
                    case LoginResult.Success:
                        frmLoading.ShowLoading(this, "ĐĂNG NHẬP THÀNH CÔNG! ĐANG VÀO HỆ THỐNG...");
                        var waitSuccess = DateTime.Now.AddMilliseconds(900);
                        while (DateTime.Now < waitSuccess)
                        {
                            Application.DoEvents();
                            System.Threading.Thread.Sleep(20);
                        }
                        frmLoading.CloseLoading();
                        DangNhapThanhCong = true;
                        NguoiDungDangNhap = nguoiDung;
                        DialogResult = DialogResult.OK;
                        Close();
                        break;

                    case LoginResult.MustChangePwd:
                        frmLoading.CloseLoading();
                        DangNhapThanhCong = true;
                        NguoiDungDangNhap = nguoiDung;
                        MessageBox.Show(
                            "Đây là lần đăng nhập đầu tiên hoặc mật khẩu đã được quản trị viên đặt lại.\nVui lòng đổi mật khẩu mới để tiếp tục.",
                            "Yêu cầu đổi mật khẩu",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information);

                        using (var formDoiMatKhau = new frmDoiMatKhau())
                        {
                            formDoiMatKhau.ShowDialog(this);
                        }
                        DialogResult = DialogResult.OK;
                        Close();
                        break;

                    case LoginResult.AccountLocked:
                        frmLoading.CloseLoading();
                        HienThiThongBao("Tài khoản này đã bị khóa. Vui lòng liên hệ quản trị viên.", true);
                        break;

                    case LoginResult.InvalidCredentials:
                        frmLoading.CloseLoading();
                        HienThiThongBao("Tên đăng nhập hoặc mật khẩu không chính xác.", true);
                        txtMatKhau.SelectAll();
                        txtMatKhau.Focus();
                        break;

                    case LoginResult.DatabaseError:
                        frmLoading.CloseLoading();
                        HienThiThongBao("Lỗi kết nối cơ sở dữ liệu. Vui lòng kiểm tra lại tệp cuahang.dll.", true);
                        break;
                }
            }
            catch (Exception ex)
            {
                frmLoading.CloseLoading();
                HienThiThongBao("Lỗi hệ thống: " + ex.Message, true);
            }
            finally
            {
                frmLoading.CloseLoading();
                btnDangNhap.Enabled = true;
                btnDangNhap.Text = "ĐĂNG NHẬP HỆ THỐNG";
                Cursor = Cursors.Default;
            }
        }

        private void HienThiThongBao(string thongBao, bool laLoi)
        {
            lblThongBao.Text = thongBao;
            lblThongBao.ForeColor = laLoi ? Color.FromArgb(220, 38, 38) : Color.FromArgb(37, 99, 235);
            lblThongBao.Visible = !string.IsNullOrEmpty(thongBao);
        }

        private void lblHint_Click(object sender, EventArgs e)
        {

        }
    }
}
