using System;
using System.Collections.Generic;
using System.Data.OleDb;
using CuahangNongduoc.BusinessObject;

namespace CuahangNongduoc.DataLayer
{
    public class BaoCaoGiamGiaRepository
    {
        private const string FromJoin = " FROM (PHIEU_BAN AS PB LEFT JOIN NGUOI_DUNG AS ND ON PB.ID_NGUOI_DUNG=ND.ID) ";

        public IList<ChiTietBaoCaoGiamGia> LayChiTiet(DateTime tuNgay, DateTime denNgay)
        {
            KiemTraNgay(tuNgay, denNgay);
            var result = new List<ChiTietBaoCaoGiamGia>();
            var cmd = new OleDbCommand("SELECT PB.ID, PB.NGAY_BAN, PB.ID_NGUOI_DUNG, ND.TEN_DANG_NHAP, ND.HOTEN, PB.LOAI_GIAM_GIA, PB.TY_LE_GIAM_GIA, PB.TIEN_HANG, PB.TIEN_GIAM_GIA, PB.TONG_TIEN" + FromJoin + "WHERE PB.NGAY_BAN >= ? AND PB.NGAY_BAN < ? AND PB.TIEN_GIAM_GIA > 0 ORDER BY ND.HOTEN, PB.NGAY_BAN, PB.ID");
            GanNgay(cmd, tuNgay, denNgay);
            var ds = new DataService(); LoadBaoCao(ds, cmd);
            foreach (System.Data.DataRow row in ds.Rows)
            {
                result.Add(new ChiTietBaoCaoGiamGia
                {
                    IdPhieuBan = Convert.ToString(row["ID"]),
                    NgayBan = Convert.ToDateTime(row["NGAY_BAN"]),
                    IdNguoiDung = row["ID_NGUOI_DUNG"] == DBNull.Value ? (int?)null : Convert.ToInt32(row["ID_NGUOI_DUNG"]),
                    TenDangNhap = row["TEN_DANG_NHAP"] == DBNull.Value ? null : Convert.ToString(row["TEN_DANG_NHAP"]),
                    HoTen = row["HOTEN"] == DBNull.Value || String.IsNullOrWhiteSpace(Convert.ToString(row["HOTEN"])) ? "Nhân viên chưa xác định" : Convert.ToString(row["HOTEN"]),
                    LoaiGiamGia = row["LOAI_GIAM_GIA"] == DBNull.Value ? "Giảm giá" : Convert.ToString(row["LOAI_GIAM_GIA"]),
                    TyLeGiamGia = row["TY_LE_GIAM_GIA"] == DBNull.Value ? 0m : Convert.ToDecimal(row["TY_LE_GIAM_GIA"]),
                    TienHang = ToLong(row["TIEN_HANG"]),
                    TienGiamGia = ToLong(row["TIEN_GIAM_GIA"]),
                    TongTien = ToLong(row["TONG_TIEN"])
                });
            }
            return result;
        }

        public IList<BaoCaoGiamGiaNhanVien> LayTongHop(DateTime tuNgay, DateTime denNgay)
        {
            KiemTraNgay(tuNgay, denNgay);
            var result = new List<BaoCaoGiamGiaNhanVien>();
            var cmd = new OleDbCommand("SELECT PB.ID_NGUOI_DUNG, ND.TEN_DANG_NHAP, ND.HOTEN, COUNT(*) AS SO_HOA_DON, SUM(PB.TIEN_HANG) AS TONG_TIEN_HANG, SUM(PB.TIEN_GIAM_GIA) AS TONG_TIEN_GIAM, SUM(PB.TONG_TIEN) AS TONG_THANH_TOAN" + FromJoin + "WHERE PB.NGAY_BAN >= ? AND PB.NGAY_BAN < ? AND PB.TIEN_GIAM_GIA > 0 GROUP BY PB.ID_NGUOI_DUNG, ND.TEN_DANG_NHAP, ND.HOTEN ORDER BY ND.HOTEN");
            GanNgay(cmd, tuNgay, denNgay);
            var ds = new DataService(); LoadBaoCao(ds, cmd);
            foreach (System.Data.DataRow row in ds.Rows)
                result.Add(new BaoCaoGiamGiaNhanVien
                {
                    IdNguoiDung = row["ID_NGUOI_DUNG"] == DBNull.Value ? (int?)null : Convert.ToInt32(row["ID_NGUOI_DUNG"]),
                    TenDangNhap = row["TEN_DANG_NHAP"] == DBNull.Value ? null : Convert.ToString(row["TEN_DANG_NHAP"]),
                    HoTen = row["HOTEN"] == DBNull.Value || String.IsNullOrWhiteSpace(Convert.ToString(row["HOTEN"])) ? "Nhân viên chưa xác định" : Convert.ToString(row["HOTEN"]),
                    SoHoaDon = Convert.ToInt32(row["SO_HOA_DON"]),
                    TongTienHang = ToLong(row["TONG_TIEN_HANG"]),
                    TongTienGiam = ToLong(row["TONG_TIEN_GIAM"]),
                    TongThanhToan = ToLong(row["TONG_THANH_TOAN"])
                });
            return result;
        }

        private static void GanNgay(OleDbCommand cmd, DateTime tuNgay, DateTime denNgay)
        {
            // OleDb binds positional placeholders; append parameters in SQL order.
            cmd.Parameters.Add("?", OleDbType.Date).Value = tuNgay.Date;
            cmd.Parameters.Add("?", OleDbType.Date).Value = denNgay.Date.AddDays(1);
        }
        private static long ToLong(object value) { return value == DBNull.Value ? 0L : Convert.ToInt64(value); }
        private static void LoadBaoCao(DataService ds, OleDbCommand cmd)
        {
            try { ds.Load(cmd); }
            catch (OleDbException ex)
            {
                if (ex.Message.IndexOf("parameter", StringComparison.OrdinalIgnoreCase) >= 0 ||
                    ex.Message.IndexOf("tham số", StringComparison.OrdinalIgnoreCase) >= 0)
                    throw new InvalidOperationException("Không tìm thấy cột ID_NGUOI_DUNG trong PHIEU_BAN. Hãy chạy Database/Migration_PhatHanhBaoCaoGiamGia.sql trên đúng file cuahang.dll rồi mở lại báo cáo.", ex);
                throw;
            }
        }
        private static void KiemTraNgay(DateTime tuNgay, DateTime denNgay)
        { if (tuNgay.Date > denNgay.Date) throw new ArgumentException("Ngày bắt đầu phải nhỏ hơn hoặc bằng ngày kết thúc."); }
    }

}
