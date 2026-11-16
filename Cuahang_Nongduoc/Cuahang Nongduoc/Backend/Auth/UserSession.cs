using System;

namespace CuahangNongduoc.Auth
{
    /* State Pattern: tách trạng thái phiên thành Đã đăng nhập và Chưa đăng nhập. */
    public interface ILoginState
    {
        bool IsLoggedIn { get; }
    }

    public sealed class LoggedInState : ILoginState
    {
        public bool IsLoggedIn { get { return true; } }
    }

    public sealed class LoggedOutState : ILoginState
    {
        public bool IsLoggedIn { get { return false; } }
    }

    /// <summary>
    /// Lưu trữ thông tin phiên đăng nhập của người dùng hiện tại.
    /// Sử dụng Singleton pattern để đảm bảo chỉ có một phiên duy nhất tại một thời điểm.
    /// </summary>
    public static class UserSession
    {
        private static ILoginState _state = new LoggedOutState();

        // ID tài khoản đăng nhập
        public static int UserId { get; private set; }

        // Tên đăng nhập
        public static string TenDangNhap { get; private set; }

        // Họ tên đầy đủ
        public static string HoTen { get; private set; }

        // Vai trò: "admin", "sales", "warehouse"
        public static string Role { get; private set; }

        // Trạng thái đăng nhập
        public static bool IsLoggedIn { get { return _state.IsLoggedIn; } }

        // Thời gian đăng nhập
        public static DateTime LoginTime { get; private set; }

        /// <summary>
        /// Khởi tạo phiên đăng nhập sau khi xác thực thành công.
        /// </summary>
        public static void Login(int userId, string tenDangNhap, string hoTen, string role)
        {
            UserId = userId;
            TenDangNhap = tenDangNhap;
            HoTen = hoTen;
            Role = (role ?? "").ToLower().Trim();
            _state = new LoggedInState();
            LoginTime = DateTime.Now;
        }

        /// <summary>
        /// Xóa thông tin phiên đăng nhập (đăng xuất).
        /// </summary>
        public static void Logout()
        {
            UserId = 0;
            TenDangNhap = null;
            HoTen = null;
            Role = null;
            _state = new LoggedOutState();
            LoginTime = DateTime.MinValue;
        }

        /// <summary>
        /// Kiểm tra người dùng có vai trò Admin không.
        /// </summary>
        public static bool IsAdmin
        {
            get { return IsLoggedIn && string.Equals(Role, "admin", StringComparison.OrdinalIgnoreCase); }
        }

        /// <summary>
        /// Kiểm tra người dùng có vai trò Sales (Nhân viên bán hàng) không.
        /// </summary>
        public static bool IsSales
        {
            get { return IsLoggedIn && string.Equals(Role, "sales", StringComparison.OrdinalIgnoreCase); }
        }

        /// <summary>
        /// Kiểm tra người dùng có vai trò Warehouse (Nhân viên kho) không.
        /// </summary>
        public static bool IsWarehouse
        {
            get { return IsLoggedIn && string.Equals(Role, "warehouse", StringComparison.OrdinalIgnoreCase); }
        }

        /// <summary>
        /// Trả về tên hiển thị của vai trò hiện tại.
        /// </summary>
        public static string RoleDisplayName
        {
            get
            {
                switch ((Role ?? "").ToLower())
                {
                    case "admin": return "Quản trị viên";
                    case "sales": return "Nhân viên bán hàng";
                    case "warehouse": return "Nhân viên quản lý kho";
                    default: return Role ?? "Không xác định";
                }
            }
        }
    }
}
