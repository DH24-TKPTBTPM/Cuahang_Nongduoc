using System;

namespace CuahangNongduoc
{
    /// <summary>
    /// [R02] VIẾT MỚI – Ngoại lệ riêng của tầng dữ liệu.
    /// Mục đích : DataService ném lỗi có thông điệp tiếng Việt + lỗi gốc (InnerException) thay vì nuốt lỗi.
    /// Tác giả  : tuananh (Team Lead, làm thay BE1) | Hỗ trợ: Claude (AI) | Kiểm tra: QA2 – Dư Thị Trúc Hòa
    /// Ngày tạo : 02/11/2026  | Ngày kiểm thử : 03/11/2026
    /// </summary>
    public class DataAccessException : Exception
    {
        // [D02] Mọi lỗi CSDL đều đi qua đây nên ghi log 1 chỗ là đủ, không phải sửa từng catch.
        // Tác giả: BE1 – Nguyễn Tuấn Anh | Hỗ trợ: Claude (AI) | Kiểm tra: QA2 – <Họ tên> | Ngày chỉnh sửa: 08/11/2026
        public DataAccessException(string message, Exception inner)
            : base(message, inner)
        {
            AppLogger.Instance.Error(message, inner);
        }
    }
}
