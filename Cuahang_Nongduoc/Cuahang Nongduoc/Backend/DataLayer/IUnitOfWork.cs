using System;

namespace CuahangNongduoc.DataLayer
{
    /// <summary>
    /// [D08] VIẾT MỚI – Mẫu thiết kế: Unit of Work (PoEAA – Martin Fowler, không thuộc 23 mẫu GoF)
    /// Mục đích : gom nhiều lệnh lưu (phiếu + chi tiết + trừ kho + công nợ) vào 1 transaction
    /// Thay thế : mỗi DataService tự mở/commit transaction riêng → lỗi giữa chừng để lại dữ liệu nửa vời
    /// Tác giả  : BE1 – Nguyễn Tuấn Anh | Hỗ trợ: Claude (AI) | Kiểm tra: QA2 – <Họ tên>
    /// Ngày tạo : 08/11/2026
    /// </summary>
    public interface IUnitOfWork : IDisposable
    {
        /// <summary>Ghi tất cả thay đổi. Không gọi Commit thì Dispose sẽ Rollback.</summary>
        void Commit();

        /// <summary>Hủy tất cả thay đổi kể từ khi mở UnitOfWork.</summary>
        void Rollback();
    }
}
