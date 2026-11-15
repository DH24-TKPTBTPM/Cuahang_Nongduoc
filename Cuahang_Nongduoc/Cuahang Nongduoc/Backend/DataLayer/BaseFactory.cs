using System;
using System.Data;
using System.Data.SqlClient;

namespace CuahangNongduoc.DataLayer
{
    /// <summary>
    /// [D03] VIẾT MỚI – Mẫu thiết kế: Template Method (Behavioral, GoF)
    /// Mục đích : lớp cha dùng chung cho 13 DAO – gom phần lặp (giữ DataService, Load, NewRow, Add, Save)
    ///            và cố định khung Save(): TruocKhiLuu → UpdateDataTable → SauKhiLuu.
    ///            DAO con chỉ viết câu SQL; cần xử lý thêm khi lưu (vd. trừ kho – R27) thì override hook.
    /// Thay thế : mỗi DAO tự khai báo "DataService m_Ds", tự viết NewRow/Add/Save giống hệt nhau
    /// Cách dùng: public class DonViTinhFactory : BaseFactory
    ///            {
    ///                public DataTable DanhsachDVT() { return Query(new SqlCommand("SELECT * FROM DON_VI_TINH")); }
    ///            }
    /// Tác giả  : BE1 – Nguyễn Tuấn Anh | Hỗ trợ: Claude (AI) | Kiểm tra: QA2 – <Họ tên>
    /// Ngày tạo : 08/11/2026
    /// </summary>
    public abstract class BaseFactory
    {
        protected readonly DataService m_Ds = new DataService();

        /// <summary>Chạy câu SELECT và trả về bảng kết quả.</summary>
        protected DataTable Query(SqlCommand cmd)
        {
            m_Ds.Load(cmd);
            return m_Ds.ResultTable;
        }

        public DataRow NewRow()
        {
            return m_Ds.NewRow();
        }

        public void Add(DataRow row)
        {
            m_Ds.Rows.Add(row);
        }

        /// <summary>Template Method: thứ tự các bước lưu cố định, lớp con không đổi được.</summary>
        public bool Save()
        {
            TruocKhiLuu(m_Ds.ResultTable);
            bool thanhCong = m_Ds.UpdateDataTable() > 0;
            SauKhiLuu(thanhCong);
            return thanhCong;
        }

        /// <summary>Hook: chạy trước khi ghi xuống CSDL (vd. kiểm tra, trừ kho các dòng Added).</summary>
        protected virtual void TruocKhiLuu(DataTable bang) { }

        /// <summary>Hook: chạy sau khi ghi xong.</summary>
        protected virtual void SauKhiLuu(bool thanhCong) { }
    }
}
