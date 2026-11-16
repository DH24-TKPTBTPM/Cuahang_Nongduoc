using System;

namespace CuahangNongduoc.Auth
{
    /// <summary>
    /// Kết quả xác thực đăng nhập.
    /// </summary>
    public enum LoginResult
    {
        Success,
        InvalidCredentials,  // Sai tên hoặc mật khẩu
        AccountLocked,       // Tài khoản bị khóa
        MustChangePwd,       // Phải đổi mật khẩu
        DatabaseError        // Lỗi kết nối/truy vấn
    }

    /// <summary>
    /// Dịch vụ xác thực người dùng.
    /// Thực hiện kiểm tra tên đăng nhập, mật khẩu, trạng thái tài khoản và tự động
    /// migrate mật khẩu cũ (plain text) sang PBKDF2 khi đăng nhập thành công.
    /// </summary>
    public class AuthenticationService
    {
        private readonly NguoiDungData _data;

        public AuthenticationService()
        {
            _data = new NguoiDungData();
        }

        /// <summary>
        /// Thực hiện đăng nhập.
        /// </summary>
        /// <param name="tenDangNhap">Tên đăng nhập</param>
        /// <param name="matKhau">Mật khẩu người dùng nhập</param>
        /// <param name="nguoiDung">Output: Thông tin tài khoản nếu thành công</param>
        /// <returns>Kết quả xác thực</returns>
        public LoginResult DangNhap(string tenDangNhap, string matKhau, out NguoiDung nguoiDung)
        {
            nguoiDung = null;
            try
            {
                // Truy vấn theo tên đăng nhập (case-insensitive ở Access)
                NguoiDung nd = _data.LayTheoTenDangNhap(tenDangNhap);

                // Tên đăng nhập không tồn tại → trả về lỗi generic (không tiết lộ chi tiết)
                if (nd == null)
                    return LoginResult.InvalidCredentials;

                // Kiểm tra trạng thái tài khoản trước khi xác thực mật khẩu
                if (!nd.IsActive)
                    return LoginResult.AccountLocked;

                // Xác thực mật khẩu
                bool passwordOk;

                if (PasswordHasher.IsPlainPassword(nd.MatKhau, nd.Salt))
                {
                    // Tài khoản cũ: mật khẩu dạng plain text
                    passwordOk = PasswordHasher.VerifyPlainPassword(matKhau, nd.MatKhau);
                    if (passwordOk)
                    {
                        // Tự động migrate sang PBKDF2 khi đăng nhập thành công
                        MigrateToHash(nd.Id, matKhau);
                    }
                }
                else
                {
                    // Tài khoản mới: mật khẩu đã băm PBKDF2
                    passwordOk = PasswordHasher.VerifyPassword(matKhau, nd.MatKhau, nd.Salt);
                }

                if (!passwordOk)
                    return LoginResult.InvalidCredentials;

                // Đăng nhập thành công - cập nhật UserSession
                nguoiDung = nd;
                UserSession.Login(nd.Id, nd.TenDangNhap, nd.HoTen, nd.Role);

                // Kiểm tra yêu cầu đổi mật khẩu
                if (nd.MustChangePwd)
                    return LoginResult.MustChangePwd;

                return LoginResult.Success;
            }
            catch (Exception ex)
            {
                // Ghi log lỗi không chứa thông tin nhạy cảm
                System.Diagnostics.Debug.WriteLine("AuthenticationService.DangNhap error: " + ex.Message);
                return LoginResult.DatabaseError;
            }
        }

        /// <summary>
        /// Đăng xuất - xóa phiên hiện tại.
        /// </summary>
        public void DangXuat()
        {
            UserSession.Logout();
        }

        // Migrate mật khẩu plain text sang PBKDF2
        private void MigrateToHash(int id, string plainPassword)
        {
            try
            {
                string salt;
                string hash = PasswordHasher.HashPassword(plainPassword, out salt);
                _data.CapNhatMatKhau(id, hash, salt);
            }
            catch (Exception ex)
            {
                // Không làm gián đoạn quá trình đăng nhập nếu migrate thất bại
                System.Diagnostics.Debug.WriteLine("Password migration failed: " + ex.Message);
            }
        }
    }
}
