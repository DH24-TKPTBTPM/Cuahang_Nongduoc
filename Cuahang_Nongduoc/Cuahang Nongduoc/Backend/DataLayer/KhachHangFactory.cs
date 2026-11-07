using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace CuahangNongduoc.DataLayer
{

    public class KhachHangFactory
    {
        DataService m_Ds = new DataService();
      
        public DataTable DanhsachKhachHang(bool loai)
        {
// Code cũ: OleDbCommand cmd = new OleDbCommand("SELECT * FROM KHACH_HANG WHERE LOAI_KH = " + loai);
            SqlCommand cmd = new SqlCommand("SELECT * FROM KHACH_HANG WHERE LOAI_KH = @LOAI_KH");
            cmd.Parameters.Add(new SqlParameter("@LOAI_KH", SqlDbType.Bit) { Value = loai });
            m_Ds.Load(cmd);

            return m_Ds.ResultTable;
        }

      
        public DataTable TimHoTen(String hoten, bool loai)
        {
// Code cũ: OleDbCommand cmd = new OleDbCommand("SELECT * FROM KHACH_HANG WHERE HO_TEN LIKE '%' + @hoten + '%' AND LOAI_KH = " + loai);
            SqlCommand cmd = new SqlCommand("SELECT * FROM KHACH_HANG WHERE HO_TEN LIKE '%' + @hoten + '%' AND LOAI_KH = @LOAI_KH");
// Code cũ: cmd.Parameters.Add("hoten", OleDbType.VarChar).Value = hoten;
            cmd.Parameters.Add("hoten", SqlDbType.VarChar).Value = hoten;
            cmd.Parameters.Add(new SqlParameter("@LOAI_KH", SqlDbType.Bit) { Value = loai });
            m_Ds.Load(cmd);

            return m_Ds.ResultTable;
        }


        public DataTable TimDiaChi(String diachi, bool loai)
        {
// Code cũ: OleDbCommand cmd = new OleDbCommand("SELECT * FROM KHACH_HANG WHERE DIA_CHI LIKE '%' + @diachi + '%' AND LOAI_KH = " + loai);
            SqlCommand cmd = new SqlCommand("SELECT * FROM KHACH_HANG WHERE DIA_CHI LIKE '%' + @diachi + '%' AND LOAI_KH = @LOAI_KH");
// Code cũ: cmd.Parameters.Add("diachi", OleDbType.VarChar).Value = diachi;
            cmd.Parameters.Add("diachi", SqlDbType.VarChar).Value = diachi;
            cmd.Parameters.Add(new SqlParameter("@LOAI_KH", SqlDbType.Bit) { Value = loai });
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
        public DataTable DanhsachKhachHang()
        {
// Code cũ: OleDbCommand cmd = new OleDbCommand("SELECT * FROM KHACH_HANG");
            SqlCommand cmd = new SqlCommand("SELECT * FROM KHACH_HANG");
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
        public DataTable LayKhachHang(String id)
        {
// Code cũ: OleDbCommand cmd = new OleDbCommand("SELECT * FROM KHACH_HANG WHERE ID = @id");
            SqlCommand cmd = new SqlCommand("SELECT * FROM KHACH_HANG WHERE ID = @id");
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
