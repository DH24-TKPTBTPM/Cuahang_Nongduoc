using System;
using System.Security.Cryptography;

namespace CuahangNongduoc.Auth
{
    /// <summary>
    /// Lớp băm và xác minh mật khẩu sử dụng PBKDF2 với SHA256.
    /// Hoàn toàn tương thích với .NET Framework 4.8.
    /// </summary>
    public static class PasswordHasher
    {
        // Số vòng lặp PBKDF2 (đủ mạnh, không quá chậm cho môi trường demo)
        private const int Iterations = 100000;
        // Độ dài salt ngẫu nhiên (bytes)
        private const int SaltSize = 32;
        // Độ dài hash (bytes)
        private const int HashSize = 32;
        /// <summary>
        /// Tạo hash mật khẩu PBKDF2 + salt ngẫu nhiên.
        /// </summary>
        /// <param name="plainPassword">Mật khẩu gốc dạng văn bản thuần</param>
        /// <param name="saltBase64">Output: Salt ngẫu nhiên dưới dạng Base64</param>
        /// <returns>Hash mật khẩu dưới dạng Base64</returns>
        public static string HashPassword(string plainPassword, out string saltBase64)
        {
            if (string.IsNullOrEmpty(plainPassword))
                throw new ArgumentException("Mật khẩu không được để trống.", "plainPassword");

            // Tạo salt ngẫu nhiên
            byte[] saltBytes = new byte[SaltSize];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(saltBytes);
            }
            saltBase64 = Convert.ToBase64String(saltBytes);

            // Tính hash PBKDF2
            byte[] hashBytes = ComputePbkdf2(plainPassword, saltBytes, Iterations, HashSize);
            return Convert.ToBase64String(hashBytes);
        }

        /// <summary>
        /// Xác minh mật khẩu người dùng nhập so với hash lưu trong database.
        /// </summary>
        /// <param name="plainPassword">Mật khẩu người dùng nhập</param>
        /// <param name="storedHash">Hash lưu trong database (Base64)</param>
        /// <param name="storedSalt">Salt lưu trong database (Base64)</param>
        /// <returns>true nếu mật khẩu đúng</returns>
        public static bool VerifyPassword(string plainPassword, string storedHash, string storedSalt)
        {
            if (string.IsNullOrEmpty(plainPassword) || string.IsNullOrEmpty(storedHash))
                return false;

            try
            {
                // Nếu salt rỗng → tài khoản chưa được chuyển đổi sang PBKDF2
                // Cần xử lý ở tầng trên, không verify ở đây
                if (string.IsNullOrEmpty(storedSalt))
                    return false;

                byte[] saltBytes = Convert.FromBase64String(storedSalt);
                byte[] storedHashBytes = Convert.FromBase64String(storedHash);

                byte[] computedHash = ComputePbkdf2(plainPassword, saltBytes, Iterations, HashSize);

                // So sánh bằng time-constant để tránh timing attack
                return SlowEquals(computedHash, storedHashBytes);
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Kiểm tra xem mật khẩu có phải dạng văn bản thuần (chưa băm) không.
        /// Dùng để nhận biết tài khoản cũ chưa được chuyển đổi.
        /// </summary>
        public static bool IsPlainPassword(string storedHash, string storedSalt)
        {
            // Nếu không có salt → đây là mật khẩu plain text
            return string.IsNullOrEmpty(storedSalt);
        }

        /// <summary>
        /// Xác minh mật khẩu plain text (cho tài khoản cũ chưa migrate).
        /// </summary>
        public static bool VerifyPlainPassword(string plainPassword, string storedPlain)
        {
            if (string.IsNullOrEmpty(plainPassword) || string.IsNullOrEmpty(storedPlain))
                return false;
            return string.Equals(plainPassword, storedPlain, StringComparison.Ordinal);
        }

        // Tính PBKDF2 dùng Rfc2898DeriveBytes (tích hợp trong .NET Framework)
        private static byte[] ComputePbkdf2(string password, byte[] salt, int iterations, int outputBytes)
        {
            using (var pbkdf2 = new Rfc2898DeriveBytes(password, salt, iterations))
            {
                return pbkdf2.GetBytes(outputBytes);
            }
        }

        // So sánh hai mảng byte với thời gian cố định để tránh timing attack
        private static bool SlowEquals(byte[] a, byte[] b)
        {
            if (a == null || b == null) return false;
            uint diff = (uint)a.Length ^ (uint)b.Length;
            int len = Math.Min(a.Length, b.Length);
            for (int i = 0; i < len; i++)
                diff |= (uint)(a[i] ^ b[i]);
            return diff == 0;
        }
    }
}
