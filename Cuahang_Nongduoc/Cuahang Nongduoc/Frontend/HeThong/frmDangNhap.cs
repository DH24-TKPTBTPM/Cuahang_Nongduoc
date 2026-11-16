using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using CuahangNongduoc.Auth;

namespace CuahangNongduoc
{
    /// <summary>
    /// Form đăng nhập hệ thống - Giao diện doanh nghiệp hiện đại.
    /// Hỗ trợ co giãn, phóng to toàn màn hình hoặc thu nhỏ linh hoạt.
    /// </summary>
    public partial class frmDangNhap : Form
    {
        private readonly IAuthenticationAdapter _authService;

        public bool DangNhapThanhCong { get; private set; }
        public Auth.NguoiDung NguoiDungDangNhap { get; private set; }

        private bool _userFocused = false;
        private bool _passFocused = false;

        public frmDangNhap()
        {
            InitializeComponent();
            _authService = new AuthenticationAdapter();
            DangNhapThanhCong = false;

            // Make badge rounded
            lblBadge.Paint += (s, e) =>
            {
                using (GraphicsPath path = GetRoundedPath(lblBadge.ClientRectangle, 6))
                using (Pen pen = new Pen(Color.FromArgb(52, 211, 153), 1))
                {
                    e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                    e.Graphics.DrawPath(pen, path);
                }
            };
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

        private void panelBody_Resize(object sender, EventArgs e)
        {
            CenterCard();
        }

        private void CenterCard()
        {
            if (panelBody == null || panelCard == null) return;
            int left = (panelBody.ClientSize.Width - panelCard.Width) / 2;
            int top = (panelBody.ClientSize.Height - panelCard.Height) / 2;
            panelCard.Location = new Point(Math.Max(10, left), Math.Max(10, top));
        }

        private void panelCard_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            using (GraphicsPath path = GetRoundedPath(new Rectangle(0, 0, panelCard.Width - 1, panelCard.Height - 1), 8))
            using (Pen pen = new Pen(Color.FromArgb(226, 232, 240), 1))
            {
                e.Graphics.DrawPath(pen, path);
            }
        }

        private void btnDangNhap_Click(object sender, EventArgs e)
        {
            ThucHienDangNhap();
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            DangNhapThanhCong = false;
            this.Close();
        }

