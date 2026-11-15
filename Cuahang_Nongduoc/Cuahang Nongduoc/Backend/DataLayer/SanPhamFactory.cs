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

        // ==================== [R28-BH] FIX ====================
        // Lỗi (Code smell) : Tìm kiếm nạp vào localDs rồi bị bỏ → lưới frmSanPham (gắn với m_Ds) không lọc
        // Kỹ thuật         : Fix Bug – tìm kiếm nạp lại vào m_Ds như bản gốc (lưới gắn đúng bảng này,
        //                    Save() sau khi tìm vẫn lưu đúng bảng). Hàm chỉ đọc (LaySanPham, LaySoLuongTon) giữ localDs.
        // Tác giả: BE1 – Nguyễn Tuấn Anh | Hỗ trợ: Claude (AI) | Kiểm tra: QA1 – <Họ tên> | Ngày chỉnh sửa: 15/11/2026
        // ===========================================================
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

        /// <summary>[R28-BH] Ghi giá nhập bình quân và tồn kho mới của 1 sản phẩm (dùng khi lưu phiếu nhập).</summary>
        public int CapNhatGiaVaSoLuong(String id, long donGiaNhap, long soLuong)
        {
            SqlCommand cmd = new SqlCommand("UPDATE SAN_PHAM SET DON_GIA_NHAP = @gia, SO_LUONG = @sl WHERE ID = @id");
            cmd.Parameters.Add("@gia", SqlDbType.BigInt).Value = donGiaNhap;
            cmd.Parameters.Add("@sl", SqlDbType.BigInt).Value = soLuong;
            cmd.Parameters.Add("@id", SqlDbType.VarChar, 50).Value = id;
            return new DataService().ExecuteNoneQuery(cmd);
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