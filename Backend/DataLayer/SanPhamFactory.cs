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
    // Hỗ trợ           : BE3
    // Kiểm tra         : QA1
    // Ngày chỉnh sửa   : 06/11/2026
    // ==============================================================
    public class SanPhamFactory
    {
        DataService m_Ds = new DataService();

        public DataTable DanhsachSanPham()
        {
            SqlCommand cmd = new SqlCommand("SELECT * FROM SAN_PHAM");
            m_Ds.Load(cmd);

            return m_Ds.ResultTable;
        }

        public DataTable TimMaSanPham(String id)
        {
            SqlCommand cmd = new SqlCommand("SELECT * FROM SAN_PHAM WHERE ID LIKE '%' + @id + '%'");
            cmd.Parameters.Add("@id", SqlDbType.VarChar, 50).Value = id;
            m_Ds.Load(cmd);

            return m_Ds.ResultTable;
        }

        public DataTable TimTenSanPham(String ten)
        {
            SqlCommand cmd = new SqlCommand("SELECT * FROM SAN_PHAM WHERE TEN_SAN_PHAM LIKE '%' + @ten + '%'");
            cmd.Parameters.Add("@ten", SqlDbType.NVarChar, 50).Value = ten;
            m_Ds.Load(cmd);

            return m_Ds.ResultTable;
        }

        public DataTable LaySanPham(String id)
        {
            SqlCommand cmd = new SqlCommand("SELECT * FROM SAN_PHAM WHERE ID = @id");
            cmd.Parameters.Add("@id", SqlDbType.VarChar, 50).Value = id;
            m_Ds.Load(cmd);
            return m_Ds.ResultTable;
        }

        public DataTable LaySoLuongTon()
        {
            SqlCommand cmd = new SqlCommand("SELECT SP.ID, SP.TEN_SAN_PHAM, SP.DON_GIA_NHAP, SP.GIA_BAN_SI, SP.GIA_BAN_LE, SP.ID_DON_VI_TINH , SP.SO_LUONG , SUM(MA.SO_LUONG) AS SO_LUONG_TON "
                + " FROM SAN_PHAM SP INNER JOIN MA_SAN_PHAM MA ON SP.ID = MA.ID_SAN_PHAM "
                + " GROUP BY SP.ID, SP.TEN_SAN_PHAM, SP.DON_GIA_NHAP, SP.GIA_BAN_SI, SP.GIA_BAN_LE, SP.ID_DON_VI_TINH, SP.SO_LUONG");
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
