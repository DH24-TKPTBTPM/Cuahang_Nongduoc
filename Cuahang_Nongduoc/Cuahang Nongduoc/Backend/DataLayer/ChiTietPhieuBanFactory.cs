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
    public class ChiTietPhieuBanFactory
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
        public DataTable LayChiTietPhieuBan(String idPhieuBan)
        {
// Code cũ: OleDbCommand cmd = new OleDbCommand("SELECT * FROM CHI_TIET_PHIEU_BAN WHERE ID_PHIEU_BAN = @id");
            SqlCommand cmd = new SqlCommand("SELECT * FROM CHI_TIET_PHIEU_BAN WHERE ID_PHIEU_BAN = @id");
// Code cũ: cmd.Parameters.Add("id", OleDbType.VarChar , 50).Value = idPhieuBan;
            cmd.Parameters.Add("id", SqlDbType.VarChar , 50).Value = idPhieuBan;
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
        public DataTable LayChiTietPhieuBan(DateTime dtNgayBan)
        {
// Code cũ: OleDbCommand cmd = new OleDbCommand("SELECT CT.* FROM CHI_TIET_PHIEU_BAN CT INNER JOIN PHIEU_BAN PB ON CT.ID_PHIEU_BAN = PB.ID " +
            SqlCommand cmd = new SqlCommand("SELECT CT.* FROM CHI_TIET_PHIEU_BAN CT INNER JOIN PHIEU_BAN PB ON CT.ID_PHIEU_BAN = PB.ID " +
                    " WHERE PB.NGAY_BAN = @ngayban");
// Code cũ: cmd.Parameters.Add("ngayban", OleDbType.Date).Value = dtNgayBan;
            cmd.Parameters.Add("ngayban", SqlDbType.DateTime).Value = dtNgayBan;
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
        public DataTable LayChiTietPhieuBan(int thang, int nam)
        {
// Code cũ: OleDbCommand cmd = new OleDbCommand("SELECT CT.* FROM CHI_TIET_PHIEU_BAN CT INNER JOIN PHIEU_BAN PB ON CT.ID_PHIEU_BAN = PB.ID " +
            SqlCommand cmd = new SqlCommand("SELECT CT.* FROM CHI_TIET_PHIEU_BAN CT INNER JOIN PHIEU_BAN PB ON CT.ID_PHIEU_BAN = PB.ID " +
                    " WHERE MONTH(PB.NGAY_BAN) = @thang AND YEAR(PB.NGAY_BAN)= @nam");
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
            foreach (DataRow row in m_Ds.Rows)
            {
                if (row.RowState == DataRowState.Added)
                {
                    CuahangNongduoc.DataLayer.MaSanPhanFactory.CapNhatSoLuong(Convert.ToString(row["ID_MA_SAN_PHAM"]), -Convert.ToInt32(row["SO_LUONG"]));
                }
            }
            return m_Ds.ExecuteNoneQuery() > 0;
        }
    }
}
