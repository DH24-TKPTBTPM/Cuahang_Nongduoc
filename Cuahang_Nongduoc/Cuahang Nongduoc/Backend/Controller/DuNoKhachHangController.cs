using System;
using System.Collections.Generic;
using System.Data;
using CuahangNongduoc.BusinessObject;
using CuahangNongduoc.DataLayer;

namespace CuahangNongduoc.Controller
{
    public class DuNoKhachHangController
    {
        private DuNoKhachHangFactory factory = new DuNoKhachHangFactory();
        private DataTable pendingTable;
        private int pendingThang;
        private int pendingNam;

        // ==================== [R24] REFACTORING ====================
        // Lỗi (Code smell) : Dính control UI, N+1 query, mất dữ liệu khi Lưu lỗi
        // Kỹ thuật         : Extract Method + Safe Transaction
        // Tác giả          : BE3 - Bùi Lê Tuấn Anh
        // Hỗ trợ           : FE3 - Nguyễn Thị Ngân Huệ
        // Kiểm duyệt       : QA2 - Dư Thị Trúc Hòa
        // Ngày chỉnh sửa   : 08/11/2026
        // ==============================================================
        public DataTable Tonghop(int thang, int nam)
        {
            int thangTruoc = 0;
            int namTruoc = 0;
            ThamSo.PreMonth(ref thangTruoc, ref namTruoc, thang, nam);

            DataTable khachHang = new KhachHangFactory().DanhsachKhachHang();
            Dictionary<string, long> dauKy = TaoBangTraCuu(
                DuNoKhachHangFactory.LayDuNoTheoKhachHang(thangTruoc, namTruoc), "CUOI_KY");
            Dictionary<string, long> phatSinh = TaoBangTraCuu(
                PhieuBanFactory.LayConNoTheoKhachHang(thang, nam), "CON_NO");
            Dictionary<string, long> daTra = TaoBangTraCuu(
                PhieuThanhToanFactory.LayTongTienTheoKhachHang(thang, nam), "TONG_TIEN");

            pendingTable = factory.LayDuNoKhachHang("-1", 0, 0);
            pendingTable.Clear();
            pendingThang = thang;
            pendingNam = nam;

            foreach (DataRow khachHangRow in khachHang.Rows)
            {
                factory.Add(TaoDongDuNo(khachHangRow, thang, nam, dauKy, phatSinh, daTra));
            }

            return pendingTable;
        }

        // ==================== [R24] REFACTORING ====================
        // Lỗi (Code smell) : Dính control UI, N+1 query, mất dữ liệu khi Lưu lỗi
        // Kỹ thuật         : Extract Method + Safe Transaction
        // Tác giả          : BE3 - Bùi Lê Tuấn Anh
        // Hỗ trợ           : FE3 - Nguyễn Thị Ngân Huệ
        // Kiểm duyệt       : QA2 - Dư Thị Trúc Hòa
        // Ngày chỉnh sửa   : 08/11/2026
        // ==============================================================
        private Dictionary<string, long> TaoBangTraCuu(DataTable bang, string cotGiaTri)
        {
            Dictionary<string, long> ketQua = new Dictionary<string, long>();
            foreach (DataRow dong in bang.Rows)
            {
                ketQua[Convert.ToString(dong["ID_KHACH_HANG"])] = Convert.ToInt64(dong[cotGiaTri]);
            }
            return ketQua;
        }

