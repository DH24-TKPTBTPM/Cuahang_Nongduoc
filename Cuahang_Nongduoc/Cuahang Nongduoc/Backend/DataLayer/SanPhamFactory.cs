using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace CuahangNongduoc.DataLayer
{
    /// <summary>Data Access Layer thao tác với bảng SAN_PHAM bằng SqlCommand có tham số.</summary>
    public class SanPhamFactory
    {
        DataService m_Ds = new DataService();

        /// <summary>Tải toàn bộ danh mục sản phẩm phục vụ màn hình quản lý và ComboBox.</summary>
        /// <returns>DataTable chứa dữ liệu sản phẩm.</returns>
        public DataTable DanhsachSanPham()
        {
            SqlCommand cmd = new SqlCommand("SELECT * FROM SAN_PHAM");
            m_Ds.Load(cmd);

            return m_Ds;
        }

        public DataTable TimMaSanPham(String id)
        {
            SqlCommand cmd = new SqlCommand("SELECT * FROM SAN_PHAM WHERE ID LIKE '%' + @id + '%'");
            cmd.Parameters.Add("id", SqlDbType.VarChar).Value = id;
            m_Ds.Load(cmd);

            return m_Ds;
        }
        public DataTable TimTenSanPham(String ten)
        {
            SqlCommand cmd = new SqlCommand("SELECT * FROM SAN_PHAM WHERE TEN_SAN_PHAM LIKE '%' + @ten + '%'");
            cmd.Parameters.Add("ten", SqlDbType.VarChar).Value = ten;
            m_Ds.Load(cmd);

            return m_Ds;
        }


        /// <summary>Truy vấn sản phẩm theo khóa chính để đọc hoặc cập nhật giá nhập bình quân.</summary>
        /// <param name="id">Mã sản phẩm.</param><returns>DataTable kết quả truy vấn.</returns>
        public DataTable LaySanPham(String id)
        {
            SqlCommand cmd = new SqlCommand("SELECT * FROM SAN_PHAM WHERE ID = @id");
            cmd.Parameters.Add("id", SqlDbType.VarChar, 50).Value = id;
            m_Ds.Load(cmd);
            return m_Ds;
        }

        /// <summary>Tổng hợp tồn kho từ các lô trong MA_SAN_PHAM theo từng sản phẩm.</summary>
        /// <returns>DataTable có số lượng tồn dùng cho báo cáo nghiệp vụ.</returns>
        public DataTable LaySoLuongTon()
        {
            SqlCommand cmd = new SqlCommand("SELECT SP.ID, SP.TEN_SAN_PHAM, SP.DON_GIA_NHAP, SP.GIA_BAN_SI, SP.GIA_BAN_LE, SP.ID_DON_VI_TINH , SP.SO_LUONG , SUM(MA.SO_LUONG) AS SO_LUONG_TON "
                + " FROM SAN_PHAM SP INNER JOIN MA_SAN_PHAM MA ON SP.ID = MA.ID_SAN_PHAM "
                + " GROUP BY SP.ID, SP.TEN_SAN_PHAM, SP.DON_GIA_NHAP, SP.GIA_BAN_SI, SP.GIA_BAN_LE, SP.ID_DON_VI_TINH, SP.SO_LUONG");
            m_Ds.Load(cmd);
            return m_Ds;
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
            return m_Ds.ExecuteNoneQuery() > 0;
        }

        /// <summary>
        /// Hoàn nguyên thay đổi chưa commit để giao diện không giữ trạng thái sai sau lỗi Foreign Key hoặc lỗi ghi DB.
        /// </summary>
        public void RejectChanges()
        {
            m_Ds.RejectChanges();
        }
    }
}
