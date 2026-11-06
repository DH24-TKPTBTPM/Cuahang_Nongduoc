using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace CuahangNongduoc.DataLayer
{

    public class PhieuNhapFactory
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
// Code cũ: OleDbCommand cmd = new OleDbCommand("SELECT * FROM PHIEU_NHAP WHERE ID='-1'");
           SqlCommand cmd = new SqlCommand("SELECT * FROM PHIEU_NHAP WHERE ID='-1'");
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
        public DataTable DanhsachPhieuNhap()
        {
// Code cũ: OleDbCommand cmd = new OleDbCommand("SELECT * FROM PHIEU_NHAP");
            SqlCommand cmd = new SqlCommand("SELECT * FROM PHIEU_NHAP");
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
        public DataTable TimPhieuNhap(String maNCC, DateTime dt)
        {
            String sql = "SELECT * FROM PHIEU_NHAP WHERE NGAY_NHAP = @ngay AND ID_NHA_CUNG_CAP = @ncc";
// Code cũ: OleDbCommand cmd = new OleDbCommand(sql);
            SqlCommand cmd = new SqlCommand(sql);
// Code cũ: cmd.Parameters.Add("ngay", OleDbType.Date).Value = dt;
            cmd.Parameters.Add("ngay", SqlDbType.DateTime).Value = dt;
// Code cũ: cmd.Parameters.Add("ncc", OleDbType.VarChar).Value = maNCC;
            cmd.Parameters.Add("ncc", SqlDbType.VarChar).Value = maNCC;
            
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
        public DataTable LayPhieuNhap(String id)
        {
// Code cũ: OleDbCommand cmd = new OleDbCommand("SELECT * FROM PHIEU_NHAP WHERE ID = @id");
            SqlCommand cmd = new SqlCommand("SELECT * FROM PHIEU_NHAP WHERE ID = @id");
// Code cũ: cmd.Parameters.Add("id", OleDbType.VarChar,50).Value = id;
            cmd.Parameters.Add("id", SqlDbType.VarChar,50).Value = id;
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

       public bool Save()
        {
           
            return m_Ds.UpdateDataTable() > 0;
        }
    }
}
