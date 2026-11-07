using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace CuahangNongduoc.DataLayer
{

    public class DuNoKhachHangFactory
    {
        DataService m_Ds = new DataService();


// ==================== [R28-NH] REFACTORING ====================
// Lỗi (Code smell) : Sử dụng provider cũ OleDb thay vì SqlClient
// Kỹ thuật         : Migrate Data Provider
// Tác giả          : BE3 - Bùi Lê Tuấn Anh
// Hỗ trợ           : BE1 - Nguyễn Tuấn Anh
// Kiểm duyệt       : QA2 - Dư Thị Trúc Hòa
// Ngày chỉnh sửa   : 06/11/2026
// Ngày kiểm thử    : 07/11/2026
// ==============================================================
        public void LoadSchema()
        {
// Code cũ: OleDbCommand cmd = new OleDbCommand("SELECT * FROM DU_NO_KH WHERE ID_KHACH_HANG='-1'");
            SqlCommand cmd = new SqlCommand("SELECT * FROM DU_NO_KH WHERE ID_KHACH_HANG='-1'");
            m_Ds.Load(cmd);

        }


// ==================== [R28-NH] REFACTORING ====================
// Lỗi (Code smell) : Sử dụng provider cũ OleDb thay vì SqlClient
// Kỹ thuật         : Migrate Data Provider
// Tác giả          : BE3 - Bùi Lê Tuấn Anh
// Hỗ trợ           : BE1 - Nguyễn Tuấn Anh
// Kiểm duyệt       : QA2 - Dư Thị Trúc Hòa
// Ngày chỉnh sửa   : 06/11/2026
// Ngày kiểm thử    : 07/11/2026
// ==============================================================
        public DataTable DanhsachDuNo(int thang, int nam)
        {
// Code cũ: OleDbCommand cmd = new OleDbCommand("SELECT * FROM DU_NO_KH WHERE THANG=@thang AND NAM=@nam");
            SqlCommand cmd = new SqlCommand("SELECT * FROM DU_NO_KH WHERE THANG=@thang AND NAM=@nam");
// Code cũ: cmd.Parameters.Add("thang", OleDbType.Integer).Value = thang;
            cmd.Parameters.Add("thang", SqlDbType.Int).Value = thang;
// Code cũ: cmd.Parameters.Add("nam", OleDbType.Integer).Value = nam;
            cmd.Parameters.Add("nam", SqlDbType.Int).Value = nam;

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
// Ngày kiểm thử    : 07/11/2026
// ==============================================================
        public DataTable LayDuNoKhachHang(String kh, int thang, int nam)
        {
// Code cũ: OleDbCommand cmd = new OleDbCommand("SELECT * FROM DU_NO_KH WHERE ID_KHACH_HANG = @kh AND THANG=@thang AND NAM=@nam");
            SqlCommand cmd = new SqlCommand("SELECT * FROM DU_NO_KH WHERE ID_KHACH_HANG = @kh AND THANG=@thang AND NAM=@nam");
// Code cũ: cmd.Parameters.Add("kh", OleDbType.VarChar, 50).Value = kh;
            cmd.Parameters.Add("kh", SqlDbType.VarChar, 50).Value = kh;
// Code cũ: cmd.Parameters.Add("thang", OleDbType.Integer).Value = thang;
            cmd.Parameters.Add("thang", SqlDbType.Int).Value = thang;
// Code cũ: cmd.Parameters.Add("nam", OleDbType.Integer).Value = nam;
            cmd.Parameters.Add("nam", SqlDbType.Int).Value = nam;

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
// Ngày kiểm thử    : 07/11/2026
// ==============================================================
        public static long LayDuNo(String kh, int thang, int nam)
        {
            DataService ds = new DataService();
// Code cũ: OleDbCommand cmd = new OleDbCommand("SELECT CUOI_KY FROM DU_NO_KH WHERE ID_KHACH_HANG = @kh AND THANG=@thang AND NAM=@nam");
            SqlCommand cmd = new SqlCommand("SELECT CUOI_KY FROM DU_NO_KH WHERE ID_KHACH_HANG = @kh AND THANG=@thang AND NAM=@nam");
// Code cũ: cmd.Parameters.Add("kh", OleDbType.VarChar, 50).Value = kh;
            cmd.Parameters.Add("kh", SqlDbType.VarChar, 50).Value = kh;
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


// ==================== [R28-NH] REFACTORING ====================
// Lỗi (Code smell) : Sử dụng provider cũ OleDb thay vì SqlClient
// Kỹ thuật         : Migrate Data Provider
// Tác giả          : BE3 - Bùi Lê Tuấn Anh
// Hỗ trợ           : BE1 - Nguyễn Tuấn Anh
// Kiểm duyệt       : QA2 - Dư Thị Trúc Hòa
// Ngày chỉnh sửa   : 06/11/2026
// Ngày kiểm thử    : 07/11/2026
// ==============================================================
        public void Clear(int thang, int nam)
        {
// Code cũ: OleDbCommand cmd = new OleDbCommand("DELETE FROM DU_NO_KH WHERE THANG=@thang AND NAM=@nam");
            SqlCommand cmd = new SqlCommand("DELETE FROM DU_NO_KH WHERE THANG=@thang AND NAM=@nam");
// Code cũ: cmd.Parameters.Add("thang", OleDbType.Integer).Value = thang;
            cmd.Parameters.Add("thang", SqlDbType.Int).Value = thang;
// Code cũ: cmd.Parameters.Add("nam", OleDbType.Integer).Value = nam;
            cmd.Parameters.Add("nam", SqlDbType.Int).Value = nam;

            m_Ds.ExecuteNoneQuery(cmd);
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
