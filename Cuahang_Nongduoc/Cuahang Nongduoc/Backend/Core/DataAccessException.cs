using System;

namespace CuahangNongduoc
{
    /// <summary>
    /// [R02] VIẾT MỚI – Ngoại lệ riêng của tầng dữ liệu.
    /// Mục đích : DataService ném lỗi có thông điệp tiếng Việt + lỗi gốc (InnerException) thay vì nuốt lỗi.
    /// Tác giả  : tuananh (Team Lead, làm thay BE1) | Hỗ trợ: Claude (AI) | Kiểm tra: QA2 – <Họ tên>
    /// Ngày tạo : 02/11/2026
    /// </summary>
    public class DataAccessException : Exception
    {
        public DataAccessException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
