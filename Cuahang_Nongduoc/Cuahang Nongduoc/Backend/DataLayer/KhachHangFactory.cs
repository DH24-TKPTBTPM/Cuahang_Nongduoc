﻿using System;
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
            SqlCommand cmd = new SqlCommand("SELECT * FROM KHACH_HANG WHERE LOAI_KH = " + (loai ? "1" : "0"));
            m_Ds.Load(cmd);

            return m_Ds;
        }
        public DataTable TimHoTen(String hoten, bool loai)
        {
            SqlCommand cmd = new SqlCommand("SELECT * FROM KHACH_HANG WHERE HO_TEN LIKE '%' + @hoten + '%' AND LOAI_KH = " + loai);
            cmd.Parameters.Add("hoten", SqlDbType.VarChar).Value = hoten;
            m_Ds.Load(cmd);

            return m_Ds;
        }

        public DataTable TimDiaChi(String diachi, bool loai)
        {
            SqlCommand cmd = new SqlCommand("SELECT * FROM KHACH_HANG WHERE DIA_CHI LIKE '%' + @diachi + '%' AND LOAI_KH = " + loai);
            cmd.Parameters.Add("diachi", SqlDbType.VarChar).Value = diachi;
            m_Ds.Load(cmd);

            return m_Ds;
        }

        public DataTable DanhsachKhachHang()
        {
            SqlCommand cmd = new SqlCommand("SELECT * FROM KHACH_HANG WHERE LOAI_KH = 1");
            m_Ds.Load(cmd);

            return m_Ds;
        }

        // Danh sách chọn đối tượng chung cho Phiếu thu/Dư nợ.
        // Không gộp hay ghi dữ liệu: chỉ đọc KHACH_HANG và DAI_LY để hiển thị.
        public DataTable DanhSachKhachHangVaDaiLy()
        {
            DataTable ketQua = new DataTable();
            ketQua.Columns.Add("ID", typeof(string));
            ketQua.Columns.Add("HO_TEN", typeof(string));
            ketQua.Columns.Add("DIA_CHI", typeof(string));
            ketQua.Columns.Add("DIEN_THOAI", typeof(string));
            ketQua.Columns.Add("LOAI_KH", typeof(bool));

            DataService dsKhachHang = new DataService();
            dsKhachHang.Load(new SqlCommand("SELECT * FROM KHACH_HANG WHERE LOAI_KH = 1"));
            foreach (DataRow row in dsKhachHang.Rows)
            {
                ketQua.Rows.Add(Convert.ToString(row["ID"]), Convert.ToString(row["HO_TEN"]),
                    Convert.ToString(row["DIA_CHI"]), Convert.ToString(row["DIEN_THOAI"]),
                    Convert.ToBoolean(row["LOAI_KH"]));
            }

            DataService dsDaiLy = new DataService();
            dsDaiLy.Load(new SqlCommand("SELECT * FROM DAI_LY"));
            foreach (DataRow row in dsDaiLy.Rows)
            {
                ketQua.Rows.Add(Convert.ToString(row["ID"]), Convert.ToString(row["DAI_LY"]),
                    Convert.ToString(row["DIA_CHI"]), Convert.ToString(row["DIEN_THOAI"]), true);
            }
            return ketQua;
        }

        public DataTable LayKhachHang(String id)
        {
            SqlCommand cmd = new SqlCommand("SELECT * FROM KHACH_HANG WHERE ID = @id");
            cmd.Parameters.Add("id", SqlDbType.VarChar,50).Value = id;
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
    }
}
