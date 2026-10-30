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
    public class MaSanPhanFactory
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
// Code cũ: OleDbCommand cmd = new OleDbCommand("SELECT * FROM MA_SAN_PHAM WHERE ID = '-1'");
            SqlCommand cmd = new SqlCommand("SELECT * FROM MA_SAN_PHAM WHERE ID = '-1'");
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
        public DataTable DanhsachMaSanPham(String sp)
        {
// Code cũ: OleDbCommand cmd = new OleDbCommand("SELECT * FROM MA_SAN_PHAM WHERE ID_SAN_PHAM=@id AND SO_LUONG > 0");
            SqlCommand cmd = new SqlCommand("SELECT * FROM MA_SAN_PHAM WHERE ID_SAN_PHAM=@id AND SO_LUONG > 0");
// Code cũ: cmd.Parameters.Add("id", OleDbType.VarChar, 50).Value = sp;
            cmd.Parameters.Add("id", SqlDbType.VarChar, 50).Value = sp;
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
        public DataTable DanhsachChiTiet(String sp)
        {
// Code cũ: OleDbCommand cmd = new OleDbCommand("SELECT * FROM MA_SAN_PHAM WHERE ID_PHIEU_NHAP=@id");
            SqlCommand cmd = new SqlCommand("SELECT * FROM MA_SAN_PHAM WHERE ID_PHIEU_NHAP=@id");
// Code cũ: cmd.Parameters.Add("id", OleDbType.VarChar, 50).Value = sp;
            cmd.Parameters.Add("id", SqlDbType.VarChar, 50).Value = sp;
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
        public DataTable LaySanPham(String idMaSanPham)
        {
// Code cũ: OleDbCommand cmd = new OleDbCommand("SELECT SP.* FROM SAN_PHAM SP INNER JOIN MA_SAN_PHAM MSP ON SP.ID = MSP.ID_SAN_PHAM WHERE MSP.ID = @id");
            SqlCommand cmd = new SqlCommand("SELECT SP.* FROM SAN_PHAM SP INNER JOIN MA_SAN_PHAM MSP ON SP.ID = MSP.ID_SAN_PHAM WHERE MSP.ID = @id");
// Code cũ: cmd.Parameters.Add("id", OleDbType.VarChar,50).Value = idMaSanPham;
            cmd.Parameters.Add("id", SqlDbType.VarChar,50).Value = idMaSanPham;
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
        public DataTable LayMaSanPham(String idMaSanPham)
        {
// Code cũ: OleDbCommand cmd = new OleDbCommand("SELECT * FROM MA_SAN_PHAM MSP WHERE MSP.ID = @id");
            SqlCommand cmd = new SqlCommand("SELECT * FROM MA_SAN_PHAM MSP WHERE MSP.ID = @id");
// Code cũ: cmd.Parameters.Add("id", OleDbType.VarChar,50).Value = idMaSanPham;
            cmd.Parameters.Add("id", SqlDbType.VarChar,50).Value = idMaSanPham;
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
        public DataTable DanhsachMaSanPhamHetHan(DateTime dt)
        {
// Code cũ: OleDbCommand cmd = new OleDbCommand("SELECT * FROM MA_SAN_PHAM WHERE SO_LUONG > 0 AND NGAY_HET_HAN <= @ngay");
            SqlCommand cmd = new SqlCommand("SELECT * FROM MA_SAN_PHAM WHERE SO_LUONG > 0 AND NGAY_HET_HAN <= @ngay");
// Code cũ: cmd.Parameters.Add("ngay", OleDbType.Date).Value = dt;
            cmd.Parameters.Add("ngay", SqlDbType.DateTime).Value = dt;
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
        public DataTable DanhsachMaSanPham()
        {
// Code cũ: OleDbCommand cmd = new OleDbCommand("SELECT * FROM MA_SAN_PHAM WHERE SO_LUONG > 0");
            SqlCommand cmd = new SqlCommand("SELECT * FROM MA_SAN_PHAM WHERE SO_LUONG > 0");
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
        public static void CapNhatSoLuong(String masp, int so_luong)
        {
            DataService ds = new DataService();
// Code cũ: OleDbCommand cmd = new OleDbCommand("UPDATE MA_SAN_PHAM SET SO_LUONG = SO_LUONG + @so WHERE ID = @id");
            SqlCommand cmd = new SqlCommand("UPDATE MA_SAN_PHAM SET SO_LUONG = SO_LUONG + @so WHERE ID = @id");
// Code cũ: cmd.Parameters.Add("so", OleDbType.Integer).Value = so_luong;
            cmd.Parameters.Add("so", SqlDbType.Int).Value = so_luong;
// Code cũ: cmd.Parameters.Add("id", OleDbType.VarChar).Value = masp;
            cmd.Parameters.Add("id", SqlDbType.VarChar).Value = masp;
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
