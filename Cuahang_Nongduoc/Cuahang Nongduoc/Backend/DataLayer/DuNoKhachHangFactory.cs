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
    public class DuNoKhachHangFactory
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
        public void LoadSchema()
        {
// Code cũ: OleDbCommand cmd = new OleDbCommand("SELECT * FROM DU_NO_KH WHERE ID_KHACH_HANG='-1'");
            SqlCommand cmd = new SqlCommand("SELECT * FROM DU_NO_KH WHERE ID_KHACH_HANG='-1'");
            m_Ds.Load(cmd);

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
