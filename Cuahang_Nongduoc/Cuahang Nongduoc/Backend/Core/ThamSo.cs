using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace CuahangNongduoc
{
    /// <summary>Trạng thái xử lý nội bộ của các màn hình nghiệp vụ cũ.</summary>
    public enum Controll
    {
        Normal,
        AddNew,
        Edit
    }

    /// <summary>
    /// Mô tả quyền chỉnh sửa của giao diện: chỉ xem, thêm bản ghi mới hoặc sửa bản ghi hiện hữu.
    /// </summary>
    public enum FormEditState
    {
        View,
        Add,
        Edit
    }

    public class ThamSo
    {
        public static void PreMonth(ref int thangtruoc, ref int namtruoc, int thang, int nam)
        {
            thangtruoc = thang - 1;
            namtruoc = nam;
            if (thangtruoc == 0)
            {
                thangtruoc = 12;
                namtruoc = nam - 1;
            }
        }

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

        public static long LayMaPhieuNhap()
        {
            DataService ds = new DataService();
            object obj = ds.ExecuteScalar(new SqlCommand("SELECT PHIEU_NHAP FROM THAM_SO"));
            if (obj == null || obj == DBNull.Value)
                return 1;
            return Convert.ToInt64(obj);
        }
        public static void GanMaPhieuNhap(long id)
        {
            if (id < 1 || id > Int32.MaxValue)
                throw new ArgumentOutOfRangeException("id", "Mã phiếu nhập phải nằm trong giới hạn số nguyên.");
            DataService ds = new DataService();
            SqlCommand command = new SqlCommand("UPDATE THAM_SO SET PHIEU_NHAP = @id");
            command.Parameters.Add("@id", SqlDbType.Int).Value = Convert.ToInt32(id);
            int affected = ds.ExecuteNoneQuery(command);
            if (affected <= 0)
                throw new InvalidOperationException("Không thể cập nhật tham số mã phiếu nhập.");
        }

        public static long LayMaPhieuBan()
        {
            DataService ds = new DataService();
            object obj = ds.ExecuteScalar(new SqlCommand("SELECT PHIEU_BAN FROM THAM_SO"));
            if (obj == null || obj == DBNull.Value) return 1;
            return Convert.ToInt64(obj);
        }
        public static void GanMaPhieuBan(long id)
        {
            DataService ds = new DataService();
            SqlCommand command = new SqlCommand("UPDATE THAM_SO SET PHIEU_BAN = @id");
            command.Parameters.Add("@id", SqlDbType.BigInt).Value = id;
            ds.ExecuteNoneQuery(command);
        }

        public static long LayMaPhieuThanhToan()
        {
            DataService ds = new DataService();
            object obj = ds.ExecuteScalar(new SqlCommand("SELECT PHIEU_THANH_TOAN FROM THAM_SO"));
            if (obj == null || obj == DBNull.Value) return 1;
            return Convert.ToInt64(obj);
        }
        public static void GanMaPhieuThanhToan(long id)
        {
            DataService ds = new DataService();
            SqlCommand command = new SqlCommand("UPDATE THAM_SO SET PHIEU_THANH_TOAN = @id");
            command.Parameters.Add("@id", SqlDbType.BigInt).Value = id;
            ds.ExecuteNoneQuery(command);
        }

        public static long SanPham
        {
            get
            {
                DataService ds = new DataService();
                object obj = ds.ExecuteScalar(new SqlCommand("SELECT SAN_PHAM FROM THAM_SO"));
                if (obj == null || obj == DBNull.Value) return 1;
                return Convert.ToInt64(obj);
            }
            set
            {
                DataService ds = new DataService();
                SqlCommand command = new SqlCommand("UPDATE THAM_SO SET SAN_PHAM = @val");
                command.Parameters.Add("@val", SqlDbType.BigInt).Value = value;
                ds.ExecuteNoneQuery(command);
            }
        }

        public static long KhachHang
        {
            get
            {
                DataService ds = new DataService();
                object obj = ds.ExecuteScalar(new SqlCommand("SELECT KHACH_HANG FROM THAM_SO"));
                if (obj == null || obj == DBNull.Value) return 1;
                return Convert.ToInt64(obj);
            }
            set
            {
                DataService ds = new DataService();
                SqlCommand command = new SqlCommand("UPDATE THAM_SO SET KHACH_HANG = @val");
                command.Parameters.Add("@val", SqlDbType.BigInt).Value = value;
                ds.ExecuteNoneQuery(command);
            }
        }

        public static long NhaCungCap
        {
            get
            {
                DataService ds = new DataService();
                object obj = ds.ExecuteScalar(new SqlCommand("SELECT NHA_CUNG_CAP FROM THAM_SO"));
                if (obj == null || obj == DBNull.Value) return 1;
                return Convert.ToInt64(obj);
            }
            set
            {
                DataService ds = new DataService();
                SqlCommand command = new SqlCommand("UPDATE THAM_SO SET NHA_CUNG_CAP = @val");
                command.Parameters.Add("@val", SqlDbType.BigInt).Value = value;
                ds.ExecuteNoneQuery(command);
            }
        }

        public static long PhieuChi
        {
            get
            {
                DataService ds = new DataService();
                object obj = ds.ExecuteScalar(new SqlCommand("SELECT PHIEU_CHI FROM THAM_SO"));
                if (obj == null || obj == DBNull.Value) return 1;
                return Convert.ToInt64(obj);
            }
            set
            {
                DataService ds = new DataService();
                SqlCommand command = new SqlCommand("UPDATE THAM_SO SET PHIEU_CHI = @val");
                command.Parameters.Add("@val", SqlDbType.BigInt).Value = value;
                ds.ExecuteNoneQuery(command);
            }
        }

        public static long MaSanPham
        {
            get
            {
                DataService ds = new DataService();
                object obj = ds.ExecuteScalar(new SqlCommand("SELECT MA_SAN_PHAM FROM THAM_SO"));
                if (obj == null || obj == DBNull.Value) return 1;
                return Convert.ToInt64(obj);
            }
            set
            {
                DataService ds = new DataService();
                SqlCommand command = new SqlCommand("UPDATE THAM_SO SET MA_SAN_PHAM = @val");
                command.Parameters.Add("@val", SqlDbType.BigInt).Value = value;
                ds.ExecuteNoneQuery(command);
            }
        }

        public static long DaiLy
        {
            get
            {
                DataService ds = new DataService();
                object obj = ds.ExecuteScalar(new SqlCommand("SELECT DAI_LY FROM THAM_SO"));
                if (obj == null || obj == DBNull.Value) return 1;
                return Convert.ToInt64(obj);
            }
            set
            {
                DataService ds = new DataService();
                SqlCommand command = new SqlCommand("UPDATE THAM_SO SET DAI_LY = @val");
                command.Parameters.Add("@val", SqlDbType.BigInt).Value = value;
                ds.ExecuteNoneQuery(command);
            }
        }

        public static CuahangNongduoc.BusinessObject.CuaHang LayCuaHang()
        {
            CuahangNongduoc.BusinessObject.CuaHang ch = new CuahangNongduoc.BusinessObject.CuaHang();
            DataService ds = new DataService();
            ds.Load(new SqlCommand("SELECT TEN_CUA_HANG, DIA_CHI, DIEN_THOAI FROM THAM_SO"));
            if (ds.Rows.Count > 0)
            {
                ch.TenCuaHang = ds.Rows[0]["TEN_CUA_HANG"].ToString();
                ch.DiaChi = ds.Rows[0]["DIA_CHI"].ToString();
                ch.DienThoai = ds.Rows[0]["DIEN_THOAI"].ToString();
            }
            return ch;
        }

        public static void GanCuaHang(String ten_cua_hang, String dia_chi, String dien_thoai)
        {
            DataService ds = new DataService();
            SqlCommand cmd = new SqlCommand("UPDATE THAM_SO SET TEN_CUA_HANG = @ten_cua_hang, DIA_CHI = @dia_chi, DIEN_THOAI = @dien_thoai");
            cmd.Parameters.Add("@ten_cua_hang", SqlDbType.NVarChar, 255).Value = ten_cua_hang ?? "";
            cmd.Parameters.Add("@dia_chi", SqlDbType.NVarChar, 255).Value = dia_chi ?? "";
            cmd.Parameters.Add("@dien_thoai", SqlDbType.NVarChar, 50).Value = dien_thoai ?? "";
            ds.ExecuteNoneQuery(cmd);
        }
    }
}
