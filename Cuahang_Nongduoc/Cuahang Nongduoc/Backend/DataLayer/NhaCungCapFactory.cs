using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.OleDb;

namespace CuahangNongduoc.DataLayer
{
    public class NhaCungCapFactory
    {
        DataService m_Ds = new DataService();

        public DataTable DanhsachNCC()
        {
            OleDbCommand cmd = new OleDbCommand("SELECT * FROM NHA_CUNG_CAP");
            m_Ds.Load(cmd);

            return m_Ds;
        }
        public DataTable TimDiaChi(String diachi)
        {
            OleDbCommand cmd = new OleDbCommand("SELECT * FROM NHA_CUNG_CAP WHERE DIA_CHI LIKE '%' + @diachi + '%' ");
            cmd.Parameters.Add("diachi", OleDbType.VarChar).Value = diachi;
            m_Ds.Load(cmd);

            return m_Ds;
        }
        public DataTable TimHoTen(String hoten)
        {
            OleDbCommand cmd = new OleDbCommand("SELECT * FROM NHA_CUNG_CAP WHERE HO_TEN LIKE '%' + @hoten + '%' ");
            cmd.Parameters.Add("hoten", OleDbType.VarChar).Value = hoten;
            m_Ds.Load(cmd);

            return m_Ds;
        }

        public DataTable LayNCC(String id)
        {
            OleDbCommand cmd = new OleDbCommand("SELECT * FROM NHA_CUNG_CAP WHERE ID = @id");
            cmd.Parameters.Add("id", OleDbType.VarChar,50).Value = id;
            m_Ds.Load(cmd);
            return m_Ds;
        }

        public DataTable TimTheoDienThoai(string dienthoai)
        {
            DataService ds = new DataService();
            OleDbCommand cmd = new OleDbCommand("SELECT * FROM NHA_CUNG_CAP WHERE DIEN_THOAI = @dienthoai");
            cmd.Parameters.Add("dienthoai", OleDbType.VarChar, 50).Value = (dienthoai ?? "").Trim();
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
