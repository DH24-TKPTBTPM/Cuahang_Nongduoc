﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace CuahangNongduoc.DataLayer
{
    /// <summary>Data Access Layer truy vấn bảng DAI_LY và kiểm tra ràng buộc giao dịch đại lý.</summary>
    public class DaiLyFactory
    {
        DataService m_Ds = new DataService();

        /// <summary>
        /// Tải danh sách đại lý hiện hành từ SQL Server.
        /// </summary>
        public DataTable DanhsachDaiLy()
        {
            SqlCommand cmd = new SqlCommand("SELECT * FROM DAI_LY");
            m_Ds.Load(cmd);
            return m_Ds;
        }

        /// Tạo danh sách tương thích bán sỉ, bao gồm đại lý hiện hành và dữ liệu đại lý lịch sử trong KHACH_HANG.
        public DataTable DanhsachDaiLyBanSi()
        {
            DataService ds = new DataService();
            SqlCommand cmd = new SqlCommand(
                "SELECT ID, DAI_LY, DIA_CHI, DIEN_THOAI FROM DAI_LY " +
                "UNION ALL " +
                "SELECT ID, HO_TEN AS DAI_LY, DIA_CHI, DIEN_THOAI FROM KHACH_HANG WHERE LOAI_KH=0");
            ds.Load(cmd);
            return ds;
        }

        /// <summary>
        /// Tìm đại lý theo tên bằng tham số SQL để an toàn với ký tự đặc biệt.
        /// </summary>
        public DataTable TimHoTen(String hoten)
        {
            SqlCommand cmd = new SqlCommand("SELECT * FROM DAI_LY WHERE DAI_LY LIKE '%' + @hoten + '%'");
            cmd.Parameters.Add("hoten", SqlDbType.NVarChar).Value = hoten;
            m_Ds.Load(cmd);
            return m_Ds;
        }

        /// <summary>
        /// Tìm đại lý theo địa chỉ bằng tham số SQL.
        /// </summary>
        public DataTable TimDiaChi(String diachi)
        {
            SqlCommand cmd = new SqlCommand("SELECT * FROM DAI_LY WHERE DIA_CHI LIKE '%' + @diachi + '%'");
            cmd.Parameters.Add("diachi", SqlDbType.NVarChar).Value = diachi;
            m_Ds.Load(cmd);
            return m_Ds;
        }

        /// <summary>
        /// Tìm đại lý theo tên và trả về DataTable độc lập cho BindingSource bên ngoài.
        /// </summary>
        public DataTable TimHoTenMoi(String hoten)
        {
            DataService ds = new DataService();
            SqlCommand cmd = new SqlCommand("SELECT * FROM DAI_LY WHERE DAI_LY LIKE '%' + @hoten + '%'");
            cmd.Parameters.Add("hoten", SqlDbType.NVarChar).Value = hoten;
            ds.Load(cmd);
            return ds;
        }

        /// <summary>
        /// Tìm đại lý theo địa chỉ và trả về DataTable độc lập cho BindingSource bên ngoài.
        /// </summary>
        public DataTable TimDiaChiMoi(String diachi)
        {
            DataService ds = new DataService();
            SqlCommand cmd = new SqlCommand("SELECT * FROM DAI_LY WHERE DIA_CHI LIKE '%' + @diachi + '%'");
            cmd.Parameters.Add("diachi", SqlDbType.NVarChar).Value = diachi;
            ds.Load(cmd);
            return ds;
        }

        /// <summary>
        /// Đọc một đại lý theo khóa chính để nạp thông tin chi tiết.
        /// </summary>
        public DataTable LayDaiLy(String id)
        {
            DataService ds = new DataService();
            SqlCommand cmd = new SqlCommand("SELECT * FROM DAI_LY WHERE ID = @id");
            cmd.Parameters.Add("id", SqlDbType.NVarChar, 50).Value = id;
            ds.Load(cmd);
            return ds;
        }

        /// <summary>
        /// Kiểm tra PHIEU_BAN và DU_NO_KH trước khi xóa để bảo toàn lịch sử giao dịch.
        /// <param name="idDaiLy">ID đại lý được tham chiếu trong các bảng nghiệp vụ.</param>
        /// <returns>True khi không phát sinh giao dịch/công nợ; false khi còn ràng buộc.</returns>
        /// </summary>
        public bool KiemTraRangBuocXoa(String idDaiLy)
        {
            DataService ds = new DataService();
            SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM PHIEU_BAN WHERE ID_KHACH_HANG = @id");
            cmd.Parameters.Add("id", SqlDbType.VarChar, 50).Value = idDaiLy;
            ds.Load(cmd);
            if (ds.Rows.Count > 0 && Convert.ToInt32(ds.Rows[0][0]) > 0) return false;

            DataService ds2 = new DataService();
            SqlCommand cmdNo = new SqlCommand("SELECT COUNT(*) FROM DU_NO_KH WHERE ID_KHACH_HANG = @id");
            cmdNo.Parameters.Add("id", SqlDbType.VarChar, 50).Value = idDaiLy;
            ds2.Load(cmdNo);
            if (ds2.Rows.Count > 0 && Convert.ToInt32(ds2.Rows[0][0]) > 0) return false;

            return true;
        }

        /// <summary>
        /// Hoàn nguyên thay đổi trong bộ nhớ khi người dùng hủy thao tác hoặc ghi DB thất bại.
        /// </summary>
        public void RejectChanges()
        {
            m_Ds.RejectChanges();
        }

        /// <summary>
        /// Tạo DataRow theo schema đã tải từ bảng DAI_LY.
        /// </summary>
        public DataRow NewRow()
        {
            return m_Ds.NewRow();
        }

        /// <summary>
        /// Đưa DataRow mới vào DataTable để Controller xử lý trong lần Save kế tiếp.
        /// </summary>
        public void Add(DataRow row)
        {
            m_Ds.Rows.Add(row);
        }

        /// <summary>
        /// Ghi các thay đổi Add/Edit/Delete xuống bảng DAI_LY trong SQL Server.
        /// </summary>
        public bool Save()
        {
            DataService.OpenConnection();
            m_Ds.Command = new SqlCommand("SELECT * FROM DAI_LY");
            return m_Ds.ExecuteNoneQuery() > 0;
        }
    }
}
