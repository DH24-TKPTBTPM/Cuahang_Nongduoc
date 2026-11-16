using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace CuahangNongduoc.DataLayer
{
    public class PhieuThanhToanFactory
    {
        DataService m_Ds = new DataService();

        public DataTable DanhsachPhieuThanhToan()
        {
            SqlCommand cmd = new SqlCommand("SELECT * FROM PHIEU_THANH_TOAN ");
            m_Ds.Load(cmd);

            return m_Ds;
        }

        // So sánh theo khoảng trong ngày, tránh lỗi cột ngày có kèm giờ
        public DataTable TimPhieuThanhToan(String kh, DateTime ngay)
        {
            SqlCommand cmd = new SqlCommand(
                "SELECT * FROM PHIEU_THANH_TOAN WHERE ID_KHACH_HANG=@kh " +
                "AND NGAY_THANH_TOAN >= @ngay1 AND NGAY_THANH_TOAN < @ngay2");
            cmd.Parameters.Add("kh", SqlDbType.VarChar, 50).Value = kh;
            cmd.Parameters.Add("ngay1", SqlDbType.DateTime).Value = ngay.Date;
            cmd.Parameters.Add("ngay2", SqlDbType.DateTime).Value = ngay.Date.AddDays(1);

            m_Ds.Load(cmd);

            return m_Ds;
        }

        public DataTable LayPhieuThanhToan(String id)
        {
            SqlCommand cmd = new SqlCommand("SELECT * FROM PHIEU_THANH_TOAN WHERE ID = @id");
            cmd.Parameters.Add("id", SqlDbType.VarChar, 50).Value = id;
            m_Ds.Load(cmd);
            return m_Ds;
        }

        public static long LayTongTien(String kh, int thang, int nam)
        {
            DataService ds = new DataService();
            SqlCommand cmd = new SqlCommand("SELECT SUM(TONG_TIEN) FROM PHIEU_THANH_TOAN WHERE ID_KHACH_HANG = @kh AND MONTH(NGAY_THANH_TOAN)=@thang AND YEAR(NGAY_THANH_TOAN)= @nam");
            cmd.Parameters.Add("kh", SqlDbType.VarChar, 50).Value = kh;
            cmd.Parameters.Add("thang", SqlDbType.Int).Value = thang;
            cmd.Parameters.Add("nam", SqlDbType.Int).Value = nam;

            object obj = ds.ExecuteScalar(cmd);

            if (obj == null)
                return 0;
            else
                return Convert.ToInt64(obj);
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
