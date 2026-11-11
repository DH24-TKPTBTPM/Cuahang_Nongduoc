using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace CuahangNongduoc
{
// ==================== [R21] REFACTORING ====================
// Lỗi (Code smell) : Tên Enum sai chính tả và không rõ nghĩa (Controll)
// Kỹ thuật         : Rename Type
// Tác giả          : BE3 - Bùi Lê Tuấn Anh
// Hỗ trợ           : FE1 - Dương Thị Kim Anh, FE2 - Nguyễn Huỳnh Ngọc Anh
// Kiểm duyệt       : QA1 - Huỳnh Lư Anh Minh
// Ngày chỉnh sửa   : 11/11/2026
// ==============================================================
    public enum FormEditState
    {
        View,
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


        // ==================== [R07] REFACTORING ====================
        // Lỗi (Code smell) : Duplicate Code – 6 hàm LayMa/GanMa + 4 property SanPham/NhaCungCap/KhachHang/PhieuChi
        //                    lặp cùng 1 câu SQL, chỉ khác tên cột; 2 người lưu cùng lúc có thể lấy trùng mã
        // Kỹ thuật         : Extract Method + Parameterize Method
        // Trước            : public static long LayMaPhieuNhap() { ... ExecuteScalar("SELECT PHIEU_NHAP FROM THAM_SO") ... }
        //                    (lặp lại cho PHIEU_BAN, PHIEU_THANH_TOAN, SAN_PHAM, NHA_CUNG_CAP, KHACH_HANG, PHIEU_CHI)
        // Sau              : dồn về GetThamSo(cot) / SetThamSo(cot, giaTri) có lock; các hàm cũ giữ tên
        //                    (gọi 1 dòng) để form không phải sửa – đổi tên dùng chung để dành GĐ4.
        // Tác giả          : BE1 – Nguyễn Tuấn Anh
        // Hỗ trợ           : Claude (AI)
        // Kiểm tra         : QA2 – <Họ tên>
        // Ngày chỉnh sửa   : 08/11/2026
        // ===========================================================
        private static readonly object m_Khoa = new object();

        // Tên cột không truyền được bằng tham số SQL nên chỉ cho phép các cột có thật trong THAM_SO
        private static readonly List<string> m_CotHopLe = new List<string>(new string[]
        {
            "PHIEU_NHAP", "PHIEU_BAN", "PHIEU_THANH_TOAN", "SAN_PHAM", "NHA_CUNG_CAP", "KHACH_HANG", "PHIEU_CHI"
        });

        private static string KiemTraCot(string cot)
        {
            if (!m_CotHopLe.Contains(cot))
                throw new ArgumentException("Cột tham số không hợp lệ: " + cot, "cot");
            return cot;
        }

        public static long GetThamSo(string cot)
        {
            lock (m_Khoa)
            {
                DataService ds = new DataService();
                object obj = ds.ExecuteScalar(new SqlCommand("SELECT " + KiemTraCot(cot) + " FROM THAM_SO"));
                return Convert.ToInt64(obj);
            }
        }

        // ==================== [R08] REFACTORING ====================
        // Lỗi (Code smell) : SQL nối chuỗi (nguy cơ SQL Injection, sai kiểu dữ liệu)
        // Kỹ thuật         : Parameterize Method
        // Trước            : ds.ExecuteNoneQuery(new SqlCommand("UPDATE THAM_SO SET PHIEU_NHAP = " + id));
        // Sau              : giá trị truyền qua tham số @giatri (SqlDbType.BigInt)
        // Tác giả          : BE1 – Nguyễn Tuấn Anh
        // Hỗ trợ           : Claude (AI)
        // Kiểm tra         : QA2 – <Họ tên>
        // Ngày chỉnh sửa   : 08/11/2026
        // ===========================================================
        public static void SetThamSo(string cot, long giaTri)
        {
            lock (m_Khoa)
            {
                DataService ds = new DataService();
                SqlCommand cmd = new SqlCommand("UPDATE THAM_SO SET " + KiemTraCot(cot) + " = @giatri");
                cmd.Parameters.Add("@giatri", SqlDbType.BigInt).Value = giaTri;
                ds.ExecuteNoneQuery(cmd);
            }
        }

        public static long LayMaPhieuNhap() { return GetThamSo("PHIEU_NHAP"); }
        public static void GanMaPhieuNhap(long id) { SetThamSo("PHIEU_NHAP", id); }

        public static long LayMaPhieuBan() { return GetThamSo("PHIEU_BAN"); }
        public static void GanMaPhieuBan(long id) { SetThamSo("PHIEU_BAN", id); }

        public static long LayMaPhieuThanhToan() { return GetThamSo("PHIEU_THANH_TOAN"); }
        public static void GanMaPhieuThanhToan(long id) { SetThamSo("PHIEU_THANH_TOAN", id); }

        public static long SanPham
        {
            get { return GetThamSo("SAN_PHAM"); }
            set { SetThamSo("SAN_PHAM", value); }
        }

        public static long NhaCungCap
        {
            get { return GetThamSo("NHA_CUNG_CAP"); }
            set { SetThamSo("NHA_CUNG_CAP", value); }
        }

        public static long KhachHang
        {
            get { return GetThamSo("KHACH_HANG"); }
            set { SetThamSo("KHACH_HANG", value); }
        }

        public static long PhieuChi
        {
            get { return GetThamSo("PHIEU_CHI"); }
            set { SetThamSo("PHIEU_CHI", value); }
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

    }
}
