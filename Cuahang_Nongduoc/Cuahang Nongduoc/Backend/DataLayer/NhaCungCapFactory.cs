using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace CuahangNongduoc.DataLayer
{
    /// <summary>Data Access Layer truy vấn bảng NHA_CUNG_CAP bằng SqlCommand tham số hóa.</summary>
    public class NhaCungCapFactory
    {
        DataService m_Ds = new DataService();

        /// <summary>Tải toàn bộ nhà cung cấp cho danh mục và các ComboBox nghiệp vụ.</summary>
        /// <returns>DataTable dữ liệu nhà cung cấp.</returns>
        public DataTable DanhsachNCC()
        {
            SqlCommand cmd = new SqlCommand("SELECT * FROM NHA_CUNG_CAP");
            m_Ds.Load(cmd);

            return m_Ds;
        }
        /// <summary>Tìm nhà cung cấp theo một phần địa chỉ.</summary>
        /// <param name="diachi">Từ khóa địa chỉ.</param><returns>DataTable kết quả tìm kiếm.</returns>
        public DataTable TimDiaChi(String diachi)
        {
            SqlCommand cmd = new SqlCommand("SELECT * FROM NHA_CUNG_CAP WHERE DIA_CHI LIKE '%' + @diachi + '%' ");
            cmd.Parameters.Add("diachi", SqlDbType.VarChar).Value = diachi;
            m_Ds.Load(cmd);

            return m_Ds;
        }
        /// <summary>Tìm nhà cung cấp theo một phần tên.</summary>
        /// <param name="hoten">Từ khóa tên.</param><returns>DataTable kết quả tìm kiếm.</returns>
        public DataTable TimHoTen(String hoten)
        {
            SqlCommand cmd = new SqlCommand("SELECT * FROM NHA_CUNG_CAP WHERE HO_TEN LIKE '%' + @hoten + '%' ");
            cmd.Parameters.Add("hoten", SqlDbType.VarChar).Value = hoten;
            m_Ds.Load(cmd);

            return m_Ds;
        }

        /// <summary>Đọc nhà cung cấp theo khóa chính để phục vụ phiếu nhập.</summary>
        /// <param name="id">ID nhà cung cấp.</param><returns>DataTable kết quả.</returns>
        public DataTable LayNCC(String id)
        {
            SqlCommand cmd = new SqlCommand("SELECT * FROM NHA_CUNG_CAP WHERE ID = @id");
            cmd.Parameters.Add("id", SqlDbType.VarChar,50).Value = id;
            m_Ds.Load(cmd);
            return m_Ds;
        }

        public DataTable TimTheoDienThoai(string dienthoai)
        {
            DataService ds = new DataService();
            SqlCommand cmd = new SqlCommand("SELECT * FROM NHA_CUNG_CAP WHERE DIEN_THOAI = @dienthoai");
            cmd.Parameters.Add("dienthoai", SqlDbType.VarChar, 50).Value = (dienthoai ?? "").Trim();
            ds.Load(cmd);
            return ds;
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
            DataService.OpenConnection();
            return m_Ds.ExecuteNoneQuery() >= 0;
        }
    }
}
