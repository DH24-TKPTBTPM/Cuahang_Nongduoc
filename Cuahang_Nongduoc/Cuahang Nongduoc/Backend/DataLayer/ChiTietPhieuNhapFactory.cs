using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace CuahangNongduoc.DataLayer
{

    public class ChiTietPhieuNhapFactory
    {
        DataService m_Ds = new DataService();


// ==================== [R28-NH] REFACTORING ====================
// Lỗi (Code smell) : Sử dụng provider cũ OleDb thay vì SqlClient
// Kỹ thuật         : Migrate Data Provider
// Tác giả          : BE3 - Bùi Lê Tuấn Anh
// Hỗ trợ           : BE1 - Nguyễn Tuấn Anh
// Kiểm duyệt       : QA2 - Dư Thị Trúc Hòa
// Ngày chỉnh sửa   : 06/11/2026
// ==============================================================
        public void LoadSchema()
        {
// Code cũ: OleDbCommand cmd = new OleDbCommand("SELECT * FROM CHI_TIET_PHIEU_NHAP WHERE ID_PHIEU_NHAP = '-1'");
            SqlCommand cmd = new SqlCommand("SELECT * FROM CHI_TIET_PHIEU_NHAP WHERE ID_PHIEU_NHAP = '-1'");
            m_Ds.Load(cmd);
        }


// ==================== [R28-NH] REFACTORING ====================
// Lỗi (Code smell) : Sử dụng provider cũ OleDb thay vì SqlClient
// Kỹ thuật         : Migrate Data Provider
// Tác giả          : BE3 - Bùi Lê Tuấn Anh
// Hỗ trợ           : BE1 - Nguyễn Tuấn Anh
// Kiểm duyệt       : QA2 - Dư Thị Trúc Hòa
// Ngày chỉnh sửa   : 06/11/2026
// ==============================================================
        public DataTable LayChiTietPhieuNhap(String id)
        {
// Code cũ: OleDbCommand cmd = new OleDbCommand("SELECT * FROM CHI_TIET_PHIEU_NHAP WHERE ID_PHIEU_NHAP = @id");
            SqlCommand cmd = new SqlCommand("SELECT * FROM CHI_TIET_PHIEU_NHAP WHERE ID_PHIEU_NHAP = @id");
// Code cũ: cmd.Parameters.Add("id", OleDbType.VarChar,50).Value = id;
            cmd.Parameters.Add("id", SqlDbType.VarChar,50).Value = id;
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
// ==============================================================
        public int XoaChiTietPhieuNhap(String id)
        {
// Code cũ: OleDbCommand cmd = new OleDbCommand("DELETE FROM CHI_TIET_PHIEU_NHAP WHERE ID_PHIEU_NHAP = @id");
            SqlCommand cmd = new SqlCommand("DELETE FROM CHI_TIET_PHIEU_NHAP WHERE ID_PHIEU_NHAP = @id");
// Code cũ: cmd.Parameters.Add("id", OleDbType.VarChar, 50).Value = id;
            cmd.Parameters.Add("id", SqlDbType.VarChar, 50).Value = id;
            return m_Ds.ExecuteNoneQuery(cmd);
        }
        
        

        public DataRow NewRow()
        {
            return m_Ds.NewRow();
        }

        public void Add(DataRow row)
        {
            m_Ds.Rows.Add(row);
        }

        public bool Save()
        {
               
           return m_Ds.UpdateDataTable() > 0;
        }
    }
}
