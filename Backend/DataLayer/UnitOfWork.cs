using System;
using System.Data.SqlClient;
using CuahangNongduoc.Patterns.Singleton;

namespace CuahangNongduoc.DataLayer
{
    /// <summary>
    /// [D08] VIẾT MỚI – Mẫu thiết kế: Unit of Work (PoEAA, không thuộc 23 mẫu GoF)
    /// Mục đích : mở 1 SqlTransaction trên kết nối dùng chung; mọi DataService chạy trong khối using
    ///            tự gắn vào transaction này, chỉ Commit 1 lần ở cuối
    /// Cách dùng:
    ///     using (IUnitOfWork uow = new UnitOfWork())
    ///     {
    ///         ctrlPhieuBan.Save();  ctrlChiTiet.Save();  ...
    ///         uow.Commit();          // lỗi ở bất kỳ bước nào → không tới đây → Dispose tự Rollback
    ///     }
    /// Tác giả  : BE1 – Nguyễn Tuấn Anh | Hỗ trợ: Claude (AI) | Kiểm tra: QA2 – <Họ tên>
    /// Ngày tạo : 08/11/2026
    /// </summary>
    public sealed class UnitOfWork : IUnitOfWork
    {
        // shortcut: chỉ 1 UnitOfWork mỗi luồng, không lồng nhau – đủ cho WinForms 1 luồng UI
        [ThreadStatic]
        private static UnitOfWork m_Current;

        private SqlTransaction m_Transaction;

        public UnitOfWork()
        {
            if (m_Current != null)
                throw new InvalidOperationException("Đã có UnitOfWork đang mở, không hỗ trợ lồng nhau.");

            m_Transaction = DatabaseManager.Instance.GetOpenConnection().BeginTransaction();
            m_Current = this;
        }

        /// <summary>Transaction của UnitOfWork đang mở (null nếu không có) – DataService dùng.</summary>
        internal static SqlTransaction CurrentTransaction
        {
            get { return m_Current == null ? null : m_Current.m_Transaction; }
        }

        public void Commit()
        {
            if (m_Transaction == null)
                throw new InvalidOperationException("UnitOfWork đã kết thúc.");
            try
            {
                m_Transaction.Commit();
            }
            catch (Exception e)
            {
                throw new DataAccessException("Lỗi xác nhận giao dịch (Commit).", e);
            }
            finally
            {
                KetThuc();
            }
        }

        public void Rollback()
        {
            if (m_Transaction == null) return;
            try
            {
                m_Transaction.Rollback();
            }
            finally
            {
                KetThuc();
            }
        }

        public void Dispose()
        {
            try
            {
                Rollback();
            }
            catch (Exception)
            {
                // Mất kết nối thì SQL Server đã tự rollback; không để lỗi này che mất lỗi gốc trong khối using
            }
        }

        private void KetThuc()
        {
            m_Transaction.Dispose();
            m_Transaction = null;
            m_Current = null;
        }
    }
}
