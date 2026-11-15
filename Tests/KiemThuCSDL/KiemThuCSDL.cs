// =============================================================
// [QA-06] Kiểm thử tự động tầng CSDL: UnitOfWork / rollback / mất kết nối / nhiều form cùng lúc
// Hỗ trợ QA2 chạy ca kiểm thử QA-06 (và kiểm tra lại D08, D11-BH, R04).
// Chạy: double-click ChayKiemThu.bat (sau khi Build chương trình ở chế độ Debug).
// Chương trình tự dọn dữ liệu thử và trả lại tham số như cũ.
// Tác giả: BE1 – Nguyễn Tuấn Anh | Hỗ trợ: Claude (AI) | Kiểm tra: QA2 – <Họ tên> | Ngày tạo: 15/11/2026
// =============================================================
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using CuahangNongduoc;
using CuahangNongduoc.BusinessObject;
using CuahangNongduoc.DataLayer;
using CuahangNongduoc.Patterns.Facade;
using CuahangNongduoc.Patterns.Singleton;

class KiemThuCSDL
{
    static int m_Dat, m_Truot;

    static void Ca(bool dat, string ten)
    {
        if (dat) m_Dat++; else m_Truot++;
        Console.WriteLine((dat ? "  [DAT]   " : "  [TRUOT] ") + ten);
    }

    static object Q(string sql)
    {
        return new DataService().ExecuteScalar(new SqlCommand(sql));
    }

    static long DocPhieuChi() { return Convert.ToInt64(Q("SELECT PHIEU_CHI FROM THAM_SO")); }

    static void GhiPhieuChi(long v)
    {
        SqlCommand c = new SqlCommand("UPDATE THAM_SO SET PHIEU_CHI = @v");
        c.Parameters.Add("@v", SqlDbType.BigInt).Value = v;
        new DataService().ExecuteNoneQuery(c);
    }