        private void txtMatKhau_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                ThucHienDangNhap();
            }
        }

        private void txtTenDangNhap_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                txtMatKhau.Focus();
            }
        }

        private void chkHienMatKhau_CheckedChanged(object sender, EventArgs e)
        {
            txtMatKhau.UseSystemPasswordChar = !chkHienMatKhau.Checked;
        }

        private void Input_Enter(object sender, EventArgs e)
        {
            if (sender == txtTenDangNhap) _userFocused = true;
            if (sender == txtMatKhau) _passFocused = true;
            pnlUserBorder.Invalidate();
            pnlPassBorder.Invalidate();
        }

        private void Input_Leave(object sender, EventArgs e)
        {
            if (sender == txtTenDangNhap) _userFocused = false;
            if (sender == txtMatKhau) _passFocused = false;
            pnlUserBorder.Invalidate();
            pnlPassBorder.Invalidate();
        }

        private void pnlInput_Paint(object sender, PaintEventArgs e)
        {
            Panel pnl = sender as Panel;
            if (pnl == null) return;

            bool isFocused = (pnl == pnlUserBorder && _userFocused) || (pnl == pnlPassBorder && _passFocused);
            Color borderColor = isFocused ? Color.FromArgb(16, 149, 93) : Color.FromArgb(203, 213, 225);
            int borderWidth = isFocused ? 2 : 1;

            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            using (GraphicsPath path = GetRoundedPath(new Rectangle(0, 0, pnl.Width - 1, pnl.Height - 1), 6))
            using (Pen pen = new Pen(borderColor, borderWidth))
            {
                e.Graphics.DrawPath(pen, path);
            }
        }

        private void panelHeader_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            // Gradient background
            using (LinearGradientBrush brush = new LinearGradientBrush(
                panelHeader.ClientRectangle,
                Color.FromArgb(16, 44, 34),
                Color.FromArgb(28, 77, 58),
                LinearGradientMode.Vertical))
            {
                e.Graphics.FillRectangle(brush, panelHeader.ClientRectangle);
            }

            // Bottom border highlight line
            using (Pen pen = new Pen(Color.FromArgb(52, 211, 153), 1))
            {
                e.Graphics.DrawLine(pen, 0, panelHeader.Height - 1, panelHeader.Width, panelHeader.Height - 1);
            }
        }

        private GraphicsPath GetRoundedPath(Rectangle rect, int radius)
        {
            GraphicsPath path = new GraphicsPath();
            int diameter = radius * 2;
            path.AddArc(rect.X, rect.Y, diameter, diameter, 180, 90);
            path.AddArc(rect.Right - diameter, rect.Y, diameter, diameter, 270, 90);
            path.AddArc(rect.Right - diameter, rect.Bottom - diameter, diameter, diameter, 0, 90);
            path.AddArc(rect.X, rect.Bottom - diameter, diameter, diameter, 90, 90);
            path.CloseFigure();
            return path;
        }

        private void ThucHienDangNhap()
        {
            lblThongBao.Text = "";
            lblThongBao.Visible = false;

            if (string.IsNullOrWhiteSpace(txtTenDangNhap.Text))
            {
                HienThiLoi("Vui lòng nhập tên đăng nhập.");
                txtTenDangNhap.Focus();
                return;
            }

            if (string.IsNullOrEmpty(txtMatKhau.Text))
            {
                HienThiLoi("Vui lòng nhập mật khẩu.");
                txtMatKhau.Focus();
                return;
            }

            btnDangNhap.Enabled = false;
            btnThoat.Enabled = false;
            this.Cursor = Cursors.WaitCursor;

            try
            {
                Auth.NguoiDung nguoiDung;
                LoginResult ketQua = _authService.DangNhap(
                    txtTenDangNhap.Text.Trim(),
                    txtMatKhau.Text,
                    out nguoiDung);

                switch (ketQua)
                {
                    case LoginResult.Success:
                        NguoiDungDangNhap = nguoiDung;
                        DangNhapThanhCong = true;
                        this.DialogResult = DialogResult.OK;
                        this.Close();
                        break;

                    case LoginResult.MustChangePwd:
                        NguoiDungDangNhap = nguoiDung;
                        DangNhapThanhCong = true;
                        this.DialogResult = DialogResult.OK;
                        this.Close();
                        break;

                    case LoginResult.AccountLocked:
                        HienThiLoi("Tài khoản này đã bị khóa. Vui lòng liên hệ quản trị viên.");
                        break;

                    case LoginResult.InvalidCredentials:
                        HienThiLoi("Tên đăng nhập hoặc mật khẩu không chính xác.");
                        txtMatKhau.Clear();
                        txtMatKhau.Focus();
                        break;

                    case LoginResult.DatabaseError:
                        HienThiLoi("Không thể kết nối cơ sở dữ liệu. Vui lòng kiểm tra lại.");
                        break;

                    default:
                        HienThiLoi("Đã xảy ra lỗi không xác định. Vui lòng thử lại.");
                        break;
                }
            }
            catch (Exception ex)
            {
                HienThiLoi("Lỗi hệ thống: " + ex.Message);
            }
            finally
            {
                btnDangNhap.Enabled = true;
                btnThoat.Enabled = true;
                this.Cursor = Cursors.Default;
            }
        }

        private void HienThiLoi(string thongBao)
        {
            lblThongBao.Text = "⚠️ " + thongBao;
            lblThongBao.Visible = true;
        }
    }
}
