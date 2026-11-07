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
    public class DonViTinhFactory
    {
        DataService m_Ds = new DataService();

        public DataTable DanhsachDVT()
        {
            SqlCommand cmd = new SqlCommand("SELECT * FROM DON_VI_TINH");
            m_Ds.Load(cmd);

            return m_Ds;
        }

        public DataTable LayDVT(int id)
        {
            SqlCommand cmd = new SqlCommand("SELECT * FROM DON_VI_TINH WHERE ID = @id");
            cmd.Parameters.Add("@id", SqlDbType.Int).Value = id;
            m_Ds.Load(cmd);
            return m_Ds;
        }

        public bool Save()
        {
            return m_Ds.UpdateDataTable() > 0;
        }
    }
}