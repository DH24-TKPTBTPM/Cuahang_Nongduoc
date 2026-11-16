using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace CuahangNongduoc.DataLayer
{
    public class PhieuChiFactory
    {
        DataService m_Ds = new DataService();

        public DataTable TimPhieuChi(int lydo, DateTime ngay)
        {
            SqlCommand cmd = new SqlCommand("SELECT * FROM PHIEU_CHI WHERE ID_LY_DO_CHI = @lydo AND NGAY_CHI = @ngay");
            cmd.Parameters.Add("lydo", SqlDbType.Int).Value = lydo;
            cmd.Parameters.Add("ngay", SqlDbType.DateTime).Value = ngay;

            m_Ds.Load(cmd);

            return m_Ds;
        }

        public DataTable DanhsachPhieuChi()
        {
            SqlCommand cmd = new SqlCommand("SELECT * FROM PHIEU_CHI ");
            m_Ds.Load(cmd);

            return m_Ds;
        }
      
        public DataTable LayPhieuChi(String id)
        {
            SqlCommand cmd = new SqlCommand("SELECT * FROM PHIEU_CHI WHERE ID = @id");
            cmd.Parameters.Add("id", SqlDbType.VarChar,50).Value = id;
            m_Ds.Load(cmd);
            return m_Ds;
        }


        public static long LayTongTien(String lydo, int thang, int nam)
        {
            DataService ds = new DataService();
            SqlCommand cmd = new SqlCommand("SELECT SUM(TONG_TIEN) FROM PHIEU_CHI WHERE ID_LY_DO_CHI = @lydo AND MONTH(NGAY_CHI)=@thang AND YEAR(NGAY_CHI)= @nam");
            cmd.Parameters.Add("lydo", SqlDbType.VarChar, 50).Value = lydo;
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
