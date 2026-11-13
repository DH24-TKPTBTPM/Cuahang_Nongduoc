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


// ==================== [R28-NH] REFACTORING ====================
// Lỗi (Code smell) : Sử dụng provider cũ OleDb thay vì SqlClient
// Kỹ thuật         : Migrate Data Provider
// Tác giả          : BE3 - Bùi Lê Tuấn Anh
// Hỗ trợ           : BE1 - Nguyễn Tuấn Anh
// Kiểm duyệt       : QA2 - Dư Thị Trúc Hòa
// Ngày chỉnh sửa   : 06/11/2026
// Ngày kiểm thử    : 07/11/2026
// ==============================================================
        public DataTable DanhsachPhieuThanhToan()
        {
// Code cũ: OleDbCommand cmd = new OleDbCommand("SELECT * FROM PHIEU_THANH_TOAN ");
            SqlCommand cmd = new SqlCommand("SELECT * FROM PHIEU_THANH_TOAN ");
            m_Ds.Load(cmd);

            return m_Ds.ResultTable;
        }

// ==================== [R32-TC] REFACTORING ====================
// Lỗi (Code smell) : Thuật toán lọc ngày phức tạp, dài dòng
// Kỹ thuật         : Substitute Algorithm
// Tác giả          : BE3 - Bùi Lê Tuấn Anh
// Hỗ trợ           :
// Kiểm duyệt       : QA2 - Dư Thị Trúc Hòa
// Ngày chỉnh sửa   : 11/11/2026
// ==============================================================
// ==================== [R28-NH] REFACTORING ====================
// Lỗi (Code smell) : Sử dụng provider cũ OleDb thay vì SqlClient
// Kỹ thuật         : Migrate Data Provider
// Tác giả          : BE3 - Bùi Lê Tuấn Anh
// Hỗ trợ           : BE1 - Nguyễn Tuấn Anh
// Kiểm duyệt       : QA2 - Dư Thị Trúc Hòa
// Ngày chỉnh sửa   : 06/11/2026
// Ngày kiểm thử    : 07/11/2026
// ==============================================================
        public DataTable TimPhieuThanhToan(String kh, DateTime ngay)
        {
// Code cũ: OleDbCommand cmd = new OleDbCommand("SELECT * FROM PHIEU_THANH_TOAN WHERE ID_KHACH_HANG=@kh AND NGAY_THANH_TOAN = @ngay");
            SqlCommand cmd = new SqlCommand("SELECT * FROM PHIEU_THANH_TOAN WHERE ID_KHACH_HANG=@kh AND NGAY_THANH_TOAN >= @ngay AND NGAY_THANH_TOAN < @ngayKeTiep");
// Code cũ: cmd.Parameters.Add("kh", OleDbType.VarChar).Value = kh;
            cmd.Parameters.Add("kh", SqlDbType.VarChar).Value = kh;
// Code cũ: cmd.Parameters.Add("ngay", OleDbType.Date).Value = ngay;
            cmd.Parameters.Add("ngay", SqlDbType.DateTime).Value = ngay.Date;
            cmd.Parameters.Add("ngayKeTiep", SqlDbType.DateTime).Value = ngay.Date.AddDays(1);

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
        public DataTable LayPhieuThanhToan(String id)
        {
// Code cũ: OleDbCommand cmd = new OleDbCommand("SELECT * FROM PHIEU_THANH_TOAN WHERE ID = @id");
            SqlCommand cmd = new SqlCommand("SELECT * FROM PHIEU_THANH_TOAN WHERE ID = @id");
// Code cũ: cmd.Parameters.Add("id", OleDbType.VarChar,50).Value = id;
            cmd.Parameters.Add("id", SqlDbType.VarChar,50).Value = id;
            m_Ds.Load(cmd);
            return m_Ds.ResultTable;
        }



// Kiểm duyệt       : QA2 - Dư Thị Trúc Hòa
// Ngày chỉnh sửa   : 06/11/2026
// Ngày kiểm thử    : 07/11/2026
// ==============================================================
        public static long LayTongTien(String kh, int thang, int nam)
        {
            DataService ds = new DataService();
// Code cũ: OleDbCommand cmd = new OleDbCommand("SELECT SUM(TONG_TIEN) FROM PHIEU_THANH_TOAN WHERE ID_KHACH_HANG = @kh AND MONTH(NGAY_THANH_TOAN)=@thang AND YEAR(NGAY_THANH_TOAN)= @nam");
            SqlCommand cmd = new SqlCommand("SELECT ISNULL(SUM(TONG_TIEN), 0) FROM PHIEU_THANH_TOAN WHERE ID_KHACH_HANG = @kh AND MONTH(NGAY_THANH_TOAN)=@thang AND YEAR(NGAY_THANH_TOAN)= @nam");
// Code cũ: cmd.Parameters.Add("kh", OleDbType.VarChar, 50).Value = kh;
            cmd.Parameters.Add("kh", SqlDbType.VarChar, 50).Value = kh;
// Code cũ: cmd.Parameters.Add("thang", OleDbType.Integer).Value = thang;
            cmd.Parameters.Add("thang", SqlDbType.Int).Value = thang;
// Code cũ: cmd.Parameters.Add("nam", OleDbType.Integer).Value = nam;
            cmd.Parameters.Add("nam", SqlDbType.Int).Value = nam;

            object obj = ds.ExecuteScalar(cmd);
            
            if (obj == null || Convert.IsDBNull(obj))
                return 0;
            else
                return Convert.ToInt64(obj);
        }

// ==================== [R24] REFACTORING ====================
// Lỗi (Code smell) : Dính control UI, N+1 query, mất dữ liệu khi Lưu lỗi
// Kỹ thuật         : Extract Method + Safe Transaction
// Tác giả          : BE3 - Bùi Lê Tuấn Anh
// Hỗ trợ           : FE3 - Nguyễn Thị Ngân Huệ
// Kiểm duyệt       : QA2 - Dư Thị Trúc Hòa
// Ngày chỉnh sửa   : 08/11/2026
// ==============================================================
        public static DataTable LayTongTienTheoKhachHang(int thang, int nam)
        {
            DataService ds = new DataService();
            SqlCommand cmd = new SqlCommand("SELECT ID_KHACH_HANG, ISNULL(SUM(TONG_TIEN), 0) AS TONG_TIEN FROM PHIEU_THANH_TOAN WHERE MONTH(NGAY_THANH_TOAN)=@thang AND YEAR(NGAY_THANH_TOAN)=@nam GROUP BY ID_KHACH_HANG");
            cmd.Parameters.Add("thang", SqlDbType.Int).Value = thang;
            cmd.Parameters.Add("nam", SqlDbType.Int).Value = nam;
            ds.Load(cmd);
            return ds.ResultTable;
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
