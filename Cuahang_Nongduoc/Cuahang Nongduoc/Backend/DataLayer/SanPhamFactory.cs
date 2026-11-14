using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace CuahangNongduoc.DataLayer
{
    // ==================== [R28-BH] REFACTORING ====================
    // ==============================================================
    // ==================== [R28-BH-FIX] FIX ====================
    // Lỗi (Code smell) : Mất dữ liệu toàn bộ bảng SanPham sau khi lưu do các hàm tìm kiếm ghi đè m_Ds chung.
    // Kỹ thuật         : Separate Query and Command / Local DataService Instance
    // Trước            : Các hàm LaySanPham, TimMaSanPham... dùng chung m_Ds.Load(cmd) làm mất dữ liệu m_Ds chính.
    // Sau              : Các hàm truy vấn dùng DataService cục bộ (localDs) để bảo toàn m_Ds cho hàm Save().
    // Tác giả          : BE2
    // Hỗ trợ           : BE3
    // Kiểm tra         : QA1
    // Ngày chỉnh sửa   : 12/11/2026
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

            DataService localDs = new DataService();
            localDs.Load(cmd);
            return localDs.ResultTable;
        }

        public DataTable TimTenSanPham(String ten)
        {
            SqlCommand cmd = new SqlCommand("SELECT * FROM SAN_PHAM WHERE TEN_SAN_PHAM LIKE '%' + @ten + '%'");
            cmd.Parameters.Add("@ten", SqlDbType.NVarChar, 50).Value = ten;

            DataService localDs = new DataService();
            localDs.Load(cmd);
            return localDs.ResultTable;
        }

        public DataTable LaySanPham(String id)
        {
            SqlCommand cmd = new SqlCommand("SELECT * FROM SAN_PHAM WHERE ID = @id");
            cmd.Parameters.Add("@id", SqlDbType.VarChar, 50).Value = id;

            DataService localDs = new DataService();
            localDs.Load(cmd);
            return localDs.ResultTable;
        }

        public DataTable LaySoLuongTon()
        {
            SqlCommand cmd = new SqlCommand("SELECT SP.ID, SP.TEN_SAN_PHAM, SP.DON_GIA_NHAP, SP.GIA_BAN_SI, SP.GIA_BAN_LE, SP.ID_DON_VI_TINH , SP.SO_LUONG , SUM(MA.SO_LUONG) AS SO_LUONG_TON "
                + " FROM SAN_PHAM SP INNER JOIN MA_SAN_PHAM MA ON SP.ID = MA.ID_SAN_PHAM "
                + " GROUP BY SP.ID, SP.TEN_SAN_PHAM, SP.DON_GIA_NHAP, SP.GIA_BAN_SI, SP.GIA_BAN_LE, SP.ID_DON_VI_TINH, SP.SO_LUONG");

            DataService localDs = new DataService();
            localDs.Load(cmd);
            return localDs.ResultTable;
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