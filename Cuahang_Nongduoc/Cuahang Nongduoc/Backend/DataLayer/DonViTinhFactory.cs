using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace CuahangNongduoc.DataLayer
{
    // ==================== [R28-BH] REFACTORING ====================
    // Lỗi (Code smell) : Tight Coupling với OleDb/Access cũ, không tương thích với SQL Server
    // Kỹ thuật         : Migrate Data Provider
    // Trước            : Dùng System.Data.OleDb (OleDbCommand, OleDbType), DataService.ExecuteNoneQuery()
    // Sau              : Chuyển sang System.Data.SqlClient (SqlCommand, SqlDbType), DataService.UpdateDataTable()
    // Tác giả          : BE2
    // Hỗ trợ           : BE1
    // Kiểm tra         : QA1
    // Ngày chỉnh sửa   : 06/11/2026
    // ==============================================================
    //
    // ==================== [D03] REFACTORING ====================
    // Lỗi (Code smell) : Duplicate Code – DAO nào cũng tự khai báo DataService m_Ds và tự viết Save()
    // Kỹ thuật         : Pull Up Method (kế thừa BaseFactory – Template Method)
    // Trước            : DataService m_Ds = new DataService(); ... m_Ds.Load(cmd); return m_Ds;
    //                    public bool Save() { return m_Ds.UpdateDataTable() > 0; }
    // Sau              : : BaseFactory, dùng Query(cmd); Save() lấy từ lớp cha (DAO mẫu cho BE2/BE3 làm theo)
    // Tác giả          : BE1 – Nguyễn Tuấn Anh
    // Hỗ trợ           : Claude (AI)
    // Kiểm tra         : QA2 – <Họ tên>
    // Ngày chỉnh sửa   : 08/11/2026
    // ===========================================================
    public class DonViTinhFactory : BaseFactory
    {
        public DataTable DanhsachDVT()
        {
            return Query(new SqlCommand("SELECT * FROM DON_VI_TINH"));
        }

        public DataTable LayDVT(int id)
        {
            SqlCommand cmd = new SqlCommand("SELECT * FROM DON_VI_TINH WHERE ID = @id");
            cmd.Parameters.Add("@id", SqlDbType.Int).Value = id;
            return Query(cmd);
        }
    }
}
