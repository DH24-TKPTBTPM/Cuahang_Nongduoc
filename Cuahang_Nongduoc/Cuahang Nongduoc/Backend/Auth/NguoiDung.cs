using System;

namespace CuahangNongduoc.Auth
{
    /// <summary>
    /// Đối tượng dữ liệu đại diện cho một tài khoản người dùng.
    /// </summary>
    public class NguoiDung
    {
        public int Id { get; set; }
        public string TenDangNhap { get; set; }
        public string MatKhau { get; set; }   // Hash PBKDF2 (Base64) hoặc plain text cũ
        public string Salt { get; set; }       // Salt (Base64), null nếu tài khoản cũ
        public string Role { get; set; }       // "admin", "sales", "warehouse"
        public string HoTen { get; set; }
        public bool IsActive { get; set; }
        public bool MustChangePwd { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        /// <summary>
        /// Tên hiển thị của vai trò.
        /// </summary>
        public string RoleDisplayName
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
