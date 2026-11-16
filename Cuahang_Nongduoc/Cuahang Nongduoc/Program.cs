using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace CuahangNongduoc
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            if (args != null && Array.Exists(args, x => String.Equals(x, "--migrate-discount-report", StringComparison.OrdinalIgnoreCase)))
            {
                try
                {
                    DataService.EnsureColumn("PHIEU_BAN", "ID_NGUOI_DUNG", "LONG");
                    MessageBox.Show("Đã cập nhật database: PHIEU_BAN.ID_NGUOI_DUNG.", "Migration hoàn tất", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Không cập nhật được database: " + ex.Message, "Migration thất bại", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            while (true)
            {
                Auth.UserSession.Logout();

                using (var login = new frmDangNhap())
                {
                    if (login.ShowDialog() != DialogResult.OK || !Auth.UserSession.IsLoggedIn)
                        return;
                }

                // Bắt buộc đổi mật khẩu trước khi truy cập màn hình chính.
                try
                {
                    var accountData = new Auth.NguoiDungData();
                    var account = accountData.LayTheoId(Auth.UserSession.UserId);
                    if (account == null) { Auth.UserSession.Logout(); return; }
                    if (account.MustChangePwd)
                    {
                        using (var change = new frmDoiMatKhau()) change.ShowDialog();
                        account = accountData.LayTheoId(Auth.UserSession.UserId);
                        if (account == null || account.MustChangePwd)
                        {
                            MessageBox.Show("Bạn phải đổi mật khẩu trước khi sử dụng hệ thống.", "Yêu cầu bảo mật");
                            Auth.UserSession.Logout();
                            return;
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Không thể kiểm tra trạng thái tài khoản: " + ex.Message,
                        "Lỗi đăng nhập", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Auth.UserSession.Logout(); return;
                }

                Application.Run(new frmMain());

                // Đăng xuất sẽ xóa phiên và quay lại màn hình đăng nhập.
                // Đóng ứng dụng bình thường vẫn giữ phiên đến đây và kết thúc vòng lặp.
                if (!Auth.UserSession.IsLoggedIn)
                    continue;

                Auth.UserSession.Logout();
                return;
            }

            Application.Run(new frmMain());
        }
    }
}
