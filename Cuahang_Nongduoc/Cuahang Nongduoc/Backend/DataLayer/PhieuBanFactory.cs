using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.OleDb;

namespace CuahangNongduoc.DataLayer
{
    public class PhieuBanFactory
    {
        DataService m_Ds = new DataService();

        public DataTable TimPhieuBan(String idKh, DateTime dt)
        {
            OleDbCommand cmd = new OleDbCommand("SELECT * FROM PHIEU_BAN WHERE NGAY_BAN = @ngay AND ID_KHACH_HANG=@kh");
            cmd.Parameters.Add("ngay", OleDbType.Date).Value = dt;
            cmd.Parameters.Add("kh", OleDbType.VarChar).Value = idKh;

            m_Ds.Load(cmd);

            return m_Ds;
        }

        public DataTable DanhsachPhieuBanLe()
        {
            OleDbCommand cmd = new OleDbCommand("SELECT PB.* FROM PHIEU_BAN PB INNER JOIN KHACH_HANG KH ON PB.ID_KHACH_HANG=KH.ID WHERE KH.LOAI_KH=TRUE");
            m_Ds.Load(cmd);

            return m_Ds;
        }
        public DataTable DanhsachPhieuBanSi()
        {
            OleDbCommand cmd = new OleDbCommand("SELECT PB.* FROM PHIEU_BAN PB INNER JOIN DAI_LY DL ON PB.ID_KHACH_HANG=DL.ID");
            m_Ds.Load(cmd);

            return m_Ds;
        }


        public DataTable LayPhieuBan(String id)
        {
            DataService ds = new DataService();
            OleDbCommand cmd = new OleDbCommand("SELECT * FROM PHIEU_BAN WHERE ID = @id");
            cmd.Parameters.Add("id", OleDbType.VarChar,50).Value = id;
            ds.Load(cmd);
            return ds;
        }


        public DataTable LayChiTietPhieuBan(String idPhieuBan)
        {
            DataService ds = new DataService();
            OleDbCommand cmd = new OleDbCommand("SELECT * FROM CHI_TIET_PHIEU_BAN WHERE ID_PHIEU_BAN = @id");
            cmd.Parameters.Add("id", OleDbType.VarChar,50).Value = idPhieuBan;
            ds.Load(cmd);
            return ds;
        }

        public static long LayConNo(String kh, int thang, int nam)
        {
            DataService ds = new DataService();
            OleDbCommand cmd = new OleDbCommand("SELECT SUM(CON_NO) FROM PHIEU_BAN WHERE ID_KHACH_HANG = @kh AND MONTH(NGAY_BAN)=@thang AND YEAR(NGAY_BAN)= @nam");
            cmd.Parameters.Add("kh", OleDbType.VarChar, 50).Value = kh;
            cmd.Parameters.Add("thang", OleDbType.Integer).Value = thang;
            cmd.Parameters.Add("nam", OleDbType.Integer).Value = nam;

            object obj = ds.ExecuteScalar(cmd);
            if (obj == null || obj == DBNull.Value)
                return 0;
            else
                return Convert.ToInt64(obj);
        }

        public static int LaySoPhieu()
        {
            DataService ds = new DataService();
            OleDbCommand cmd = new OleDbCommand("SELECT COUNT(*) FROM PHIEU_BAN");
            
            object obj = ds.ExecuteScalar(cmd);
            if (obj == null)
                return 0;
            else
                return Convert.ToInt32(obj);
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
        public DataTable ThongKeTongHopChiPhiBanHang(DateTime tuNgay, DateTime denNgay)
        {
            OleDbCommand cmd = new OleDbCommand(@"
            SELECT 
                COUNT(ID) AS SO_HOA_DON,
                SUM(TIEN_HANG) AS TONG_TIEN_HANG,
                SUM(PHI_VAN_CHUYEN) AS TONG_PHI_VAN_CHUYEN,
                SUM(TONG_DICH_VU) AS TONG_DICH_VU,
                SUM(TIEN_GIAM_GIA) AS TONG_TIEN_GIAM,
                SUM(TONG_TIEN) AS TONG_DOANH_THU
            FROM PHIEU_BAN
            WHERE NGAY_BAN >= @tuNgay AND NGAY_BAN <= @denNgay
        ");
            cmd.Parameters.Add("tuNgay", OleDbType.Date).Value = tuNgay;
            cmd.Parameters.Add("denNgay", OleDbType.Date).Value = denNgay;

            DataService ds = new DataService();
            ds.Load(cmd);
            return ds;
        }

        public DataTable ThongKeChiTietDichVuPhu(DateTime tuNgay, DateTime denNgay)
        {
            OleDbCommand cmd = new OleDbCommand(@"
            SELECT 
                DV.TEN_DICH_VU, 
                COUNT(DV.ID) AS SO_LAN, 
                SUM(DV.SO_TIEN) AS TONG_TIEN
            FROM CHI_TIET_DICH_VU_PHIEU_BAN DV
            INNER JOIN PHIEU_BAN PB ON DV.ID_PHIEU_BAN = PB.ID
            WHERE PB.NGAY_BAN >= @tuNgay AND PB.NGAY_BAN <= @denNgay
            GROUP BY DV.TEN_DICH_VU
        ");
            cmd.Parameters.Add("tuNgay", OleDbType.Date).Value = tuNgay;
            cmd.Parameters.Add("denNgay", OleDbType.Date).Value = denNgay;

            DataService ds = new DataService();
            ds.Load(cmd);
            return ds;
        }
    }
}
