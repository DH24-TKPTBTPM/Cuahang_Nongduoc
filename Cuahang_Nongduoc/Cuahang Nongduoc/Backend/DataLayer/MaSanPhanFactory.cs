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
    public class MaSanPhanFactory
    {
        DataService m_Ds = new DataService();

        public void LoadSchema()
        {
            SqlCommand cmd = new SqlCommand("SELECT * FROM MA_SAN_PHAM WHERE ID = '-1'");
            m_Ds.Load(cmd);
        }

        public DataTable DanhsachMaSanPham(String sp)
        {
            SqlCommand cmd = new SqlCommand("SELECT * FROM MA_SAN_PHAM WHERE ID_SAN_PHAM = @id AND SO_LUONG > 0");
            cmd.Parameters.Add("@id", SqlDbType.VarChar, 50).Value = sp;
            m_Ds.Load(cmd);

            return m_Ds.ResultTable;
        }

        public DataTable DanhsachChiTiet(String sp)
        {
            SqlCommand cmd = new SqlCommand("SELECT * FROM MA_SAN_PHAM WHERE ID_PHIEU_NHAP = @id");
            cmd.Parameters.Add("@id", SqlDbType.VarChar, 50).Value = sp;
            m_Ds.Load(cmd);

            return m_Ds.ResultTable;
        }

        public DataTable LaySanPham(String idMaSanPham)
        {
            SqlCommand cmd = new SqlCommand("SELECT SP.* FROM SAN_PHAM SP INNER JOIN MA_SAN_PHAM MSP ON SP.ID = MSP.ID_SAN_PHAM WHERE MSP.ID = @id");
            cmd.Parameters.Add("@id", SqlDbType.VarChar, 50).Value = idMaSanPham;
            m_Ds.Load(cmd);
            return m_Ds.ResultTable;
        }

        public DataTable LayMaSanPham(String idMaSanPham)
        {
            SqlCommand cmd = new SqlCommand("SELECT * FROM MA_SAN_PHAM MSP WHERE MSP.ID = @id");
            cmd.Parameters.Add("@id", SqlDbType.VarChar, 50).Value = idMaSanPham;
            m_Ds.Load(cmd);
            return m_Ds.ResultTable;
        }

        public DataTable DanhsachMaSanPhamHetHan(DateTime dt)
        {
            SqlCommand cmd = new SqlCommand("SELECT * FROM MA_SAN_PHAM WHERE SO_LUONG > 0 AND NGAY_HET_HAN <= @ngay");
            cmd.Parameters.Add("@ngay", SqlDbType.DateTime).Value = dt;
            m_Ds.Load(cmd);

            return m_Ds.ResultTable;
        }

        public DataTable DanhsachMaSanPham()
        {
            SqlCommand cmd = new SqlCommand("SELECT * FROM MA_SAN_PHAM WHERE SO_LUONG > 0");
            m_Ds.Load(cmd);

            return m_Ds.ResultTable;
        }

        // ==================== [D07] REFACTORING ====================
        // Lỗi (Code smell) : Thiếu cơ chế thông báo khi tồn kho thay đổi (cập nhật kho, cảnh báo hết hàng/hết hạn)
        // Kỹ thuật         : Observer Pattern (GoF)
        // Trước            : Chỉ chạy SQL UPDATE MA_SAN_PHAM, không phát sự kiện tồn kho thay đổi
        // Sau              : Gọi InventoryService.Instance.NotifyInventoryChanged(masp, so_luong) sau khi cập nhật kho
        // Tác giả          : BE2
        // Hỗ trợ           : BE3
        // Kiểm tra         : QA1
        // Ngày chỉnh sửa   : 12/11/2026
        // ==============================================================
        public static void CapNhatSoLuong(String masp, int so_luong)
        {
            DataService ds = new DataService();
            SqlCommand cmd = new SqlCommand("UPDATE MA_SAN_PHAM SET SO_LUONG = SO_LUONG + @so WHERE ID = @id");
            cmd.Parameters.Add("@so", SqlDbType.Int).Value = so_luong;
            cmd.Parameters.Add("@id", SqlDbType.VarChar, 50).Value = masp;
            ds.ExecuteNoneQuery(cmd);
            CuahangNongduoc.Controller.InventoryService.Instance.NotifyInventoryChanged(masp, so_luong);
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
