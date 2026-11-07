using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;


namespace CuahangNongduoc.DataLayer
{
    public class LyDoChiFactory
    {
        DataService m_Ds = new DataService();

// ==================== [R28-NH] REFACTORING ====================
// Lỗi (Code smell) : Sử dụng provider cũ OleDb thay vì SqlClient
// Kỹ thuật         : Migrate Data Provider
// Tác giả          : BE3 - Bùi Lê Tuấn Anh
// Hỗ trợ           : BE1 - Nguyễn Tuấn Anh
// Kiểm duyệt       : QA2 - Dư Thị Trúc Hòa
// Ngày chỉnh sửa   : 06/11/2026
// Ngày kiểm thử    : 07/11/2026
// ==============================================================
        public DataTable DanhsachLyDo()
        {
// Code cũ: OleDbCommand cmd = new OleDbCommand("SELECT * FROM LY_DO_CHI");
            SqlCommand cmd = new SqlCommand("SELECT * FROM LY_DO_CHI");
            m_Ds.Load(cmd);

            return m_Ds.ResultTable;
        }

// ==================== [R28-NH] REFACTORING ====================
// Lỗi (Code smell) : Sử dụng provider cũ OleDb thay vì SqlClient
// Kỹ thuật         : Migrate Data Provider
// Tác giả          : BE3 - Bùi Lê Tuấn Anh
// Hỗ trợ           : BE1 - Nguyễn Tuấn Anh
// Kiểm duyệt       : QA2 - Dư Thị Trúc Hòa
// Ngày chỉnh sửa   : 06/11/2026
// Ngày kiểm thử    : 07/11/2026
// ==============================================================
        public DataTable LayLyDoChi(long id)
        {
// Code cũ: OleDbCommand cmd = new OleDbCommand("SELECT * FROM LY_DO_CHI WHERE ID = " + id);
            SqlCommand cmd = new SqlCommand("SELECT * FROM LY_DO_CHI WHERE ID = " + id);
            m_Ds.Load(cmd);

            return m_Ds.ResultTable;
        }

        public bool Save()
        {
            return m_Ds.UpdateDataTable() > 0;
        }
    }
}
