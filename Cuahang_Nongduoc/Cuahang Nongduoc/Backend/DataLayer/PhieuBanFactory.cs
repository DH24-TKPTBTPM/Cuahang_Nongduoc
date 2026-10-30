using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace CuahangNongduoc.DataLayer
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
    public class PhieuBanFactory
    {
        DataService m_Ds = new DataService();

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
        public DataTable TimPhieuBan(String idKh, DateTime dt)
        {
// Code cũ: OleDbCommand cmd = new OleDbCommand("SELECT * FROM PHIEU_BAN WHERE NGAY_BAN = @ngay AND ID_KHACH_HANG=@kh");
            SqlCommand cmd = new SqlCommand("SELECT * FROM PHIEU_BAN WHERE NGAY_BAN = @ngay AND ID_KHACH_HANG=@kh");
// Code cũ: cmd.Parameters.Add("ngay", OleDbType.Date).Value = dt;
            cmd.Parameters.Add("ngay", SqlDbType.DateTime).Value = dt;
// Code cũ: cmd.Parameters.Add("kh", OleDbType.VarChar).Value = idKh;
            cmd.Parameters.Add("kh", SqlDbType.VarChar).Value = idKh;

            m_Ds.Load(cmd);

            return m_Ds.ResultTable;
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
        public DataTable DanhsachPhieuBanLe()
        {
// Code cũ: OleDbCommand cmd = new OleDbCommand("SELECT PB.* FROM PHIEU_BAN PB INNER JOIN KHACH_HANG KH ON PB.ID_KHACH_HANG=KH.ID WHERE KH.LOAI_KH=FALSE");
            SqlCommand cmd = new SqlCommand("SELECT PB.* FROM PHIEU_BAN PB INNER JOIN KHACH_HANG KH ON PB.ID_KHACH_HANG=KH.ID WHERE KH.LOAI_KH=0");
            m_Ds.Load(cmd);

            return m_Ds.ResultTable;
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
        public DataTable DanhsachPhieuBanSi()
        {
// Code cũ: OleDbCommand cmd = new OleDbCommand("SELECT PB.* FROM PHIEU_BAN PB INNER JOIN KHACH_HANG KH ON PB.ID_KHACH_HANG=KH.ID WHERE KH.LOAI_KH=TRUE");
            SqlCommand cmd = new SqlCommand("SELECT PB.* FROM PHIEU_BAN PB INNER JOIN KHACH_HANG KH ON PB.ID_KHACH_HANG=KH.ID WHERE KH.LOAI_KH=1");
            m_Ds.Load(cmd);

            return m_Ds.ResultTable;
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
        public DataTable LayPhieuBan(String id)
        {
// Code cũ: OleDbCommand cmd = new OleDbCommand("SELECT * FROM PHIEU_BAN WHERE ID = @id");
            SqlCommand cmd = new SqlCommand("SELECT * FROM PHIEU_BAN WHERE ID = @id");
// Code cũ: cmd.Parameters.Add("id", OleDbType.VarChar,50).Value = id;
            cmd.Parameters.Add("id", SqlDbType.VarChar,50).Value = id;
            m_Ds.Load(cmd);
            return m_Ds.ResultTable;
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
        public DataTable LayChiTietPhieuBan(String idPhieuBan)
        {
// Code cũ: OleDbCommand cmd = new OleDbCommand("SELECT * FROM CHI_TIET_PHIEU_BAN WHERE ID_PHIEU_BAN = @id");
            SqlCommand cmd = new SqlCommand("SELECT * FROM CHI_TIET_PHIEU_BAN WHERE ID_PHIEU_BAN = @id");
// Code cũ: cmd.Parameters.Add("id", OleDbType.VarChar,50).Value = idPhieuBan;
            cmd.Parameters.Add("id", SqlDbType.VarChar,50).Value = idPhieuBan;
            m_Ds.Load(cmd);
            return m_Ds.ResultTable;
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
        public static long LayConNo(String kh, int thang, int nam)
        {
            DataService ds = new DataService();
// Code cũ: OleDbCommand cmd = new OleDbCommand("SELECT SUM(CON_NO) FROM PHIEU_BAN WHERE ID_KHACH_HANG = @kh AND MONTH(NGAY_BAN)=@thang AND YEAR(NGAY_BAN)= @nam");
            SqlCommand cmd = new SqlCommand("SELECT SUM(CON_NO) FROM PHIEU_BAN WHERE ID_KHACH_HANG = @kh AND MONTH(NGAY_BAN)=@thang AND YEAR(NGAY_BAN)= @nam");
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
        public static int LaySoPhieu()
        {
            DataService ds = new DataService();
// Code cũ: OleDbCommand cmd = new OleDbCommand("SELECT COUNT(*) FROM PHIEU_BAN");
            SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM PHIEU_BAN");
            
            object obj = ds.ExecuteScalar(cmd);
            if (obj == null)
                return 0;
            else
                return Convert.ToInt32(obj);
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
        public DataRow NewRow()
        {
            return m_Ds.NewRow();
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
        public void Add(DataRow row)
        {
            m_Ds.Rows.Add(row);
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
       public bool Save()
        {
           
            return m_Ds.ExecuteNoneQuery() > 0;
        }
    }
}
