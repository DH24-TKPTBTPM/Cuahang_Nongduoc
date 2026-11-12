using System;
using System.Collections.Generic;
using System.Data;
using CuahangNongduoc.BusinessObject;
using CuahangNongduoc.Controller;

namespace CuahangNongduoc.DataLayer.Mappers
{
    public delegate T RowMapper<T>(DataRow row);

    // ==================== [D05] REFACTORING ====================
    // Lỗi (Code smell) : Code chuyển đổi từng dòng bảng dữ liệu (DataRow) sang đối tượng bị viết lặp lại ở nhiều Controller
    // Kỹ thuật         : Mapper Pattern (PoEAA)
    // Trước            : Mỗi Controller tự đọc từng cột (ID, TEN_SAN_PHAM, SO_LUONG...) để tạo SanPham, MaSanPham, PhieuBan, ChiTietPhieuBan và dễ lỗi khi gặp DBNull
    // Sau              : Gom hết phần chuyển đổi DataRow -> Entity (có kiểm tra DBNull an toàn) về một chỗ là EntityMapper để các Controller gọi lại gọn gàng
    // Tác giả          : BE2
    // Hỗ trợ           : BE3
    // Kiểm tra         : QA1
    // Ngày chỉnh sửa   : 12/11/2026
    // ==============================================================
    public static class EntityMapper
    {
        public static IList<T> MapList<T>(DataTable tbl, RowMapper<T> mapRow)
        {
            IList<T> ds = new List<T>();
            if (tbl == null || mapRow == null) return ds;
            foreach (DataRow row in tbl.Rows)
                ds.Add(mapRow(row));
            return ds;
        }

        public static SanPham ToSanPham(DataRow row)
        {
            return new SanPham
            {
                Id = ToStr(row, "ID"),
                TenSanPham = ToStr(row, "TEN_SAN_PHAM"),
                SoLuong = ToInt(row, "SO_LUONG"),
                DonGiaNhap = ToLong(row, "DON_GIA_NHAP"),
                GiaBanLe = ToLong(row, "GIA_BAN_LE"),
                GiaBanSi = ToLong(row, "GIA_BAN_SI"),
                DonViTinh = new DonViTinhController().LayDVT(ToInt(row, "ID_DON_VI_TINH"))
            };
        }

        public static SoLuongTon ToSoLuongTon(DataRow row)
        {
            return new SoLuongTon
            {
                SanPham = ToSanPham(row),
                SoLuong = ToInt(row, "SO_LUONG_TON")
            };
        }

        public static MaSanPham ToMaSanPham(DataRow row)
        {
            int soLuong = ToInt(row, "SO_LUONG");
            long giaNhap = ToLong(row, "DON_GIA_NHAP");
            return new MaSanPham
            {
                Id = ToStr(row, "ID"),
                SoLuong = soLuong,
                GiaNhap = giaNhap,
                ThanhTien = (long)soLuong * giaNhap,
                NgayNhap = ToDate(row, "NGAY_NHAP"),
                NgaySanXuat = ToDate(row, "NGAY_SAN_XUAT"),
                NgayHetHan = ToDate(row, "NGAY_HET_HAN"),
                SanPham = new SanPhamController().LaySanPham(ToStr(row, "ID_SAN_PHAM"))
            };
        }

        public static PhieuBan ToPhieuBan(DataRow row)
        {
            string id = ToStr(row, "ID");
            return new PhieuBan
            {
                Id = id,
                NgayBan = ToDate(row, "NGAY_BAN"),
                TongTien = ToLong(row, "TONG_TIEN"),
                DaTra = ToLong(row, "DA_TRA"),
                ConNo = ToLong(row, "CON_NO"),
                KhachHang = new KhachHangController().LayKhachHang(ToStr(row, "ID_KHACH_HANG")),
                ChiTiet = new ChiTietPhieuBanController().ChiTietPhieuBan(id)
            };
        }

        public static ChiTietPhieuBan ToChiTietPhieuBan(DataRow row)
        {
            return new ChiTietPhieuBan
            {
                DonGia = ToLong(row, "DON_GIA"),
                SoLuong = ToInt(row, "SO_LUONG"),
                ThanhTien = ToLong(row, "THANH_TIEN"),
                MaSanPham = new MaSanPhamController().LayMaSanPham(ToStr(row, "ID_MA_SAN_PHAM"))
            };
        }

        // ==================== [R35] REFACTORING ====================
        // Lỗi (Code smell) : ToMaSanPham() và ToChiTietPhieuBan() gọi lồng Controller khác cho từng dòng gây N+1 query
        // Kỹ thuật         : Replace Loop with Bulk Query
        // Trước            : Mỗi dòng lại truy vấn thêm MaSanPham -> SanPham -> DonViTinh
        // Sau              : Bổ sung ToMaSanPhamDayDu() và ToChiTietPhieuBanDayDu() đọc trực tiếp từ kết quả JOIN sẵn
        // Tác giả          : BE2
        // Hỗ trợ           : BE1
        // Kiểm tra         : QA2
        // Ngày chỉnh sửa   : 12/11/2026
        // ==============================================================
        public static MaSanPham ToMaSanPhamDayDu(DataRow row)
        {
            if (row.IsNull("MSP_ID")) return null;
            int soLuong = ToInt(row, "MSP_SO_LUONG");
            long giaNhap = ToLong(row, "MSP_DON_GIA_NHAP");
            DonViTinh dvt = row.IsNull("DVT_ID") ? null : new DonViTinh(ToInt(row, "DVT_ID"), ToStr(row, "TEN_DON_VI"));
            SanPham sp = row.IsNull("SP_ID") ? null : new SanPham
            {
                Id = ToStr(row, "SP_ID"),
                TenSanPham = ToStr(row, "TEN_SAN_PHAM"),
                SoLuong = ToInt(row, "SP_SO_LUONG"),
                DonGiaNhap = ToLong(row, "SP_DON_GIA_NHAP"),
                GiaBanLe = ToLong(row, "GIA_BAN_LE"),
                GiaBanSi = ToLong(row, "GIA_BAN_SI"),
                DonViTinh = dvt
            };
            return new MaSanPham
            {
                Id = ToStr(row, "MSP_ID"),
                SoLuong = soLuong,
                GiaNhap = giaNhap,
                ThanhTien = (long)soLuong * giaNhap,
                NgayNhap = ToDate(row, "MSP_NGAY_NHAP"),
                NgaySanXuat = ToDate(row, "MSP_NGAY_SAN_XUAT"),
                NgayHetHan = ToDate(row, "MSP_NGAY_HET_HAN"),
                SanPham = sp
            };
        }

        public static ChiTietPhieuBan ToChiTietPhieuBanDayDu(DataRow row)
        {
            return new ChiTietPhieuBan
            {
                DonGia = ToLong(row, "DON_GIA"),
                SoLuong = ToInt(row, "SO_LUONG"),
                ThanhTien = ToLong(row, "THANH_TIEN"),
                MaSanPham = ToMaSanPhamDayDu(row)
            };
        }

        private static string ToStr(DataRow r, string col) { return r.IsNull(col) ? string.Empty : Convert.ToString(r[col]); }
        private static int ToInt(DataRow r, string col) { return r.IsNull(col) ? 0 : Convert.ToInt32(r[col]); }
        private static long ToLong(DataRow r, string col) { return r.IsNull(col) ? 0 : Convert.ToInt64(r[col]); }
        private static DateTime ToDate(DataRow r, string col) { return r.IsNull(col) ? DateTime.MinValue : Convert.ToDateTime(r[col]); }
    }
}
