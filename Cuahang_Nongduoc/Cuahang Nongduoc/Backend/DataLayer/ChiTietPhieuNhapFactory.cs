﻿﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace CuahangNongduoc.DataLayer
{
    /// <summary>Data Access Layer đọc/ghi các dòng chi tiết phiếu nhập theo mã phiếu.</summary>
    public class ChiTietPhieuNhapFactory
    {
        DataService m_Ds = new DataService();

        /// <summary>Nạp schema rỗng để tạo DataRow chi tiết tương thích với bảng SQL Server.</summary>
        public void LoadSchema()
        {
            SqlCommand cmd = new SqlCommand("SELECT * FROM CHI_TIET_PHIEU_NHAP WHERE ID_PHIEU_NHAP = '-1'");
            m_Ds.Load(cmd);
        }

        /// <summary>Đọc toàn bộ lô thuộc một phiếu nhập để hiển thị hoặc đối soát.</summary>
        /// <param name="id">Mã phiếu nhập.</param><returns>DataTable các dòng chi tiết.</returns>
        public DataTable LayChiTietPhieuNhap(String id)
        {
            SqlCommand cmd = new SqlCommand("SELECT * FROM CHI_TIET_PHIEU_NHAP WHERE ID_PHIEU_NHAP = @id");
            cmd.Parameters.Add("@id", SqlDbType.VarChar, 50).Value = id;
            m_Ds.Load(cmd);
            return m_Ds;
        }

        /// <summary>Xóa các dòng chi tiết theo mã phiếu; thường được gọi trong transaction cấp cao hơn.</summary>
        /// <param name="id">Mã phiếu cần xóa chi tiết.</param><returns>Số dòng bị ảnh hưởng.</returns>
        public int XoaChiTietPhieuNhap(String id)
        {
            SqlCommand cmd = new SqlCommand("DELETE FROM CHI_TIET_PHIEU_NHAP WHERE ID_PHIEU_NHAP = @id");
            cmd.Parameters.Add("@id", SqlDbType.VarChar, 50).Value = id;
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
               
           return m_Ds.ExecuteNoneQuery() > 0;
        }
    }
}
