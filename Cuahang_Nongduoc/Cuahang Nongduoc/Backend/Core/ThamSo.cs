using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace CuahangNongduoc
{
    public enum Controll
    {
        Normal,
        AddNew,
        Edit
    }
/// <summary>
/// [REFACTORING GURU]
/// - Code Smell: Tight Coupling với OleDb và Primitive Obsession trong tầng truy cập dữ liệu
/// - Kỹ thuật Refactoring (1 trong 64 mẫu): Extract Class
/// - Trước khi sửa: Dùng OleDbCommand và OleDbType của Access
/// - Sau khi sửa: Chuyển sang SqlCommand và SqlDbType chuẩn SQL Server
/// - Tác giả (Author): Bùi Lê Tuấn Anh
/// - Hỗ trợ (Supporter): BE Team
/// - Kiểm tra (Reviewer/Tester): QA Team
/// - Ngày chỉnh sửa (Date): 30/10/2026
/// </summary>
    public class ThamSo
    {
/// <summary>
/// [REFACTORING GURU]
/// - Code Smell: Tight Coupling với OleDb và Primitive Obsession trong tầng truy cập dữ liệu
/// - Kỹ thuật Refactoring (1 trong 64 mẫu): Extract Class
/// - Trước khi sửa: Dùng OleDbCommand và OleDbType của Access
/// - Sau khi sửa: Chuyển sang SqlCommand và SqlDbType chuẩn SQL Server
/// - Tác giả (Author): Bùi Lê Tuấn Anh
/// - Hỗ trợ (Supporter): BE Team
/// - Kiểm tra (Reviewer/Tester): QA Team
/// - Ngày chỉnh sửa (Date): 30/10/2026
/// </summary>
        public static void PreMonth(ref int thangtruoc, ref int namtruoc, int thang, int nam)
        {
            thangtruoc = thang - 1;
            namtruoc = nam;
            if (thangtruoc == 0)
            {
                thangtruoc = 12;
                namtruoc= nam - 1;
            }
        }

/// <summary>
/// [REFACTORING GURU]
/// - Code Smell: Tight Coupling với OleDb và Primitive Obsession trong tầng truy cập dữ liệu
/// - Kỹ thuật Refactoring (1 trong 64 mẫu): Extract Class
/// - Trước khi sửa: Dùng OleDbCommand và OleDbType của Access
/// - Sau khi sửa: Chuyển sang SqlCommand và SqlDbType chuẩn SQL Server
/// - Tác giả (Author): Bùi Lê Tuấn Anh
/// - Hỗ trợ (Supporter): BE Team
/// - Kiểm tra (Reviewer/Tester): QA Team
/// - Ngày chỉnh sửa (Date): 30/10/2026
/// </summary>
        public static bool LaSoNguyen(String so)
        {
            try
            {
                Convert.ToInt64(so);
                return true;
            }
            catch
            {
                return false;
            }
        }

/// <summary>
/// [REFACTORING GURU]
/// - Code Smell: Tight Coupling với OleDb và Primitive Obsession trong tầng truy cập dữ liệu
/// - Kỹ thuật Refactoring (1 trong 64 mẫu): Extract Class
/// - Trước khi sửa: Dùng OleDbCommand và OleDbType của Access
/// - Sau khi sửa: Chuyển sang SqlCommand và SqlDbType chuẩn SQL Server
/// - Tác giả (Author): Bùi Lê Tuấn Anh
/// - Hỗ trợ (Supporter): BE Team
/// - Kiểm tra (Reviewer/Tester): QA Team
/// - Ngày chỉnh sửa (Date): 30/10/2026
/// </summary>
        public static long LayMaPhieuNhap()
        {
            DataService ds = new DataService();
// Code cũ: object obj = ds.ExecuteScalar(new OleDbCommand("SELECT PHIEU_NHAP FROM THAM_SO"));
            object obj = ds.ExecuteScalar(new SqlCommand("SELECT PHIEU_NHAP FROM THAM_SO"));
            return Convert.ToInt64(obj);
        }
/// <summary>
/// [REFACTORING GURU]
/// - Code Smell: Tight Coupling với OleDb và Primitive Obsession trong tầng truy cập dữ liệu
/// - Kỹ thuật Refactoring (1 trong 64 mẫu): Extract Class
/// - Trước khi sửa: Dùng OleDbCommand và OleDbType của Access
/// - Sau khi sửa: Chuyển sang SqlCommand và SqlDbType chuẩn SQL Server
/// - Tác giả (Author): Bùi Lê Tuấn Anh
/// - Hỗ trợ (Supporter): BE Team
/// - Kiểm tra (Reviewer/Tester): QA Team
/// - Ngày chỉnh sửa (Date): 30/10/2026
/// </summary>
        public static void GanMaPhieuNhap(long id)
        {
            DataService ds = new DataService();
// Code cũ: ds.ExecuteNoneQuery(new OleDbCommand("UPDATE THAM_SO SET PHIEU_NHAP = " + id));
            ds.ExecuteNoneQuery(new SqlCommand("UPDATE THAM_SO SET PHIEU_NHAP = " + id));
            
        }

/// <summary>
/// [REFACTORING GURU]
/// - Code Smell: Tight Coupling với OleDb và Primitive Obsession trong tầng truy cập dữ liệu
/// - Kỹ thuật Refactoring (1 trong 64 mẫu): Extract Class
/// - Trước khi sửa: Dùng OleDbCommand và OleDbType của Access
/// - Sau khi sửa: Chuyển sang SqlCommand và SqlDbType chuẩn SQL Server
/// - Tác giả (Author): Bùi Lê Tuấn Anh
/// - Hỗ trợ (Supporter): BE Team
/// - Kiểm tra (Reviewer/Tester): QA Team
/// - Ngày chỉnh sửa (Date): 30/10/2026
/// </summary>
        public static long LayMaPhieuBan()
        {
            DataService ds = new DataService();
// Code cũ: object obj = ds.ExecuteScalar(new OleDbCommand("SELECT PHIEU_BAN FROM THAM_SO"));
            object obj = ds.ExecuteScalar(new SqlCommand("SELECT PHIEU_BAN FROM THAM_SO"));
            return Convert.ToInt64(obj);
        }
/// <summary>
/// [REFACTORING GURU]
/// - Code Smell: Tight Coupling với OleDb và Primitive Obsession trong tầng truy cập dữ liệu
/// - Kỹ thuật Refactoring (1 trong 64 mẫu): Extract Class
/// - Trước khi sửa: Dùng OleDbCommand và OleDbType của Access
/// - Sau khi sửa: Chuyển sang SqlCommand và SqlDbType chuẩn SQL Server
/// - Tác giả (Author): Bùi Lê Tuấn Anh
/// - Hỗ trợ (Supporter): BE Team
/// - Kiểm tra (Reviewer/Tester): QA Team
/// - Ngày chỉnh sửa (Date): 30/10/2026
/// </summary>
        public static void GanMaPhieuBan(long id)
        {
            DataService ds = new DataService();
// Code cũ: ds.ExecuteNoneQuery(new OleDbCommand("UPDATE THAM_SO SET PHIEU_BAN = " + id));
            ds.ExecuteNoneQuery(new SqlCommand("UPDATE THAM_SO SET PHIEU_BAN = " + id));

        }

/// <summary>
/// [REFACTORING GURU]
/// - Code Smell: Tight Coupling với OleDb và Primitive Obsession trong tầng truy cập dữ liệu
/// - Kỹ thuật Refactoring (1 trong 64 mẫu): Extract Class
/// - Trước khi sửa: Dùng OleDbCommand và OleDbType của Access
/// - Sau khi sửa: Chuyển sang SqlCommand và SqlDbType chuẩn SQL Server
/// - Tác giả (Author): Bùi Lê Tuấn Anh
/// - Hỗ trợ (Supporter): BE Team
/// - Kiểm tra (Reviewer/Tester): QA Team
/// - Ngày chỉnh sửa (Date): 30/10/2026
/// </summary>
        public static long LayMaPhieuThanhToan()
        {
            DataService ds = new DataService();
// Code cũ: object obj = ds.ExecuteScalar(new OleDbCommand("SELECT PHIEU_THANH_TOAN FROM THAM_SO"));
            object obj = ds.ExecuteScalar(new SqlCommand("SELECT PHIEU_THANH_TOAN FROM THAM_SO"));
            return Convert.ToInt64(obj);
        }
/// <summary>
/// [REFACTORING GURU]
/// - Code Smell: Tight Coupling với OleDb và Primitive Obsession trong tầng truy cập dữ liệu
/// - Kỹ thuật Refactoring (1 trong 64 mẫu): Extract Class
/// - Trước khi sửa: Dùng OleDbCommand và OleDbType của Access
/// - Sau khi sửa: Chuyển sang SqlCommand và SqlDbType chuẩn SQL Server
/// - Tác giả (Author): Bùi Lê Tuấn Anh
/// - Hỗ trợ (Supporter): BE Team
/// - Kiểm tra (Reviewer/Tester): QA Team
/// - Ngày chỉnh sửa (Date): 30/10/2026
/// </summary>
        public static void GanMaPhieuThanhToan(long id)
        {
            DataService ds = new DataService();
// Code cũ: ds.ExecuteNoneQuery(new OleDbCommand("UPDATE THAM_SO SET PHIEU_THANH_TOAN = " + id));
            ds.ExecuteNoneQuery(new SqlCommand("UPDATE THAM_SO SET PHIEU_THANH_TOAN = " + id));

        }



/// <summary>
/// [REFACTORING GURU]
/// - Code Smell: Tight Coupling với OleDb và Primitive Obsession trong tầng truy cập dữ liệu
/// - Kỹ thuật Refactoring (1 trong 64 mẫu): Extract Class
/// - Trước khi sửa: Dùng OleDbCommand và OleDbType của Access
/// - Sau khi sửa: Chuyển sang SqlCommand và SqlDbType chuẩn SQL Server
/// - Tác giả (Author): Bùi Lê Tuấn Anh
/// - Hỗ trợ (Supporter): BE Team
/// - Kiểm tra (Reviewer/Tester): QA Team
/// - Ngày chỉnh sửa (Date): 30/10/2026
/// </summary>
        public static long SanPham
        {
            get 
            {
                DataService ds = new DataService();
// Code cũ: object obj = ds.ExecuteScalar(new OleDbCommand("SELECT SAN_PHAM FROM THAM_SO"));
                object obj = ds.ExecuteScalar(new SqlCommand("SELECT SAN_PHAM FROM THAM_SO"));
                return Convert.ToInt64(obj);
            }
            set 
            {
                DataService ds = new DataService();
// Code cũ: ds.ExecuteNoneQuery(new OleDbCommand("UPDATE THAM_SO SET SAN_PHAM = " + value));
                ds.ExecuteNoneQuery(new SqlCommand("UPDATE THAM_SO SET SAN_PHAM = " + value));
            }
        }
	
        
/// <summary>
/// [REFACTORING GURU]
/// - Code Smell: Tight Coupling với OleDb và Primitive Obsession trong tầng truy cập dữ liệu
/// - Kỹ thuật Refactoring (1 trong 64 mẫu): Extract Class
/// - Trước khi sửa: Dùng OleDbCommand và OleDbType của Access
/// - Sau khi sửa: Chuyển sang SqlCommand và SqlDbType chuẩn SQL Server
/// - Tác giả (Author): Bùi Lê Tuấn Anh
/// - Hỗ trợ (Supporter): BE Team
/// - Kiểm tra (Reviewer/Tester): QA Team
/// - Ngày chỉnh sửa (Date): 30/10/2026
/// </summary>
        public static CuahangNongduoc.BusinessObject.CuaHang LayCuaHang()
        {
            CuahangNongduoc.BusinessObject.CuaHang ch = new CuahangNongduoc.BusinessObject.CuaHang();
            DataService ds = new DataService();
// Code cũ: ds.Load(new OleDbCommand("SELECT TEN_CUA_HANG, DIA_CHI, DIEN_THOAI FROM THAM_SO"));
            ds.Load(new SqlCommand("SELECT TEN_CUA_HANG, DIA_CHI, DIEN_THOAI FROM THAM_SO"));
            if (ds.Rows.Count > 0)
            {
                ch.TenCuaHang = ds.Rows[0]["TEN_CUA_HANG"].ToString();
                ch.DiaChi = ds.Rows[0]["DIA_CHI"].ToString();
                ch.DienThoai = ds.Rows[0]["DIEN_THOAI"].ToString();
            }
            return ch;
        }
/// <summary>
/// [REFACTORING GURU]
/// - Code Smell: Tight Coupling với OleDb và Primitive Obsession trong tầng truy cập dữ liệu
/// - Kỹ thuật Refactoring (1 trong 64 mẫu): Extract Class
/// - Trước khi sửa: Dùng OleDbCommand và OleDbType của Access
/// - Sau khi sửa: Chuyển sang SqlCommand và SqlDbType chuẩn SQL Server
/// - Tác giả (Author): Bùi Lê Tuấn Anh
/// - Hỗ trợ (Supporter): BE Team
/// - Kiểm tra (Reviewer/Tester): QA Team
/// - Ngày chỉnh sửa (Date): 30/10/2026
/// </summary>
        public static void GanCuaHang(String ten_cua_hang, String dia_chi , String dien_thoai)
        {
            DataService ds = new DataService();
// Code cũ: OleDbCommand cmd = new OleDbCommand("UPDATE THAM_SO SET TEN_CUA_HANG = @ten_cua_hang, DIA_CHI = @dia_chi, DIEN_THOAI = @dien_thoai ");
            SqlCommand cmd = new SqlCommand("UPDATE THAM_SO SET TEN_CUA_HANG = @ten_cua_hang, DIA_CHI = @dia_chi, DIEN_THOAI = @dien_thoai ");
// Code cũ: cmd.Parameters.Add("@ten_cua_hang", OleDbType.VarChar).Value = ten_cua_hang;
            cmd.Parameters.Add("@ten_cua_hang", SqlDbType.VarChar).Value = ten_cua_hang;
// Code cũ: cmd.Parameters.Add("@dia_chi", OleDbType.VarChar).Value = dia_chi;
            cmd.Parameters.Add("@dia_chi", SqlDbType.VarChar).Value = dia_chi;
// Code cũ: cmd.Parameters.Add("@dien_thoai", OleDbType.VarChar).Value = dien_thoai;
            cmd.Parameters.Add("@dien_thoai", SqlDbType.VarChar).Value = dien_thoai;

            ds.ExecuteNoneQuery(cmd);
        }

        

/// <summary>
/// [REFACTORING GURU]
/// - Code Smell: Tight Coupling với OleDb và Primitive Obsession trong tầng truy cập dữ liệu
/// - Kỹ thuật Refactoring (1 trong 64 mẫu): Extract Class
/// - Trước khi sửa: Dùng OleDbCommand và OleDbType của Access
/// - Sau khi sửa: Chuyển sang SqlCommand và SqlDbType chuẩn SQL Server
/// - Tác giả (Author): Bùi Lê Tuấn Anh
/// - Hỗ trợ (Supporter): BE Team
/// - Kiểm tra (Reviewer/Tester): QA Team
/// - Ngày chỉnh sửa (Date): 30/10/2026
/// </summary>
        public static long NhaCungCap
        {
            get
            {
                DataService ds = new DataService();
// Code cũ: object obj = ds.ExecuteScalar(new OleDbCommand("SELECT NHA_CUNG_CAP FROM THAM_SO"));
                object obj = ds.ExecuteScalar(new SqlCommand("SELECT NHA_CUNG_CAP FROM THAM_SO"));
                return Convert.ToInt64(obj);
            }
            set
            {
                DataService ds = new DataService();
// Code cũ: ds.ExecuteNoneQuery(new OleDbCommand("UPDATE THAM_SO SET NHA_CUNG_CAP = " + value));
                ds.ExecuteNoneQuery(new SqlCommand("UPDATE THAM_SO SET NHA_CUNG_CAP = " + value));
            }
        }

/// <summary>
/// [REFACTORING GURU]
/// - Code Smell: Tight Coupling với OleDb và Primitive Obsession trong tầng truy cập dữ liệu
/// - Kỹ thuật Refactoring (1 trong 64 mẫu): Extract Class
/// - Trước khi sửa: Dùng OleDbCommand và OleDbType của Access
/// - Sau khi sửa: Chuyển sang SqlCommand và SqlDbType chuẩn SQL Server
/// - Tác giả (Author): Bùi Lê Tuấn Anh
/// - Hỗ trợ (Supporter): BE Team
/// - Kiểm tra (Reviewer/Tester): QA Team
/// - Ngày chỉnh sửa (Date): 30/10/2026
/// </summary>
        public static long KhachHang
        {
            get
            {
                DataService ds = new DataService();
// Code cũ: object obj = ds.ExecuteScalar(new OleDbCommand("SELECT KHACH_HANG FROM THAM_SO"));
                object obj = ds.ExecuteScalar(new SqlCommand("SELECT KHACH_HANG FROM THAM_SO"));
                return Convert.ToInt64(obj);
            }
            set
            {
                DataService ds = new DataService();
// Code cũ: ds.ExecuteNoneQuery(new OleDbCommand("UPDATE THAM_SO SET KHACH_HANG = " + value));
                ds.ExecuteNoneQuery(new SqlCommand("UPDATE THAM_SO SET KHACH_HANG = " + value));
            }
        }

/// <summary>
/// [REFACTORING GURU]
/// - Code Smell: Tight Coupling với OleDb và Primitive Obsession trong tầng truy cập dữ liệu
/// - Kỹ thuật Refactoring (1 trong 64 mẫu): Extract Class
/// - Trước khi sửa: Dùng OleDbCommand và OleDbType của Access
/// - Sau khi sửa: Chuyển sang SqlCommand và SqlDbType chuẩn SQL Server
/// - Tác giả (Author): Bùi Lê Tuấn Anh
/// - Hỗ trợ (Supporter): BE Team
/// - Kiểm tra (Reviewer/Tester): QA Team
/// - Ngày chỉnh sửa (Date): 30/10/2026
/// </summary>
        public static long PhieuChi
        {
            get
            {
                DataService ds = new DataService();
// Code cũ: object obj = ds.ExecuteScalar(new OleDbCommand("SELECT PHIEU_CHI FROM THAM_SO"));
                object obj = ds.ExecuteScalar(new SqlCommand("SELECT PHIEU_CHI FROM THAM_SO"));
                return Convert.ToInt64(obj);
            }
            set
            {
                DataService ds = new DataService();
// Code cũ: ds.ExecuteNoneQuery(new OleDbCommand("UPDATE THAM_SO SET PHIEU_CHI = " + value));
                ds.ExecuteNoneQuery(new SqlCommand("UPDATE THAM_SO SET PHIEU_CHI = " + value));
            }
        }

       
    }
}