    static int Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        Console.WriteLine("KIEM THU TANG CSDL – " + DateTime.Now.ToString("dd/MM/yyyy HH:mm"));
        if (!DataService.OpenConnection())
        {
            Console.WriteLine("Khong ket noi duoc CSDL: " + DataService.LastError.Message);
            return 2;
        }
        long goc = DocPhieuChi();
        try
        {
            KiemThuUnitOfWork(goc);
            KiemThuMatKetNoi(goc);
            KiemThuNhieuFormCungLuc();
            KiemThuLuuXoaHoaDon();
        }
        catch (Exception e)
        {
            Ca(false, "Loi khong mong doi: " + e);
        }
        finally
        {
            GhiPhieuChi(goc);
            Q("DELETE FROM DU_NO_KH WHERE DAU_KY = 0 AND PHAT_SINH = 0 AND DA_TRA = 0 AND CUOI_KY = 0 AND NAM = " + DateTime.Today.Year);
        }
        Console.WriteLine();
        Console.WriteLine("KET QUA: " + m_Dat + " dat, " + m_Truot + " truot");
        return m_Truot == 0 ? 0 : 1;
    }

    // ---- 1. D08 UnitOfWork: commit / rollback ----
    static void KiemThuUnitOfWork(long goc)
    {
        Console.WriteLine("1. UnitOfWork (D08)");
        using (IUnitOfWork u = new UnitOfWork()) { GhiPhieuChi(goc + 100); }
        Ca(DocPhieuChi() == goc, "Khong goi Commit -> tu Rollback");

        using (IUnitOfWork u = new UnitOfWork()) { GhiPhieuChi(goc + 7); u.Commit(); }
        Ca(DocPhieuChi() == goc + 7, "Goi Commit -> du lieu duoc luu");
        GhiPhieuChi(goc);

        try
        {
            using (IUnitOfWork u = new UnitOfWork())
            {
                GhiPhieuChi(goc + 50);
                new DataService().ExecuteNoneQuery(new SqlCommand("UPDATE BANG_KHONG_CO SET X = 1"));
                u.Commit();
            }
        }
        catch (DataAccessException) { }
        Ca(DocPhieuChi() == goc, "Loi o lenh thu 2 -> Rollback ca lenh thu 1");
    }

    // ---- 2. QA-06: mất kết nối giữa lúc lưu ----
    static void KiemThuMatKetNoi(long goc)
    {
        Console.WriteLine("2. Mat ket noi giua luc luu (QA-06)");
        string csdl = DatabaseManager.Instance.ConnectionString;
        bool coLoi = false;
        try
        {
            using (IUnitOfWork u = new UnitOfWork())
            {
                GhiPhieuChi(goc + 999);
                int spid = Convert.ToInt32(Q("SELECT @@SPID"));
                using (SqlConnection khac = new SqlConnection(csdl))
                {
                    khac.Open();
                    new SqlCommand("KILL " + spid, khac).ExecuteNonQuery();   // mô phỏng rớt mạng / tắt SQL Server
                }
                GhiPhieuChi(goc + 1000);   // lệnh tiếp theo phải lỗi
                u.Commit();
            }
        }
        catch (Exception) { coLoi = true; }
        Ca(coLoi, "Lenh sau khi mat ket noi bao loi (khong im lang)");
        Ca(DataService.OpenConnection(), "Chuong trinh tu ket noi lai duoc");
        Ca(DocPhieuChi() == goc, "Du lieu dang luu do KHONG bi ghi xuong CSDL");
    }

    // ---- 3. R04: nhiều form (nhiều DAO) cùng mở, mỗi bảng giữ dữ liệu riêng ----
    static void KiemThuNhieuFormCungLuc()
    {
        Console.WriteLine("3. Nhieu form mo cung luc (R04)");
        DataTable kh = new KhachHangFactory().DanhsachKhachHang();
        DataTable sp = new SanPhamFactory().DanhsachSanPham();
        DataTable pn = new PhieuNhapFactory().DanhsachPhieuNhap();
        int soKH = kh.Rows.Count;
        new KhachHangFactory().DanhsachKhachHang();   // form khác nạp lại cùng bảng
        Ca(soKH > 0 && kh.Rows.Count == soKH && kh.Columns.Contains("HO_TEN"), "Bang Khach hang giu nguyen khi form khac nap lai");
        Ca(sp.Columns.Contains("TEN_SAN_PHAM") && pn.Columns.Contains("NGAY_NHAP"), "Bang San pham / Phieu nhap khong bi ghi de lan nhau");
    }

    // ---- 4. D11-BH: lưu + xóa hóa đơn qua Facade trong 1 transaction ----
    static void KiemThuLuuXoaHoaDon()
    {
        Console.WriteLine("4. Luu / xoa hoa don qua BanHangFacade (D11-BH)");
        long maGoc = ThamSo.LayMaPhieuBan();
        string kh = Convert.ToString(Q("SELECT TOP 1 ID FROM KHACH_HANG WHERE LOAI_KH = 0 ORDER BY ID"));
        string msp = Convert.ToString(Q("SELECT TOP 1 ID FROM MA_SAN_PHAM WHERE SO_LUONG >= 2 ORDER BY ID"));
        int tonTruoc = Convert.ToInt32(Q("SELECT SO_LUONG FROM MA_SAN_PHAM WHERE ID = '" + msp + "'"));
        string id = "TEST" + DateTime.Now.ToString("HHmmss");

        PhieuBan pb = new PhieuBan();
        pb.Id = id; pb.KhachHang = new KhachHang(kh); pb.NgayBan = DateTime.Today; pb.TongTien = 2000; pb.DaTra = 500;
        ChiTietPhieuBan ct = new ChiTietPhieuBan();
        ct.MaSanPham = new MaSanPham(msp, 2); ct.SoLuong = 2; ct.DonGia = 1000;
        List<ChiTietPhieuBan> ds = new List<ChiTietPhieuBan>(); ds.Add(ct);

        BanHangFacade f = new BanHangFacade();
        try
        {
            Ca(f.LuuHoaDon(pb, ds, new List<MaSanPham>()), "LuuHoaDon thanh cong " + f.LastErrorMessage);
            Ca(Convert.ToInt32(Q("SELECT SO_LUONG FROM MA_SAN_PHAM WHERE ID = '" + msp + "'")) == tonTruoc - 2, "Tru kho dung 2");
            Ca(Convert.ToInt64(Q("SELECT CON_NO FROM PHIEU_BAN WHERE ID = '" + id + "'")) == 1500, "CON_NO = TONG_TIEN - DA_TRA");
            Ca(f.XoaPhieuBan(id), "XoaPhieuBan thanh cong " + f.LastErrorMessage);
            Ca(Convert.ToInt32(Q("SELECT SO_LUONG FROM MA_SAN_PHAM WHERE ID = '" + msp + "'")) == tonTruoc, "Xoa phieu -> hoan kho");
            Ca(!f.LuuHoaDon(pb, new List<ChiTietPhieuBan>(), new List<MaSanPham>()), "Chan phieu khong co chi tiet");
        }
        finally
        {
            if (Convert.ToInt32(Q("SELECT COUNT(*) FROM PHIEU_BAN WHERE ID = '" + id + "'")) > 0) f.XoaPhieuBan(id);
            ThamSo.GanMaPhieuBan(maGoc);
        }
    }
}
