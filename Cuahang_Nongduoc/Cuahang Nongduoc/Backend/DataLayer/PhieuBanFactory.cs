using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace CuahangNongduoc.DataLayer
{
    // ==================== [R28-BH] REFACTORING ====================
    // Lỗi (Code smell) : Tight Coupling với OleDb/Access cũ, không tương thích với SQL Server
    // Kỹ thuật         : Migrate Data Provider
    // Trước            : Dùng System.Data.OleDb (OleDbCommand, OleDbType), cú pháp Access (TRUE/FALSE), DataService.ExecuteNoneQuery()
    // Sau              : Chuyển sang System.Data.SqlClient (SqlCommand, SqlDbType), chuẩn hóa cú pháp SQL Server (1/0), DataService.UpdateDataTable()
    // Tác giả          : BE2
    // Hỗ trợ           : BE1
    // Kiểm tra         : QA1
    // Ngày chỉnh sửa   : 06/11/2026
    // ==============================================================
    public class PhieuBanFactory
    {
        DataService m_Ds = new DataService();

        public DataTable TimPhieuBan(String idKh, DateTime dt)
        {
            SqlCommand cmd = new SqlCommand("SELECT * FROM PHIEU_BAN WHERE CONVERT(date, NGAY_BAN) = CONVERT(date, @ngay) AND ID_KHACH_HANG = @kh");
            cmd.Parameters.Add("@ngay", SqlDbType.DateTime).Value = dt;
            cmd.Parameters.Add("@kh", SqlDbType.VarChar, 50).Value = idKh;

            m_Ds.Load(cmd);

            return m_Ds;
        }

        public DataTable DanhsachPhieuBanLe()
        {
            // Trong SQL Server, kiểu Boolean tương ứng với BIT (0: Bán lẻ, 1: Đại lý/Bán sỉ)
            SqlCommand cmd = new SqlCommand("SELECT PB.* FROM PHIEU_BAN PB INNER JOIN KHACH_HANG KH ON PB.ID_KHACH_HANG = KH.ID WHERE KH.LOAI_KH = 0");
            m_Ds.Load(cmd);

            return m_Ds;
        }

        public DataTable DanhsachPhieuBanSi()
        {
            // Trong SQL Server, kiểu Boolean tương ứng với BIT (1: Đại lý/Bán sỉ)
            SqlCommand cmd = new SqlCommand("SELECT PB.* FROM PHIEU_BAN PB INNER JOIN KHACH_HANG KH ON PB.ID_KHACH_HANG = KH.ID WHERE KH.LOAI_KH = 1");
            m_Ds.Load(cmd);

            return m_Ds;
        }

        public DataTable LayPhieuBan(String id)
        {
            SqlCommand cmd = new SqlCommand("SELECT * FROM PHIEU_BAN WHERE ID = @id");
            cmd.Parameters.Add("@id", SqlDbType.VarChar, 50).Value = id;
            m_Ds.Load(cmd);

            return m_Ds;
        }

        public DataTable LayChiTietPhieuBan(String idPhieuBan)
        {
            SqlCommand cmd = new SqlCommand("SELECT * FROM CHI_TIET_PHIEU_BAN WHERE ID_PHIEU_BAN = @id");
            cmd.Parameters.Add("@id", SqlDbType.VarChar, 50).Value = idPhieuBan;
            m_Ds.Load(cmd);

            return m_Ds;
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
// ==================== [R29] REFACTORING ====================
// Lỗi (Code smell) : Không kiểm tra giá trị DBNull khi hàm SUM() trả về rỗng
// Kỹ thuật         : Introduce Null Object / Guard
// Tác giả          : BE3 - Bùi Lê Tuấn Anh
// Hỗ trợ           : BE2 - Trần Tuấn Anh
// Kiểm duyệt       : QA2 - Dư Thị Trúc Hòa
// Ngày chỉnh sửa   : 06/11/2026
// ==============================================================
        public static long LayConNo(String kh, int thang, int nam)
        {
            DataService ds = new DataService();
// Code cũ: OleDbCommand cmd = new OleDbCommand("SELECT SUM(CON_NO) FROM PHIEU_BAN WHERE ID_KHACH_HANG = @kh AND MONTH(NGAY_BAN)=@thang AND YEAR(NGAY_BAN)= @nam");
            SqlCommand cmd = new SqlCommand("SELECT ISNULL(SUM(CON_NO), 0) FROM PHIEU_BAN WHERE ID_KHACH_HANG = @kh AND MONTH(NGAY_BAN)=@thang AND YEAR(NGAY_BAN)= @nam");
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

        public static int LaySoPhieu()
        {
            DataService ds = new DataService();
            SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM PHIEU_BAN");

            object obj = ds.ExecuteScalar(cmd);
            if (obj == null || obj == DBNull.Value)
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
            return m_Ds.UpdateDataTable() > 0;
        }
    }
}