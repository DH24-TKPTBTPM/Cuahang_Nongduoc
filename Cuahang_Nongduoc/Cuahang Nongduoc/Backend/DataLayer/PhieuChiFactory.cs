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


// ==================== [R28-NH] REFACTORING ====================
// Lỗi (Code smell) : Sử dụng provider cũ OleDb thay vì SqlClient
// Kỹ thuật         : Migrate Data Provider
// Tác giả          : BE3 - Bùi Lê Tuấn Anh
// Hỗ trợ           : BE1 - Nguyễn Tuấn Anh
// Kiểm duyệt       : QA2 - Dư Thị Trúc Hòa
// Ngày chỉnh sửa   : 06/11/2026
// ==============================================================
        public DataTable TimPhieuChi(int lydo, DateTime ngay)
        {
// Code cũ: OleDbCommand cmd = new OleDbCommand("SELECT * FROM PHIEU_CHI WHERE ID_LY_DO_CHI = @lydo AND NGAY_CHI = @ngay");
            SqlCommand cmd = new SqlCommand("SELECT * FROM PHIEU_CHI WHERE ID_LY_DO_CHI = @lydo AND NGAY_CHI = @ngay");
// Code cũ: cmd.Parameters.Add("lydo", OleDbType.Integer).Value = lydo;
            cmd.Parameters.Add("lydo", SqlDbType.Int).Value = lydo;
// Code cũ: cmd.Parameters.Add("ngay", OleDbType.Date).Value = ngay;
            cmd.Parameters.Add("ngay", SqlDbType.DateTime).Value = ngay;

            m_Ds.Load(cmd);

            return m_Ds.ResultTable;
        }


// ==================== [R28-NH] REFACTORING ====================
// Lỗi (Code smell) : Sử dụng provider cũ OleDb thay vì SqlClient
// Kỹ thuật         : Migrate Data Provider
// Tác giả          : BE3 - Bùi Lê Tuấn Anh
// Hỗ trợ           : BE1 - Nguyễn Tuấn Anh
// Kiểm duyệt       : QA2 - Dư Thị Trúc Hòa
// Ngày chỉnh sửa   : 06/11/2026
// ==============================================================
        public DataTable DanhsachPhieuChi()
        {
// Code cũ: OleDbCommand cmd = new OleDbCommand("SELECT * FROM PHIEU_CHI ");
            SqlCommand cmd = new SqlCommand("SELECT * FROM PHIEU_CHI ");
            m_Ds.Load(cmd);

            return m_Ds.ResultTable;
        }
      

// ==================== [R28-NH] REFACTORING ====================
// Lỗi (Code smell) : Sử dụng provider cũ OleDb thay vì SqlClient
// Kỹ thuật         : Migrate Data Provider
// Tác giả          : BE3 - Bùi Lê Tuấn Anh
// Hỗ trợ           : BE1 - Nguyễn Tuấn Anh
// Kiểm duyệt       : QA2 - Dư Thị Trúc Hòa
// Ngày chỉnh sửa   : 06/11/2026
// ==============================================================
        public DataTable LayPhieuChi(String id)
        {
// Code cũ: OleDbCommand cmd = new OleDbCommand("SELECT * FROM PHIEU_CHI WHERE ID = @id");
            SqlCommand cmd = new SqlCommand("SELECT * FROM PHIEU_CHI WHERE ID = @id");
// Code cũ: cmd.Parameters.Add("id", OleDbType.VarChar,50).Value = id;
            cmd.Parameters.Add("id", SqlDbType.VarChar,50).Value = id;
            m_Ds.Load(cmd);
            return m_Ds.ResultTable;
        }



// ==================== [R28-NH] REFACTORING ====================
// Lỗi (Code smell) : Sử dụng provider cũ OleDb thay vì SqlClient
// Kỹ thuật         : Migrate Data Provider
// Tác giả          : BE3 - Bùi Lê Tuấn Anh
// Hỗ trợ           : BE1 - Nguyễn Tuấn Anh
// Kiểm duyệt       : QA2 - Dư Thị Trúc Hòa
// Ngày chỉnh sửa   : 06/11/2026
// ==============================================================
        public static long LayTongTien(String lydo, int thang, int nam)
        {
            DataService ds = new DataService();
// Code cũ: OleDbCommand cmd = new OleDbCommand("SELECT SUM(TONG_TIEN) FROM PHIEU_CHI WHERE ID_LY_DO_CHI = @lydo AND MONTH(NGAY_CHI)=@thang AND YEAR(NGAY_CHI)= @nam");
            SqlCommand cmd = new SqlCommand("SELECT SUM(TONG_TIEN) FROM PHIEU_CHI WHERE ID_LY_DO_CHI = @lydo AND MONTH(NGAY_CHI)=@thang AND YEAR(NGAY_CHI)= @nam");
// Code cũ: cmd.Parameters.Add("lydo", OleDbType.VarChar, 50).Value = lydo;
            cmd.Parameters.Add("lydo", SqlDbType.VarChar, 50).Value = lydo;
// Code cũ: cmd.Parameters.Add("thang", OleDbType.Integer).Value = thang;
            cmd.Parameters.Add("thang", SqlDbType.Int).Value = thang;
// Code cũ: cmd.Parameters.Add("nam", OleDbType.Integer).Value = nam;
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
           
            return m_Ds.UpdateDataTable() > 0;
        }
    }
}