        // ==================== [R24] REFACTORING ====================
        // Lỗi (Code smell) : Dính control UI, N+1 query, mất dữ liệu khi Lưu lỗi
        // Kỹ thuật         : Extract Method + Safe Transaction
        // Tác giả          : BE3 - Bùi Lê Tuấn Anh
        // Hỗ trợ           : FE3 - Nguyễn Thị Ngân Huệ
        // Kiểm duyệt       : QA2 - Dư Thị Trúc Hòa
        // Ngày chỉnh sửa   : 08/11/2026
        // ==============================================================
        private DataRow TaoDongDuNo(DataRow khachHang, int thang, int nam,
            Dictionary<string, long> dauKy, Dictionary<string, long> phatSinh,
            Dictionary<string, long> daTra)
        {
            string idKhachHang = Convert.ToString(khachHang["ID"]);
            long giaTriDauKy = LayGiaTri(dauKy, idKhachHang);
            long giaTriPhatSinh = LayGiaTri(phatSinh, idKhachHang);
            long giaTriDaTra = LayGiaTri(daTra, idKhachHang);

            DataRow dong = factory.NewRow();
            dong["ID_KHACH_HANG"] = idKhachHang;
            dong["THANG"] = thang;
            dong["NAM"] = nam;
            dong["DAU_KY"] = giaTriDauKy;
            dong["PHAT_SINH"] = giaTriPhatSinh;
            dong["DA_TRA"] = giaTriDaTra;
            dong["CUOI_KY"] = giaTriDauKy + giaTriPhatSinh - giaTriDaTra;
            return dong;
        }

        // ==================== [R24] REFACTORING ====================
        // Lỗi (Code smell) : Dính control UI, N+1 query, mất dữ liệu khi Lưu lỗi
        // Kỹ thuật         : Extract Method + Safe Transaction
        // Tác giả          : BE3 - Bùi Lê Tuấn Anh
        // Hỗ trợ           : FE3 - Nguyễn Thị Ngân Huệ
        // Kiểm duyệt       : QA2 - Dư Thị Trúc Hòa
        // Ngày chỉnh sửa   : 08/11/2026
        // ==============================================================
        private long LayGiaTri(Dictionary<string, long> bang, string idKhachHang)
        {
            long giaTri;
            return bang.TryGetValue(idKhachHang, out giaTri) ? giaTri : 0;
        }

        // ==================== [R24] REFACTORING ====================
        // Lỗi (Code smell) : Dính control UI, N+1 query, mất dữ liệu khi Lưu lỗi
        // Kỹ thuật         : Extract Method + Safe Transaction
        // Tác giả          : BE3 - Bùi Lê Tuấn Anh
        // Hỗ trợ           : FE3 - Nguyễn Thị Ngân Huệ
        // Kiểm duyệt       : QA2 - Dư Thị Trúc Hòa
        // Ngày chỉnh sửa   : 08/11/2026
        // ==============================================================
        private void NapLaiDuLieuChoLuu(DataTable bangTam)
        {
            factory.LayDuNoKhachHang("-1", 0, 0);
            foreach (DataRow dongCu in bangTam.Rows)
            {
                DataRow dongMoi = factory.NewRow();
                dongMoi.ItemArray = (object[])dongCu.ItemArray.Clone();
                factory.Add(dongMoi);
            }
        }

        // ==================== [R24] REFACTORING ====================
        // Lỗi (Code smell) : Dính control UI, N+1 query, mất dữ liệu khi Lưu lỗi
        // Kỹ thuật         : Extract Method + Safe Transaction
        // Tác giả          : BE3 - Bùi Lê Tuấn Anh
        // Hỗ trợ           : FE3 - Nguyễn Thị Ngân Huệ
        // Kiểm duyệt       : QA2 - Dư Thị Trúc Hòa
        // Ngày chỉnh sửa   : 08/11/2026
        // ==============================================================
        public bool Save()
        {
            if (pendingTable == null)
            {
                return false;
            }

            DataTable bangTam = pendingTable.Copy();
            try
            {
                using (IUnitOfWork transaction = new UnitOfWork())
                {
                    factory.Clear(pendingThang, pendingNam);
                    NapLaiDuLieuChoLuu(bangTam);
                    if (!factory.Save())
                    {
                        transaction.Rollback();
                        return false;
                    }

                    transaction.Commit();
                    pendingTable = null;
                    return true;
                }
            }
            catch
            {
                pendingTable = null;
                return false;
            }
        }
    }
}